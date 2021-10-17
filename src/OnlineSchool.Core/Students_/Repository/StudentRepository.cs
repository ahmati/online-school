using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Students;
using OnlineSchool.Contract.Subject;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Students_.Repository
{
    public class StudentRepository : OnlineSchoolBaseRepository, IStudentRepository
    {
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT * FROM Student");

                var entities = await Connection.QueryAsync<Student>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            try
            {
                var prms = new DynamicParameters();
                prms.Add("@Id", id);

                var query = "SELECT * FROM Student WHERE Id=@Id";
                var entities = await Connection.QueryFirstOrDefaultAsync<Student>(query, prms);
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Student> GetByEmailAsync(string email)
        {
            try
            {
                var prms = new DynamicParameters();
                prms.Add("@Email", email);

                var query = "SELECT * FROM Student WHERE Email=@email";
                var entities = await Connection.QueryFirstOrDefaultAsync<Student>(query, prms);
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Student> CreateAsync(Student student)
        {
            try
            {
                return await AddAsync(student);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            try
            {
                var result = await base.UpdateAsync(student);
                return result > 0;
            }
            catch (Exception e)
            {
                base.Logger.Error(e, "Errore di imprevisto");
                return false;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await DeleteByIdAsync<Student>(id);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<int> GetCountAsync()
        {
            try
            {
                return await Context.Student.CountAsync();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return 0;
            }
        }

        #region Student - Course

        public async Task<IEnumerable<Course>> GetStudentCoursesAsync(int studentId)
        {
            var result = await Context.BookedCourse
                .Include(s => s.Course.TeacherSubject.Subject)
                .Include(s => s.Course.TeacherSubject.Teacher)
                .Where(s => s.StudentId == studentId)
                .ToListAsync();

            return result.Select(r => r.Course);
        }

        #endregion

        #region Student - Session

        public async Task<IEnumerable<Session>> GetUpcomingSessionsAsync(int studentId)
        {
            try
            {
                var myCoursesIds = await Context.BookedCourse
                    .Where(bc => bc.StudentId == studentId)
                    .Select(bc => bc.CourseId)
                    .ToListAsync();

                var timeLimit = DateTime.Now.AddMinutes(10).TimeOfDay;

                var entities = await Context.Session
                    .Include(s => s.Course)
                        .ThenInclude(c => c.TeacherSubject)
                            .ThenInclude(ts => ts.Subject)
                                
                    .Where(s =>
                        myCoursesIds.Contains(s.CourseId) &&
                        s.Course.IsPublished == true &&
                        (s.Date.Date > DateTime.Now.Date || (s.Date.Date == DateTime.Now.Date && s.EndTime >= timeLimit))
                    )
                    .OrderBy(s => s.Date)
                    .ThenBy(s => s.StartTime)
                    .ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<Session> GetSessionByIdAsync(int studentId, int sessionId)
        {
            var courseIds = await Context.BookedCourse
                .Where(bc => bc.StudentId == studentId)
                .Select(bc => bc.CourseId)
                .ToListAsync();

            return await Context.Session.FirstOrDefaultAsync(s =>
                s.Id == sessionId &&
                courseIds.Contains(s.CourseId) &&
                s.Course.IsPublished == true
            );
        }

        #endregion

        #region OLD methods

        public async Task<Student> GetStudentByIdForUpdate(int id)
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine($"SELECT * FROM Student WHERE Id={id}");
                var entities = await Connection.QueryFirstOrDefaultAsync<Student>(sQuery.ToString());
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BookedCourse> GetExistingBookedCourse(string studentEmail, int courseId)
        {
            var bookedCourse = await Context.BookedCourse.FirstOrDefaultAsync(s => s.Student.Email == studentEmail && s.CourseId == courseId);
            return bookedCourse;
        }

        #endregion
    }
}
