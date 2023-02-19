using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using umlaut;

HHruAPI api = new HHruAPI();

var hrefs = await api.GetProfileHrefs();


foreach (var href in hrefs)
{
    Console.WriteLine(href);
}
Console.WriteLine(hrefs.Count());



//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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
