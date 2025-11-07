using BemEstar.ApiMotivacional.Service;
using BemEstar.ApiMotivacional.Infra.Config;
using BemEstar.ApiMotivacional.Infra.Db;
using BemEstar.ApiMotivacional.Infra.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEndLocal",
    policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5501")
            .AllowAnyMethod()   // permite qualquer método (GET, POST, PUT, DELETE...)
            .AllowAnyHeader();  // permite qualquer header
    });
});

builder.Configuration
       .SetBasePath(AppContext.BaseDirectory)
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddUserSecrets<Program>()
       .AddEnvironmentVariables();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<AppConfiguration>();
builder.Services.AddSingleton<IDbConnectionFactory, MySqlDataConnectionFactory>();
builder.Services.AddScoped<MotivacionalService>();
builder.Services.AddScoped<MotivacionalRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontEndLocal");

app.UseAuthorization();

app.MapControllers();

app.Run();
