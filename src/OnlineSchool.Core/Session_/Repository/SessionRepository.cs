using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Session_.Repository
{
    public class SessionRepository : OnlineSchoolBaseRepository, ISessionRepository
    {

        public async Task<IEnumerable<Session>> GetSessionsThatNeedRemindingAsync()
        {
            try
            {
                var limitSpan = DateTime.Now.AddMinutes(180).TimeOfDay;

                var result = await Context.Session
                   .Include(s => s.Course)
                       .Include(c => c.Course.TeacherSubject)
                       .Include(ts => ts.Course.TeacherSubject.Subject)
                       .Include(t => t.Course.TeacherSubject.Teacher)
                       .Where(s => 
                            s.ReminderEmail == false &&
                            s.Date == DateTime.Now.Date &&
                            limitSpan >= s.StartTime &&
                            limitSpan <= s.EndTime
                        )
                       .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int sessionId)
        {
            var result = await DeleteByIdAsync<Session>(sessionId);
            return result > 0;
        }

        public async Task<Session> GetByIdAsync(int sessionId)
        {
            var data = await GetByIdAsync<Session>(sessionId);
            return data;
        }
        public async Task<IEnumerable<Student>> GetStudentsByCourseIdAsync(int courseId)
        {
            var students = await Context.BookedCourse
                    .Where(bc => bc.CourseId == courseId)
                    .Select(bc => bc.Student)
                    .ToListAsync();
            return students;
        }

        public async Task<bool> UpdateReminderEmailAsync(int sessionId)
        {
            try
            {
                var course = await Context.Session
                    .FirstOrDefaultAsync(s => s.Id == sessionId);

                course.ReminderEmail = true;

                var result = await base.UpdateAsync(course);
                return result > 0;
            }
            catch (Exception e)
            {
                base.Logger.Error(e, "Errore di imprevisto");
                return false;
            }
        }
    }
}
