using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        !context.Request.Path.StartsWithSegments("/image"))
    {
        context.Response.Headers["Cache-Control"] = "public,max-age=60";
    }
    await next();
});

app.MapControllers();
app.Run();
