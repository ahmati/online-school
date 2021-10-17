using OnlineSchool.Contract.Calendar;
using OnlineSchool.Contract.SpotMeeting;
using System.Threading.Tasks;

namespace OnlineSchool.Core.IcsCalendar.Service
{
    public interface IIcsCalendarService
    {
        Task<bool> SendIcsFile(IcsCalendarModel calendarModel);
    }
}