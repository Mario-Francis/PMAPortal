using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class SurveyService: ISurveyService
    {
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Survey> surveyRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly ILoggerService<SurveyService> logger;
        private readonly IHttpContextAccessor accessor;

        public SurveyService(
            IRepository<User> userRepo,
            IRepository<Survey> surveyRepo,
             IRepository<Customer> customerRepo,
            ILoggerService<SurveyService> logger,
            IHttpContextAccessor accessor)
        {
            this.userRepo = userRepo;
            this.surveyRepo = surveyRepo;
            this.customerRepo = customerRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // get customers pending survey
        public IEnumerable<Customer> GetUnassigned()
        {
            return customerRepo.GetWhere(c => c.Surveys.Count() == 0);
        }

        // get assigned survey
        public IEnumerable<Customer> GetAssigned()
        {
            return customerRepo.GetWhere(c => c.Surveys.Count() > 0 && c.Surveys.First().SurveyRemark==null);
        }

        // get customers with completed survey
        public IEnumerable<Survey> GetCompleted()
        {
            //return customerRepo.GetWhere(c => c.Surveys.Count() > 0 && c.Surveys.First().SurveyRemark != null);
            return surveyRepo.GetWhere(s => s.SurveyRemark != null);
        }

        public async Task AssignSurvey(long surveyStaffId, long customerId)
        {
            var surveyStaff = await userRepo.GetById(surveyStaffId);
            if (surveyStaff == null)
            {
                throw new AppException($"Survey staff id is invalid");
            }
            var customer = await customerRepo.GetById(customerId);
            if (customer == null)
            {
                throw new AppException($"Customer id is invalid");
            }

            if (!surveyStaff.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.SURVEY_STAFF))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.SURVEY_STAFF.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var survey = new Survey
            {
                AssignedBy = currentUser.Id,
                SurveyStaffId = surveyStaffId,
                CustomerId = customerId,
                SurveryCompany = surveyStaff.CompanyName,
                CreatedBy = currentUser.Id,
                UpdatedBy = currentUser.Id,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
            await surveyRepo.Insert(survey, false);

            // log activity
            await logger.LogActivity(ActivityActionType.ASSIGN_SURVEY_TASK, currentUser.Email, surveyRepo.TableName, new Survey(), survey,
                $"Assigned survey task of {customer.CustomerName} to {surveyStaff.FirstName} {surveyStaff.LastName}");
        }
        public async Task ReassignSurvey(long surveyStaffId, long surveyId)
        {
            var surveyStaff = await userRepo.GetById(surveyStaffId);
            if (surveyStaff == null)
            {
                throw new AppException($"Survey staff id is invalid");
            }
            var survey = await surveyRepo.GetById(surveyId);
            if (survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            if (!surveyStaff.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.SURVEY_STAFF))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.SURVEY_STAFF.ToString()}' role");
            }

           
            var currentUser = accessor.HttpContext.GetUserSession();
            if(survey.SurveyStaffId != surveyStaffId)
            {
                var _oldSurvey = survey.Clone<Survey>();

                survey.AssignedBy = currentUser.Id;
                survey.SurveyStaffId = surveyStaffId;
                survey.SurveryCompany = surveyStaff.CompanyName;
                survey.UpdatedBy = currentUser.Id;
                survey.UpdatedDate = DateTimeOffset.Now;

                await surveyRepo.Update(survey, false);

                // log activity
                await logger.LogActivity(ActivityActionType.REASSIGN_SURVEY_TASK, currentUser.Email, surveyRepo.TableName, _oldSurvey, survey,
                    $"Reassigned survey task of {survey.Customer.CustomerName} to {surveyStaff.FirstName} {surveyStaff.LastName}");
            }

           
        }

        // assign surveys
        public async Task AssignSurveys(long surveyStaffId, IEnumerable<long> customerIds)
        {
            customerIds = customerIds.Distinct();
            var surveyStaff = await userRepo.GetById(surveyStaffId);
            if (surveyStaff == null)
            {
                throw new AppException($"Survey staff id is invalid");
            }
            var invalidCustomerIds = customerIds.Where(id => !customerRepo.Any(c => c.Id == id));
            if(invalidCustomerIds.Count() > 0)
            {
                throw new AppException($"Customer id(s) '{string.Join(", ", customerIds)}' are invalid");
            }

            if (!surveyStaff.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.SURVEY_STAFF))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.SURVEY_STAFF.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var surveys = customerIds.Select(id => new Survey
            {
                AssignedBy = currentUser.Id,
                SurveyStaffId = surveyStaffId,
                CustomerId = id,
                SurveryCompany = surveyStaff.CompanyName,
                CreatedBy = currentUser.Id,
                UpdatedBy = currentUser.Id,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            });
            await surveyRepo.InsertRange(surveys, false);

            // log activity
            await logger.LogActivity(ActivityActionType.ASSIGN_SURVEY_TASKS, currentUser.Email, 
                $"Assigned {customerIds.Count()} survey tasks to {surveyStaff.FirstName} {surveyStaff.LastName}");
        }

        public async Task ReassignSurveys(long surveyStaffId, IEnumerable<long> surveyIds)
        {
            surveyIds = surveyIds.Distinct();
            var surveyStaff = await userRepo.GetById(surveyStaffId);
            if (surveyStaff == null)
            {
                throw new AppException($"Survey staff id is invalid");
            }
            var invalidSurveyIds = surveyIds.Where(id => !surveyRepo.Any(s => s.Id == id));
            if (invalidSurveyIds.Count() > 0)
            {
                throw new AppException($"Survey id(s) '{string.Join(", ", surveyIds)}' are invalid");
            }

            if (!surveyStaff.UserRoles.Any(ur => ur.RoleId == (long)AppRoles.SURVEY_STAFF))
            {
                throw new AppException($"Specified user does not have the '{AppRoles.SURVEY_STAFF.ToString()}' role");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var surveys = surveyRepo.GetWhere(s => surveyIds.Contains(s.Id) && s.SurveyStaffId != surveyStaffId).ToList();
            surveys.ForEach(s =>
            {
                s.AssignedBy = currentUser.Id;
                s.SurveyStaffId = surveyStaffId;
                s.SurveryCompany = surveyStaff.CompanyName;
                s.UpdatedBy = currentUser.Id;
                s.UpdatedDate = DateTimeOffset.Now;
            });

            await surveyRepo.UpdateRange(surveys, false);

            // log activity
            await logger.LogActivity(ActivityActionType.REASSIGN_SURVEY_TASKS, currentUser.Email,
                $"Assigned {surveyIds.Count()} survey tasks to {surveyStaff.FirstName} {surveyStaff.LastName}");
        }


        // unassign survey
        public async Task UnassignSurvey(long surveyId)
        {
            var survey = await surveyRepo.GetById(surveyId);
            if (survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            if (survey.SurveyRemark!=null)
            {
                throw new AppException($"Survey cannot be unassigned as it has already been completed");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            var _oldSurvey = survey.Clone<Survey>();

            
            await surveyRepo.Delete(survey.Id, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UNASSIGN_SURVEY_TASK, currentUser.Email, surveyRepo.TableName, _oldSurvey, new Survey(),
                $"Unassigned survey task of {_oldSurvey.Customer.CustomerName} from {_oldSurvey.SurveyStaff.FirstName} {_oldSurvey.SurveyStaff.LastName}");

        }

        public async Task UnassignSurveys(IEnumerable<long> surveyIds)
        {
            surveyIds = surveyIds.Distinct();
            var completedSurveyIds = surveyIds.Where(id => surveyRepo.Any(s => s.Id == id && s.SurveyRemark != null));

            if (completedSurveyIds.Count() > 0)
            {
                throw new AppException($"Survey(s) with id(s) '{string.Join(", ", completedSurveyIds)}' are already completed hence canot be unassigned");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            await surveyRepo.DeleteRange(surveyIds, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UNASSIGN_SURVEY_TASKS, currentUser.Email,
                $"Unassigned survey tasks");

        }

        // schedule survey && send notification
        public async Task ScheduleSurvey(long surveyId, DateTimeOffset scheduleDate)
        {
            var survey = await surveyRepo.GetById(surveyId);
            if (survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            if (survey.SurveyStaffId != currentUser.Id)
            {
                throw new AppException($"Survey cannot be scheduled as you are not assigned to it");
            }

            var isReschedule = survey.ScheduleDate != null;
            var oldSurvey = survey.Clone<Survey>();

            
            survey.ScheduleDate = scheduleDate;
            survey.UpdatedBy = currentUser.Id;
            survey.UpdatedDate = DateTimeOffset.Now;

            await surveyRepo.Update(survey, false);

            // log activity
            await logger.LogActivity(ActivityActionType.SCHEDULE_SURVEY, currentUser.Email, surveyRepo.TableName, oldSurvey, survey,
                $"Scheduled survey for {scheduleDate.ToString("yyyy-MM-dd")}");
        }
        public async Task ScheduleSurveys(IEnumerable<long> surveyIds, DateTimeOffset scheduleDate)
        {
            surveyIds = surveyIds.Distinct();
            var invalidSurveyIds = surveyIds.Where(id => !surveyRepo.Any(s => s.Id == id));
            if (invalidSurveyIds.Count() > 0)
            {
                throw new AppException($"Survey id(s) '{string.Join(", ", surveyIds)}' are invalid");
            }

            var currentUser = accessor.HttpContext.GetUserSession();

            var surveys = surveyRepo.GetWhere(s => surveyIds.Contains(s.Id)).ToList();
            if(surveys.Any(s=> s.SurveyStaffId != currentUser.Id))
            {
                throw new AppException($"One or more surveys cannot be scheduled as you are not assigned to them");
            }

            var surveysForReschedule = surveys.Where(s => s.ScheduleDate != null);
            var surveysForSchedule = surveys.Where(s => s.ScheduleDate == null);

            surveys.ForEach(s =>
            {
                s.ScheduleDate = scheduleDate;
                s.UpdatedBy = currentUser.Id;
                s.UpdatedDate = DateTimeOffset.Now;
            });

            await surveyRepo.UpdateRange(surveys, false);

            // log activity
            await logger.LogActivity(ActivityActionType.SCHEDULE_SURVEYS, currentUser.Email,
                $"Scheduled surveys with ids {string.Join(", ", surveys.Select(s => s.Id))} for {scheduleDate.ToString("yyyy-MM-dd")}");
        }
        
        public async Task UpdateSurvey(Survey survey)
        {
            var _survey = await surveyRepo.GetById(survey.Id);
            if (_survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            var oldSurvey = _survey.Clone<Survey>();
            var currentUser = accessor.HttpContext.GetUserSession();

            _survey.AccountSeparationRequired = survey.AccountSeparationRequired;
            _survey.AdditionalComment = survey.AdditionalComment;
            _survey.CustomerBillMatchUploadedData = survey.CustomerBillMatchUploadedData;
            _survey.EstimatedTotalLoadInAmps = survey.EstimatedTotalLoadInAmps;
            _survey.ExistingMeterNumber = survey.ExistingMeterNumber;
            _survey.ExistingMeterType = survey.ExistingMeterType;
            _survey.InstallationMode = survey.InstallationMode;
            _survey.LoadWireSeparationRequired = survey.LoadWireSeparationRequired;
            _survey.MAP = survey.MAP;
            _survey.NumberOf1QRequired = survey.NumberOf1QRequired;
            _survey.NumberOf3QRequired = survey.NumberOf3QRequired;
            _survey.OccupierPhoneNumber = survey.OccupierPhoneNumber;
            _survey.ReadyToPay = survey.ReadyToPay;
            _survey.RecommendedMeterType = survey.RecommendedMeterType;
            _survey.SurveyRemark = survey.SurveyRemark;
            _survey.TypeOfApartment = survey.TypeOfApartment;
            _survey.BedroomCount = survey.BedroomCount;
            _survey.UpdatedBy = currentUser.Id;
            _survey.UpdatedDate = DateTimeOffset.Now;

            await surveyRepo.Update(_survey, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UPDATE_SURVEY, currentUser.Email, surveyRepo.TableName, oldSurvey, _survey,
                $"Survey updated");
        }

        public async Task CompleteSurvey(Survey survey)
        {
            var _survey = await surveyRepo.GetById(survey.Id);
            if (_survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            var oldSurvey = _survey.Clone<Survey>();
            var currentUser = accessor.HttpContext.GetUserSession();

            if(currentUser.Id != _survey.SurveyStaffId && !currentUser.Roles.Contains(Constants.ROLE_ADMIN))
            {
                throw new AppException($"You cannot submit this survey as it was not assigned to you");
            }

            _survey.AccountSeparationRequired = survey.AccountSeparationRequired;
            _survey.AdditionalComment = survey.AdditionalComment;
            _survey.CustomerBillMatchUploadedData = survey.CustomerBillMatchUploadedData;
            _survey.EstimatedTotalLoadInAmps = survey.EstimatedTotalLoadInAmps;
            _survey.ExistingMeterNumber = survey.ExistingMeterNumber;
            _survey.ExistingMeterType = survey.ExistingMeterType;
            _survey.InstallationMode = survey.InstallationMode;
            _survey.LoadWireSeparationRequired = survey.LoadWireSeparationRequired;
            _survey.MAP = survey.MAP;
            _survey.NumberOf1QRequired = survey.NumberOf1QRequired;
            _survey.NumberOf3QRequired = survey.NumberOf3QRequired;
            _survey.OccupierPhoneNumber = survey.OccupierPhoneNumber;
            _survey.ReadyToPay = survey.ReadyToPay;
            _survey.RecommendedMeterType = survey.RecommendedMeterType;
            _survey.SurveyRemark = survey.SurveyRemark;
            _survey.TypeOfApartment = survey.TypeOfApartment;
            _survey.BedroomCount = survey.BedroomCount;
            _survey.SurveyDate = DateTimeOffset.Now;
            _survey.UpdatedBy = currentUser.Id;
            _survey.UpdatedDate = DateTimeOffset.Now;

            await surveyRepo.Update(_survey, false);

            // log activity
            await logger.LogActivity(ActivityActionType.COMPLETE_SURVEY, currentUser.Email, surveyRepo.TableName, oldSurvey, _survey,
                $"Survey updated");
        }

        // update surevey remark - by supervisor
        public async Task UpdateRemark(long surveyId, string remark)
        {
            var _survey = await surveyRepo.GetById(surveyId);
            if (_survey == null)
            {
                throw new AppException($"Survey id is invalid");
            }

            var oldSurvey = _survey.Clone<Survey>();
            var currentUser = accessor.HttpContext.GetUserSession();

            if (!(currentUser.Roles.Contains(Constants.ROLE_ADMIN) || currentUser.Roles.Contains(Constants.ROLE_SUPERVISOR)))
            {
                throw new AppException($"You cannot update this survey remark");
            }

            _survey.SurveyRemark = remark;
            _survey.UpdatedBy = currentUser.Id;
            _survey.UpdatedDate = DateTimeOffset.Now;

            await surveyRepo.Update(_survey, false);

            // log activity
            await logger.LogActivity(ActivityActionType.UPDATE_SURVEY, currentUser.Email, surveyRepo.TableName, oldSurvey, _survey,
                $"Survey updated");
        }

        // get assigned for surveystaff
        public IEnumerable<Survey> GetAssigned(long surveyStaffId)
        {
            return surveyRepo.GetWhere(s =>  s.SurveyRemark == null && s.SurveyStaffId == surveyStaffId);
        }

        public IEnumerable<Survey> GetCompleted(long surveyStaffId)
        {
            return surveyRepo.GetWhere(s => s.SurveyRemark != null && s.SurveyStaffId == surveyStaffId);
        }

        // get suurvey
        public async Task<Survey> GetSurvey(long surveyId)
        {
            return await surveyRepo.GetById(surveyId);
        }
    }
}
