

using DownTrack.Application;
using DownTrack.Infrastructure;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var services = builder.Services;

services.AddHttpContextAccessor();
services.AddAplication(builder.Configuration);

services.AddDefaultIdentity<IdentityUser>()
        .AddEntityFrameworkStores<DownTrackContext>();


services.AddInfrastructure(builder.Configuration);

// configurate cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin() // permite solicitudes desde cualquier origen
               .AllowAnyHeader() // permite encabezados HTTP
               .AllowAnyMethod(); // permite cualquier metodo HTTP (GET,POST,PUT,DELETE)
    });
});


// nueva configuracion de cors

// services.AddCors(options =>
// {
//     options.AddPolicy("LocalhostPolicy", builder =>
//     {
//         builder.WithOrigins("http://localhost:5173/") // Permite solo este origen
//                .AllowAnyHeader() // Permite cualquier encabezado, como Authorization o Content-Type
//                .AllowAnyMethod(); // Permite cualquier método HTTP, como GET, POST, PUT, DELETE
//     });
// });


var app = builder.Build();



// app.UseCors("LocalhostPolicy");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //await app.InitializeDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();