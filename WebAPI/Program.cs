using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FoodOrderDBContext>(c =>
        c.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Cho phép truy cập file tĩnh từ thư mục Images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
      Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

