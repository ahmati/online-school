using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Infrastructure.Enums;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using OnlineSchool.Core.Identity_.Repository;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Roles_.Services;
using OnlineSchool.Core.Students_.Repository;
using OnlineSchool.Core.Subjects_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Students_.Service
{    public class StudentService : CoreBase, IStudentService
    {
        public IStudentRepository StudentRepository { get; set; }
        public ISubjectRepository SubjectRepository { get; set; }
        public IIdentityService IdentityService { get; set; }
        public IRoleService RoleService { get; set; }
        public IFileService FileService { get; set; }

        public async Task<IEnumerable<StudentModel>> GetAllAsync()
        {
            try
            {
                var data = await StudentRepository.GetAllAsync();
                return Mapper.Map<IEnumerable<StudentModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<StudentModel> GetByIdAsync (int id)
        {
            try
            {
                var data = await StudentRepository.GetByIdAsync(id);
                return Mapper.Map<StudentModel>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null; ;
            }
        }
        
        public async Task<StudentModel> GetByEmailAsync (string email)
        {
            try
            {
                var data = await StudentRepository.GetByEmailAsync(email);
                return Mapper.Map<StudentModel>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null; ;
            }
        }

        public async Task<int> GetCountAsync()
        {
            try
            {
                return await StudentRepository.GetCountAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error while getting student count.", ex.Message);
                return 0;
            }
        }

        public async Task<ResponseBase<StudentModel>> CreateAsync (CreateStudentModel model)
        {
            var output = new ResponseBase<StudentModel>();
            try
            {
                var exists = (await IdentityService.FindByEmailAsync(model.Email)) != null;
                if(exists)
                {
                    output.AddError($"User with email '{model.Email}' already exists");
                    return output;
                }

                // Step 1
                var student = Mapper.Map<Student>(model);
                student = await StudentRepository.CreateAsync(student);

                // Step 2
                var user = Mapper.Map<ApplicationUser>(model);
                var identityResult = await IdentityService.CreateAsync(user, model.Password);

                if (!identityResult.Succeeded)
                {
                    // If step 2 fails, rollback step 1
                    await StudentRepository.DeleteByIdAsync(student.Id);

                    foreach (var err in identityResult.Errors)
                    {
                        output.AddError(err.Description);
                    }
                    return output;
                }

                // Step 3
                var role = await RoleService.GetRoleByNameAsync(Roles.Student.GetDescription());
                var roleResult = await IdentityService.AddToRoleAsync(user.Id, role.Id);

                // Step 4
                var fileResult = await FileService.UploadSingleAsync(model.ImageFile, $"wwwroot\\uploads\\students\\{student.Id}");

                output.Output = Mapper.Map<StudentModel>(student);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<bool>> UpdateAsync (UpdateStudentModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                // Step 1
                var student = await StudentRepository.GetByIdAsync(model.Id);
                student.Name = model.Name;
                student.Surname = model.Surname;
                student.Gender = model.Gender;
                student.Description = model.Description;
                if(model.ImageFile != null)
                    student.ImagePath = model.ImageFile.FileName;

                var result1 = await StudentRepository.UpdateAsync(student);

                // Step 2
                var user = await IdentityService.FindByEmailAsync(student.Email);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Gender = model.Gender;
                user.Description = model.Description;
                if (model.ImageFile != null)
                    user.ImagePath = model.ImageFile.FileName;

                var result2 = await IdentityService.UpdateAsync(user);

                if(!result2.Succeeded)
                {
                    // If step 2 fails, rollback step 1
                    foreach(var err in result2.Errors)
                    {
                        output.AddError(err.Description);
                    }
                    return output;
                }

                // Step 3
                var fileResult = await ReplaceImageAsync(model.ImageFile, student.Id);

                output.Output = result2.Succeeded;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteByIdAsync (int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                // Operation 1
                var student = await StudentRepository.GetByIdAsync(id);
                var result1 = await StudentRepository.DeleteByIdAsync(id);

                // Operation 2
                var user = await IdentityService.FindByEmailAsync(student.Email);
                var result2 = await IdentityService.DeleteUserAsync(user.Id);

                if(!result2)
                {
                    // If operation 2 fails, rollback operation 1
                    output.AddError("Failed to delete student account");
                    return output;
                }

                output.Output = result2;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int studentId)
        {
            var output = new ResponseBase<bool>();

            if (file is null)
                return output;

            if (studentId < 1)
                throw new ArgumentException("Invalid student id");

            try
            {
                var destination = $"wwwroot\\uploads\\students\\{studentId}";

                if(Directory.Exists(destination))
                {
                    var existingFiles = Directory.GetFiles(destination);
                    foreach (var f in existingFiles)
                        File.Delete(f);
                }

                var result = await FileService.UploadSingleAsync(file, destination);
                return result;
            }
            catch(Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        #region Student - Course

        public async Task<IEnumerable<CourseModel>> GetStudentCoursesAsync(int studentId)
        {
            try
            {
                var data = await StudentRepository.GetStudentCoursesAsync(studentId);
                return Mapper.Map<IEnumerable<CourseModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<BookedCourseModel> GetExistingBookedCourseAsync(string studentEmail, int courseId)
        {
            try
            {
                var data = await StudentRepository.GetExistingBookedCourse(studentEmail, courseId);
                return Mapper.Map<BookedCourseModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                throw ex;
            }
        }

        #endregion

        #region Student - Session

        public async Task<IEnumerable<SessionModel>> GetUpcmonigSessions(int studentId)
        {
            try
            {
                var result = await StudentRepository.GetUpcomingSessionsAsync(studentId);
                return Mapper.Map<IEnumerable<SessionModel>>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                throw ex;
            }
        }

        public async Task<SessionModel> GetSessionByIdAsync(int studentId, int sessionId)
        {
            try
            {
                var data = await StudentRepository.GetSessionByIdAsync(studentId, sessionId);
                return Mapper.Map<SessionModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                return null;
            }
        }
       
        #endregion
    }
}
