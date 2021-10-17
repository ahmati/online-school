using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace OnlineSchool.Domain.OnlineSchoolDB.EF.Context
{
    public partial class OnlineSchoolDbContext 
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BookedCourse> BookedCourse { get; set; }
        public virtual DbSet<BookedSpotMeeting> BookedSpotMeeting { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<LessonCalendar> LessonCalendar { get; set; }
        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<LessonMaterial> LessonMaterial { get; set; }
        public virtual DbSet<LocationType> LocationType { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MultimediaType> MultimediaType { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Session> Session { get; set; } 
        public virtual DbSet<SocialNetwork> SocialNetwork { get; set; }
        public virtual DbSet<SpotMeeting> SpotMeeting { get; set; }
        public virtual DbSet<SpotMeetingMaterial> SpotMeetingMaterial { get; set; }
        public virtual DbSet<SpotMeetingTeacher> SpotMeetingTeacher { get; set; }
        public virtual DbSet<StudentEnrollment> StudentEnrollment { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentDocument> StudentDocument { get; set; }
        public virtual DbSet<SubjectCategory> SubjectCategory { get; set; }
        public virtual DbSet<SubjectLocation> SubjectLocation { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<TeacherDocument> TeacherDocument { get; set; }
        public virtual DbSet<TeacherSocialNetwork> TeacherSocialNetwork { get; set; }
        public virtual DbSet<TeacherSubject> TeacherSubject { get; set; }
        public virtual DbSet<TeacherSubjectMaterial> TeacherSubjectMaterial { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<WebexGuestIssuer> WebexGuestIssuer { get; set; }
        public virtual DbSet<WebexIntegration> WebexIntegration { get; set; }


        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Data Source=DESKTOP-HHDM1CG\\MSSQLSERVER01;Initial Catalog=OnlineSchoolDB;Integrated Security=True");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => e.CommentId);
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Body).HasMaxLength(50);
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).HasName("name_Index").HasFilter("([NormalizedUserName] IS NOT NULL)");
                entity.HasIndex(e => e.Email).HasName("email_Index").IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.UserName).HasMaxLength(256);
                // Custom props
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Surname).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Gender).HasMaxLength(1).HasColumnType("char").IsRequired();
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.ImagePath).HasMaxLength(100);
            });

            modelBuilder.Entity<BookedCourse>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.StudentId).HasName("studentId_Index");
                entity.HasIndex(e => e.CourseId).HasName("courseId_Index");

                entity.Property(e => e.StudentId).HasColumnType("int").IsRequired();
                entity.Property(e => e.CourseId).HasColumnType("int").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();


                entity.HasOne(s => s.Student)
                .WithMany(b => b.BookedCourses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_BookedCourses_Students");

                entity.HasOne(s => s.Course)
                .WithMany(b => b.BookedCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_BookedCourses_Courses");
            });

            modelBuilder.Entity<BookedSpotMeeting>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UserId).HasName("userId_Index");
                entity.HasIndex(e => e.SpotMeetingId).HasName("SpotMeetingId_Index");

                entity.Property(e => e.UserId).HasColumnType("nvarchar(450)").IsRequired();
                entity.Property(e => e.SpotMeetingId).HasColumnType("int").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(s => s.User)
                .WithMany(b => b.BookedSpotMeetings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_BookedSpotMeeting_Users");

                entity.HasOne(s => s.SpotMeeting)
                .WithMany(b => b.BookedSpotMeetings)
                .HasForeignKey(d => d.SpotMeetingId)
                .HasConstraintName("FK_BookedSpotMeeting_SpotMeetings");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.TeacherSubjectId).HasName("teacherSubjectId_Index");

                entity.Property(e => e.StartDate).HasColumnType("date").IsRequired();
                entity.Property(e => e.EndDate).HasColumnType("date").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.IsPublished).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(500)");
                entity.Property(e => e.AvailableSpots).HasColumnType("int").IsRequired();

                entity.HasOne(d => d.TeacherSubject)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherSubjectId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Course_TeacherSubject");
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.StripeKeyName).HasMaxLength(1000);

                entity.Property(e => e.Value).HasMaxLength(1000);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).HasName("name_Index");

                entity.Property(e => e.Name).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(300)");
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.TeacherSubject)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TeacherSubjectId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Lesson_TeacherSubjects");
            });

            modelBuilder.Entity<LessonMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.LessonId, e.MaterialId }).HasName("lessonId_materialId_Index").IsUnique();

                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.LessonMaterials)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonMaterial_Lesson");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.LessonMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonMaterial_Material");
            });

            modelBuilder.Entity<LocationType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.FileName).HasName("fileName_Index");

                entity.Property(e => e.FileName).HasColumnType("nvarchar(1000)").IsRequired();
                entity.Property(e => e.MimeType).HasColumnType("nvarchar(50)");
                entity.Property(e => e.FileSize).HasColumnType("int").HasDefaultValue(0).IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
            });

            modelBuilder.Entity<MultimediaType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EndTimezone).HasMaxLength(50);

                entity.Property(e => e.RecurrenceException).HasMaxLength(50);

                entity.Property(e => e.RecurrenceId).HasColumnName("RecurrenceID");

                entity.Property(e => e.RecurrenceRule).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StartTimezone).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_Schedules_Colors");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_Schedules_Subjects");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_Schedules_Teachers");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.CourseId).HasName("courseId_Index");

                entity.Property(e => e.Topic).HasColumnType("nvarchar(1000)").IsRequired();
                entity.Property(e => e.Date).HasColumnType("date").IsRequired();
                entity.Property(e => e.StartTime).HasColumnType("time").IsRequired();
                entity.Property(e => e.EndTime).HasColumnType("time").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
                entity.Property(e => e.MeetingId).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingTitle).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingSipAddress).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingPassword).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.ReminderEmail).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_Session_Course");
            });

            modelBuilder.Entity<SocialNetwork>(entity =>
            {
                entity.Property(e => e.AuthDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IconPath)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SpotMeeting>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Title).HasName("title_Index");

                entity.Property(e => e.StartDate).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.Duration).HasColumnType("int").IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.IsPublished).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(500)");
                entity.Property(e => e.Title).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(300)");
                entity.Property(e => e.IsRecursiveSpotMeeting).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();
                entity.Property(e => e.MeetingId).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingTitle).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingSipAddress).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.MeetingPassword).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.ReminderEmail).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();
                entity.Property(e => e.AvailableSpots).HasColumnType("int").IsRequired();
            });

            modelBuilder.Entity<SpotMeetingMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.SpotMeetingId, e.MaterialId }).HasName("lessonId_materialId_Index").IsUnique();

                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.SpotMeeting)
                    .WithMany(p => p.SpotMeetingMaterials)
                    .HasForeignKey(d => d.SpotMeetingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpotMeetingMaterial_SpotMeeting");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.SpotMeetingMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpotMeetingMaterial_Material");
            });

            modelBuilder.Entity<SpotMeetingTeacher>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.SpotMeetingId, e.TeacherId }).HasName("lessonId_teacherId_Index").IsUnique();

                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
                entity.Property(e => e.IsHost).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();

                entity.HasOne(d => d.SpotMeeting)
                    .WithMany(p => p.SpotMeetingTeachers)
                    .HasForeignKey(d => d.SpotMeetingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpotMeetingTeacher_SpotMeeting");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.SpotMeetingTeachers)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpotMeetingTeacher_Teacher");
            });

            modelBuilder.Entity<StudentEnrollment>(entity =>
            {
                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentEnrollments)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_StudentEnrollments_Students");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).HasName("name_Index");
                entity.HasIndex(e => e.Email).HasName("email_Index").IsUnique();

                entity.Property(e => e.Name).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Surname).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Gender).HasColumnType("char").HasMaxLength(1).IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(500)");
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(500)");
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
            });

            modelBuilder.Entity<StudentDocument>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.FileName).HasName("fileName_Index");

                entity.Property(e => e.FileName).HasColumnType("nvarchar(1000)").IsRequired();
                entity.Property(e => e.MimeType).HasColumnType("nvarchar(50)");
                entity.Property(e => e.FileSize).HasColumnType("int").HasDefaultValue(0).IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.StudentDocuments)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .HasConstraintName("FK_StudentDocument_DocumentType");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentDocuments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StudentDocument_Student");
            });

            modelBuilder.Entity<SubjectCategory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnType("nvarchar(50)").IsRequired();
            });

            modelBuilder.Entity<SubjectLocation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Location)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.SubjectLocationType)
                    .WithMany(p => p.SubjectLocations)
                    .HasForeignKey(d => d.SubjectLocationTypeId)
                    .HasConstraintName("FK_SubjectLocation_LocationType");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).HasName("name_Index").IsUnique();

                entity.Property(e => e.Name).HasColumnType("nvarchar(50)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(300)");
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.Color).HasColumnType("nvarchar(50)").HasDefaultValue("#FFFFFF");
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subjects_SubjectCategory");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).HasName("name_Index");
                entity.HasIndex(e => e.Email).HasName("email_Index").IsUnique();

                entity.Property(e => e.Name).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Surname).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Email).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Gender).HasColumnType("char").HasMaxLength(1).IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(500)");
                entity.Property(e => e.ImagePath).HasColumnType("nvarchar(500)");
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
            });

            modelBuilder.Entity<TeacherDocument>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.FileName).HasName("fileName_Index");

                entity.Property(e => e.FileName).HasColumnType("nvarchar(1000)").IsRequired();
                entity.Property(e => e.MimeType).HasColumnType("nvarchar(50)");
                entity.Property(e => e.FileSize).HasColumnType("int").HasDefaultValue(0).IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.TeacherDocuments)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .HasConstraintName("FK_TeacherDocument_DocumentType");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherDocuments)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TeacherDocument_Teacher");
            });

            modelBuilder.Entity<TeacherSocialNetwork>(entity =>
            {
                entity.HasKey(e => new { e.TeacherId, e.SocialNetworkId });

                entity.Property(e => e.Link).HasColumnType("nvarchar(500)").IsRequired();
                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.SocialNetwork)
                    .WithMany(p => p.TeacherSocialNetworks)
                    .HasForeignKey(d => d.SocialNetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSocialNetwork_SocialNetworks");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherSocialNetworks)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSocialNetwork_Teachers");
            });

            modelBuilder.Entity<TeacherSubject>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.TeacherId, e.SubjectId }).HasName("teacherId_subjectId_Index").IsUnique();

                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
                entity.Property(e => e.Deleted).HasColumnType("bit").HasDefaultValue(0).IsRequired();

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TeacherSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSubject_Subjects");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherSubjects)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSubject_Teachers");
            });

            modelBuilder.Entity<TeacherSubjectMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => new { e.TeacherSubjectId, e.MaterialId }).HasName("teacherSubjectId_materialId_Index").IsUnique();

                entity.Property(e => e.AuthDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();

                entity.HasOne(d => d.TeacherSubject)
                    .WithMany(p => p.TeacherSubjectMaterials)
                    .HasForeignKey(d => d.TeacherSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSubjectMaterial_TeacherSubject");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.TeacherSubjectMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSubjectMaterial_Material");
            });

            modelBuilder.Entity<WebexGuestIssuer>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.Secret).HasColumnType("nvarchar(300)").IsRequired();
            });

            modelBuilder.Entity<WebexIntegration>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ClientId).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.ClientSecret).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.RedirectUri).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.AccessToken).HasColumnType("nvarchar(300)").IsRequired();
                entity.Property(e => e.ExpiresIn).HasColumnType("bigint").IsRequired();
                entity.Property(e => e.LastUpdated).HasColumnType("datetime").HasDefaultValueSql("(getdate())").IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
