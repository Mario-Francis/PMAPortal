using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class InstallationService : IInstallationService
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<InstallationLog> installationLogRepo;
        private readonly IRepository<Installation> installationRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly ILoggerService<InstallationService> logger;
        private readonly IHttpContextAccessor accessor;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IMailService mailService;
        private readonly ITokenService tokenService;

        public InstallationService(
            IWebHostEnvironment hostEnvironment,
            IRepository<User> userRepo,
             IRepository<InstallationLog> installationLogRepo,
             IRepository<Installation> installationRepo,
             IRepository<Customer> customerRepo,
            ILoggerService<InstallationService> logger,
            IHttpContextAccessor accessor,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IMailService mailService,
            ITokenService tokenService)
        {
            this.hostEnvironment = hostEnvironment;
            this.userRepo = userRepo;
            this.installationLogRepo = installationLogRepo;
            this.installationRepo = installationRepo;
            this.customerRepo = customerRepo;
            this.logger = logger;
            this.accessor = accessor;
            this.appSettingsDelegate = appSettingsDelegate;
            this.mailService = mailService;
            this.tokenService = tokenService;
        }

        // get customers pending installation assignment
        public IEnumerable<Customer> GetUnassigned()
        {
            return installationRepo.GetWhere(i => i.InstallationStatusId == (long)InstallationStatuses.Pending && i.InstallerId == null).Select(i => i.Customer);
        }

        // get assigned installation
        public IEnumerable<Customer> GetAssigned()
        {
            return installationRepo.GetWhere(i =>
            (i.InstallationStatusId == (long)InstallationStatuses.Pending ||
            i.InstallationStatusId == (long)InstallationStatuses.Scheduled_for_Installation ||
            i.InstallationStatusId == (long)InstallationStatuses.Installation_In_Progress)
            && i.InstallerId != null).Select(i => i.Customer);
        }

        // get completed installation
        public IEnumerable<Installation> GetCompleted()
        {
            return installationRepo.GetWhere(i => i.InstallationStatusId == (long)InstallationStatuses.Installation_Completed);
        }

        // get disco approved
        public IEnumerable<Installation> GetDiscoApproved()
        {
            return installationRepo.GetWhere(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Successful);
        }

        // get failed installation
        public IEnumerable<Installation> GetRejected()
        {
            return installationRepo.GetWhere(i => i.InstallationStatusId == (long)InstallationStatuses.Installation_Failed);
        }

        public IEnumerable<Installation> GetDiscoRejected()
        {
            return installationRepo.GetWhere(i => i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Failed);
        }

        public async Task AssignInstallation(long installerId, long installationId)
        {
            var installer = await userRepo.GetById(installerId);
            if (installer == null)
            {
                throw new AppException($"Installer id is invalid");
            }
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (!installer.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.INSTALLER))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.INSTALLER.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var oldInstallation = installation.Clone<Installation>();

            installation.AssignedBy = currentUser.Id;
            installation.InstallerId = installerId;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await installationRepo.Update(installation, false);
            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Pending, $"Assigned installation to {installer.FirstName} {installer.LastName}", null, false);
            // log activity
            await logger.LogActivity(ActivityActionType.ASSIGN_INSTALLATION_TASK, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Assigned installation task of {installation.Customer.CustomerName} to {installer.FirstName} {installer.LastName}");

            var installerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installer.Email,
                            FirstName=installer.FirstName,
                            LastName=installer.LastName
                        }
                    },
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerEmail = installation.Customer.Email,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                AssignedByName = currentUser.FullName,
                AssignedByEmail = currentUser.Email
            };
            await mailService.ScheduleNewAssignmentMailToInstaller(installerMail);
        }

        public async Task ReassignInstallation(long installerId, long installationId)
        {
            var installer = await userRepo.GetById(installerId);
            if (installer == null)
            {
                throw new AppException($"Installer id is invalid");
            }
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (!installer.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.INSTALLER))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.INSTALLER.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            if (installation.InstallerId != installerId)
            {
                var _oldInstallation = installation.Clone<Installation>();

                installation.AssignedBy = currentUser.Id;
                installation.InstallerId = installerId;
                installation.UpdatedBy = currentUser.Id;
                installation.UpdatedDate = DateTimeOffset.Now;

                await installationRepo.Update(installation, false);
                await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Pending, $"Reassigned installation to {installer.FirstName} {installer.LastName}", null, false);
                // log activity
                await logger.LogActivity(ActivityActionType.REASSIGN_INSTALLATION_TASK, currentUser.Email, installationRepo.TableName, _oldInstallation, installation,
                    $"Reassigned installation task of {installation.Customer.CustomerName} to {installer.FirstName} {installer.LastName}");

                var installerMail = new MailObject
                {
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installer.Email,
                            FirstName=installer.FirstName,
                            LastName=installer.LastName
                        }
                    },
                    CustomerName = installation.Customer.CustomerName,
                    CustomerPhoneNo = installation.Customer.PhoneNumber,
                    CustomerEmail = installation.Customer.Email,
                    CustomerAccountNumber = installation.Customer.AccountNumber,
                    AssignedByName = currentUser.FullName,
                    AssignedByEmail = currentUser.Email
                };
                await mailService.ScheduleNewAssignmentMailToInstaller(installerMail);
            }


        }

        // assign installations
        public async Task AssignInstallations(long installerId, IEnumerable<long> installationIds)
        {
            installationIds = installationIds.Distinct();
            var installer = await userRepo.GetById(installerId);
            if (installer == null)
            {
                throw new AppException($"Installer id is invalid");
            }
            var invalidInstallationIds = installationIds.Where(id => !installationRepo.Any(i => i.Id == id));
            if (invalidInstallationIds.Count() > 0)
            {
                throw new AppException($"Installation id(s) '{string.Join(", ", invalidInstallationIds)}' are invalid");
            }

            if (!installer.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.INSTALLER))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.INSTALLER.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var installations = installationRepo.GetWhere(i => installationIds.Contains(i.Id)).ToList();
            foreach (var i in installations)
            {
                await AddInstallationLog(i.Id, currentUser.Id, (long)InstallationStatuses.Pending, $"Assigned installation to {installer.FirstName} {installer.LastName}", null, false);
            }
            installations.ForEach(i =>
            {
                i.AssignedBy = currentUser.Id;
                i.InstallerId = installerId;
                i.UpdatedBy = currentUser.Id;
                i.UpdatedDate = DateTimeOffset.Now;
            });

            await installationRepo.UpdateRange(installations, false);

            // log activity
            await logger.LogActivity(ActivityActionType.ASSIGN_INSTALLATION_TASKS, currentUser.Email,
                $"Assigned {installationIds.Count()} installation tasks to {installer.FirstName} {installer.LastName}");

            foreach (var i in installations)
            {
                var installerMail = new MailObject
                {
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installer.Email,
                            FirstName=installer.FirstName,
                            LastName=installer.LastName
                        }
                    },
                    CustomerName = i.Customer.CustomerName,
                    CustomerPhoneNo = i.Customer.PhoneNumber,
                    CustomerEmail = i.Customer.Email,
                    CustomerAccountNumber = i.Customer.AccountNumber,
                    AssignedByName = currentUser.FullName,
                    AssignedByEmail = currentUser.Email
                };
                await mailService.ScheduleNewAssignmentMailToInstaller(installerMail);
            }
        }

        public async Task ReassignInstallations(long installerId, IEnumerable<long> installationIds)
        {
            installationIds = installationIds.Distinct();
            var installer = await userRepo.GetById(installerId);
            if (installer == null)
            {
                throw new AppException($"Installer id is invalid");
            }
            var invalidInstallationIds = installationIds.Where(id => !installationRepo.Any(s => s.Id == id));
            if (invalidInstallationIds.Count() > 0)
            {
                throw new AppException($"Installation id(s) '{string.Join(", ", invalidInstallationIds)}' are invalid");
            }

            if (!installer.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.INSTALLER))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.INSTALLER.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var installations = installationRepo.GetWhere(i => installationIds.Contains(i.Id) && i.InstallerId != installerId).ToList();
            foreach (var i in installations)
            {
                await AddInstallationLog(i.Id, currentUser.Id, (long)InstallationStatuses.Pending, $"Reassigned installation to {installer.FirstName} {installer.LastName}", null, false);
            }
            installations.ForEach(i =>
            {
                i.AssignedBy = currentUser.Id;
                i.InstallerId = installerId;
                i.UpdatedBy = currentUser.Id;
                i.UpdatedDate = DateTimeOffset.Now;
            });

            await installationRepo.UpdateRange(installations, false);

            // log activity
            await logger.LogActivity(ActivityActionType.REASSIGN_INSTALLATION_TASKS, currentUser.Email,
                $"Assigned {installationIds.Count()} installation tasks to {installer.FirstName} {installer.LastName}");

            foreach (var i in installations)
            {
                var installerMail = new MailObject
                {
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installer.Email,
                            FirstName=installer.FirstName,
                            LastName=installer.LastName
                        }
                    },
                    CustomerName = i.Customer.CustomerName,
                    CustomerPhoneNo = i.Customer.PhoneNumber,
                    CustomerEmail = i.Customer.Email,
                    CustomerAccountNumber = i.Customer.AccountNumber,
                    AssignedByName = currentUser.FullName,
                    AssignedByEmail = currentUser.Email
                };
                await mailService.ScheduleNewAssignmentMailToInstaller(installerMail);
            }
        }


        // unassign installation
        public async Task UnassignInstallation(long installationId)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (installation.InstallationStatusId != (long)InstallationStatuses.Pending)
            {
                throw new AppException($"Installation cannot be unassigned as installer has started processing. Try reassign");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var _oldInstallation = installation.Clone<Installation>();

            installation.AssignedBy = null;
            installation.InstallerId = null;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await installationRepo.Update(installation, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UNASSIGN_INSTALLATION_TASK, currentUser.Email, installationRepo.TableName, _oldInstallation, installation,
                $"Unassigned installation task of {installation.Customer.CustomerName} from {_oldInstallation.Installer.FirstName} {_oldInstallation.Installer.LastName}");

        }

        public async Task UnassignInstallations(IEnumerable<long> installationIds)
        {
            installationIds = installationIds.Distinct();
            var startedInstallationIds = installationIds.Where(id => installationRepo.Any(s => s.Id == id && s.InstallationStatusId != (long)InstallationStatuses.Pending));

            if (startedInstallationIds.Count() > 0)
            {
                throw new AppException($"Installations(s) with id(s) '{string.Join(", ", startedInstallationIds)}' are been processed hence canot be unassigned. Try reassign");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            var installations = installationRepo.GetWhere(i => installationIds.Contains(i.Id)).ToList();
            installations.ForEach(i =>
            {
                i.AssignedBy = null;
                i.InstallerId = null;
                i.UpdatedBy = currentUser.Id;
                i.UpdatedDate = DateTimeOffset.Now;
            });

            await installationRepo.UpdateRange(installations, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UNASSIGN_INSTALLATION_TASKS, currentUser.Email,
                $"Unassigned installation tasks");

        }

        // schedule installation && send notification
        public async Task ScheduleInstallation(long installationId, DateTimeOffset scheduleDate)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            if (installation.InstallerId != currentUser.Id)
            {
                throw new AppException($"Installation cannot be scheduled as you are not assigned to it");
            }

            var isReschedule = installation.ScheduleDate != null;
            var oldInstallation = installation.Clone<Installation>();


            installation.ScheduleDate = scheduleDate;
            installation.InstallationStatusId = (long)InstallationStatuses.Scheduled_for_Installation;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await installationRepo.Update(installation, false);
            var descripTerm = oldInstallation.ScheduleDate == null ? $"Scheduled" : "Rescheduled";
            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Scheduled_for_Installation, $"{descripTerm} installation date for {scheduleDate.ToString("dd-MM-yyyy")}");
            // log activity
            await logger.LogActivity(ActivityActionType.SCHEDULE_INSTALLATION, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Scheduled installation for {scheduleDate.ToString("yyyy-MM-dd")}");

            var customerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Customer.Email,
                            FirstName=installation.Customer.CustomerName,
                            LastName=null
                        }
                    },
                InstallerEmail = currentUser.Email,
                InstallerName = currentUser.FullName,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                ScheduleDate = scheduleDate.ToString("MMM d, yyyy 'at' hh:mm")
            };
            await mailService.ScheduleInstallationScheduleMailToCustomer(customerMail);
        }
        public async Task ScheduleInstallations(IEnumerable<long> installationIds, DateTimeOffset scheduleDate)
        {
            installationIds = installationIds.Distinct();
            var invalidInstallationIds = installationIds.Where(id => !installationRepo.Any(s => s.Id == id));
            if (invalidInstallationIds.Count() > 0)
            {
                throw new AppException($"Installation id(s) '{string.Join(", ", installationIds)}' are invalid");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            var installations = installationRepo.GetWhere(s => installationIds.Contains(s.Id)).ToList();
            if (installations.Any(s => s.InstallerId != currentUser.Id))
            {
                throw new AppException($"One or more installations cannot be scheduled as you are not assigned to them");
            }

            var installationsForReschedule = installations.Where(s => s.ScheduleDate != null);
            var installationsForSchedule = installations.Where(s => s.ScheduleDate == null);

            foreach (var i in installations)
            {
                var descripTerm = i.ScheduleDate == null ? $"Scheduled" : "Rescheduled";
                await AddInstallationLog(i.Id, currentUser.Id, (long)InstallationStatuses.Scheduled_for_Installation, $"{descripTerm} installation date for {scheduleDate.ToString("dd-MM-yyyy")}", null, false);
            }

            installations.ForEach(s =>
            {
                s.ScheduleDate = scheduleDate;
                s.InstallationStatusId = (long)InstallationStatuses.Scheduled_for_Installation;
                s.UpdatedBy = currentUser.Id;
                s.UpdatedDate = DateTimeOffset.Now;
            });

            await installationRepo.UpdateRange(installations, false);

            // log activity
            await logger.LogActivity(ActivityActionType.SCHEDULE_INSTALLATIONS, currentUser.Email,
                $"Scheduled installations with ids {string.Join(", ", installations.Select(s => s.Id))} for {scheduleDate.ToString("yyyy-MM-dd")}");

            foreach (var i in installations)
            {
                var customerMail = new MailObject
                {
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=i.Customer.Email,
                            FirstName=i.Customer.CustomerName,
                            LastName=null
                        }
                    },
                    InstallerEmail = currentUser.Email,
                    InstallerName = currentUser.FullName,
                    CustomerAccountNumber = i.Customer.AccountNumber,
                    ScheduleDate = scheduleDate.ToString("MMM d, yyyy 'at' hh:mm")
                };
                await mailService.ScheduleInstallationScheduleMailToCustomer(customerMail);
            }

        }

        // get assigned for installer
        public IEnumerable<Installation> GetPendingInstaller(long installerId)
        {
            return installationRepo.GetWhere(i =>
            (i.InstallationStatusId == (long)InstallationStatuses.Pending ||
            i.InstallationStatusId == (long)InstallationStatuses.Scheduled_for_Installation ||
            i.InstallationStatusId == (long)InstallationStatuses.Installation_In_Progress)
            && i.InstallerId == installerId);
        }

        public IEnumerable<Installation> GetCompletedByInstaller(long installerId)
        {
            return installationRepo.GetWhere(i =>
             (i.InstallationStatusId == (long)InstallationStatuses.Installation_Completed ||
            i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Successful)
            && i.InstallerId == installerId);
        }
        public IEnumerable<Installation> GetRejetcedByInstaller(long installerId)
        {
            return installationRepo.GetWhere(i =>
             (i.InstallationStatusId == (long)InstallationStatuses.Installation_Failed)
            && i.InstallerId == installerId);
        }
        public IEnumerable<Installation> GetDiscoRejected(long installerId)
        {
            return installationRepo.GetWhere(i =>
             (i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Failed)
            && i.InstallerId == installerId);
        }
        public IEnumerable<Installation> GetDiscoApproved(long installerId)
        {
            return installationRepo.GetWhere(i =>
             (i.InstallationStatusId == (long)InstallationStatuses.Disco_Confirmation_Successful)
            && i.InstallerId == installerId);
        }

        // get installation
        public async Task<Installation> GetInstallation(long installationId)
        {
            return await installationRepo.GetById(installationId);
        }
        public IEnumerable<Installation> GetInstallations()
        {
            return installationRepo.GetAll();
        }

        // update installation meter info
        public async Task UpdateMeterInfo(long installationId, string meterType, string meterNo)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            if (installationRepo.Any(i => i.MeterNumber == meterNo && i.Id != installationId))
            {
                throw new AppException($"An installation with meter number '{meterNo}' already exist");
            }

            var oldInstallation = installation.Clone<Installation>();

            installation.MeterType = meterType;
            installation.MeterNumber = meterNo;
            installation.InstallationStatusId = (long)InstallationStatuses.Installation_In_Progress;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await installationRepo.Update(installation, false);
            if (oldInstallation.MeterNumber == null)
            {
                await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Installation_In_Progress, "Started installation");
            }
            // log activity
            await logger.LogActivity(ActivityActionType.UPDATE_INSTALLATION_METER_INFO, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Update installation number to {meterNo}");

            var customerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Customer.Email,
                            FirstName=installation.Customer.CustomerName,
                            LastName=null
                        }
                    },
                InstallerEmail = currentUser.Email,
                InstallerName = currentUser.FullName,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name
            };
            await mailService.ScheduleInstallationStatusUpdateMail(customerMail);
        }

        // Upload image
        public async Task<string> UploadImage(long installationId, string imageFieldName, IFormFile file)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            var maxUploadSize = appSettingsDelegate.Value.MaxUploadSize;
            if (file == null)
            {
                throw new AppException("No file uploaded");
            }
            else if (file.Length > (maxUploadSize * 1024 * 1024))
            {
                throw new AppException($"Max upload size exceeded. Max size is {maxUploadSize}MB");
            }
            else
            {

                var ext = Path.GetExtension(file.FileName);
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    throw new AppException($"Invalid file format. Supported file formats include .jpg, .jpeg and .png");
                }

                var path = SaveFile(file);

                var oldInstallation = installation.Clone<Installation>();

                var currentUser = accessor.HttpContext.GetUserSession();

                installation.UpdatedBy = currentUser.Id;
                installation.UpdatedDate = DateTimeOffset.Now;
                installation.SetProperty(imageFieldName, path);

                await installationRepo.Update(installation, false);

                // log activity
                await logger.LogActivity(ActivityActionType.UPLOAD_INSTALLATION_IMAGE, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                    $"Upload installation image");

                return path;
            }
        }

        private string SaveFile(IFormFile file)
        {
            string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "uploads/images");
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var _filePath = "uploads/images/" + fileName;
            return _filePath;
        }

        private void DeleteFile(string filePath)
        {
            string fullPath = Path.Combine(hostEnvironment.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        // Delete image
        public async Task DeleteImage(long installationId, string imageFieldName)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            var oldInstallation = installation.Clone<Installation>();

            var currentUser = accessor.HttpContext.GetUserSession();

            var path = installation.GetProperty(imageFieldName) as string;
            DeleteFile(path);

            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;
            installation.SetProperty(imageFieldName, null);

            await installationRepo.Update(installation, false);

            // log activity
            await logger.LogActivity(ActivityActionType.DELETE_INSTALLATION_IMAGE, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Delete installation image");

        }

        // complete installation
        public async Task CompleteInstallation(long installationId, string comment = null)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (installation.MeterType == null || installation.MeterNumber == null ||
                installation.CompleteWithSealImagePath == null || installation.CustomerBillImagePath == null ||
                installation.InProgessImagePath == null || installation.LocationFrontViewImagePath == null ||
                installation.MeterCardImagePath == null || installation.MeterNamePlateImagePath == null ||
                installation.MeterPointBeforeInstallationImagePath == null ||
                installation.MeterSealImagePath == null ||
                 installation.SupplyCableToThePremisesImagePath == null ||
                installation.SupplyCableVisibleToMeterImagePath == null)
            {
                throw new AppException($"Installation cannot be completed as there are one or more required images");
            }

            var oldInstallation = installation.Clone<Installation>();

            var currentUser = accessor.HttpContext.GetUserSession();

            installation.InstallationStatusId = (long)InstallationStatuses.Installation_Completed;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await installationRepo.Update(installation, false);
            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Installation_Completed, "Completed installation", comment);
            // log activity
            await logger.LogActivity(ActivityActionType.COMPLETE_INSTALLATION, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Complete installation");

            var customerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Customer.Email,
                            FirstName=installation.Customer.CustomerName,
                            LastName=null
                        }
                    },
                InstallerEmail = currentUser.Email,
                InstallerName = currentUser.FullName,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber
            };
            await mailService.ScheduleInstallationCompletedMailToCustomer(customerMail);

            var discos = userRepo.GetWhere(u => u.UserRoles.Any(r => r.RoleId == (long)AppRoles.DISCO_PERSONNEL));
            var discoMail = new MailObject
            {
                Recipients = discos.Select(d => new Recipient
                {
                    Email = d.Email,
                    FirstName = d.FirstName,
                    LastName = d.LastName
                }),
                CustomerEmail = installation.Customer.Email,
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber
            };
            await mailService.ScheduleInstallationCompletedMailToDisco(discoMail);
        }

        public async Task AddInstallationLog(long installationId, long? actionBy, long statusId, string description = null, string comment = null, bool save = true)
        {
            var log = new InstallationLog
            {
                InstallationId = installationId,
                ActionBy = actionBy,
                InstallationStatusId = statusId,
                Description = description,
                Comment = comment,
                CreatedBy = actionBy,
                CreatedDate = DateTimeOffset.Now
            };
            await installationLogRepo.Insert(log, save);
        }
        // reject installation
        public async Task RejectInstallation(long installationId, string comment)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new AppException($"Comment is required");
            }

            var oldInstallation = installation.Clone<Installation>();

            var currentUser = accessor.HttpContext.GetUserSession();

            installation.InstallationStatusId = (long)InstallationStatuses.Installation_Failed;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Installation_Failed, "Rejected installation", comment);
            await installationRepo.Update(installation, false);

            // log activity
            await logger.LogActivity(ActivityActionType.REJECT_INSTALLATION, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Reject installation");
        }

        // disco approve installation
        public async Task DiscoApproveInstallation(long installationId, string comment = null)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }


            var oldInstallation = installation.Clone<Installation>();

            var currentUser = accessor.HttpContext.GetUserSession();

            installation.InstallationStatusId = (long)InstallationStatuses.Disco_Confirmation_Successful;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Disco_Confirmation_Successful, "Disco Approved installation", comment);
            await installationRepo.Update(installation, false);

            // log activity
            await logger.LogActivity(ActivityActionType.DISCO_APPROVE_INSTALLATION, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Disco Approved installation");

            var installerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Installer.Email,
                            FirstName=installation.Installer.FirstName,
                            LastName=installation.Installer.LastName
                        }
                    },
                CustomerEmail = installation.Customer.Email,
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber,
                Comment = comment
            };
            await mailService.ScheduleDiscoApprovalMailToInstaller(installerMail);

            var customerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Customer.Email,
                            FirstName=installation.Customer.CustomerName,
                            LastName=null,
                            Token=tokenService.GenerateTokenFromData(installationId.ToString())
                        }
                    },
                CustomerEmail = installation.Customer.Email,
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber,
                Comment = comment
            };
            await mailService.ScheduleInstallationApprovedMailToCustomer(customerMail);
        }
        // disco reject installation
        public async Task DiscoRejectInstallation(long installationId, string comment)
        {
            var installation = await installationRepo.GetById(installationId);
            if (installation == null)
            {
                throw new AppException($"Installation id is invalid");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new AppException($"Comment is required");
            }

            var oldInstallation = installation.Clone<Installation>();

            var currentUser = accessor.HttpContext.GetUserSession();

            installation.InstallationStatusId = (long)InstallationStatuses.Disco_Confirmation_Failed;
            installation.UpdatedBy = currentUser.Id;
            installation.UpdatedDate = DateTimeOffset.Now;

            await AddInstallationLog(installationId, currentUser.Id, (long)InstallationStatuses.Disco_Confirmation_Failed, "Disco Rejected installation", comment);
            await installationRepo.Update(installation, false);

            // log activity
            await logger.LogActivity(ActivityActionType.DISCO_REJECT_INSTALLATION, currentUser.Email, installationRepo.TableName, oldInstallation, installation,
                $"Disco Reject installation");

            var installerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Installer.Email,
                            FirstName=installation.Installer.FirstName,
                            LastName=installation.Installer.LastName
                        }
                    },
                CustomerEmail = installation.Customer.Email,
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber,
                Comment = comment
            };
            await mailService.ScheduleDiscoRejectionMailToInstaller(installerMail);

            var customerMail = new MailObject
            {
                Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Email=installation.Customer.Email,
                            FirstName=installation.Customer.CustomerName,
                            LastName=null
                        }
                    },
                CustomerEmail = installation.Customer.Email,
                CustomerName = installation.Customer.CustomerName,
                CustomerPhoneNo = installation.Customer.PhoneNumber,
                CustomerAccountNumber = installation.Customer.AccountNumber,
                InstallationStatus = installation.InstallationStatus.Name,
                MeterType = installation.MeterType,
                MeterNo = installation.MeterNumber,
                Comment = comment
            };
            await mailService.ScheduleDiscoRejectionMailToCustomer(customerMail);
        }
    }
}
