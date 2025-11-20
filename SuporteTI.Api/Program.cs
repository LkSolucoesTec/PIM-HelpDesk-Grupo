using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- TODA A AUTENTICAÇÃO E JWT FORAM REMOVIDOS ---

builder.Services.AddAuthorization(); // Deixamos apenas isso

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// O TokenService FOI REMOVIDO
builder.Services.AddScoped<ChatbotService>();
builder.Services.AddSingleton(x =>
    new OpenAIClient(builder.Configuration["OpenAI:ApiKey"]));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
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

// app.UseHttpsRedirection(); // <-- Comentado
app.UseCors("AllowAll");
// app.UseAuthentication(); // <-- REMOVIDO
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();