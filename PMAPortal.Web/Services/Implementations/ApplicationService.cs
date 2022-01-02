using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class ApplicationService:IApplicationService
    {
        private readonly ILoggerService<ApplicationService> logger;
        private readonly IRepository<Application> applicationRepo;
        private readonly IRepository<Applicant> applicantRepo;
        private readonly IRepository<ApplicantAddress> addressRepo;
        private readonly IRepository<ApplicationAppliance> applicationApplianceRepo;
        private readonly IRepository<ApplicationPet> applicationPetRepo;
        private readonly IRepository<ApplicationStatus> applicationStatusRepo;
        private readonly IRepository<ApplicationStatusLog> applicationStatusLogRepo;
        private readonly IApplicationRepo appRepo;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IMailService mailService;
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly Random Rand;

        public ApplicationService(ILoggerService<ApplicationService> logger,
            IRepository<Application> applicationRepo,
            IRepository<Applicant> applicantRepo,
            IRepository<ApplicantAddress> addressRepo,
            IRepository<ApplicationAppliance> applicationApplianceRepo,
            IRepository<ApplicationPet> applicationPetRepo,
            IRepository<ApplicationStatus> applicationStatusRepo,
            IRepository<ApplicationStatusLog> applicationStatusLogRepo,
            IApplicationRepo appRepo,
            IHttpContextAccessor contextAccessor,
            IMailService mailService,
            IUserService userService,
            ITokenService tokenService)
        {
            this.logger = logger;
            this.applicationRepo = applicationRepo;
            this.applicantRepo = applicantRepo;
            this.addressRepo = addressRepo;
            this.applicationApplianceRepo = applicationApplianceRepo;
            this.applicationPetRepo = applicationPetRepo;
            this.applicationStatusRepo = applicationStatusRepo;
            this.applicationStatusLogRepo = applicationStatusLogRepo;
            this.appRepo = appRepo;
            this.contextAccessor = contextAccessor;
            this.mailService = mailService;
            this.userService = userService;
            this.tokenService = tokenService;
            this.Rand = new Random();
        }

        public async Task<(string trackNo, long applicationId)> AddApplication(Application application, Applicant applicant, ApplicantAddress address, IEnumerable<ApplicationAppliance> appliances, IEnumerable<ApplicationPet> pets)
        {
            //if(applicant.Id == 0 && (await applicantRepo.Any(a=>a.Email == applicant.Email)))
            //{
            //    throw new AppException($"A customer with email '{applicant.Email}' already exist. Please use a different email");
            //}

            if(applicant.Id != 0)
            {
                //await applicantRepo.Update(applicant, false);
                //await addressRepo.Update(address);
                await UpdateApplicant(applicant);
                await UpdateAddress(address);
            }
            else
            {
                applicant.ApplicantAddresses = new List<ApplicantAddress> { address };
                await applicantRepo.Insert(applicant, true);
                //address.ApplicantId = applicant.Id;
                //await addressRepo.Insert(address);
            }

            application.ApplicantId = applicant.Id;
            application.ApplicationAppliances = appliances.ToList();
            application.ApplicationPets = pets.ToList();
            await applicationRepo.Insert(application);

            var trackNo = GenerateTrackingNumber(application.Id);
            application.TrackNumber = trackNo;
            await applicationRepo.Update(application);

            return (application.TrackNumber, application.Id);
        }


        private string GenerateTrackingNumber(long applicationId)
        {
            var randomText1 = Rand.Next(999999).ToString().PadLeft(6, '0');
            var randomText2 = Rand.Next(999999).ToString().PadLeft(6, '0');

            return randomText1 + applicationId.ToString().PadLeft(4, '0') + randomText2;
        }

        public async Task UpdateApplicationStatus(long applicationId, long statusId, string comment = null)
        {
            var application = await applicationRepo.GetById(applicationId);
            if (application == null)
            {
                throw new AppException("Application is not found");
            }

            if(!(await applicationStatusRepo.Any(s=>s.Id == statusId))){
                throw new AppException("Invalid application status");
            }

            var currentUser = contextAccessor.HttpContext?.GetUserSession();
            application.ApplicationStatusId = statusId;
            application.UpdatedDate = DateTimeOffset.Now;
            application.UpdatedBy = currentUser?.Id;
            await applicationRepo.Update(application, false);

            var statusLog = new ApplicationStatusLog
            {
                ActionBy = currentUser?.Id,
                ApplicationId = application.Id,
                ApplicationStatusId = statusId,
                Comment = comment,
                CreatedBy = currentUser?.Id,
                CreatedDate = DateTimeOffset.Now
            };
            await applicationStatusLogRepo.Insert(statusLog);
            await ScheduleMailOnStatusUpdate(applicationId, statusId, comment);
        }

        public async Task AssignInstaller(long applicationId, long installerId)
        {
            var application = await applicationRepo.GetById(applicationId);
            if (application == null)
            {
                throw new AppException("Application is not found");
            }

            if ((await userService.GetUser(installerId))==null)
            {
                throw new AppException("Invalid installer id");
            }

            var currentUser = contextAccessor.HttpContext?.GetUserSession();
            application.InstallerId = installerId;
            application.AssignedBy = currentUser.Id;
            application.UpdatedDate = DateTimeOffset.Now;
            application.UpdatedBy = currentUser?.Id;
            await applicationRepo.Update(application, false);


            await ScheduleMailToInstallerOnNewAssignment(applicationId, installerId, currentUser.FullName, currentUser.Email);
        }

        public async Task UpdateApplicant(Applicant applicant)
        {
            if (applicant == null)
            {
                throw new AppException("Applicant is required");
            }
            var _applicant = await applicantRepo.GetById(applicant.Id);
            if (_applicant == null)
            {
                throw new AppException($"Applicant with id '{applicant.Id}' does not exist");
            }
            var oldApplicant = _applicant.Clone<Applicant>();

            _applicant.FirstName = applicant.FirstName;
            _applicant.LastName = applicant.LastName;
            _applicant.PhoneNumber = applicant.PhoneNumber;
            _applicant.Email = applicant.Email;

            _applicant.UpdatedDate = DateTimeOffset.Now;

            await applicantRepo.Update(_applicant, false);
            // log action
            await logger.LogActivity(
                ActivityActionType.UPDATE_APPLICANT,
                "Applicant - "+_applicant.Email, applicantRepo.TableName, oldApplicant, applicant, $"Updated applicant");
        }

        public async Task UpdateAddress(ApplicantAddress address)
        {
            if (address == null)
            {
                throw new AppException("Address is required");
            }
            var _address = await addressRepo.GetById(address.Id);
            if (_address == null)
            {
                throw new AppException($"Address with id '{address.Id}' does not exist");
            }
            var oldAddress = _address.Clone<ApplicantAddress>();

            _address.Area = address.Area;
            _address.HouseNumber = address.HouseNumber;
            _address.ApartmentNumber = address.ApartmentNumber;
            _address.Street = address.Street;
            _address.Landmark = address.Landmark;
            _address.Description = address.Description;

            _address.UpdatedDate = DateTimeOffset.Now;

            await addressRepo.Update(_address, false);
            // log action
            await logger.LogActivity(
                ActivityActionType.UPDATE_APPLICANT_ADDRESS,
                "Applicant - " + _address.Applicant.Email, applicantRepo.TableName, oldAddress, address, $"Updated applicant address");
        }

        public async Task<Applicant> GetApplicant(string email)
        {
            var applicant = await applicantRepo.GetSingleWhere(a => a.Email == email);
            return applicant;
        }

        public async Task ScheduleNewApplicationMail(long applicationId)
        {
            var application = await applicationRepo.GetById(applicationId);
            var applicant = application.Applicant;
            var supervisors = userService.GetUsers((int)AppRoles.SUPERVISOR);

            var applicantMail = new MailObject
            {
                Recipients = new List<Recipient>
                {
                    new Recipient
                    {
                        Email=applicant.Email,
                        FirstName=applicant.FirstName,
                        LastName=applicant.LastName
                    }
                },
                TrackNo = application.TrackNumber
            };
            await mailService.ScheduleApplicationReceivedMail(applicantMail);

            var supervisorMail = new MailObject
            {
                Recipients = supervisors.Select(u => new Recipient
                {
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }),
                ApplicantEmail = applicant.Email,
                ApplicantName = applicant.FirstName + " " + applicant.LastName,
                MeterType = application.Meter.Name,
                TrackNo = application.TrackNumber
            };
            await mailService.ScheduleNewApplicationMailToSupervisors(supervisorMail);
        }

        private async Task ScheduleMailOnStatusUpdate(long applicationId, long statusId, string comment)
        {
            var application = await applicationRepo.GetById(applicationId);
            var applicant = application.Applicant;
            var installers = userService.GetUsers((int)AppRoles.INSTALLER);
            var discos = userService.GetUsers((int)AppRoles.DISCO_PERSONNEL);
            var status = await applicationStatusRepo.GetById(statusId);

            if (new long[] {3,4,5,6,8 }.Contains(statusId))
            {
                var applicantMail = new MailObject
                {
                    Recipients = new List<Recipient>
                {
                    new Recipient
                    {
                        Email=applicant.Email,
                        FirstName=applicant.FirstName,
                        LastName=applicant.LastName,
                        Token=tokenService.GenerateTokenFromData(applicationId.ToString())
                    }
                },
                    TrackNo = application.TrackNumber,
                    ApplicationStatus = status.Name,
                    Comment = comment,
                    MeterType = application.Meter.Name
                };
                if(statusId==3 || statusId == 4 || statusId== 6)
                {
                    await mailService.ScheduleInstallationUpdateMail(applicantMail);
                }else if (statusId == 5)
                {
                    await mailService.ScheduleInstallationFailedMail(applicantMail);
                }
                else
                {
                    await mailService.ScheduleInstallationCompletedMail(applicantMail);
                }
            }
            if (new long[] { 7,8 }.Contains(statusId))
            {
                var installerMail = new MailObject
                {
                    Recipients = installers.Select(u => new Recipient
                    {
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }),
                    ApplicantEmail = applicant.Email,
                    ApplicantName = applicant.FirstName + " " + applicant.LastName,
                    MeterType = application.Meter.Name,
                    TrackNo = application.TrackNumber,
                    ApplicationStatus = status.Name,
                    Comment = comment
                };
                if (statusId == 7)
                {
                    await mailService.ScheduleDiscoDeclineMailToInstallers(installerMail);
                }
                else
                {
                    await mailService.ScheduleDiscoApproveMailToInstallers(installerMail);
                }
               
            }

            if (new long[] { 6 }.Contains(statusId))
            {
                var discoMail = new MailObject
                {
                    Recipients = discos.Select(u => new Recipient
                    {
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    }),
                    ApplicantEmail = applicant.Email,
                    ApplicantName = applicant.FirstName + " " + applicant.LastName,
                    MeterType = application.Meter.Name,
                    TrackNo = application.TrackNumber,
                    ApplicationStatus=status.Name,
                    Comment=comment
                };
                await mailService.ScheduleInstallationCompletedMailToDisco(discoMail);
            }
        }
        private async Task ScheduleMailToInstallerOnNewAssignment(long applicationId, long installerId, string assignedByName, string assignedByEmail)
        {
            var application = await applicationRepo.GetById(applicationId);
            var applicant = application.Applicant;
            var installer = await userService.GetUser(installerId);

            var installerMail = new MailObject
            {
                Recipients=new List<Recipient>
                {
                    new Recipient
                    {
                        Email=installer.Email,
                        FirstName=installer.FirstName,
                        LastName=installer.LastName
                    }
                },
                ApplicantEmail = applicant.Email,
                ApplicantName = applicant.FirstName + " " + applicant.LastName,
                ApplicantPhoneNo=applicant.PhoneNumber,
                MeterType = application.Meter.Name,
                TrackNo = application.TrackNumber,
                AssignedByEmail=assignedByEmail,
                AssignedByName=assignedByName
            };
            await mailService.ScheduleNewAssignmentMailToInstaller(installerMail);
        }


        public async Task<(bool exist, Applicant applicant)> ApplicantExists(string email)
        {
            var applicant = await applicantRepo.GetSingleWhere(a => a.Email == email);
            var exists = applicant != null;

            return (exists, applicant);
        }

        
        //===================================
        public IEnumerable<Application> GetApplications()
        {
            return appRepo._GetAll();
        }
        public IEnumerable<Application> GetApplications(long[] statuses)
        {
            return appRepo._GetWhere(a=> statuses.Contains(a.ApplicationStatusId));
        }
        public async Task<Application> GetApplication(long id)
        {
            return await applicationRepo.GetById(id);
        }
        public async Task<Application> GetApplication(string trackNumber)
        {
            return await applicationRepo.GetSingleWhere(a => a.TrackNumber == trackNumber);
        }


    }
}
