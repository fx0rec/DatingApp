using API.Extensions;
using API.Middleware;

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

app.Run();

