namespace TestPraktik;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using TestPraktik.Models;
using TestPraktik.Dtos;
using TestPraktik.Data;
using TestPraktik.Services;
using System.Text.Json;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins(
                        "https://playful-crumble-968db4.netlify.app",
                        "https://peppy-pie-f8c8e8.netlify.app",
                        "https://ownbooklist.netlify.app",
                        "http://localhost:4200"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("create_book", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("remove_book", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("update_book", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("get_books", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("get_book", policy => policy.RequireAuthenticatedUser());
        });

        // Get database connection string from environment variables
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (string.IsNullOrEmpty(connectionString))
        {
            var currentUser = Environment.GetEnvironmentVariable("USER") ?? "adamkosovic";
            connectionString = $"Host=localhost;Database=bookappdb;Username={currentUser};Password=;";
        }
        else
        {
            // Railway provides DATABASE_URL in format: postgresql://user:pass@host:port/db
            // Convert to Entity Framework format
            var uri = new Uri(connectionString);
            connectionString = $"Host={uri.Host};Database={uri.LocalPath.TrimStart('/')};Username={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]};Port={uri.Port};SSL Mode=Require;Trust Server Certificate=true;";
        }

        Console.WriteLine($"Using connection string: {connectionString}");

        builder.Services.AddDbContext<BookDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        // Simplified authentication setup
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
        })
        .AddBearerToken(IdentityConstants.BearerScheme)
        .AddCookie(IdentityConstants.ApplicationScheme);

        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<BookDbContext>()
            .AddSignInManager()
            .AddApiEndpoints();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            // Relax password requirements for testing
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
        });

        builder.Services.AddControllers();
        builder.Services.AddScoped<BookService>();
        builder.Services.AddScoped<QuoteService>();
        
        var app = builder.Build();

        // Initialize database
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();
            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                Console.WriteLine("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    Console.WriteLine("EXCEPTION: " + error.Error.ToString());
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal server error" }));
                }
            });
        });

        app.UseRouting();

        app.UseCors("AllowFrontend");

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Map Identity API endpoints
        app.MapIdentityApi<User>();

        // Health check endpoint
        app.MapGet("/health", () => "OK - Updated for Railway deployment");

        // Test endpoint to verify basic functionality
        app.MapGet("/test", () => new { message = "API is working", timestamp = DateTime.UtcNow });

        // Initialize database
        app.MapGet("/init-db", async (BookDbContext dbContext) =>
        {
            try
            {
                await dbContext.Database.EnsureCreatedAsync();
                return "Database initialized successfully";
            }
            catch (Exception ex)
            {
                return $"Database initialization failed: {ex.Message}";
            }
        });

        // Remove static file serving since frontend is on Netlify
        // app.UseDefaultFiles();
        // app.UseStaticFiles();
        // app.MapFallbackToFile("index.html");

        app.Run();
    }
}
