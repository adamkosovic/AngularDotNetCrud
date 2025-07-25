namespace TestPraktik;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
                        "https://serene-pudding-2024c3.netlify.app",
                        "https://lustrous-bunny-35c900.netlify.app",
                        "https://serene-pudding-2024c3.netlify.app",
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
                // Use migrations instead of EnsureCreatedAsync
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database migration failed: {ex.Message}");
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

        // Manual fix for quotes table
        app.MapGet("/fix-quotes-table", async (BookDbContext dbContext) =>
        {
            try
            {
                var connection = dbContext.Database.GetDbConnection();
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    DROP TABLE IF EXISTS ""quotes"" CASCADE;
                    
                    CREATE TABLE ""quotes"" (
                        ""id"" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
                        ""text"" text NOT NULL,
                        ""Author "" text NOT NULL,
                        ""createdat"" date NOT NULL,
                        ""userid"" text NOT NULL,
                        CONSTRAINT ""FK_quotes_AspNetUsers_userid"" FOREIGN KEY (""userid"") REFERENCES ""AspNetUsers"" (""Id"") ON DELETE CASCADE
                    );
                    
                    CREATE INDEX ""IX_quotes_userid"" ON ""quotes"" (""userid"");
                ";

                await command.ExecuteNonQueryAsync();

                return "Quotes table fixed successfully!";
            }
            catch (Exception ex)
            {
                return $"Failed to fix quotes table: {ex.Message}";
            }
        });

        // Test quotes table structure
        app.MapGet("/test-quotes", async (BookDbContext dbContext) =>
        {
            try
            {
                var connection = dbContext.Database.GetDbConnection();
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT column_name, data_type 
                    FROM information_schema.columns 
                    WHERE table_name = 'quotes' 
                    ORDER BY ordinal_position";

                var columns = new List<object>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    columns.Add(new
                    {
                        column = reader.GetString(0),
                        type = reader.GetString(1)
                    });
                }

                return Results.Json(new { columns, message = "Quotes table structure" });
            }
            catch (Exception ex)
            {
                return Results.Json(new { error = ex.Message, message = "Failed to get table structure" });
            }
        });

        // Debug endpoint to test database connectivity
        app.MapGet("/debug-db", async (BookDbContext dbContext) =>
        {
            try
            {
                // Test if we can connect to the database
                var canConnect = await dbContext.Database.CanConnectAsync();

                // Check if tables exist
                var tables = new List<string>();
                if (canConnect)
                {
                    var connection = dbContext.Database.GetDbConnection();
                    await connection.OpenAsync();
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT table_name 
                        FROM information_schema.tables 
                        WHERE table_schema = 'public' 
                        ORDER BY table_name";

                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }

                return Results.Json(new
                {
                    canConnect,
                    tables,
                    message = "Database debug info"
                });
            }
            catch (Exception ex)
            {
                return Results.Json(new
                {
                    canConnect = false,
                    error = ex.Message,
                    message = "Database debug failed"
                });
            }
        });

        // Initialize database
        app.MapGet("/init-db", async (BookDbContext dbContext) =>
        {
            try
            {
                await dbContext.Database.MigrateAsync();
                return "Database migrations applied successfully";
            }
            catch (Exception ex)
            {
                return $"Database migration failed: {ex.Message}";
            }
        });

        // Remove static file serving since frontend is on Netlify
        // app.UseDefaultFiles();
        // app.UseStaticFiles();
        // app.MapFallbackToFile("index.html");

        app.Run();
    }
}
