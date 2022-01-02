using PMAPortal.Web.DTOs;
using PMAPortal.Web.Models;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IMailService
    {
        Task ScheduleApplicationReceivedMail(MailObject mail);
        Task ScheduleInstallationUpdateMail(MailObject mail);
        Task ScheduleInstallationCompletedMail(MailObject mail);
        Task ScheduleInstallationFailedMail(MailObject mail);
        Task ScheduleNewApplicationMailToSupervisors(MailObject mail);
        Task ScheduleNewAssignmentMailToInstaller(MailObject mail);
        Task ScheduleReminderMailToSupervisorsAndInstallers(MailObject mail);
        Task ScheduleDiscoDeclineMailToInstallers(MailObject mail);
        Task ScheduleDiscoApproveMailToInstallers(MailObject mail);
        Task ScheduleInstallationCompletedMailToDisco(MailObject mail);
        Task ScheduleWelcomeMail(MailObject mail);

        Task SchedulePasswordResetMail(MailObject mail);
       
        Task<bool> SendMail(Mail mail);
        Task ProcessUnsentMails();
        Task DeleteOldMails();
    }
}
