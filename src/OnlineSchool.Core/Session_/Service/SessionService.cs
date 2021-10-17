using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Session;
using OnlineSchool.Contract.Students;
using OnlineSchool.Core.Session_.Repository;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Session_.Service
{
    public class SessionService : CoreBase, ISessionService
    {
        public ISessionRepository SessionRepository { get; set; }

        public async Task<IEnumerable<SessionModel>> GetSessionsThatNeedRemindingAsync()
        {
            try
            {
                var sessions = await SessionRepository.GetSessionsThatNeedRemindingAsync();

                return Mapper.Map<IEnumerable<SessionModel>>(sessions);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }
        public async Task<SessionModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await SessionRepository.GetByIdAsync(id);
                return Mapper.Map<SessionModel>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Session could not be deleted.";
            try
            {
                var session = await SessionRepository.GetByIdAsync(id);
                if (session is null)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                var deleted = await SessionRepository.DeleteAsync(id);
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

        public async Task<IEnumerable<StudentModel>> GetStudentsByCourseIdAsync(int courseId)
        {
            try
            {
                var data = await SessionRepository.GetStudentsByCourseIdAsync(courseId);
                return Mapper.Map<IEnumerable<StudentModel>>(data);

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<bool>> UpdateReminderEmailAsync(int sessionId)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var result = await SessionRepository.UpdateReminderEmailAsync(sessionId);

                output.Output = result;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }

        }

    }
}
