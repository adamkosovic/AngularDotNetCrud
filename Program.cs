
namespace TestPraktik;


using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using TestPraktik.Models;
using TestPraktik.Dtos;
using TestPraktik.Data;
using BookApp.Services;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
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
        });

        builder.Services.AddDbContext<BookDbContext>(
            options => options.UseNpgsql("Host=localhost;Database=bookappdb;Username=postgres;Password=password"
            )
        );

        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder
            .Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<BookDbContext>()
            .AddApiEndpoints();
        builder.Services.AddControllers();
        builder.Services.AddScoped<BookService>();

        var app = builder.Build();

        app.UseCors("AllowFrontend");

        app.MapIdentityApi<User>();
        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseDefaultFiles(); 
        app.UseStaticFiles();  

        app.MapFallbackToFile("index.html");


        app.Run();
    }
}

