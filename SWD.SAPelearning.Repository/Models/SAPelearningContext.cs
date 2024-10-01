using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class SAPelearningContext : DbContext
    {
        public SAPelearningContext()
        {
        }

        public SAPelearningContext(DbContextOptions<SAPelearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
        public virtual DbSet<CertificateModule> CertificateModules { get; set; } = null!;
        public virtual DbSet<CertificateQuestion> CertificateQuestions { get; set; } = null!;
        public virtual DbSet<CertificateSampletest> CertificateSampletests { get; set; } = null!;
        public virtual DbSet<CertificateTestAttempt> CertificateTestAttempts { get; set; } = null!;
        public virtual DbSet<CertificateTestQuestion> CertificateTestQuestions { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseMaterial> CourseMaterials { get; set; } = null!;
        public virtual DbSet<CourseSession> CourseSessions { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Usertb> Usertbs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI;Database=SAPelearning;User Id=sa;Password=12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CertificateName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Certificate_name");

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
                        "SapModule",
                        l => l.HasOne<CertificateModule>().WithMany().HasForeignKey("ModuleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SAP_Modul__Modul__5BE2A6F2"),
                        r => r.HasOne<Certificate>().WithMany().HasForeignKey("CertificateId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SAP_Modul__Certi__5AEE82B9"),
                        j =>
                        {
                            j.HasKey("CertificateId", "ModuleId").HasName("PK__SAP_Modu__D94FE0BB3E49DBA3");

                            j.ToTable("SAP_Module");

                            j.IndexerProperty<string>("CertificateId").HasMaxLength(50);

                            j.IndexerProperty<string>("ModuleId").HasMaxLength(50);
                        });
            });

            modelBuilder.Entity<CertificateModule>(entity =>
            {
                entity.ToTable("Certificate_Module");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ModuleDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Module_description");

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Module_name");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CertificateQuestion>(entity =>
            {
                entity.ToTable("Certificate_Question");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CertificateId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.CertificateQuestions)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Certifica__Certi__4E88ABD4");
            });

            modelBuilder.Entity<CertificateSampletest>(entity =>
            {
                entity.ToTable("Certificate_Sampletest");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AttemptId).HasMaxLength(50);

                entity.Property(e => e.CertificateId).HasMaxLength(50);

                entity.Property(e => e.SampleTestName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Sample_test_name");

                entity.HasOne(d => d.Attempt)
                    .WithMany(p => p.CertificateSampletests)
                    .HasForeignKey(d => d.AttemptId)
                    .HasConstraintName("FK__Certifica__Attem__4F7CD00D");

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.CertificateSampletests)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Certifica__Certi__5070F446");
            });

            modelBuilder.Entity<CertificateTestAttempt>(entity =>
            {
                entity.ToTable("Certificate_Test_Attempt");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AttemptDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Attempt_date");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CertificateTestAttempts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Certifica__UserI__5165187F");
            });

            modelBuilder.Entity<CertificateTestQuestion>(entity =>
            {
                entity.ToTable("Certificate_Test_Question");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Questionid).HasMaxLength(50);

                entity.Property(e => e.SampleTestId).HasMaxLength(50);

                entity.HasOne(d => d.QuestionNavigation)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.Questionid)
                    .HasConstraintName("FK__Certifica__Quest__52593CB8");

                entity.HasOne(d => d.SampleTest)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.SampleTestId)
                    .HasConstraintName("FK__Certifica__Sampl__534D60F1");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CertificateId).HasMaxLength(50);

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Course_name");

                entity.Property(e => e.EndTime)
                    .HasColumnType("date")
                    .HasColumnName("End_time");

                entity.Property(e => e.Endregisterdate).HasColumnType("date");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Mode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime)
                    .HasColumnType("date")
                    .HasColumnName("Start_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalStudent).HasColumnName("Total_student");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Course__Certific__5441852A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Course__UserId__5535A963");
            });

            modelBuilder.Entity<CourseMaterial>(entity =>
            {
                entity.ToTable("Course_Material");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CourseId).HasMaxLength(50);

                entity.Property(e => e.FileMaterial)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("File_material");

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Material_name");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseMaterials)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Course_Ma__Cours__5629CD9C");
            });

            modelBuilder.Entity<CourseSession>(entity =>
            {
                entity.ToTable("Course_Session");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CourseId).HasMaxLength(50);

                entity.Property(e => e.SessionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Session_date");

                entity.Property(e => e.SessionDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Session_description");

                entity.Property(e => e.SessionName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Session_name");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseSessions)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Course_Se__Cours__571DF1D5");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CourseId).HasMaxLength(50);

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnType("date")
                    .HasColumnName("Enrollment_date");

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Enrollmen__Cours__5812160E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Enrollmen__UserI__59063A47");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.EnrollmentId).HasMaxLength(50);

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Payment_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .HasColumnName("Transaction_id");

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.EnrollmentId)
                    .HasConstraintName("FK__Payment__Enrollm__59FA5E80");
            });

            modelBuilder.Entity<Usertb>(entity =>
            {
                entity.ToTable("Usertb");

                entity.Property(e => e.Id).HasMaxLength(50);

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
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsOnline).HasColumnName("Is_Online");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_Login");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("Registration_Date");

                entity.Property(e => e.Rolename).HasMaxLength(20);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
