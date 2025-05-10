using Presentation.Extensions;
using Presentation.Middlewares;
using DataSeeder.Seeders;
using Service.Base;
using Service.Contracts.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.AddScoped<SeederRunner>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureApiBahavior();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddAuthorization();
builder.Services.ConfigureTokenService();
builder.Services.ConfigureFileService();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureJWTSettings(builder.Configuration);
builder.Services.ConfigureUserContextService();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<SeederRunner>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
