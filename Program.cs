using Microsoft.EntityFrameworkCore;
using courses_platform.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var serverVersion = new MySqlServerVersion(new Version(10, 4));
builder.Services.AddDbContext<CoursesDbContext>(options => options.UseMySql(
                serverVersion: serverVersion,
                connectionString: builder.Configuration.GetConnectionString("Default")
            ));
builder.Services.AddMvc();
// builder.Services.AddApiVersioning();

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
