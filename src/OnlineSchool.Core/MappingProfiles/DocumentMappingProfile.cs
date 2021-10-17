using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Documents;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.Roles;
using OnlineSchool.Contract.Scheduler;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Contract.Students;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.SubjectCategory;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Contract.Users;
using OnlineSchool.Contract.Webex;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;

namespace OnlineSchool.Core.MappingProfiles
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<ApplicationUser, RegisterUtente>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, AspNetUsers>().ReverseMap();

            CreateMap<Ruoli, AspNetRoles>().ReverseMap();

            CreateMap<Ruoli, IdentityRole>().ReverseMap();

            CreateMap<RuoliUtente, AspNetUserRoles>().ReverseMap();

            CreateMap<UserModel, AspNetUsers>().ReverseMap();

            CreateMap<MaterialModel, Material>().ReverseMap();

            CreateMap<LessonModel, Lesson>().ReverseMap();

            #region Document-related

            CreateMap<DocumentTypeModel, DocumentType>().ReverseMap();

            CreateMap<StudentDocument, StudentDocumentModel>().ReverseMap();

            CreateMap<TeacherDocument, TeacherDocumentModel>().ReverseMap();

            CreateMap<CreateStudentDocumentModel, StudentDocument>().ReverseMap();

            CreateMap<CreateTeacherDocumentModel, TeacherDocument>().ReverseMap();

            #endregion

            CreateMap<SchedulerModel, Schedule>().ReverseMap();

            #region Student

            CreateMap<StudentModel, Student>().ReverseMap();

            CreateMap<CreateStudentModel, Student>().ReverseMap();
            CreateMap<RegisterUtente, CreateStudentModel>();
            CreateMap<CreateStudentModel, ApplicationUser>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<Student, UpdateStudentModel>().ReverseMap();
            CreateMap<StudentModel, UpdateStudentModel>().ReverseMap();

            #endregion

            #region Subject

            CreateMap<SubjectModel, Subject>().ReverseMap();

            CreateMap<CreateSubjectModel, Subject>().ReverseMap();

            CreateMap<UpdateSubjectModel, SubjectModel>().ReverseMap();
            CreateMap<UpdateSubjectModel, Subject>().ReverseMap();

            #endregion

            CreateMap<ApplicationUser, AspNetUsers>().ReverseMap();
            CreateMap<ApplicationUser, CreateTeacherModel>().ReverseMap()
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
            CreateMap<IdentityRole, RoleUserModel>().ReverseMap();
            CreateMap<IdentityRole, AspNetUserRoles>().ReverseMap();

            #region Teacher
            CreateMap<CreateTeacherModel, Teacher>().ReverseMap();
            CreateMap<TeacherModel, Teacher>().ReverseMap();
            CreateMap<TeacherModel, CreateTeacherModel>().ReverseMap();
            CreateMap<UpdateTeacherModel, Teacher>().ReverseMap();
            CreateMap<UpdateTeacherModel, TeacherModel>().ReverseMap();
            #endregion

            CreateMap<SocialNetwork, SocialNetworkModel>().ReverseMap();
            CreateMap<TeacherSocialNetworkModel, TeacherSocialNetwork>().ReverseMap();

            #region TeacherSubject

            CreateMap<TeacherSubjectModel, TeacherSubject>().ReverseMap();
            CreateMap<TeacherSubjectMaterialModel, TeacherSubjectMaterial>().ReverseMap();
            CreateMap<LessonMaterialModel, LessonMaterial>().ReverseMap();
            CreateMap<LessonMaterialModel, Lesson>().ReverseMap();

            #endregion

            #region Course
            CreateMap<CourseModel, Course>().ReverseMap();
            CreateMap<BookedCourseModel, BookedCourse>().ReverseMap();
            CreateMap<CreateCourseModel, CourseModel>().ReverseMap();
            CreateMap<CreateCourseModel, Course>().ReverseMap();
            CreateMap<UpdateCourseModel, CourseModel>().ReverseMap();
            CreateMap<UpdateCourseModel, Course>().ReverseMap();
            #endregion

            #region Session

            CreateMap<SessionModel, Session>().ReverseMap()
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.Date.Add(src.StartTime)))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.Date.Add(src.EndTime)))
                .ReverseMap();
            CreateMap<CreateSessionModel, Session>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.HasValue ? src.Date.Value : DateTime.Now))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.HasValue ? src.StartTime.Value.TimeOfDay : DateTime.Now.TimeOfDay))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.HasValue ? src.EndTime.Value.TimeOfDay : DateTime.Now.TimeOfDay));

            #endregion

            #region SpotMeeting

            CreateMap<SpotMeetingModel, SpotMeeting>().ReverseMap();
            CreateMap<CreateSpotMeetingModel, SpotMeeting>().ReverseMap();
            CreateMap<CreateSpotMeetingModel, SpotMeetingModel>().ReverseMap()
                .ForMember(d => d.HostId, opt => opt.MapFrom(src => src.Host.TeacherId))
                .ReverseMap();
            CreateMap<UpdateSpotMeetingModel, SpotMeeting>().ReverseMap();
            CreateMap<UpdateSpotMeetingModel, SpotMeetingModel>().ReverseMap()
                .ForMember(d => d.HostId, opt => opt.MapFrom(src => src.Host.TeacherId))
                .ReverseMap();
            CreateMap<SpotMeetingMaterialModel, SpotMeetingMaterial>().ReverseMap();
            CreateMap<SpotMeetingTeacherModel, SpotMeetingTeacher>().ReverseMap();
            CreateMap<BookedSpotMeetingModel, BookedSpotMeeting>().ReverseMap();

            #endregion

            #region Lesson
            CreateMap<LessonModel, Lesson>().ReverseMap();

            #endregion

            CreateMap<SubjectCategoryModel, SubjectCategory>().ReverseMap();

            #region Webex

            CreateMap<WebexGuestIssuerModel, WebexGuestIssuer>().ReverseMap();
            CreateMap<WebexIntegrationModel, WebexIntegration>().ReverseMap();

            #endregion
        }
    }
}
