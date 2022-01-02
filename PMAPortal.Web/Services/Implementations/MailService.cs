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

        // schedule application received mail with track no
        public async Task ScheduleApplicationReceivedMail(MailObject mail)
        {
           // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var htmlBody = $@"
Dear {{name}},

This is to notify you that your application for a prepaid meter on our portal has been received and pending installation. Your application tracking number is {mail.TrackNo}. 

Our installation team will reach out to you in less than 7 business days.

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
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Application Received - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }

        // schedule installation status update mail to applicant
        public async Task ScheduleInstallationUpdateMail(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var htmlBody = $@"
Dear {{name}},

This is a status update on your prepaid meter installation with tracking number {mail.TrackNo}. Kindly find status below

Status: {mail.ApplicationStatus}.

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
                var _mail = new Mail
                {
                    Body = _htmlBody,
                    CreatedDate = DateTimeOffset.Now,
                    UpdatedDate = DateTimeOffset.Now,
                    Email = m.Email,
                    Subject = $"Prepaid Meter Installation Update - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }

        // schedule installation completed mail to applicant with feedback link
        public async Task ScheduleInstallationCompletedMail(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that your prepaid meter installation with tracking number {mail.TrackNo} has been successfully completed. We will appreciate getting a feeback on your experience with us. 

Kindly rate our services and suggest ways you think we can improve using the link below

Feedback link: {baseUrl}Applications/Feedback/{{token}}

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"Prepaid Meter Installation Completed - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // schedule installation failed with comment
        public async Task ScheduleInstallationFailedMail(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that your prepaid meter installation with tracking number {mail.TrackNo} failed. Kindly find comment from our installation team below

Comment: {mail.Comment}

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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

        // schedule mail to installers on new application
        public async Task ScheduleNewApplicationMailToSupervisors(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that there has been a new prepaid meter application pending installation. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Phone No.  : {mail.ApplicantPhoneNo}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}

Please attend to the above request as soon as possible.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"New Application Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        public async Task ScheduleNewAssignmentMailToInstaller(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that a new prepaid meter application has been assigned to you for installation by {mail.AssignedByName}. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Phone No.  : {mail.ApplicantPhoneNo}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}

Please attend to the above request as soon as possible.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"New Installation Assignment Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // schedule reminder to installer on pending application
        public async Task ScheduleReminderMailToSupervisorsAndInstallers(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is a gentle reminder on the prepaid meter application pending installation. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Phone No.  : {mail.ApplicantPhoneNo}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}

Please attend to the above request as soon as possible.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"Pending Installation Reminder - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // schedule mail to installer on decline from disco with comment
        public async Task ScheduleDiscoDeclineMailToInstallers(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that the prepaid meter installation which you completed has been declined confirmation by the Disco team. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}
Disco Team's Comment : {mail.Comment}

Please attend to the above and update accordingly.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"Declined Disco Confirmation Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // schedule mail to installer on approve from disco with comment
        public async Task ScheduleDiscoApproveMailToInstallers(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that the prepaid meter installation which you completed has been confirmed by the Disco team. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}
Disco Team's Comment : {mail.Comment ?? "---"}

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
                    Subject = $"Disco Confirmation Alert - MOS Team"
                };
                _mails.Add(_mail);
            }
            await mailRepo.InsertBulk(_mails);
        }
        // schedule mail to disco on installation completed by installers
        public async Task ScheduleInstallationCompletedMailToDisco(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

This is to notify you that the prepaid meter installation with tracking number {mail.TrackNo} is completed and pending your confirmation. Kindly find application details below

Applicant Name       : {mail.ApplicantName}
Applicant Email      : {mail.ApplicantEmail}
Requested Meter Type : {mail.MeterType}
Tracking Number      : {mail.TrackNo}

Please review and confirm as soon as possible.

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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

        // schedule mail for new user
        public async Task ScheduleWelcomeMail(MailObject mail)
        {
            // var templatePath = Path.Combine(hostEnvironment.WebRootPath, "templates", "email_verification_template.html");
            var baseUrl = contextAccessor.HttpContext.GetBaseUrl();
            var htmlBody = $@"
Dear {{name}},

You have just been profiled on the MOS Portal by an administrator. Kindly find login credentials below

Email    : {{email}}
Password : {{password}}

Use the link below to login to your dashboard
Link: {baseUrl}Dashboard

Kindly note this is an automated mail, hence, do not reply.

Best regards,
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
