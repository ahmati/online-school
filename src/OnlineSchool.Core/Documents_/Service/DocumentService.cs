using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Documents;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Core.Documents_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Documents_.Service
{
    public class DocumentService : CoreBase, IDocumentService
    {
        public IDocumentRepository DocumentRepository { get; set; }

        #region StudentDocument

        public async Task<ResponseBase<StudentDocumentModel>> CreateStudentDocumentAsync(CreateStudentDocumentModel model)
        {
            var output = new ResponseBase<StudentDocumentModel>();
            try
            {
                var data = Mapper.Map<StudentDocument>(model);
                var result = await DocumentRepository.CreateStudentDocumentAsync(data);
                output.Output = Mapper.Map<StudentDocumentModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while storing student document.");
            }
            return output;
        }

        public async Task<ResponseBase<StudentDocumentModel>> DeleteStudentDocumentAsync(int studentDocumentId)
        {
            var output = new ResponseBase<StudentDocumentModel>();
            try
            {
                var result = await DocumentRepository.DeleteStudentDocumentAsync(studentDocumentId);
                if (result is null)
                    output.AddError("An error occurred while deleting student document.");
                else
                    output.Output = Mapper.Map<StudentDocumentModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while deleting student document.");
            }
            return output;
        }

        public async Task<ResponseBase<StudentDocumentModel>> GetStudentDocumentByFileNameAsync(int studentId, string fileName)
        {
            var output = new ResponseBase<StudentDocumentModel>();
            try
            {
                var data = await DocumentRepository.GetStudentDocumentByFileNameAsync(studentId, fileName);
                var result = Mapper.Map<StudentDocumentModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving student document.");
            }
            return output;
        }

        public async Task<ResponseBase<StudentDocumentModel>> GetStudentDocumentByIdAsync(int id)
        {
            var output = new ResponseBase<StudentDocumentModel>();
            try
            {
                var data = await DocumentRepository.GetStudentDocumentByIdAsync(id);
                var result = Mapper.Map<StudentDocumentModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving student document.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<StudentDocumentModel>>> GetStudentDocumentsByIdsAsync(int[] ids)
        {
            var output = new ResponseBase<IEnumerable<StudentDocumentModel>>();

            if (ids is null)
                throw new ArgumentNullException("StudentDocument ids required");

            if (ids.Length == 0)
                return output;

            try
            {
                var data = await DocumentRepository.GetStudentDocumentsByIdsAsync(ids);
                var result = Mapper.Map<IEnumerable<StudentDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving student documents.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<StudentDocumentModel>>> GetStudentDocumentsAsync()
        {
            var output = new ResponseBase<IEnumerable<StudentDocumentModel>>();
            try
            {
                var data = await DocumentRepository.GetStudentDocumentsAsync();
                var result = Mapper.Map<IEnumerable<StudentDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving student documents.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<StudentDocumentModel>>> SearchStudentDocumentsByFileNameAsync(string fileName)
        {
            var output = new ResponseBase<IEnumerable<StudentDocumentModel>>();
            try
            {
                var data = await DocumentRepository.SearchStudentDocumentsByFileNameAsync(fileName);
                var result = Mapper.Map<IEnumerable<StudentDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while searching student documents");
            }
            return output;
        }

        #endregion

        #region TeacherDocument

        public async Task<ResponseBase<TeacherDocumentModel>> CreateTeacherDocumentAsync(CreateTeacherDocumentModel model)
        {
            var output = new ResponseBase<TeacherDocumentModel>();
            try
            {
                var data = Mapper.Map<TeacherDocument>(model);
                var result = await DocumentRepository.CreateTeacherDocumentAsync(data);
                output.Output = Mapper.Map<TeacherDocumentModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while storing teacher document.");
            }
            return output;
        }

        public async Task<ResponseBase<TeacherDocumentModel>> DeleteTeacherDocumentAsync(int teacherDocumentId)
        {
            var output = new ResponseBase<TeacherDocumentModel>();
            try
            {
                var result = await DocumentRepository.DeleteTeacherDocumentAsync(teacherDocumentId);
                if(result is null)
                    output.AddError("An error occurred while deleting teacher document.");
                else
                    output.Output = Mapper.Map<TeacherDocumentModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while deleting teacher document.");
            }
            return output;
        }

        public async Task<ResponseBase<TeacherDocumentModel>> GetTeacherDocumentByFileNameAsync(int teacherId, string fileName)
        {
            var output = new ResponseBase<TeacherDocumentModel>();
            try
            {
                var data = await DocumentRepository.GetTeacherDocumentByFileNameAsync(teacherId, fileName);
                var result = Mapper.Map<TeacherDocumentModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving teacher document.");
            }
            return output;
        }

        public async Task<ResponseBase<TeacherDocumentModel>> GetTeacherDocumentByIdAsync(int id)
        {
            var output = new ResponseBase<TeacherDocumentModel>();
            try
            {
                var data = await DocumentRepository.GetTeacherDocumentByIdAsync(id);
                var result = Mapper.Map<TeacherDocumentModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving teacher document.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> GetTeacherDocumentsAsync()
        {
            var output = new ResponseBase<IEnumerable<TeacherDocumentModel>>();
            try
            {
                var data = await DocumentRepository.GetTeacherDocumentsAsync();
                var result = Mapper.Map<IEnumerable<TeacherDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving teacher documents.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> SearchTeacherDocumentsByFileNameAsync(string fileName)
        {
            var output = new ResponseBase<IEnumerable<TeacherDocumentModel>>();
            try
            {
                var data = await DocumentRepository.SearchTeacherDocumentsByFileNameAsync(fileName);
                var result = Mapper.Map<IEnumerable<TeacherDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while searching teacher documents.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> GetTeacherDocumentsByIdsAsync(int[] ids)
        {
            var output = new ResponseBase<IEnumerable<TeacherDocumentModel>>();

            if (ids is null)
                throw new ArgumentNullException("TeacherDocument ids required");

            if (ids.Length == 0)
                return output;

            try
            {
                var data = await DocumentRepository.GetTeacherDocumentsByIdsAsync(ids);
                var result = Mapper.Map<IEnumerable<TeacherDocumentModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving teacher documents.");
            }
            return output;
        }

        #endregion
    }
}
