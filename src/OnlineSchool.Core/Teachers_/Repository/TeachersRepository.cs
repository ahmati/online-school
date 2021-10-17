using Dapper;
using DapperExtensions;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Contract.Material;
using OnlineSchool.Contract.SocialNetworks;
using OnlineSchool.Contract.Teachers;
using OnlineSchool.Domain.OnlineSchoolDB;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace OnlineSchool.Core.Teachers_.Repository
{
    public class TeachersRepository : OnlineSchoolBaseRepository, ITeachersRepository
    {
        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT * FROM Teacher ");
                var entities = await Connection.QueryAsync<Teacher>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Teacher> GetByIdAsync(int id) 
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine($"SELECT * FROM Teacher WHERE Id={id}");
                var entities = await Connection.QueryFirstOrDefaultAsync<Teacher>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {

                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<Teacher> GetByEmailAsync(string email)
        {
            var data = await Context.Teacher.FirstOrDefaultAsync(t => t.Email == email);
            return data;
        }

        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            try
            {
                return await base.AddAsync(teacher);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Teacher teacher)
        {
            try
            {
                var result = await base.UpdateAsync(teacher);
                return result > 0 ;
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

                await DeleteByIdAsync<Teacher>(id);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TeacherSubject>> GetTeacherSubjectsAsync(int teacherId)
        {
            try
            {
                var entities = await Context.TeacherSubject
                    .Include(t => t.Teacher)
                    .Include(t => t.Subject)
                    .Where(t => t.TeacherId == teacherId)
                    .ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        #region Teacher - Course

        public async Task<IEnumerable<Course>> GetTeacherCoursesAsync(int teacherId)
        {
            try
            {
                var entities = await Context.Course
                    .Include(c => c.TeacherSubject.Subject)
                    .Where(c => c.TeacherSubject.TeacherId == teacherId)
                    .ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        #endregion

        #region Teacher - Session

        public async Task<IEnumerable<Session>> GetUpcomingSessionsAsync(int teacherId)
        {
            try
            {
                var timeLimit = DateTime.Now.AddMinutes(10).TimeOfDay;

                var entities = await Context.Session
                    .Include(s => s.Course)
                        .ThenInclude(c => c.TeacherSubject)
                            .ThenInclude(ts => ts.Subject)
                    .Where(s => 
                        s.Course.TeacherSubject.TeacherId == teacherId && 
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

        public async Task<Session> GetSessionByIdAsync(int teacherId, int sessionId)
        {
            return await Context.Session.FirstOrDefaultAsync(s => 
                s.Id == sessionId && 
                s.Course.TeacherSubject.TeacherId == teacherId &&
                s.Course.IsPublished == true
            );
        }

        #endregion

        #region Teacher - TeacherSocialNetwork

        public async Task<IEnumerable<TeacherSocialNetwork>> GetTeacherSocialNetworksAsync(int teacherId)
        {
            try
            {
                var data = await Context.TeacherSocialNetwork
                    .Include(ts => ts.SocialNetwork)
                    .Where(ts => ts.TeacherId == teacherId)
                    .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSocialNetwork> GetTeacherSocialNetworkByIdAsync(int teacherId, int socialNetworkId)
        {
            try
            {
                var teacherSocial = await Context.TeacherSocialNetwork
                    .Include(ts => ts.SocialNetwork)
                    .FirstOrDefaultAsync(a => a.TeacherId == teacherId && a.SocialNetworkId == socialNetworkId);
                return teacherSocial;
            }
            catch (Exception e)
            {

                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<TeacherSocialNetwork> CreateTeacherSocialNetworkAsync(TeacherSocialNetwork teacherSocial)
        {
            try
            {
                var result = await AddAsync(teacherSocial);

                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateTeacherSocialNetworkAsync(TeacherSocialNetwork teacherSocial)
        {
            try
            {
                var result = await base.UpdateAsync(teacherSocial);
                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }
        
        public async Task<bool> DeleteTeacherSocialNetworkAsync(int teacherId, int socialNetworkId)
        {
            try
            {
                var prms = new DynamicParameters();
                prms.Add("@TeacherId", teacherId);
                prms.Add("@SocialNetworkId", socialNetworkId);

                var query = "DELETE FROM TeacherSocialNetwork WHERE TeacherId = @TeacherId AND SocialNetworkId = @SocialNetworkId";
                var result = await Context.Connection.ExecuteAsync(query, prms);

                return result > 0;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Errore di imprevisto", ex.Message);
                return false;
            }
        }

        #endregion

        #region Old methods

        public async Task<TeacherModel> GetTeacherProfileById(int id)
        {
            try
            {
                var teachers = await (from t in Context.Teacher.Where(t => t.Id == id)
                                      join ts in Context.TeacherSubject
                                      on t.Id equals ts.TeacherId
                                      join s in Context.Subject
                                      on ts.SubjectId equals s.Id
                                      //join tsn in Context.TeacherSocialNetwork
                                      //on t.Id equals tsn.TeacherId
                                      //join sn in Context.SocialNetworks
                                      //on tsn.SocialNetworkId equals sn.Id

                                      select new TeacherModel
                                      {
                                          Name = t.Name,
                                          Surname = t.Surname,
                                          Description = t.Description,
                                          ImagePath = t.ImagePath
                                      }).ToListAsync();
                return teachers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TeacherModel>> GetAllTeachersAsync()
        {
            try
            {
                var list = new List<TeacherModel>();
                var teachers = await Context.Teacher
                    .Include(t => t.TeacherSocialNetworks)
                        .ThenInclude(t => t.SocialNetwork)
                    .ToListAsync();

                foreach (var item in teachers)
                {
                    list.Add(new TeacherModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Surname = item.Surname,
                        Description = item.Description,
                        ImagePath = item.ImagePath,
                        AuthDate = item.AuthDate,
                        TeacherSocialNetwork = item.TeacherSocialNetworks.Select(s => new TeacherSocialNetworkModel
                        {
                            Link = s.Link,
                            SocialNetworkId = s.SocialNetworkId,
                            TeacherId = s.TeacherId,
                            SocialNetwork = new SocialNetworkModel
                            {
                                Deleted = s.SocialNetwork.Deleted,
                                Description = s.SocialNetwork.Description,
                                IconPath = s.SocialNetwork.IconPath,
                                Id = s.SocialNetwork.Id
                            }
                        }).ToList()
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }
        
        public async Task<IEnumerable<TeacherModel>> GetTeacherAsync(int Id)
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT Subjects.*, b.Semester,b.StartDate,b.EndDate,b.Year,c.DayOfTheWeek,c.StartTime,c.EndTime ");
                sQuery.AppendLine("FROM Subject v ");
                sQuery.AppendLine("LEFT JOIN TeacherSubject a ON v.Id = a.SubjectId ");
                sQuery.AppendLine("LEFT JOIN SubjectTerm b ON b.TeacherSubjectId =TeacherSubject.Id ");
                sQuery.AppendLine("LEFT JOIN SubjectTime c ON b.Id =c.SubjectTermId ");
                sQuery.AppendLine("LEFT JOIN SubjectLocation d ON c.Id=d.SubjectTimeId");
                sQuery.AppendLine($"WHERE a.TeacherId={Id}");

                var entities = await Connection.QueryAsync<TeacherModel>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Errore di imprevisto");
                return null;
            }
        }

        public async Task<MaterialModel> CreateCommentAsync(MaterialModel model, string name, string body, int id, string teacherName)
        {
            // var getId = await GetTeacherById(id);
            var prms = new DynamicParameters();
            //prms.Add("@Name", model.TeacherName);
            //prms.Add("@Body", model.Body);
            prms.Add("@Name", name);
            prms.Add("@Message", body);
            //  prms.Add("@TeacherId", getId);
            //prms.Add("@TeacherName", teacherName);
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("INSERT INTO Comment (Name,Body) Values (@Name, @Message)");

                var entities = await Connection.ExecuteScalarAsync<MaterialModel>(sQuery.ToString(), prms);
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeacherById(int id)
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT * FROM Teacher ");
                sQuery.AppendLine($"WHERE Id={id}");

                var entities = await Connection.QueryAsync<TeacherModel>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                Logger.Error(e, "eErrore di imprevisto");
                return null;
            }
        }
       
        public async Task<IEnumerable<MaterialModel>> GetTeacherComment()
        {
            try
            {
                var sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT Name as CommentName, Body FROM Comment ");

                var entities = await Connection.QueryAsync<MaterialModel>(sQuery.ToString());
                return entities;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
