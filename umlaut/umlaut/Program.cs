using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using umlaut;

//HHruAPI api = new HHruAPI();


//var g = await api.GetGraduate("bb5b392d0003daf91e009253956e434d373238");


//var list = await api.GetProfileHrefs();

//foreach (var item in list)
//{
//    Console.WriteLine(item);
//}



var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddD
var app = builder.Build();

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
