using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class Current : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Surname = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "char", maxLength: 1, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    ImagePath = table.Column<string>(maxLength: 100, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Body = table.Column<string>(maxLength: 50, nullable: true),
                    TeacherId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    StripeKeyName = table.Column<string>(maxLength: 1000, nullable: true),
                    Value = table.Column<string>(maxLength: 1000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationType",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FileSize = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MultimediaType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultimediaType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialNetwork",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    IconPath = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetwork", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Gender = table.Column<string>(type: "char", maxLength: 1, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectCategory",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Gender = table.Column<string>(type: "char", maxLength: 1, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectLocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    SubjectTimeId = table.Column<int>(nullable: false),
                    SubjectLocationTypeId = table.Column<byte>(nullable: true),
                    Location = table.Column<string>(unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectLocation_LocationType",
                        column: x => x.SubjectLocationTypeId,
                        principalTable: "LocationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FileSize = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDocument_DocumentType",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentDocument_Student",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherSubjectId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentEnrollments_Students",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CategoryId = table.Column<byte>(nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", nullable: true, defaultValue: "#FFFFFF"),
                    Price = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_SubjectCategory",
                        column: x => x.CategoryId,
                        principalTable: "SubjectCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(nullable: true),
                    TeacherId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    FileSize = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherDocument_DocumentType",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherDocument_Teacher",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSocialNetwork",
                columns: table => new
                {
                    TeacherId = table.Column<int>(nullable: false),
                    SocialNetworkId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSocialNetwork", x => new { x.TeacherId, x.SocialNetworkId });
                    table.ForeignKey(
                        name: "FK_TeacherSocialNetwork_SocialNetworks",
                        column: x => x.SocialNetworkId,
                        principalTable: "SocialNetwork",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSocialNetwork_Teachers",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookedCourse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    AuthDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedCourses_Students",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedCourses_Subjects",
                        column: x => x.StudentId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 450, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TeacherId = table.Column<int>(nullable: true),
                    SubjectId = table.Column<int>(nullable: true),
                    ColorId = table.Column<int>(nullable: true),
                    EndTimezone = table.Column<string>(maxLength: 50, nullable: true),
                    RecurrenceRule = table.Column<string>(maxLength: 50, nullable: true),
                    RecurrenceID = table.Column<int>(nullable: true),
                    RecurrenceException = table.Column<string>(maxLength: 50, nullable: true),
                    IsAllDay = table.Column<bool>(nullable: true),
                    StartTimezone = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Colors",
                        column: x => x.ColorId,
                        principalTable: "Color",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Subjects",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Teachers",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSubject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSubject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSubject_Subjects",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSubject_Teachers",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherSubjectId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_TeacherSubject",
                        column: x => x.TeacherSubjectId,
                        principalTable: "TeacherSubject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TeacherSubjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesson_TeacherSubjects",
                        column: x => x.TeacherSubjectId,
                        principalTable: "TeacherSubject",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherSubjectMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherSubjectId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSubjectMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSubjectMaterial_Material",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSubjectMaterial_TeacherSubject",
                        column: x => x.TeacherSubjectId,
                        principalTable: "TeacherSubject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Course",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonCalendar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterId = table.Column<int>(nullable: false),
                    AspNetUserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    AuthDate = table.Column<DateTime>(nullable: false),
                    LessonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonCalendar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonCalendar_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonMaterial_Lesson",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonMaterial_Material",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "email_Index",
                table: "AspNetUsers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "name_Index",
                table: "AspNetUsers",
                column: "Name",
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_BookedCourse_StudentId",
                table: "BookedCourse",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentId",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "teacherSubjectId_Index",
                table: "Course",
                column: "TeacherSubjectId");

            migrationBuilder.CreateIndex(
                name: "name_Index",
                table: "Lesson",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_TeacherSubjectId",
                table: "Lesson",
                column: "TeacherSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonCalendar_LessonId",
                table: "LessonCalendar",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_MaterialId",
                table: "LessonMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "lessonId_materialId_Index",
                table: "LessonMaterial",
                columns: new[] { "LessonId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fileName_Index",
                table: "Material",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ColorId",
                table: "Schedule",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SubjectId",
                table: "Schedule",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeacherId",
                table: "Schedule",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "courseId_Index",
                table: "Session",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "email_Index",
                table: "Student",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name_Index",
                table: "Student",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocument_DocumentTypeId",
                table: "StudentDocument",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "fileName_Index",
                table: "StudentDocument",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDocument_StudentId",
                table: "StudentDocument",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_StudentId",
                table: "StudentEnrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_CategoryId",
                table: "Subject",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "name_Index",
                table: "Subject",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLocation_SubjectLocationTypeId",
                table: "SubjectLocation",
                column: "SubjectLocationTypeId");

            migrationBuilder.CreateIndex(
                name: "email_Index",
                table: "Teacher",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name_Index",
                table: "Teacher",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDocument_DocumentTypeId",
                table: "TeacherDocument",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "fileName_Index",
                table: "TeacherDocument",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherDocument_TeacherId",
                table: "TeacherDocument",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSocialNetwork_SocialNetworkId",
                table: "TeacherSocialNetwork",
                column: "SocialNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubject_SubjectId",
                table: "TeacherSubject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "teacherId_subjectId_Index",
                table: "TeacherSubject",
                columns: new[] { "TeacherId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjectMaterial_MaterialId",
                table: "TeacherSubjectMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "teacherSubjectId_materialId_Index",
                table: "TeacherSubjectMaterial",
                columns: new[] { "TeacherSubjectId", "MaterialId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookedCourse");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "LessonCalendar");

            migrationBuilder.DropTable(
                name: "LessonMaterial");

            migrationBuilder.DropTable(
                name: "MultimediaType");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "StudentDocument");

            migrationBuilder.DropTable(
                name: "StudentEnrollment");

            migrationBuilder.DropTable(
                name: "SubjectLocation");

            migrationBuilder.DropTable(
                name: "TeacherDocument");

            migrationBuilder.DropTable(
                name: "TeacherSocialNetwork");

            migrationBuilder.DropTable(
                name: "TeacherSubjectMaterial");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "LocationType");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "SocialNetwork");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "TeacherSubject");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "SubjectCategory");
        }
    }
}
