using ApiBlog.Auth.Repository;
using ApiBlog.Data;
using ApiBlog.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using ApiBlog.Post.Repository;
using ApiBlog.Tag.Repository;
using ApiBlog.Interacao.Repository;

var builder = WebApplication.CreateBuilder(args);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

// CORS liberado para testes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

ConfigurationManager configuration = builder.Configuration;
string? SERVER = configuration.GetConnectionString("SERVER");
string? DATABASE = configuration.GetConnectionString("DATABASE");
string? USERDB = configuration.GetConnectionString("USERDB");
string? PASSWORDUSER = configuration.GetConnectionString("PASSWORDUSER");

builder.Services.AddDbContext<APIContext>(options =>
    options.UseSqlServer($"Data Source={SERVER};Initial Catalog={DATABASE};Persist Security Info=True;User ID={USERDB};Password={PASSWORDUSER};Trust Server Certificate=True;Application Name=APIPetShopControl"));

// Serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração Swagger com JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ApiBlog", Version = "v1" });

    // Definindo o esquema de segurança Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe o token JWT no formato: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Aplicando o esquema de segurança globalmente nos endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IInteracaoRepository, InteracaoRepository>();

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiBlog v1");
        c.RoutePrefix = "swagger"; // Para acessar o Swagger na raiz (opcional)
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // CORS precisa vir antes da autenticação

app.UseAuthentication(); // Ativa autenticação JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
