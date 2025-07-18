namespace TestPraktik;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using TestPraktik.Models;
using TestPraktik.Dtos;
using TestPraktik.Data;
using TestPraktik.Services;

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
            connectionString = "Host=localhost;Database=bookappdb;Username=postgres;Password=password";
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
        });

        builder.Services.AddControllers();
        builder.Services.AddScoped<BookService>();

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
        app.MapGet("/health", () => "OK");

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

        app.MapPost("/register", async (UserManager<User> userManager, RegisterRequest req) =>
        {
            try
            {
                Console.WriteLine($"Registering user: {req.Email}");

                var user = new User
                {
                    Email = req.Email,
                    UserName = req.Email
                };

                var result = await userManager.CreateAsync(user, req.Password);
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Registration failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    return Results.BadRequest(result.Errors);
                }

                Console.WriteLine($"User registered successfully: {req.Email}");
                return Results.Ok(new { message = "User registered successfully", email = req.Email });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in register: {ex.Message}");
                return Results.Problem(ex.Message);
            }
        })
        .RequireCors("AllowFrontend");

        app.MapPost("/login", async (SignInManager<User> signInManager, UserManager<User> userManager, LoginRequest req) =>
        {
            try
            {
                Console.WriteLine($"Login attempt for: {req.Email}");

                var result = await signInManager.PasswordSignInAsync(req.Email, req.Password, false, false);
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Login failed for: {req.Email}");
                    return Results.BadRequest("Invalid email or password");
                }

                // Get the user
                var user = await userManager.FindByEmailAsync(req.Email);
                if (user == null)
                {
                    return Results.BadRequest("User not found");
                }

                Console.WriteLine($"Login successful for: {req.Email}");
                return Results.Ok(new
                {
                    message = "Login successful",
                    email = req.Email,
                    userId = user.Id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in login: {ex.Message}");
                return Results.Problem(ex.Message);
            }
        })
        .RequireCors("AllowFrontend");

        // Remove static file serving since frontend is on Netlify
        // app.UseDefaultFiles();
        // app.UseStaticFiles();
        // app.MapFallbackToFile("index.html");

        app.Run();
    }
}
