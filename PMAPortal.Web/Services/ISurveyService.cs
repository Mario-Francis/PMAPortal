using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface ISurveyService
    {
        IEnumerable<Customer> GetUnassigned();
        IEnumerable<Customer> GetAssigned();
        IEnumerable<Survey> GetCompleted();
        Task AssignSurvey(long surveyStaffId, long customerId);
        Task ReassignSurvey(long surveyStaffId, long surveyId);
        Task AssignSurveys(long surveyStaffId, IEnumerable<long> customerIds);
        Task ReassignSurveys(long surveyStaffId, IEnumerable<long> surveyIds);
        Task UnassignSurvey(long surveyId);
        Task UnassignSurveys(IEnumerable<long> surveyIds);
        Task ScheduleSurvey(long surveyId, DateTimeOffset scheduleDate);
        Task ScheduleSurveys(IEnumerable<long> surveyIds, DateTimeOffset scheduleDate);
        Task UpdateSurvey(Survey survey);
        Task CompleteSurvey(Survey survey);
        Task UpdateRemark(long surveyId, string remark);
        IEnumerable<Survey> GetAssigned(long surveyStaffId);
        IEnumerable<Survey> GetCompleted(long surveyStaffId);
        Task<Survey> GetSurvey(long surveyId);
    }
}
