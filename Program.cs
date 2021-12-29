using Microsoft.EntityFrameworkCore;
using courses_platform.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header sing the Bearer Scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = ApiVersion.Default;
});

var serverVersion = new MySqlServerVersion(new Version(10, 4));
builder.Services.AddDbContext<CoursesDbContext>(options => options.UseMySql(
    serverVersion: serverVersion,
    connectionString: builder.Configuration.GetConnectionString("Default")
));
builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        // options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 7;
        // options.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<CoursesDbContext>();

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SignKey"])),
            ValidateIssuerSigningKey = true,
        };
    });

// builder.Services.AddTransient<IEmailSender, EmailSender>();



// builder.Services.AddApiVersioning();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetService<CoursesDbContext>()?.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    string path = context.Request.Path.Value;

    Console.WriteLine(path);

    if (
        string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
        (app.Environment.IsDevelopment() &&
        string.Equals(path, "/swagger", StringComparison.OrdinalIgnoreCase)))
    {
        // The request token can be sent as a JavaScript-readable cookie, 
        // and Angular uses it by default.
        var tokens = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            new CookieOptions() { HttpOnly = false });
    }

    return next(context);

});

app.MapControllers();

app.Run();
