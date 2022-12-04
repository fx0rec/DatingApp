using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


//===Middleware===
// ORDER IS ALWAYS IMPORTANT!

var app = builder.Build();

//Runs on top of all the other middleware and handles all exceptions, as if we write everything in a try{} catch{}
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP(S) request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// Asks whether user has a valid token.
app.UseAuthentication();
// What Authorization the authenticated user has. 
app.UseAuthorization();

app.MapControllers();


//SEEDING OUR DATA in code, without using ef database update 
using var scope = app.Services.CreateScope(); //Gives us access to all services within program.cs
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");   
}

app.Run();

