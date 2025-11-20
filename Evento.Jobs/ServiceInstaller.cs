using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Evento.Jobs;

public static class ServiceInstaller
{
    public static IServiceCollection AddQuartzJobs(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("UnpaidBookingResetJob");

            q.AddJob<UnpaidBookingResetJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("UnpaidBookingResetTrigger")
                .WithCronSchedule("0 0 9 ? * MON")); 
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
} 