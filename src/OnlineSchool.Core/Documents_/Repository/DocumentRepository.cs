using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Documents_.Repository
{
    public class DocumentRepository : OnlineSchoolBaseRepository, IDocumentRepository
    {
        #region StudentDocument

        public async Task<StudentDocument> CreateStudentDocumentAsync(StudentDocument studentDocument)
        {
            var data = await AddAsync(studentDocument);
            return data;
        }

        public async Task<StudentDocument> DeleteStudentDocumentAsync(int studentDocumentId)
        {
            var data = await Context.StudentDocument.FirstOrDefaultAsync(d => d.Id == studentDocumentId);
            if (data is null)
                return null;

            Context.Entry(data).State = EntityState.Deleted;
            var result = await Context.SaveChangesAsync();

            return result > 0 ? data : null;
        }

        public async Task<StudentDocument> GetStudentDocumentByFileNameAsync(int studentId, string fileName)
        {
            var data = await Context.StudentDocument.FirstOrDefaultAsync(d =>
                d.StudentId == studentId &&
                d.FileName == fileName);
            return data;
        }

        public async Task<StudentDocument> GetStudentDocumentByIdAsync(int id)
        {
            var data = await Context.StudentDocument.FirstOrDefaultAsync(d => d.Id == id);
            return data;
        }

        public async Task<IEnumerable<StudentDocument>> GetStudentDocumentsByIdsAsync(int[] ids)
        {
            var data = await Context.StudentDocument
                .Include(d => d.Student)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<StudentDocument>> GetStudentDocumentsAsync()
        {
            var data = await Context.StudentDocument
                .Include(t => t.Student)
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<StudentDocument>> SearchStudentDocumentsByFileNameAsync(string fileName)
        {
            var data = await Context.StudentDocument
                .Where(d => d.FileName.StartsWith(fileName))
                .ToListAsync();
            return data;
        }

        #endregion

        #region TeacherDocument

        public async Task<TeacherDocument> CreateTeacherDocumentAsync(TeacherDocument teacherDocument)
        {
            var data = await AddAsync(teacherDocument);
            return data;
        }

        public async Task<TeacherDocument> DeleteTeacherDocumentAsync(int teacherDocumentId)
        {
            var data = await Context.TeacherDocument.FirstOrDefaultAsync(d => d.Id == teacherDocumentId);
            if (data is null)
                return null;

            Context.Entry(data).State = EntityState.Deleted;
            var result = await Context.SaveChangesAsync();

            return result > 0 ? data : null;
        }

        public async Task<TeacherDocument> GetTeacherDocumentByFileNameAsync(int teacherId, string fileName)
        {
            var data = await Context.TeacherDocument.FirstOrDefaultAsync(d =>
                d.TeacherId == teacherId &&
                d.FileName == fileName);
            return data;
        }

        public async Task<TeacherDocument> GetTeacherDocumentByIdAsync(int id)
        {
            var data = await Context.TeacherDocument.FirstOrDefaultAsync(d => d.Id == id);
            return data;
        }

        public async Task<IEnumerable<TeacherDocument>> GetTeacherDocumentsAsync()
        {
            var data = await Context.TeacherDocument
                .Include(d => d.Teacher)
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<TeacherDocument>> SearchTeacherDocumentsByFileNameAsync(string fileName)
        {
            var data = await Context.TeacherDocument
                .Where(d => d.FileName.StartsWith(fileName))
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<TeacherDocument>> GetTeacherDocumentsByIdsAsync(int[] ids)
        {
            var data = await Context.TeacherDocument
                .Include(d => d.Teacher)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            return data;
        }

        #endregion
    }
}
