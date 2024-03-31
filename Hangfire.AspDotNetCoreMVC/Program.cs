using Hangfire;
using Hangfire.AspDotNetCoreMVC.Services;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true
       }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseHangfireDashboard("/dashboard");
//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
//    IsReadOnlyFunc = (DashboardContext context) => true //read-only dashboard view prevents users from changing anything, such as deleting or enqueueing jobs
//});

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = "Admin",
                                PasswordClear = "123"

                            }
                        }
                    }) },
    IsReadOnlyFunc = (DashboardContext context) => false, //read-only dashboard view prevents users from changing anything, such as deleting or enqueueing jobs
    IgnoreAntiforgeryToken = true // This line ensures that Hangfire ignores the antiforgery token, useful when using basic authentication
});

app.UseRouting();

app.UseAuthorization();

RecurringJobOptions DashboardOptions = new RecurringJobOptions()
{
    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time")
};
//RecurringJob.AddOrUpdate<IUploadExcel>("FTP Scheduler Job", x => Console.WriteLine("Hello, world!"), Cron.Daily(1), DashboardOptions);
RecurringJob.AddOrUpdate("jobId", () => Console.WriteLine("Welcome user in Recurring Job Demo!"), Cron.Daily);
BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
