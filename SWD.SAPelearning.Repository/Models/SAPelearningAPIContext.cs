using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class SAPelearningAPIContext : DbContext
    {
        public SAPelearningAPIContext()
        {
        }

        public SAPelearningAPIContext(DbContextOptions<SAPelearningAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
        public virtual DbSet<CertificateQuestion> CertificateQuestions { get; set; } = null!;
        public virtual DbSet<CertificateSampleTest> CertificateSampleTests { get; set; } = null!;
        public virtual DbSet<CertificateTestAttempt> CertificateTestAttempts { get; set; } = null!;
        public virtual DbSet<CertificateTestQuestion> CertificateTestQuestions { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseMaterial> CourseMaterials { get; set; } = null!;
        public virtual DbSet<CourseSession> CourseSessions { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<SapModule> SapModules { get; set; } = null!;
        public virtual DbSet<TopicArea> TopicAreas { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sapelearning.database.windows.net,1433;Initial Catalog=SAPelearningAPI;Persist Security Info=False;User ID=sapelearning;Password=@Admin123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");

                entity.Property(e => e.CertificateName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Environment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(d => d.Modules)
                    .WithMany(p => p.Certificates)
                    .UsingEntity<Dictionary<string, object>>(
                        "CertificateModule",
                        l => l.HasOne<SapModule>().WithMany().HasForeignKey("ModuleId").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_CertificateModule_Module"),
                        r => r.HasOne<Certificate>().WithMany().HasForeignKey("CertificateId").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_CertificateModule_Certificate"),
                        j =>
                        {
                            j.HasKey("CertificateId", "ModuleId");

                            j.ToTable("CertificateModule");
                        });
            });

            modelBuilder.Entity<CertificateQuestion>(entity =>
            {
                entity.ToTable("CertificateQuestion");

                entity.Property(e => e.Answer)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionText)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.CertificateQuestions)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_CertificateQuestion_Topic");
            });

            modelBuilder.Entity<CertificateSampleTest>(entity =>
            {
                entity.ToTable("CertificateSampleTest");

                entity.Property(e => e.SampleTestName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.CertificateSampleTests)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK_CertificateSampleTest_Certificate");
            });

            modelBuilder.Entity<CertificateTestAttempt>(entity =>
            {
                entity.ToTable("CertificateTestAttempt");

                entity.Property(e => e.AttemptDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.SampleTest)
                    .WithMany(p => p.CertificateTestAttempts)
                    .HasForeignKey(d => d.SampleTestId)
                    .HasConstraintName("FK_CertificateTestAttempt_SampleTest");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CertificateTestAttempts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CertificateTestAttempt_User");
            });

            modelBuilder.Entity<CertificateTestQuestion>(entity =>
            {
                entity.ToTable("CertificateTestQuestion");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_CertificateTestQuestion_Question");

                entity.HasOne(d => d.SampleTest)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.SampleTestId)
                    .HasConstraintName("FK_CertificateTestQuestion_SampleTest");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Mode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK_Course_Certificate");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_Course_Instructor");
            });

            modelBuilder.Entity<CourseMaterial>(entity =>
            {
                entity.ToTable("CourseMaterial");

                entity.Property(e => e.FileMaterial).IsUnicode(false);

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseMaterials)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseMaterial_Course");
            });

            modelBuilder.Entity<CourseSession>(entity =>
            {
                entity.ToTable("CourseSession");

                entity.Property(e => e.SessionDate).HasColumnType("datetime");

                entity.Property(e => e.SessionDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SessionName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseSessions)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_CourseSession_Course");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.CourseSessions)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_CourseSession_Instructor");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.CourseSessions)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_CourseSession_Topic");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Enrollment_Course");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Enrollment_User");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("Instructor");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Instructors)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Instructor_User");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.EnrollmentId)
                    .HasConstraintName("FK_Payment_Enrollment");
            });

            modelBuilder.Entity<SapModule>(entity =>
            {
                entity.ToTable("SapModule");

                entity.Property(e => e.ModuleDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TopicArea>(entity =>
            {
                entity.ToTable("TopicArea");

                entity.Property(e => e.TopicName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.TopicAreas)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK_CertificateTopicArea_Certificate");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Education)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("date");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
