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
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usertb> Usertbs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MinhHung\\MINHHUNG;uid=sa;pwd=12345;database=SAPelearning;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");

                entity.Property(e => e.CertificateId)
                    .HasMaxLength(50)
                    .HasColumnName("Certificate_id");

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
                        l => l.HasOne<CertificateModule>().WithMany().HasForeignKey("ModuleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SAP_Modul__Modul__5FB337D6"),
                        r => r.HasOne<Certificate>().WithMany().HasForeignKey("CertificateId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__SAP_Modul__Certi__5EBF139D"),
                        j =>
                        {
                            j.HasKey("CertificateId", "ModuleId").HasName("PK__SAP_Modu__2B3B48E8ADD2982A");

                            j.ToTable("SAP_Module");

                            j.IndexerProperty<string>("CertificateId").HasMaxLength(50).HasColumnName("Certificate_id");

                            j.IndexerProperty<string>("ModuleId").HasMaxLength(50).HasColumnName("Module_id");
                        });
            });

            modelBuilder.Entity<CertificateModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("PK__Certific__1DE5ECA0D8479E51");

                entity.ToTable("Certificate_Module");

                entity.Property(e => e.ModuleId)
                    .HasMaxLength(50)
                    .HasColumnName("Module_id");

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
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__Certific__B0B3E0BE40ECD261");

                entity.ToTable("Certificate_Question");

                entity.Property(e => e.QuestionId)
                    .HasMaxLength(50)
                    .HasColumnName("Question_id");

                entity.Property(e => e.CertificateId)
                    .HasMaxLength(50)
                    .HasColumnName("Certificate_id");

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.CertificateQuestions)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Certifica__Certi__52593CB8");
            });

            modelBuilder.Entity<CertificateSampletest>(entity =>
            {
                entity.HasKey(e => e.SampleTestId)
                    .HasName("PK__Certific__16F2820B05FB2783");

                entity.ToTable("Certificate_Sampletest");

                entity.Property(e => e.SampleTestId)
                    .HasMaxLength(50)
                    .HasColumnName("Sample_test_id");

                entity.Property(e => e.AttemptId)
                    .HasMaxLength(50)
                    .HasColumnName("Attempt_id");

                entity.Property(e => e.CertificateId)
                    .HasMaxLength(50)
                    .HasColumnName("Certificate_id");

                entity.Property(e => e.SampleTestName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Sample_test_name");

                entity.HasOne(d => d.Attempt)
                    .WithMany(p => p.CertificateSampletests)
                    .HasForeignKey(d => d.AttemptId)
                    .HasConstraintName("FK__Certifica__Attem__534D60F1");

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.CertificateSampletests)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Certifica__Certi__5441852A");
            });

            modelBuilder.Entity<CertificateTestAttempt>(entity =>
            {
                entity.HasKey(e => e.AttemptId)
                    .HasName("PK__Certific__BBD6418A8CBB1AE0");

                entity.ToTable("Certificate_Test_Attempt");

                entity.Property(e => e.AttemptId)
                    .HasMaxLength(50)
                    .HasColumnName("Attempt_id");

                entity.Property(e => e.AttemptDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Attempt_date");

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CertificateTestAttempts)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK__Certifica__Useri__5535A963");
            });

            modelBuilder.Entity<CertificateTestQuestion>(entity =>
            {
                entity.HasKey(e => e.TestQuestionId)
                    .HasName("PK__Certific__ED15837179A56AE3");

                entity.ToTable("Certificate_Test_Question");

                entity.Property(e => e.TestQuestionId)
                    .HasMaxLength(50)
                    .HasColumnName("Test_question_id");

                entity.Property(e => e.QuestionId)
                    .HasMaxLength(50)
                    .HasColumnName("Question_id");

                entity.Property(e => e.SampleTestId)
                    .HasMaxLength(50)
                    .HasColumnName("Sample_test_id");

                entity.HasOne(d => d.QuestionNavigation)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__Certifica__Quest__5629CD9C");

                entity.HasOne(d => d.SampleTest)
                    .WithMany(p => p.CertificateTestQuestions)
                    .HasForeignKey(d => d.SampleTestId)
                    .HasConstraintName("FK__Certifica__Sampl__571DF1D5");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(50)
                    .HasColumnName("Course_id");

                entity.Property(e => e.CertificateId)
                    .HasMaxLength(50)
                    .HasColumnName("Certificate_id");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Course_name");

                entity.Property(e => e.EndTime)
                    .HasColumnType("date")
                    .HasColumnName("End_time");

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

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

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CertificateId)
                    .HasConstraintName("FK__Course__Certific__5812160E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK__Course__Userid__59063A47");
            });

            modelBuilder.Entity<CourseMaterial>(entity =>
            {
                entity.HasKey(e => e.MaterialId)
                    .HasName("PK__Course_M__3A0EACE54108D992");

                entity.ToTable("Course_Material");

                entity.Property(e => e.MaterialId)
                    .HasMaxLength(50)
                    .HasColumnName("Material_id");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(50)
                    .HasColumnName("Course_id");

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
                    .HasConstraintName("FK__Course_Ma__Cours__59FA5E80");
            });

            modelBuilder.Entity<CourseSession>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK__Course_S__E9B4BF3A36D6630B");

                entity.ToTable("Course_Session");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(50)
                    .HasColumnName("Session_id");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(50)
                    .HasColumnName("Course_id");

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
                    .HasConstraintName("FK__Course_Se__Cours__5AEE82B9");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");

                entity.Property(e => e.EnrollmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Enrollment_id");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(50)
                    .HasColumnName("Course_id");

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnType("date")
                    .HasColumnName("Enrollment_date");

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .HasColumnName("Payment_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Userid).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Enrollmen__Cours__5BE2A6F2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK__Enrollmen__Useri__5CD6CB2B");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .HasColumnName("Payment_id");

                entity.Property(e => e.EnrollmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Enrollment_id");

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
                    .HasConstraintName("FK__Payment__Enrollm__5DCAEF64");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Roleid).HasMaxLength(50);

                entity.Property(e => e.Rolename)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usertb>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PK__Usertb__1797D024CDF9D61D");

                entity.ToTable("Usertb");

                entity.Property(e => e.Userid).HasMaxLength(50);

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

                entity.Property(e => e.Roleid).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("Roleid").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__Roleid__60A75C0F"),
                        r => r.HasOne<Usertb>().WithMany().HasForeignKey("Userid").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__Userid__619B8048"),
                        j =>
                        {
                            j.HasKey("Userid", "Roleid").HasName("PK__UserRole__2F388C87213B4E2B");

                            j.ToTable("UserRole");

                            j.IndexerProperty<string>("Userid").HasMaxLength(50);

                            j.IndexerProperty<string>("Roleid").HasMaxLength(50);
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
