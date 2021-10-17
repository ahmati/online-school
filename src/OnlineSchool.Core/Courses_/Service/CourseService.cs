using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Calendar;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Contract.Webex;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.Helper_;
using OnlineSchool.Core.IcsCalendar.Service;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Payment_.Service;
using OnlineSchool.Core.Session_.Repository;
using OnlineSchool.Core.Students_.Service;
using OnlineSchool.Core.Subjects_.Repository;
using OnlineSchool.Core.TeacherSubject_.Repository;
using OnlineSchool.Core.Webex_.Services;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Subjects_.Services
{
    public class CourseService : CoreBase, ICourseService
    {
        public ICourseRepository CourseRepository { get; set; }
        public ISessionRepository SessionRepository { get; set; }
        public ITeacherSubjectRepository TeacherSubjectRepository { get; set; }
        public IFileService FileService { get; set; }
        public IWebexService WebexService { get; set; }
        public IStudentService StudentService { get; set; }
        public IPaymentService PaymentService { get; set; }
        public IEmailService EmailService { get; set; }
        public IIcsCalendarService CalendarService { get; set; }

        public async Task<CourseModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await CourseRepository.GetByIdAsync(id);

                var result = Mapper.Map<CourseModel>(data);

                foreach (var item in result.Sessions)
                {
                    item.StartDate = item.StartDate.ConvertTimeFromUtc();
                    item.EndDate = item.EndDate.ConvertTimeFromUtc();
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            try
            {
                var data = await CourseRepository.GetAllAsync();
                var result = Mapper.Map<IEnumerable<CourseModel>>(data);
                foreach (var item in result)
                {
                    item.Buttons = GridHelper.ConfigureCourseActions(item, true);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<CreateCourseModel>> CreateAsync(CreateCourseModel model)
        {
            var output = new ResponseBase<CreateCourseModel>();
            try
            {
                var validation = await this.CreateCourseValidation(Mapper.Map<CourseModel>(model));
                if(validation.HasErrors)
                {
                    output.AddErrors(validation.Errors);
                    return output;
                }

                var course = Mapper.Map<Course>(model);
                course.IsPublished = false;

                course = await CourseRepository.CreateAsync(course);

                var fileResult = await FileService.UploadSingleAsync(model.ImageFile, $"wwwroot\\uploads\\courses\\{course.Id}");

                output.Output = Mapper.Map<CreateCourseModel>(course);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError("An error occurred. The course could not be created.");
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UpdateAsync(UpdateCourseModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var validateCourse = await CreateCourseValidation(Mapper.Map<CourseModel>(model));

                if (validateCourse.HasErrors)
                    return validateCourse;


                var course = await CourseRepository.GetByIdAsync(model.Id);
                if (course is null)
                {
                    output.AddError("An error occurred. The subject was not updated.");
                    return output;
                }

                course.TeacherSubjectId = model.TeacherSubjectId;
                course.StartDate = model.StartDate;
                course.EndDate = model.EndDate;
                course.Price = model.Price;
                if (model.ImageFile != null)
                    course.ImagePath = model.ImageFile.FileName;

                var updated = await CourseRepository.UpdateAsync(course);
                if (!updated)
                {
                    output.AddError("An error occurred. The subject was not updated.");
                    return output;
                }

                var fileResult = await ReplaceImageAsync(model.ImageFile, course.Id);

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Course could not be deleted.";
            try
            {
                var course = await GetByIdAsync(id);
                if(course is null)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                if(course.Status == CourseStatus.Published || course.Status == CourseStatus.InProgress || course.Status == CourseStatus.Finished)
                {
                    output.AddError($"Cannot delete a course with status '{course.Status.GetDescription()}'.");
                    return output;
                }

                var deleted = await CourseRepository.DeleteAsync(id);
                if (!deleted)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<IEnumerable<BookedCourseModel>> GetBookedCoursesByTeacherSubjectIdAsync(int teacherSubjectId)
        {
            try
            {
                var data = await CourseRepository.GetBookedCoursesByTeacherSubjectIdAsync(teacherSubjectId);
                return Mapper.Map<IEnumerable<BookedCourseModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> PublishAsync(int courseId)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var course = await CourseRepository.GetByIdAsync(courseId);
                if (course is null)
                {
                    output.AddError("The course could not be found.");
                    return output;
                }

                var validation = this.PublishCourseValidation(Mapper.Map<CourseModel>(course));
                if(validation.HasErrors)
                {
                    output.AddErrors(validation.Errors);
                    return output;
                }

                course.IsPublished = true;
                var updated = await CourseRepository.UpdateAsync(course);
                if (!updated)
                {
                    output.AddError("An error occurred. Course could not be published.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UnpublishAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var course = await CourseRepository.GetByIdAsync(id);
                if (course is null)
                {
                    output.AddError("The meeting could not be found.");
                    return output;
                }

                course.IsPublished = false;
                var updated = await CourseRepository.UpdateAsync(course);
                if (!updated)
                {
                    output.AddError("An error occurred. Meeting could not be unpublished.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<IEnumerable<CourseModel>> GetAllPublishedAsync()
        {
            try
            {
                var data = await CourseRepository.GetAllPublishedAsync();
                var result = Mapper.Map<IEnumerable<CourseModel>>(data);

                foreach (var item in result)
                {
                    var isLive = await HasOngoingSessions(item);
                    item.IsLive = isLive.Output;
                }

                return result;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<BookedCourseModel>> CreateBookedCourseAsync(BookedCourseModel model)
        {
            var output = new ResponseBase<BookedCourseModel>();
            try
            {
                var bookedCourse = Mapper.Map<BookedCourse>(model);

                bookedCourse = await CourseRepository.CreateBookedCourseAsync(bookedCourse);

                output.Output = Mapper.Map<BookedCourseModel>(bookedCourse);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<IEnumerable<CourseModel>> GetAllPublishedAsync(string searchString)
        {
            try
            {
                var data = await CourseRepository.GetAllPublishedAsync(searchString);
                return Mapper.Map<IEnumerable<CourseModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int courseId)
        {
            var output = new ResponseBase<bool>();

            if (file is null)
                return output;

            if (courseId < 1)
                throw new ArgumentException("Invalid course id");

            try
            {
                var destination = $"wwwroot\\uploads\\courses\\{courseId}";

                if (Directory.Exists(destination))
                {
                    var existingFiles = Directory.GetFiles(destination);
                    foreach (var f in existingFiles)
                        File.Delete(f);
                }

                var result = await FileService.UploadSingleAsync(file, destination);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        #region Session

        public async Task<IEnumerable<SessionModel>> GetSessionsAsync(int courseId)
        {
            try
            {
                var data = await CourseRepository.GetSessionsAsync(courseId);
                return Mapper.Map<IEnumerable<SessionModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<SessionModel>> CreateSessionAsync(CreateSessionModel model)
        {
            /* Workflow
             * 1. Make sure courseId exists
             * 2. Validate the model
             * 3. If model is valid, create the Webex Meeting. If this fails, don't continue.
             * 4. If Webex meeting is created successfully, add session to db with the required data
            */
            var output = new ResponseBase<SessionModel>();
            try
            {
                if (model is null) throw new ArgumentNullException(nameof(model));

                var course = await this.GetByIdAsync(model.CourseId);
                if(course is null)
                {
                    output.AddError("Invalid course.");
                    return output;
                }

                var validation = await CreateSessionValidation(course, model);
                if (validation.HasErrors)
                {
                    output.AddErrors(validation.Errors);
                    return output;
                }

                var createMeetingModel = new CreateMeetingModel()
                {
                    title = $"Course-{course.Id}_{Guid.NewGuid()}",
                    start = model.Date.Value.Date.Add(model.StartTime.Value.TimeOfDay).ToString("s", CultureInfo.InvariantCulture),
                    end = model.Date.Value.Date.Add(model.EndTime.Value.TimeOfDay).ToString("s", CultureInfo.InvariantCulture)
                };
                var meeting = await WebexService.CreateMeetingAsync(createMeetingModel);
                if(meeting.HasErrors)
                {
                    output.AddErrors(meeting.Errors);
                    return output;
                }

                var session = Mapper.Map<Session>(model);
                session.MeetingId = meeting.Output.id;
                session.MeetingTitle = meeting.Output.title;
                session.MeetingPassword = meeting.Output.password;
                session.MeetingSipAddress = meeting.Output.sipAddress;
                session.StartTime = session.StartTime.ConvertTimeToUtc();
                session.EndTime = session.EndTime.ConvertTimeToUtc();

                session = await CourseRepository.CreateSessionAsync(session);
                output.Output = Mapper.Map<SessionModel>(session);

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError("An error occurred. The session could not be created.");
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteSessionAsync(int sessionId)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Session could not be deleted.";
            try
            {
                var session = await SessionRepository.GetByIdAsync(sessionId);
                if (session is null)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                var course = await GetByIdAsync(session.CourseId);
                if(course.Status != CourseStatus.NotPublished)
                {
                    output.AddError($"Cannot delete session from course with status '{course.Status.GetDescription()}'.");
                    return output;
                }

                var meetingDeletion = await WebexService.DeleteMeetingAsync(session.MeetingId);
                if(meetingDeletion.HasErrors)
                {
                    output.AddErrors(meetingDeletion.Errors);
                    return output;
                }

                var deleted = await SessionRepository.DeleteAsync(sessionId);
                if (!deleted)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        #endregion

        #region Private methods

        private async Task<ResponseBase<bool>> CreateCourseValidation(CourseModel course)
        {
            var output = new ResponseBase<bool>();

            if (course is null)
            {
                output.AddError("Missing information.");
                return output;
            }

            var start = course.StartDate.Date;
            var end = course.EndDate.Date;
            var today = DateTime.Now.Date;

            var teacherSubject = await TeacherSubjectRepository.GetByIdAsync(course.TeacherSubjectId);
            if (teacherSubject is null)
                output.AddError("Invalid subject/teacher.");

            if (start < today)
                output.AddError("Start date cannot be before today.");
            if (end < today)
                output.AddError("End date cannot be before today.");
            if ((start - today).Days < 1)
                output.AddError("Start date should be at least 1 days after the date of creation.");
            if ((end - start).Days < 1)
                output.AddError("Course should be at least 1 day long.");
            if (course.Price < 0)
                output.AddError("Invalid price");

            return output;
        }

        private ResponseBase<bool> PublishCourseValidation(CourseModel course)
        {
            var output = new ResponseBase<bool>();

            if (course.IsPublished)
            {
                output.AddError("Course is already published.");
                return output;
            }

            if (course.Status == CourseStatus.NotPublished_PastDue)
            {
                output.AddError("Cannot publish the course. Due date has passed.");
                return output;
            }

            if (course.Sessions is null || course.Sessions.Count == 0)
            {
                output.AddError("Course should have at least 1 session.");
                return output;
            }

            return output;
        }

        private async Task<ResponseBase<bool>> CreateSessionValidation(CourseModel course, CreateSessionModel model)
        {
            var output = new ResponseBase<bool>();

            var date = model.Date;
            var start = model.StartTime;
            var end = model.EndTime;

            if(course.Status != CourseStatus.NotPublished)
            {
                output.AddError($"Cannot edit course with status '{course.Status.GetDescription()}'.");
                return output;
            }

            if(string.IsNullOrEmpty(model.Topic) || string.IsNullOrWhiteSpace(model.Topic))
                output.AddError("Session topic is required.");

            if (model.Topic.Length > 1000)
                output.AddError("Topic cannot be longer than 1000 characters.");

            if(date.HasValue == false)
            {
                output.AddError("Date is required");
                return output;
            }
            if (start.HasValue == false)
            {
                output.AddError("Start time is required");
                return output;
            }
            if (end.HasValue == false)
            {
                output.AddError("End time is required");
                return output;
            }

            var timeRange = end.Value - start.Value;
            if (timeRange.Hours < 1 || timeRange.Hours > 24 )
            {
                output.AddError("Session duration must be at least 1 hour and no longer than 24 hours.");
                return output;
            }
        
            if (date.Value.Date < course.StartDate || date.Value.Date > course.EndDate)
            {
                output.AddError($"Date should be between {course.StartDate.ToShortDateString()} and {course.EndDate.ToShortDateString()}.");
                return output;
            }

            var sessions = await this.GetSessionsAsync(model.CourseId);
            var overlaps = sessions.Any(s =>
                s.Date.Date.Equals(model.Date.Value.Date) && (
                    (model.StartTime.Value.TimeOfDay <= s.StartTime && model.EndTime.Value.TimeOfDay > s.StartTime) ||
                    ((model.StartTime.Value.TimeOfDay < s.EndTime && model.EndTime.Value.TimeOfDay > s.StartTime))
                ));

            if(overlaps)
            {
                output.AddError("Session's date/time overlaps with an existing session of this course.");
                return output;
            }

            return output;
        }

        private async Task<ResponseBase<bool>> HasOngoingSessions(CourseModel model)
        {
            var result = new ResponseBase<bool>();
            foreach (var session in model.Sessions)
            {
                result = await WebexService.CheckHasMeetingStartedAsync(session.MeetingId);
            }

            return result;
        }
        #endregion

        #region ------------------------------------- PurchasableItem Methods -------------------------------------
        public async Task<PurchasableItemModel> GetPurchasableItemByIdAsync(int id)
        {
            try
            {
                var data = await GetByIdAsync(id);

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> GetExistingPurchasedItemAsync(string email, int id)
        {
            var result = new ResponseBase<bool>();
            try
            {
                var data = await StudentService.GetExistingBookedCourseAsync(email, id);
                if (data != null)
                {
                    result.Output = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> BuyAsync(string stripeToken, int id, string email)
        {
            #region Validations 
            if (string.IsNullOrWhiteSpace(stripeToken))
                throw new ArgumentNullException(nameof(stripeToken));

            var course = await GetByIdAsync(id);
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            var student = await StudentService.GetByEmailAsync(email);
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            #endregion

            var result = new ResponseBase<bool>();
            try
            {
                var result2 = await PaymentService.PayIt(stripeToken, (decimal)course.Price, student.Name, email);
                StringBuilder message ;

                if (result2.Output)
                {
                    var model = new BookedCourseModel
                    {
                        StudentId = student.Id,
                        CourseId = id,
                        Price = (decimal)course.Price
                    };

                    await CreateBookedCourseAsync(model);

                    message = new StringBuilder(EmailSharedResources.BuyCourseSuccess);
                    message.Replace("studentName", student.Name.ToUpper());
                    message.Replace("courseName", course.TeacherSubject?.Subject?.Name);
                    message.Replace("coursePrice", course.Price.ToString());
                    message.Replace("startDate", course.StartDate.Date.ToString());

                }
                else
                {
                    message = new StringBuilder(EmailSharedResources.BuyCourseError);
                    message.Replace("studentName", student.Name.ToUpper());
                    message.Replace("courseName", course.TeacherSubject?.Subject?.Name);
                }

                await EmailService.SendEmailAsync(email, EmailSharedResources.PaymentConfirmation, message.ToString());

                var sessions = await GetSessionsAsync(id);
                foreach (var session in sessions)
                {
                    var calendarModel = new IcsCalendarModel()
                    {
                        Email = email,
                        UserName = student.Name,
                        Description = course.TeacherSubject.Subject.Description,
                        StartDate = session.Date,
                        StartTime = session.StartTime,
                        Title = session.Topic,
                        Duration = (int)session.EndTime.Subtract(session.StartTime).TotalHours
                    };
                    var emailSent = await CalendarService.SendIcsFile(calendarModel);
                }

                result.Output = true;
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return result;
            }

        }

        public async Task<ResponseBase<bool>> JoinAsync(int id, string email)
        {
            #region Validations 
            var course = await GetByIdAsync(id);
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            var student = await StudentService.GetByEmailAsync(email);
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            #endregion

            var result = new ResponseBase<bool>();
            try
            {
                var model = new BookedCourseModel
                {
                    StudentId = student.Id,
                    CourseId = id,
                    Price = (decimal)course.Price
                };

                var result2 = await CreateBookedCourseAsync(model);

                var sessions = await GetSessionsAsync(id);
                foreach (var session in sessions)
                {
                    var calendarModel = new IcsCalendarModel()
                    {
                        Email = email,
                        UserName = student.Name,
                        Description = course.TeacherSubject.Subject.Description,
                        StartDate = session.Date,
                        StartTime = session.StartTime,
                        Title = session.Topic,
                        Duration = (int)session.EndTime.Subtract(session.StartTime).TotalHours
                    };
                    var emailSent = await CalendarService.SendIcsFile(calendarModel);
                }
                
                result.Output = (result2 != null);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return result;
            }
        }
        #endregion
    }
}
