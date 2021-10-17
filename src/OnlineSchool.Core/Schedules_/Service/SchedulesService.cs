using ItalWebConsulting.Infrastructure.BusinessLogic;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Lessons;
using OnlineSchool.Contract.Scheduler;
using OnlineSchool.Core.Lessons_.Repository;
using OnlineSchool.Core.Schedules_.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Schedules_.Service
{
    public class SchedulesService : CoreBase, IScheduleService
    {
        public ISchedulesRepository ScheduleRepository { get; set; }
        public ILessonRepository LessonRepository { get; set; }
    }
}
