using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Services;
using System.Text;
using SAPelearning_bakend.Repositories.Services;
using SWD.SAPelearning.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    // Add any other JSON serialization settings as needed
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<SAPelearningAPIContext>(op =>
   op.UseSqlServer(builder.Configuration.GetConnectionString("SAPelearning")));

builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Dependency Injection for Services
builder.Services.AddScoped<IAuth, SAuth>();
builder.Services.AddScoped<ICertificate, SCertificate>();
builder.Services.AddScoped<ICertificateQuestion, SCertificateQuestion>();
builder.Services.AddScoped<ICertificateSampletest, SCertificateSampletest>();
builder.Services.AddScoped<ICertificateTestAttempt, SCertificateTestAttempt>();
builder.Services.AddScoped<ICertificateTestQuestion, SCertificateTestQuestion>();
builder.Services.AddScoped<ICourse, SCourse>();
builder.Services.AddScoped<ICourseMaterial, SCourseMaterial>();
builder.Services.AddScoped<ICourseSession, SCourseSession>();
builder.Services.AddScoped<IEnrollment, SEnrollment>();
builder.Services.AddScoped<IInstructor, SInstructor>();
builder.Services.AddScoped<IPayment, SPayment>();
builder.Services.AddScoped<ISapModule, SSapModule>();
builder.Services.AddScoped<ITopicArea, STopicArea>();
builder.Services.AddScoped<IUser, SUser>();

// Swagger Configuration
builder.Services.AddSwaggerGen(option =>
{
    // JWT Token Authorization setup
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Google Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];
    options.CallbackPath = "/signin-google";
});

// Enable Swagger only in Development and Production environments
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/AWS/swagger.json", "AWSApi v1"));
}

app.UseHttpsRedirection();

app.UseCors("MyCors");

app.UseAuthentication();  // Authentication middleware

app.UseAuthorization();   // Authorization middleware

app.MapControllers();

app.Run();
