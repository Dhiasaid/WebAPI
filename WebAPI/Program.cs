using WebAPI.Auth;
using WebAPI.Modeles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using System.Text;
using WebAPI.Auth;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddDbContext<MovieContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MovieContext")));
builder.Services.AddDbContext<ApplicationsDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("MovieContext")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//for Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationsDbContext>()
    .AddDefaultTokenProviders();
//Adding authentification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Adding Jwt Bearer
  .AddJwtBearer(options =>
  {
      options.SaveToken = true;
      options.RequireHttpsMetadata = false;
      options.TokenValidationParameters = new TokenValidationParameters()
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidAudience = configuration["Jwt:validAudience"],
          ValidIssuer = configuration["Jwt:validIssuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
          
          
          RequireExpirationTime = false,
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero,
      };

  });
var app = builder.Build();
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