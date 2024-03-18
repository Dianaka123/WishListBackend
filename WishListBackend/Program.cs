using Microsoft.EntityFrameworkCore;
using WishListBackend.Models;
using WishListBackend.Other.Implementation;
using WishListBackend.Other.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

string con = "Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;";
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(con));

builder.Services.AddSingleton<IPasswordEncoder, PasswordEncoder>();
builder.Services.AddSingleton<IRegistrationDataValidator, RegistrationDataValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7035"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
