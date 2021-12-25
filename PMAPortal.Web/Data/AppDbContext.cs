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

        //===== Audit ====
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditLogChange> AuditLogChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Applicant>()
            //    .HasIndex(x => x.Email).IsUnique();
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

            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.Cascade);


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
                    Name = "Disco Personnel",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 3,
                    Name = "Installer",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                }
           );

            modelBuilder.Entity<ApplicationStatus>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Pending",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 2,
                    Name = "Submitted",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                 new Role
                 {
                     Id = 3,
                     Name = "Scheduled for Installation",
                     CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                 },
                  new Role
                  {
                      Id = 4,
                      Name = "Installation In Progress",
                      CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                  },
                new Role
                {
                    Id = 5,
                    Name = "Installation Failed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 6,
                    Name = "Installation Completed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 7,
                    Name = "Disco Confirmation Failed",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                },
                new Role
                {
                    Id = 8,
                    Name = "Disco Confirmation Successful",
                    CreatedDate = new DateTimeOffset(2021, 10, 29, 18, 38, 0, TimeSpan.FromMinutes(60))
                }
           );


        }

    }
}
