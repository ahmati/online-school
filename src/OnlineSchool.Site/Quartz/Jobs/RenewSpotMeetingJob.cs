using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Core.SpotMeeting_.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Quartz.Jobs
{
    public class RenewSpotMeetingJob : IJob
    {
        private readonly ILogger<RenewSpotMeetingJob> _logger;
        private ISpotMeetingService SpotMeetingService { get; set; }
        public IMapper Mapper { get; set; }

        public RenewSpotMeetingJob(ILogger<RenewSpotMeetingJob> logger, ISpotMeetingService spotMeetingService, IMapper mapper)
        {
            _logger = logger;
            SpotMeetingService = spotMeetingService;
            Mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            RenewRecursiveSpotMeeting();
            return Task.CompletedTask;
        }


        private async void RenewRecursiveSpotMeeting()
        {
            try
            {
                var recursiveSpotMeetings = await SpotMeetingService.GetAllRecursiveAsync();
                foreach (var spotMeeting in recursiveSpotMeetings)
                {
                    if (spotMeeting.Status == SpotMeetingStatus.Finished)
                    {
                        spotMeeting.StartDate = spotMeeting.StartDate.AddDays(7);
                        await SpotMeetingService.RenewSpotMeetingAsync(Mapper.Map<UpdateSpotMeetingModel>(spotMeeting));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
