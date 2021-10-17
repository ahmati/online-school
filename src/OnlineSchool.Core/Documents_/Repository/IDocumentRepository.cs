using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Documents_.Repository
{
    public interface IDocumentRepository
    {
        // ----------------------------------------- Student -----------------------------------------
        Task<StudentDocument> CreateStudentDocumentAsync(StudentDocument studentDocument);
        Task<IEnumerable<StudentDocument>> GetStudentDocumentsAsync();
        Task<StudentDocument> GetStudentDocumentByIdAsync(int id);
        Task<IEnumerable<StudentDocument>> GetStudentDocumentsByIdsAsync(int[] ids);
        Task<StudentDocument> GetStudentDocumentByFileNameAsync(int studentId, string fileName);
        Task<IEnumerable<StudentDocument>> SearchStudentDocumentsByFileNameAsync(string fileName);
        Task<StudentDocument> DeleteStudentDocumentAsync(int studentDocumentId);

        // ----------------------------------------- Teacher -----------------------------------------
        Task<TeacherDocument> CreateTeacherDocumentAsync(TeacherDocument teacherDocument);
        Task<IEnumerable<TeacherDocument>> GetTeacherDocumentsAsync();
        Task<TeacherDocument> GetTeacherDocumentByIdAsync(int id);
        Task<TeacherDocument> GetTeacherDocumentByFileNameAsync(int teacherId, string fileName);
        Task<IEnumerable<TeacherDocument>> SearchTeacherDocumentsByFileNameAsync(string fileName);
        Task<TeacherDocument> DeleteTeacherDocumentAsync(int teacherDocumentId);
        Task<IEnumerable<TeacherDocument>> GetTeacherDocumentsByIdsAsync(int[] ids);
    }
}
