using Dapper;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Subjects_.Repository
{
    public class CourseRepository : OnlineSchoolBaseRepository, ICourseRepository
    {
        public async Task<Course> GetByIdAsync(int id)
        {
            try
            {
                var data = await Context.Course
                    .Include(t => t.TeacherSubject)
                    .Include(t => t.TeacherSubject.Teacher)
                    .Include(t => t.TeacherSubject.Subject)
                    .Include(t => t.Sessions)
                    .Where(t => t.Id == id )
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            try
            {
                var courses = await Context.Course
                    .Include(c => c.TeacherSubject.Teacher)
                    .Include(c => c.TeacherSubject.Subject)
                    .ToListAsync();

                return courses;
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<Course> CreateAsync(Course course)
        {
            try
            {
                return await AddAsync(course);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            try
            {
                var result = await base.UpdateAsync(course);
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sessions = await Context.Session.Where(s => s.CourseId == id).ToListAsync();
            Context.Session.RemoveRange(sessions);

            await Context.SaveChangesAsync();

            var result = await DeleteByIdAsync<Course>(id);
            return result > 0;
        }

        public async Task<bool> GetByTeacherSubjectIdAsync(int id)
        {
            var exists = await Context.Course
                    .Where(c => c.TeacherSubjectId == id )
                    .FirstOrDefaultAsync();

            return exists != null;
        }

        public async Task<BookedCourse> CreateBookedCourseAsync(BookedCourse bookedCourse)
        {
            try
            {
                return await AddAsync(bookedCourse);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<BookedCourse>> GetBookedCoursesByTeacherSubjectIdAsync(int teacherSubjectId)
        {
            try
            {
                var data = await Context.BookedCourse
                    .Where(c => c.Course.TeacherSubjectId == teacherSubjectId)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<Course>> GetAllPublishedAsync()
        {
            try
            {
                var data = await Context.Course
                    .Include(c => c.TeacherSubject)
                    .Include(c => c.TeacherSubject.Teacher)
                    .Include(c => c.TeacherSubject.Subject)
                    .Include(c => c.Sessions)
                    .Where(c => c.EndDate > DateTime.Now && c.IsPublished == true)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<Course>> GetAllPublishedAsync(string searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return await this.GetAllPublishedAsync();

                var data = await Context.Course
                    .Include(c => c.TeacherSubject)
                    .Include(c => c.TeacherSubject.Teacher)
                    .Include(c => c.TeacherSubject.Subject)
                    .Where(c => 
                        c.IsPublished == true &&
                        c.StartDate > DateTime.Now && (
                            c.TeacherSubject.Teacher.Name.Contains(searchString) || 
                            c.TeacherSubject.Teacher.Surname.Contains(searchString) || 
                            c.TeacherSubject.Subject.Name.Contains(searchString)
                        )
                    ).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        #region Session actions

        public async Task<IEnumerable<Session>> GetSessionsAsync(int courseId)
        {
            try
            {
                var data = await Context.Session
                    .Include(t => t.Course)
                    .Where(t => t.CourseId == courseId)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Session> CreateSessionAsync(Session session)
        {
            return await AddAsync(session);
        }

        #endregion
    }
        
}


