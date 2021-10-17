using Microsoft.EntityFrameworkCore;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SpotMeeting_.Repository
{
    public class SpotMeetingRepository : OnlineSchoolBaseRepository, ISpotMeetingRepository
    {
        public async Task<SpotMeeting> GetByIdAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeeting
                    .Include(m => m.SpotMeetingMaterials)
                        .ThenInclude(m => m.Material)
                    .Include(m => m.SpotMeetingTeachers)
                        .ThenInclude(m => m.Teacher)
                    .Include(m => m.BookedSpotMeetings)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<SpotMeeting>> GetAllAsync()
        {
            try
            {
                var data = await Context.SpotMeeting
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeeting>> GetAllPublishedAsync()
        {
            try
            {
                var data = await Context.SpotMeeting
                    .Where(m => m.IsPublished == true)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeeting>> GetAllRecursiveAsync()
        {
            try
            {
                var data = await Context.SpotMeeting
                    .Where(m => m.IsRecursiveSpotMeeting == true)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<SpotMeetingTeacher> GetHostTeacherAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Include(smt => smt.Teacher)
                    .Where(smt => smt.SpotMeetingId == id && smt.IsHost == true)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<SpotMeeting>> GetSpotMeetingsThatNeedRemindingAsync()
        {
            try
            {
                var limitSpan = DateTime.Now.AddHours(3);

                var data = await Context.SpotMeeting
                    .Where(s => s.ReminderEmail == false &&
                             limitSpan >= s.StartDate &&
                             limitSpan <= s.AuthDate &&
                             s.StartDate.Date == DateTime.Now.Date
                    )
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<BookedSpotMeeting>> GetUsersBySpotMeetingAsync(int spotMeetingId)
        {
            try
            {
                var data = await Context.BookedSpotMeeting
                    .Where(s => s.SpotMeetingId == spotMeetingId)
                    .Include(s => s.SpotMeeting)
                    .Include(s => s.User)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<BookedSpotMeeting>> GetBookedSpotMeetingsBySpotMeetingIdAsync(int id)
        {
            try
            {
                var data = await Context.BookedSpotMeeting
                    .Where(bsm => bsm.SpotMeetingId == id)
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<SpotMeeting> CreateAsync(SpotMeeting spotMeeting)
        {
            try
            {
                return await AddAsync(spotMeeting);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateReminderEmailAsync(int spotMeetingId)
        {
            try
            {
                var course = await Context.SpotMeeting
                    .FirstOrDefaultAsync(s => s.Id == spotMeetingId);

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

        public async Task<BookedSpotMeeting> CreateBookedSpotMeetingAsync(BookedSpotMeeting bookedSpotMeeting)
        {
            try
            {
                return await AddAsync(bookedSpotMeeting);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(SpotMeeting spotMeeting)
        {
            try
            {
                var result = await base.UpdateAsync(spotMeeting);
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
            try
            {
                var result = await DeleteByIdAsync<SpotMeeting>(id);
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        #region ----------------------------------------- Material Actions -----------------------------------------
        public async Task<IEnumerable<SpotMeetingMaterial>> GetMaterialsAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeetingMaterial
                    .Include(m => m.Material)
                    .Where(m => m.SpotMeetingId == id)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<SpotMeetingMaterial> GetSpotMeetingMaterialByIdAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeetingMaterial
                    .Include(m => m.SpotMeeting)
                    .Include(m => m.Material)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }
        public async Task<IEnumerable<SpotMeetingMaterial>> GetSpotMeetingMaterialByIdsAsync(int[] ids)
        {
            try
            {
                var data = await Context.SpotMeetingMaterial
                    .Include(m => m.SpotMeeting)
                    .Include(m => m.Material)
                    .Where(d => ids.Contains(d.Id))
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<SpotMeetingMaterial> GetSpotMeetingMaterialByFileNameAsync(int spotMeetingId, string fileName)
        {
            try
            {
                var data = await Context.SpotMeetingMaterial
                    .Include(m => m.SpotMeeting)
                    .Include(m => m.Material)
                    .Where(m => m.SpotMeetingId == spotMeetingId && m.Material.FileName == fileName)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<SpotMeetingMaterial> CreateSpotMeetingMaterialAsync(SpotMeetingMaterial spotMeetingMaterial)
        {
            try
            {
                return await AddAsync(spotMeetingMaterial);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteSpotMeetingMaterialAsync(int id)
        {
            try
            {
                var entity = await Context.SpotMeetingMaterial.FirstOrDefaultAsync(t => t.Id == id);
                Context.SpotMeetingMaterial.Remove(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }
        #endregion

        #region ----------------------------------------- Teacher Actions -----------------------------------------
        public async Task<IEnumerable<SpotMeetingTeacher>> GetTeachersAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Include(m => m.Teacher)
                    .Where(m => m.SpotMeetingId == id)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<int[]> GetSpotMeetingIdsByTeacherEmailAsync(string email)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Where(m => m.Teacher.Email == email)
                    .ToListAsync();

                return data.Select(d => d.SpotMeetingId).ToArray();
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<SpotMeeting>> GetTeacherSpotMeetingsAsync(int[] spotmeetingIds)
        {
            try
            {
                var data = await Context.SpotMeeting
                    .Include(m => m.SpotMeetingTeachers)
                    .Where(m => spotmeetingIds.Contains(m.Id))
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<SpotMeetingTeacher> GetSpotMeetingTeacherAsync(string email, int id)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Where(smt => smt.Teacher.Email == email && smt.SpotMeetingId == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<SpotMeetingTeacher> GetSpotMeetingTeacherByIdAsync(int id)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Where(smt => smt.TeacherId == id)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<SpotMeetingTeacher> GetSpotMeetingTeacherByIdsAsync(int spotMeetingId, int teacherId)
        {
            try
            {
                var data = await Context.SpotMeetingTeacher
                    .Where(smt => smt.TeacherId == teacherId && smt.SpotMeetingId == spotMeetingId)
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }
        public async Task<SpotMeetingTeacher> CreateSpotMeetingTeacherAsync(SpotMeetingTeacher spotMeetingTeacher)
        {
            try
            {
                return await AddAsync(spotMeetingTeacher);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }
        
        public async Task<bool> UpdateSpotMeetingTeacherAsync(SpotMeetingTeacher spotMeetingTeacher)
        {
            try
            {
                var result = await base.UpdateAsync(spotMeetingTeacher);
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> DeleteSpotMeetingTeacherAsync(int id)
        {
            try
            {
                var entity = await Context.SpotMeetingTeacher.FirstOrDefaultAsync(t => t.Id == id);
                Context.SpotMeetingTeacher.Remove(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        #endregion
    }
}
