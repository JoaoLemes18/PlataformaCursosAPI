using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PlataformaCursosAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext para MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)) // Ajuste para sua versão MySQL
    ));

// Adiciona os serviços de controllers e Swagger
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Durante dev, especifique a URL do frontend
                          policy.WithOrigins("http://localhost:5173/")
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlataformaCursosAPI v1");
    });
}

app.UseHttpsRedirection();

// CORS precisa vir ANTES do UseStaticFiles e UseAuthorization
app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles();

var imagensPath = Path.Combine(builder.Environment.ContentRootPath, "imagens");
var materiaisPath = Path.Combine(builder.Environment.ContentRootPath, "materiais");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagensPath),
    RequestPath = "/imagens"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(materiaisPath),
    RequestPath = "/materiais"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
