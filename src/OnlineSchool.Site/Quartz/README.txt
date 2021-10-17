SOURCE: https://andrewlock.net/creating-a-quartz-net-hosted-service-with-asp-net-core/

Instructions:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
1. Create the jobs inside 'Jobs' folder
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    EXAMPLE:
    public class HelloWorldJob : IJob 
    {
        public HelloWorldJob()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            ... code here ...
        }
    }

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
2. Register the jobs in 'Registration' class
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    services.AddSingleton<HelloWorldJob>();
    services.AddSingleton(new JobSchedule(
        jobType: typeof(HelloWorldJob),
        cronExpression: "0/5 * * * * ?")
    );

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
3. In Startup.cs, finish the setup by calling the extension method: 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    services.AddQuartzService();