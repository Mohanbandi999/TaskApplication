using Microsoft.EntityFrameworkCore;
using TaskApplication.Data;
using TaskApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TaskContext>(opt =>
    opt.UseInMemoryDatabase("TaskList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // your Angular dev URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

// Seed data before app.Run
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskContext>();
    if (!context.Tasks.Any()) // Optional: only seed if empty
    {
        context.Tasks.AddRange(
            new TaskItem
            {
                Title = "Sample 1",
                Description = "Demo task",
                Status = TaskApplication.Models.TaskStatus.ToDo,
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2)
            },
            new TaskItem
            {
                Title = "Sample 2",
                Description = "Another task",
                Status = TaskApplication.Models.TaskStatus.InProgress,
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3)
            }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
