using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext para MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)) // Ajuste para a versão do seu MySQL
    ));

// Adiciona os serviços de controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()  // Permite todas as origens
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});
var app = builder.Build(); 

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins); 
app.MapControllers();

app.Run();
