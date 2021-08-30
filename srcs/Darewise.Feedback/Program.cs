using System;
using System.Text;
using System.Threading.Tasks;
using Darewise.Feedback.Controllers;
using Darewise.Feedback.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Feedback", Version = "v1" }); 
    
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

string? jwtAuth = builder.Configuration.GetSection("Authentication").GetValue<string>("JwtPrivateKey");

builder.Services.AddAuthentication(s =>
{
    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(s =>
{
    s.RequireHttpsMetadata = false;
    s.SaveToken = true;
    s.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuth)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
    };
});


builder.Services.Configure<AttachmentUploadOptions>(builder.Configuration);
builder.Services.AddSingleton<JwtSecurityOptions>(new JwtSecurityOptions() { JwtPrivateKey = jwtAuth });
builder.Services.AddSingleton(typeof(IMapper<,>), typeof(GenericMapsterMapper<,>));


// I would have preferred InMemoryDatabase but EFCore.InMemory does not support SQL & NoSQL inline
builder.Services.AddDbContextFactory<FeedbackDbContext>(options =>
{
    options.UseNpgsql($"Host=localhost;Database=feedback-api;Username=postgres;Password=strong_pass2018");
});
builder.Services.AddLogging();
builder.Services.AddSingleton<IFeedbackRepository, EfCoreFeedbackRepository>();



WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();