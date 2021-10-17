using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Identity.Models;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Infrastructure.Enums;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Contract.TeacherSubject;
using OnlineSchool.Core.Helper_;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Roles_.Services;
using OnlineSchool.Core.Teachers_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Teachers_.Service
{
    public class TeacherService : CoreBase, ITeacherService
    {
        public ITeachersRepository TeacherRepository { get; set; }
        public IIdentityService IdentityService { get; set; }
        public IRoleService RoleService { get; set; }
        public IFileService FileService { get; set; }

        public async Task<IEnumerable<TeacherModel>> GetAllAsync()
        {
            try
            {
                var data = await TeacherRepository.GetAllAsync();

                return Mapper.Map<IEnumerable<TeacherModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<TeacherModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await TeacherRepository.GetByIdAsync(id);
                return Mapper.Map<TeacherModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<TeacherModel> GetByEmailAsync(string email)
        {
            try
            {
                var data = await TeacherRepository.GetByEmailAsync(email);
                return Mapper.Map<TeacherModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<TeacherModel>> CreateAsync(CreateTeacherModel model)
        {
            var output = new ResponseBase<TeacherModel>();
            try
            {
                var teacher = Mapper.Map<Teacher>(model);
                teacher = await TeacherRepository.CreateAsync(teacher);

                var user = Mapper.Map<ApplicationUser>(model);
                var result = await IdentityService.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    // If operation 2 fails, rollback operation 1
                    await TeacherRepository.DeleteByIdAsync(teacher.Id);

                    foreach (var err in result.Errors)
                    {
                        output.AddError(err.Description);
                    }
                    return output;
                }

                var role = await RoleService.GetRoleByNameAsync(Roles.Teacher.GetDescription());
                await IdentityService.AddToRoleAsync(user.Id, role.Id);

                var fileResult = await FileService.UploadSingleAsync(model.ImageFile, $"wwwroot\\uploads\\teachers\\{teacher.Id}");

                output.Output = Mapper.Map<TeacherModel>(teacher);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<bool>> UpdateAsync(UpdateTeacherModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var teacher = await TeacherRepository.GetByIdAsync(model.Id);
                teacher.Name = model.Name;
                teacher.Surname = model.Surname;
                teacher.Gender = model.Gender;
                teacher.Description = model.Description;
                if(model.ImageFile != null)
                    teacher.ImagePath = model.ImageFile.FileName;

                var result1 = await TeacherRepository.UpdateAsync(teacher);

                var user = await IdentityService.FindByEmailAsync(teacher.Email);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Gender = model.Gender;
                user.Description = model.Description;
                if (model.ImageFile != null)
                    user.ImagePath = model.ImageFile.FileName;

                var result2 = await IdentityService.UpdateAsync(user);

                if (!result2.Succeeded)
                {
                    // If operation 2 fails, rollback operation 1
                    foreach (var err in result2.Errors)
                    {
                        output.AddError(err.Description);
                    }
                    return output;
                }

                var fileResult = await ReplaceImageAsync(model.ImageFile, teacher.Id);

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

        public async Task<ResponseBase<bool>> DeleteByIdAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {

                var teacher = await TeacherRepository.GetByIdAsync(id);
                var result1 = await TeacherRepository.DeleteByIdAsync(id);

                var user = await IdentityService.FindByEmailAsync(teacher.Email);
                var result2 = await IdentityService.DeleteUserAsync(user.Id);
                if (!result2)
                {
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

        public async Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int teacherId)
        {
            var output = new ResponseBase<bool>();

            if (file is null)
                return output;

            if (teacherId < 1)
                throw new ArgumentException("Invalid teacher id");

            try
            {
                var destination = $"wwwroot\\uploads\\teachers\\{teacherId}";

                if(Directory.Exists(destination))
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

        public async Task<IEnumerable<TeacherSubjectModel>> GetTeacherSubjectsAsync(int teacherId)
        {
            try
            {
                var data = await TeacherRepository.GetTeacherSubjectsAsync(teacherId);
                return Mapper.Map<IEnumerable<TeacherSubjectModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<CourseModel>> GetTeacherCoursesAsync(int teacherId)
        {
            try
            {
                var data = await TeacherRepository.GetTeacherCoursesAsync(teacherId);
                var result = Mapper.Map<IEnumerable<CourseModel>>(data);
                foreach (var item in result)
                {
                    item.Buttons = GridHelper.ConfigureCourseActions(item, false);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        #region Teacher - Session

        public async Task<IEnumerable<SessionModel>> GetUpcomingSessionsAsync(int teacherId)
        {
            try
            {
                var data = await TeacherRepository.GetUpcomingSessionsAsync(teacherId);
                return Mapper.Map<IEnumerable<SessionModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<SessionModel> GetSessionByIdAsync(int teacherId, int sessionId)
        {
            try
            {
                var data = await TeacherRepository.GetSessionByIdAsync(teacherId, sessionId);
                return Mapper.Map<SessionModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                return null;
            }
        }

        #endregion

        #region TeacherSocialNetwork

        public async Task<IEnumerable<TeacherSocialNetworkModel>> GetTeacherSocialNetworksAsync(int teacherId)
        {
            try
            {
                var teacherSocial = await TeacherRepository.GetTeacherSocialNetworksAsync(teacherId);
                var result = Mapper.Map<IEnumerable<TeacherSocialNetworkModel>>(teacherSocial);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseBase<TeacherSocialNetworkModel>> CreateTeacherSocialNetworkAsync(TeacherSocialNetworkModel model)
        {
            var output = new ResponseBase<TeacherSocialNetworkModel>();
            try
            {
                var existing = await TeacherRepository.GetTeacherSocialNetworkByIdAsync(model.TeacherId, model.SocialNetworkId);
                if(existing != null)
                {
                    output.AddError($"Teacher already has a {existing.SocialNetwork.Description} account.");
                    return output;
                }

                var social = Mapper.Map<TeacherSocialNetwork>(model);
                social = await TeacherRepository.CreateTeacherSocialNetworkAsync(social);
                output.Output = Mapper.Map<TeacherSocialNetworkModel>(social);

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError("An error occurred. The teacher social could not be added.");
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UpdateTeacherSocialNetworkAsync(TeacherSocialNetworkModel socialModel)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. The teacher's social could not be updated.";
            try
            {
                var social = await TeacherRepository.GetTeacherSocialNetworkByIdAsync(socialModel.TeacherId,socialModel.SocialNetworkId);
                if(social is null)
                {
                    output.AddError(errorMessage);
                    return output;
                }
                social.Link = socialModel.Link;
                social.AuthDate = DateTime.Now;
                
                var result = await TeacherRepository.UpdateTeacherSocialNetworkAsync(social);
                if (!result)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = true;
                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteTeacherSocialNetworkAsync(int teacherId, int socialNetworkId)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Teacher social could not be deleted.";
            try
            {
                var deleted = await TeacherRepository.DeleteTeacherSocialNetworkAsync(teacherId, socialNetworkId);
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

        #region Old methods

        public async Task<List<TeacherModel>> GetAllTeachersAsync()
        {
            try
            {
                return await TeacherRepository.GetAllTeachersAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<TeacherModel>> GetTeacherAsync(int Id)
        {
            try
            {
                return await TeacherRepository.GetTeacherAsync(Id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<TeacherModel> GetTeacherProfileById(int id)
        {
            return await TeacherRepository.GetTeacherProfileById(id);
        }

        public async Task<MaterialModel> CreateCommentAsync(MaterialModel model, string name, string body, int id, string teacherName)
        {
            return await TeacherRepository.CreateCommentAsync(model, name, body, id, teacherName);
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeacherById(int id)
        {
            return await TeacherRepository.GetAllTeacherById(id);
        }

        public async Task<IEnumerable<MaterialModel>> GetTeacherComment()
        {
            return await TeacherRepository.GetTeacherComment();

        }

        #endregion
    }
}

