using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NUglify;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class MailService:IMailService
    {
        private readonly ILoggerService<MailService> logger;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IRepository<Mail> mailRepo;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IWebHostEnvironment hostEnvironment;

        public MailService(ILoggerService<MailService> logger,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IServiceScopeFactory serviceScopeFactory,
            IRepository<Mail> mailRepo,
            IHttpContextAccessor contextAccessor,
             IWebHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.appSettingsDelegate = appSettingsDelegate;
            this.serviceScopeFactory = serviceScopeFactory;
            this.mailRepo = mailRepo;
            this.contextAccessor = contextAccessor;
            this.hostEnvironment = hostEnvironment;
        }


        // send mail to  survey staffs on assignment
        public async Task ScheduleNewAssignmentMailToSurveyStaff(MailObject mail)
        {
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that a new metering eligibility survey task has been assigned to you by {mail.AssignedByName}. Kindly find customer details below
<br /><br />
Customer Name       : {mail.CustomerName}<br />
Customer Phone No.  : {mail.CustomerPhoneNo}<br />
Customer Email      : {mail.CustomerEmail}<br />
Account Number      : {mail.CustomerAccountNumber}
<br /><br />
Please attend to the above request as soon as possible.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"New Survey Task Assignment Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to customer on survey schedule/reschedule
        public async Task ScheduleSurveyScheduleMailToCustomer(MailObject mail, bool isReschedule=false)
        {
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your metering eligibility survey is {(isReschedule?"rescheduled":"scheduled")} for {mail.ScheduleDate}. Kindly find our survey staff contact below
<br /><br />
Name       : {mail.SurveyStaffName}<br />
Phone No.  : {mail.SurveyStaffPhoneNo}<br />
Email      : {mail.SurveyStaffEmail}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Metering Eligibility Survey Schedule Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send  mail to customer on survey completion
        public async Task ScheduleSurveyCompletionMailToCustomer(MailObject mail)
        {
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your metering eligibility survey has been completed. We will get back to you on the installation process if you are eligible.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Metering Eligibility Survey Completion Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to installers on assignment
        public async Task ScheduleNewAssignmentMailToInstaller(MailObject mail)
        {
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that a new meter installation task has been assigned to you by {mail.AssignedByName}. Kindly find customer details below
<br /><br />
Customer Name       : {mail.CustomerName}<br />
Customer Phone No.  : {mail.CustomerPhoneNo}<br />
Customer Email      : {mail.CustomerEmail}<br />
Account Number      : {mail.CustomerAccountNumber}
<br /><br />
Please attend to the above request as soon as possible.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"New Installation Task Assignment Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to customers on installation schedule
        public async Task ScheduleInstallationScheduleMailToCustomer(MailObject mail, bool isReschedule = false)
        {
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your meter installation is {(isReschedule ? "rescheduled" : "scheduled")} for {mail.ScheduleDate}. Kindly find our installer contact below
<br /><br />
Name       : {mail.InstallerName}<br />
Phone No.  : {mail.InstallerPhoneNo}<br />
Email      : {mail.InstallerEmail}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Meter Installation Schedule Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to customers on installation status update
        public async Task ScheduleInstallationStatusUpdateMail(MailObject mail)
        {
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is a status update on your meter installation. Kindly find status below
<br /><br />
Status: {mail.InstallationStatus}.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Meter Installation Update - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }

        // send mail to customer on installation completed with meter info
        public async Task ScheduleInstallationCompletedMailToCustomer(MailObject mail)
        {
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your meter installation is completed and pending approval from our disco team. We will notify you once it is approved. Kindly find details below
<br /><br />
Meter Type           : {mail.MeterType}<br />
Meter Number         : {mail.MeterNo}<br />
Installer Name       : {mail.InstallerName}<br />
Installer Phone No.  : {mail.InstallerPhoneNo}<br />
Installer Email      : {mail.InstallerEmail}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Meter Installation Completed - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to disco on installation completion for review
        public async Task ScheduleInstallationCompletedMailToDisco(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that the meter installation with account number {mail.CustomerAccountNumber} is completed and pending your approval. Kindly find details below
<br /><br />
Customer Name       : {mail.CustomerName}<br />
Customer Email      : {mail.CustomerEmail}<br />
Customer Phone No   : {mail.CustomerPhoneNo}<br />
Meter Type          : {mail.MeterType}<br />
Meter Number        : {mail.MeterNo}
<br /><br />
Please review and confirm as soon as possible.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"New Completed Installation Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to installer on disco rejection
        public async Task ScheduleDiscoRejectionMailToInstaller(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that the meter installation for customer with account number '{mail.CustomerAccountNumber}' which you completed has been rejected by the Disco team. Kindly find details below
<br /><br />
Customer Name          : {mail.CustomerName}<br />
Customer Email         : {mail.CustomerEmail}<br />
Customer Phone No.     : {mail.CustomerPhoneNo}<br />
Meter Type             : {mail.MeterType}<br />
Meter Number           : {mail.MeterNo}<br />
Disco Team's Comment   : {mail.Comment}
<br /><br />
Please attend to the above and update accordingly.
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Rejected Installation Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to customer on disco rejection
        public async Task ScheduleDiscoRejectionMailToCustomer(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your meter installation with account number {mail.CustomerAccountNumber} was rejected and our installation team will look into it. Kindly find comment from our disco team below
<br /><br />
Comment: {mail.Comment}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Prepaid Meter Installation Failed - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to installer on disco approval
        public async Task ScheduleDiscoApprovalMailToInstaller(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that the meter installation for customer with account number '{mail.CustomerAccountNumber}' which you completed has been approved by the Disco team. Kindly find details below
<br /><br />
Customer Name          : {mail.CustomerName}<br />
Customer Email         : {mail.CustomerEmail}<br />
Customer Phone No.     : {mail.CustomerPhoneNo}<br />
Meter Type             : {mail.MeterType}<br />
Meter Number           : {mail.MeterNo}<br />
Disco Team's Comment   : {mail.Comment}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Rejected Installation Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // send mail to customer with feedback link on disco approval
        public async Task ScheduleInstallationApprovedMailToCustomer(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
This is to notify you that your meter installation has been approved by our disco team. We will appreciate getting a feeback on your experience with us. 
<br /><br />
Kindly rate our services and suggest ways you think we can improve using the link below
<br /><br />
Feedback link: {baseUrl}Feedbacks/Feedback/{{token}}
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                _htmlBody = _htmlBody.Replace("{token}", m.Token);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Meter Installation Approved - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }

        
        // schedule mail for new user
        public async Task ScheduleWelcomeMail(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},
<br /><br />
You have just been profiled on the MOS Portal by an administrator. Kindly find login credentials below
<br /><br />
Email    : {{email}}<br />
Password : {{password}}
<br /><br />
Use the link below to login to your dashboard<br />
Link: {baseUrl}Dashboard
<br /><br />
Kindly note this is an automated mail, hence, do not reply.
<br /><br />
Best regards,<br />
MOS Team.
";
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;

                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                _htmlBody = _htmlBody.Replace("{email}", m.Email);
                _htmlBody = _htmlBody.Replace("{password}", m.Password);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Welcome - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }

        // schedule password reset mail
        public async Task SchedulePasswordResetMail(MailObject mail)
        {
            //var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "mail_template.html");
            var htmlBody = $@"
Dear {{name}},

This is to notify you that your accout password on our portal has bee reset by a administrator. Kindly finid new password below

Password : {{password}}

You are advised to change your password after login.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
MOS Team.
";
            //var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            //htmlBody = htmlBody.Replace("{base_url}", baseUrl);

            var _mails = new List<Mail>();
            foreach (var m in mail.Recipients)
            {
                var _htmlBody = htmlBody;
                _htmlBody = _htmlBody.Replace("{name}", m.FirstName);
                _htmlBody = _htmlBody.Replace("{password}", m.Password);
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Password Reset Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }




        // send mail
        public async Task<bool> SendMail(Mail mail)
        {
            try
            {
                // create email message
                var email = new MimeMessage();
                var _from = new MailboxAddress(appSettingsDelegate.Value.EmailSMTPConfig.FromName, appSettingsDelegate.Value.EmailSMTPConfig.Username);
                // from
                email.From.Add(_from);
                // to
                email.To.Add(MailboxAddress.Parse(mail.Email));
                //  subject
                email.Subject = mail.Subject;
                // body
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = mail.Body;
                bodyBuilder.TextBody = Uglify.HtmlToText(mail.Body).Code;

                email.Body = bodyBuilder.ToMessageBody();

                // send email
                using var smtp = new SmtpClient();

                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.CheckCertificateRevocation = false;

                await smtp.ConnectAsync(appSettingsDelegate.Value.EmailSMTPConfig.Host, appSettingsDelegate.Value.EmailSMTPConfig.Port, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(appSettingsDelegate.Value.EmailSMTPConfig.Username, appSettingsDelegate.Value.EmailSMTPConfig.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                //await loggerService.LogException(ex);
                logger.LogException(ex, $"An error was encountered while sending mail to {mail.Email}");
                return false;
            }
        }

        // for mail background service
        public async Task ProcessUnsentMails()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var mailRepo = scope.ServiceProvider.GetRequiredService<IRepository<Mail>>();
                var unsentMails = mailRepo.GetWhere(m => !m.IsSent);

                await unsentMails.ParallelForEachAsync(async (mail) =>
                {
                    using (var _scope = serviceScopeFactory.CreateScope())
                    {
                        var _mailRepo = _scope.ServiceProvider.GetRequiredService<IRepository<Mail>>();
                        var _mailService = _scope.ServiceProvider.GetRequiredService<IMailService>();
                        var isSent = await _mailService.SendMail(mail);
                        if (isSent)
                        {
                            mail.IsSent = isSent;
                            mail.SentDate = DateTimeOffset.Now;
                            mail.UpdatedDate = DateTimeOffset.Now;

                            await _mailRepo.Update(mail);
                        }
                    }
                }, Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2)));
            }
        }

        public async Task DeleteOldMails()
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var mailRepo = scope.ServiceProvider.GetRequiredService<IRepository<Mail>>();
                var mailRetentionPeriod = appSettingsDelegate.Value.MailRetentionPeriod;
                var now = DateTimeOffset.Now;
                await mailRepo.DeleteWhere(m => EF.Functions.DateDiffDay(now, m.CreatedDate) > mailRetentionPeriod);
            }
        }
 }
}
