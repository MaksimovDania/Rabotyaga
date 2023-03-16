using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Umlaut;
using Umlaut.WebService.DBUpdaterService;
using Umlaut.WebService.DBUpdaterService.DBUpdaters;
using Umlaut.Database;
using Umlaut.Database.Repositories.FacultyRepository;
using Umlaut.Database.Repositories.GraduateRepository;
using Umlaut.Database.Repositories.LocationRepository;
using Umlaut.Database.Repositories.SpecializationRepository;
using System.Net.WebSockets;
using Umlaut.Database.Models;

HHruAPI api = new();
string[] hrefs = { "83d9691d0008be50d80039ed1f393245654c52", "7e3e29e20007e3f8000039ed1f623870566f31", "286f89510006903e250039ed1f4d3676495745",
                   "cae385ed0000b01d3a0039ed1f736563726574", "262b52fc00012f570f0039ed1f4d4843744461", "32ae84c600044dabd40039ed1f4c67774b4c42",
                    "3ae92eea0005753bbd0039ed1f4e6b6a423739", "3e11071d00015152e10039ed1f4c474d4b764a" };

List<Graduate> gr = new();
foreach (var item in hrefs)
{
    gr.Add(await api.GetGraduate(item));
}
Console.WriteLine("aboba");

//var builder = WebApplication.CreateBuilder(args);
//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//var connectionString = builder.Configuration.GetConnectionString("PostgresGraduate");
//builder.Services.AddDbContext<UmlautDBContext>(options => options.UseNpgsql(connectionString));
//builder.Services.AddTransient<HHruAPI>();
//builder.Services.AddTransient<DBUpdateJob>();
//builder.Services.AddTransient<IFacultyRepository, FacultyRepository>();
//builder.Services.AddTransient<IGraduateRepository, GraduateRepository>();
//builder.Services.AddTransient<ILocationRepository, LocationRepositopy>();
//builder.Services.AddTransient<ISpecializationRepository, SpecializationRepositopy>();
//builder.Services.AddTransient<GraduateDBUpdater>();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();
//    q.ScheduleJob<DBUpdateJob>(trigger => trigger
//            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
//            .WithSimpleSchedule(x => x
//                .WithIntervalInHours(10)
//                .RepeatForever())
//        );
//});

//builder.Services.AddQuartzHostedService(options =>
//{
//    // when shutting down we want jobs to complete gracefully
//    options.WaitForJobsToComplete = true;
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
