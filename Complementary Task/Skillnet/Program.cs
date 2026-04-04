using Microsoft.EntityFrameworkCore;
using SkillNet.Repositories.Contexts;
using SkillNet.Repositories.Contracts;
using SkillNet.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Base de datos
builder.Services.AddDbContext<SkillNet_Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorio
builder.Services.AddScoped<IUser_Repository, User_Repository>();

// 1. Aquí definimos la política como "AllowReact"
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReact", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. ¡OJO AQUÍ!: El nombre debe coincidir exactamente con el de arriba
app.UseCors("AllowReact");

app.MapControllers();
app.Run();