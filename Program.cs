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
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
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

        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ??
            "Host=localhost;Database=bookappdb;Username=postgres;Password=password";

        builder.Services.AddDbContext<BookDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
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

        app.MapPost("/register", async (UserManager<User> userManager, RegisterRequest req) =>
        {
            var user = new User
            {
                Email = req.Email,
                UserName = req.Email
            };

            var result = await userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok();
        })
        .RequireCors("AllowFrontend");

        app.MapPost("/login", async (SignInManager<User> signInManager, LoginRequest req) =>
        {
            var result = await signInManager.PasswordSignInAsync(req.Email, req.Password, false, false);
            if (!result.Succeeded)
                return Results.BadRequest("Invalid email or password");

            return Results.Ok();
        })
        .RequireCors("AllowFrontend");

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
