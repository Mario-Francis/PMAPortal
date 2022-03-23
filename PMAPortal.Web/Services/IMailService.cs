using PMAPortal.Web.DTOs;
using PMAPortal.Web.Models;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IMailService
    {
        Task ScheduleNewAssignmentMailToSurveyStaff(MailObject mail);
        Task ScheduleSurveyScheduleMailToCustomer(MailObject mail, bool isReschedule = false);
        Task ScheduleSurveyCompletionMailToCustomer(MailObject mail);
        Task ScheduleNewAssignmentMailToInstaller(MailObject mail);
        Task ScheduleInstallationScheduleMailToCustomer(MailObject mail, bool isReschedule = false);
        Task ScheduleInstallationStatusUpdateMail(MailObject mail);
        Task ScheduleInstallationCompletedMailToCustomer(MailObject mail);
        Task ScheduleInstallationCompletedMailToDisco(MailObject mail);
        Task ScheduleDiscoRejectionMailToInstaller(MailObject mail);
        Task ScheduleDiscoRejectionMailToCustomer(MailObject mail);
        Task ScheduleDiscoApprovalMailToInstaller(MailObject mail);
        Task ScheduleInstallationApprovedMailToCustomer(MailObject mail);

        Task ScheduleWelcomeMail(MailObject mail);

        Task SchedulePasswordResetMail(MailObject mail);
       
        Task<bool> SendMail(Mail mail);
        Task ProcessUnsentMails();
        Task DeleteOldMails();
    }
}
