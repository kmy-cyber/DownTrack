

using DownTrack.Api;
using DownTrack.Application;
using DownTrack.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var services = builder.Services;


services.AddPresentation();
services.AddAplication(builder.Configuration);
services.AddInfrastructure(builder.Configuration);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
//app.UseCors("LocalhostPolicy");
        
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();