using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagement_V2.Application.Common.Auth;
using ProductManagement_V2.Application.Common.Behaviors;
using ProductManagement_V2.Application.Common.Constants;
using ProductManagement_V2.Domain.Entities;
using ProductManagement_V2.Infrastructuree.Interceptors;
using ProductManagement_V2.Infrastructuree.Security;
using ProductManagement_V2.Infrastructuree.Seeders;
using System.Text;
using Microsoft.OpenApi.Models;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Infrastructuree.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .SelectMany(entry => entry.Value!.Errors.Select(error =>
                string.IsNullOrWhiteSpace(entry.Key)
                    ? error.ErrorMessage
                    : $"{entry.Key}: {error.ErrorMessage}"))
            .Distinct()
            .ToList();

        var response = ApiResponse<object>.FailResponse(
            errors.FirstOrDefault() ?? "Validation error",
            StatusCodes.Status400BadRequest,
            errors);

        return new OkObjectResult(response);
    };
});
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductManagement API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
                     ?? throw new InvalidOperationException("JWT settings are not configured.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.FailResponse(
                "Your session has expired. Please login again.",
                StatusCodes.Status401Unauthorized);

            await context.Response.WriteAsJsonAsync(response);
        },
        OnForbidden = async context =>
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.FailResponse(
                "Access denied. You do not have permission.",
                StatusCodes.Status403Forbidden);

            await context.Response.WriteAsJsonAsync(response);
        }
    };
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProductsView", policy =>
        policy.RequireClaim("permission", AppClaims.ProductsView));

    options.AddPolicy("ProductsCreate", policy =>
        policy.RequireClaim("permission", AppClaims.ProductsCreate));

    options.AddPolicy("ProductsDelete", policy =>
        policy.RequireClaim("permission", AppClaims.ProductsDelete));

    options.AddPolicy("ProductsChangeStatus", policy =>
        policy.RequireClaim("permission", AppClaims.ProductsChangeStatus));

    options.AddPolicy("ProductStatusHistoriesView", policy =>
    policy.RequireClaim("permission", AppClaims.ProductStatusHistoriesView));

    options.AddPolicy("UsersView", policy =>
        policy.RequireClaim("permission", AppClaims.UsersView));

    options.AddPolicy("UsersCreate", policy =>
        policy.RequireClaim("permission", AppClaims.UsersCreate));

    options.AddPolicy("RolesView", policy =>
        policy.RequireClaim("permission", AppClaims.RolesView));

    options.AddPolicy("StatisticsView", policy =>
        policy.RequireClaim("permission", AppClaims.StatisticsView));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", policy =>
    {
        policy
            .WithOrigins("http://20.20.1.158:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

    


    var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

// Must wrap the pipeline before endpoint execution so unhandled exceptions use the API result shape.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("FrontendCors");

// Binds ApiResponse.StatusCode from result responses to the actual HTTP response code.
app.UseMiddleware<ResultStatusCodeMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    await RolesAndClaimsSeeder.SeedAsync(scope.ServiceProvider);
    await AdminUserSeeder.SeedAsync(scope.ServiceProvider);
}


app.Run();
