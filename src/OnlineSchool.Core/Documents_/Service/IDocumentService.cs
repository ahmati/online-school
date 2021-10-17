using OnlineSchool.Contract.Documents;
using OnlineSchool.Contract.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Documents_.Service
{
    public interface IDocumentService
    {
        // ----------------------------------------- Student -----------------------------------------
        Task<ResponseBase<StudentDocumentModel>> CreateStudentDocumentAsync(CreateStudentDocumentModel model);
        Task<ResponseBase<IEnumerable<StudentDocumentModel>>> GetStudentDocumentsAsync();
        Task<ResponseBase<StudentDocumentModel>> GetStudentDocumentByIdAsync(int id);
        Task<ResponseBase<IEnumerable<StudentDocumentModel>>> GetStudentDocumentsByIdsAsync(int[] ids);
        Task<ResponseBase<StudentDocumentModel>> GetStudentDocumentByFileNameAsync(int studentId, string fileName);
        Task<ResponseBase<IEnumerable<StudentDocumentModel>>> SearchStudentDocumentsByFileNameAsync(string fileName);
        Task<ResponseBase<StudentDocumentModel>> DeleteStudentDocumentAsync(int studentDocumentId);

        // ----------------------------------------- Teacher -----------------------------------------
        Task<ResponseBase<TeacherDocumentModel>> CreateTeacherDocumentAsync(CreateTeacherDocumentModel model);
        Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> GetTeacherDocumentsAsync();
        Task<ResponseBase<TeacherDocumentModel>> GetTeacherDocumentByIdAsync(int id);
        Task<ResponseBase<TeacherDocumentModel>> GetTeacherDocumentByFileNameAsync(int teacherId, string fileName);
        Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> SearchTeacherDocumentsByFileNameAsync(string fileName);
        Task<ResponseBase<TeacherDocumentModel>> DeleteTeacherDocumentAsync(int teacherDocumentId);
        Task<ResponseBase<IEnumerable<TeacherDocumentModel>>> GetTeacherDocumentsByIdsAsync(int[] ids);
    }
}
