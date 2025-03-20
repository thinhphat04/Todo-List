using Microsoft.EntityFrameworkCore;
using Todo_Api.Data;
using Todo_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to Database
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))); // Lấy chuỗi kết nối từ appsettings.json
// Register Service
builder.Services.AddScoped<ITaskService, TaskService>();
// Add MemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Optional Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();


app.Run();

