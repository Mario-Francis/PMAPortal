using Microsoft.EntityFrameworkCore;
using PMAPortal.Web.Models;
using PMAPortal.Web.Models.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantAddress> ApplicantAddresses { get; set; }
        public DbSet<ApplicantFeedback> ApplicantFeedbacks { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationAppliance> ApplicationAppliances{ get; set; }
        public DbSet<ApplicationPet> ApplicationPets{ get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<ApplicationStatusLog> ApplicationStatusLogs { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<HouseType> HouseTypes { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentLog> PaymentLogs { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FeedbackQuestion> FeedbackQuestions { get; set; }
        public DbSet<FeedbackAnswer> FeedbackAnswers { get; set; }


        public DbSet<Batch> Batches { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Installation> Installations { get; set; }
        public DbSet<InstallationStatus> InstallationStatuses { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CustomerStatusLog> CustomerStatusLogs { get; set; }

        public DbSet<InstallationBatch> InstallationBatches { get; set; }
        public DbSet<InstallationBatchItem> InstallationBatchItems { get; set; }
        public DbSet<InstallationLog> InstallationLogs { get; set; }


        //===== Audit ====
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditLogChange> AuditLogChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<ApplicantAddress>()
                .HasOne(x=>x.Applicant)
                .WithMany(x => x.ApplicantAddresses)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicantFeedback>()
                .HasOne(x => x.Applicant)
                .WithMany(x => x.ApplicantFeedbacks)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicantFeedback>()
                .HasOne(x => x.Application)
                .WithMany(x=>x.ApplicantFeedbacks)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
                .HasOne(x => x.Applicant)
                .WithMany(x=>x.Applications)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
                .HasOne(x => x.Meter)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
                .HasOne(x => x.HouseType)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
                .HasOne(x => x.ApplicationStatus)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationAppliance>()
                .HasOne(x => x.Application)
                .WithMany(x=>x.ApplicationAppliances)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationPet>()
               .HasOne(x => x.Application)
               .WithMany(x => x.ApplicationPets)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationStatusLog>()
               .HasOne(x => x.Application)
               .WithMany(x => x.ApplicationStatusLogs)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationStatusLog>()
               .HasOne(x => x.ApplicationStatus)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
               .HasOne(x => x.Application)
               .WithMany(x => x.Payments)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PaymentLog>()
               .HasOne(x => x.Payment)
               .WithMany(x => x.PaymentLogs)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
               .HasOne(x => x.AssignedByUser)
               .WithMany()
               .HasForeignKey(x=>x.AssignedBy)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicantFeedback>()
               .HasMany(x => x.FeedbackAnswers)
               .WithOne(x=>x.ApplicantFeedback)
               .HasForeignKey(x => x.ApplicantFeedbackId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeedbackAnswer>()
               .HasOne(x => x.FeedbackQuestion)
               .WithMany()
               .HasForeignKey(x => x.FeedbackQuestionId)
               .OnDelete(DeleteBehavior.NoAction);


            //=====================================
            
            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.AccountNumber).IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.PhoneNumber).IsUnique();

            modelBuilder.Entity<Customer>()
               .HasOne(x => x.Batch)
               .WithMany(x => x.Customers)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Survey>()
               .HasOne(x => x.Customer)
               .WithMany(x => x.Surveys)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Survey>()
               .HasOne(x => x.SurveyStaff)
               .WithMany()
               .HasForeignKey(x=>x.SurveyStaffId)
               .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Survey>()
               .HasOne(x => x.AssignedByUser)
               .WithMany()
               .HasForeignKey(x => x.AssignedBy)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Installation>()
              .HasOne(x => x.Customer)
              .WithMany(x=> x.Installations)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Installation>()
               .HasOne(x => x.Installer)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Installation>()
               .HasOne(x => x.InstallationStatus)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Installation>()
               .HasOne(x => x.AssignedByUser)
               .WithMany()
               .HasForeignKey(x => x.AssignedBy)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRole>()
               .HasOne(x => x.User)
               .WithMany(x=>x.UserRoles)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
              .HasOne(x => x.Role)
              .WithMany(x => x.UserRoles)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerStatusLog>()
              .HasOne(x => x.Customer)
              .WithMany(x => x.CustomerStatusLogs)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CustomerStatusLog>()
              .HasOne(x => x.InstallationStatus)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CustomerStatusLog>()
              .HasOne(x => x.ActionByUser)
              .WithMany()
              .HasForeignKey(x=>x.ActionBy)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
               .HasOne(x => x.UpdatedByUser)
               .WithMany()
               .HasForeignKey(x => x.UpdatedBy)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InstallationBatchItem>()
              .HasOne(x => x.Customer)
              .WithMany()
              .HasForeignKey(x=>x.AccountNumber)
              .HasPrincipalKey(x=>x.AccountNumber)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InstallationLog>()
               .HasOne(x => x.ActionByUser)
               .WithMany()
               .HasForeignKey(x => x.ActionBy)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InstallationLog>()
               .HasOne(x => x.InstallationStatus)
               .WithMany()
               .HasForeignKey(x => x.InstallationStatusId)
               .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<InstallationLog>()
            //   .HasOne(x => x.Installation)
            //   .WithMany(x=>x.InstallationLogs)
            //   .HasForeignKey(x => x.InstallationId)
            //   .OnDelete(DeleteBehavior.NoAction);

            SeeData(modelBuilder);
        }

        private void SeeData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Administrator",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 2,
                    Name = "Supervisor",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 3,
                    Name = "Disco Personnel",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 4,
                    Name = "Installer",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                 new Role
                 {
                     Id = 5,
                     Name = "Survey Staff",
                     CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                 }
           );

            modelBuilder.Entity<InstallationStatus>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Pending",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                 new Role
                 {
                     Id = 2,
                     Name = "Scheduled for Installation",
                     CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                 },
                  new Role
                  {
                      Id = 3,
                      Name = "Installation In Progress",
                      CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                  },
                new Role
                {
                    Id = 4,
                    Name = "Installation Failed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 5,
                    Name = "Installation Completed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 6,
                    Name = "Disco Confirmation Failed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 7,
                    Name = "Disco Confirmation Successful",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                }
           );


        }

    }
}
