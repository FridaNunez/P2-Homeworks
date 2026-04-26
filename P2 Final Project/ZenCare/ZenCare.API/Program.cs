using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Infrastructure.Data;
using ZenCare.Infrastructure.UnitOfWork;

// Solución para DateTime con PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// ── PostgreSQL ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ZenCareDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ZenCareDb"),
        b => b.MigrationsAssembly("ZenCare.Infrastructure")));

// ── Unit of Work ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ── Business services ─────────────────────────────────────────────────────────
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ISpecialistService, SpecialistService>();
builder.Services.AddScoped<ITherapyService, TherapyService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ISessionNoteService, SessionNoteService>();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ZenCare API",
        Version = "v1",
        Description = "REST API — ZenCare Alternative Therapy Center"
    });
});

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Auto-apply migrations ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ZenCareDbContext>();
    db.Database.Migrate();
}

// ── Pipeline ──────────────────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZenCare API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();