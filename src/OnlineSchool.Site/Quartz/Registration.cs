using Microsoft.Extensions.DependencyInjection;
using OnlineSchool.Site.Quartz.Jobs;
using OnlineSchool.Site.Quartz.Setup;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace OnlineSchool.Site.Quartz
{
    public static class Registration
    {
        public static void AddQuartzService(this IServiceCollection services)
        {
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            #region ------------------------ Jobs registration ------------------------

            services.AddSingleton<ClearTempFilesJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(ClearTempFilesJob),
                cronExpression: "0 0 0 1/1 * ? *"));

            services.AddSingleton<RenewSpotMeetingJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RenewSpotMeetingJob),
                cronExpression: "0 0 0 * * ?"));
            
            services.AddSingleton<MeetingReminderJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(MeetingReminderJob),
                cronExpression: "0 0 * ? * * *"));

            #endregion

            services.AddHostedService<QuartzHostedService>();
        }
    }
}
