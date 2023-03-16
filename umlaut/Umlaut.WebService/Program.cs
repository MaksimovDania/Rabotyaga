using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using Quartz;
using umlaut;
using umlaut.GraduateDBUpdaterBGService;
using Umlaut.Database;
using Umlaut.Database.Repositories.FacultyRepository;
using Umlaut.Database.Repositories.GraduateRepository;
using Umlaut.Database.Repositories.LocationRepository;
using Umlaut.Database.Repositories.SpecializationRepository;
//HHruAPI api = new();


//var g = await api.GetGraduate("60d28e9e0004445b6e0039ed1f36324c766f6b");
//UmlautDBContext context = new();
//var a = context.Faculties.Add(g.Faculty);
//context.SaveChanges();
//Console.WriteLine(a);

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionString = builder.Configuration.GetConnectionString("PostgresGraduate");
builder.Services.AddDbContext<UmlautDBContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<HHruAPI>();
builder.Services.AddTransient<DBUpdateJob>();
builder.Services.AddTransient<IFacultyRepository, FacultyRepository>();
builder.Services.AddTransient<IGraduateRepository, GraduateRepository>();
builder.Services.AddTransient<ILocationRepository, LocationRepositopy>();
builder.Services.AddTransient<ISpecializationRepository, SpecializationRepositopy>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.ScheduleJob<DBUpdateJob>(trigger => trigger
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(100)
                .RepeatForever())
        );
});

builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
