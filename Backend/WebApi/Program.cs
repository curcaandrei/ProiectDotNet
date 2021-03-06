using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.MongoDb;
using Persistence.TwitterExternalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.Configure<MongoSettings>(options =>
{
    options.Connection = builder.Configuration.GetSection("MongoSettings:Connection").Value;
    options.DatabaseName = builder.Configuration.GetSection("MongoSettings:DatabaseName").Value;
    
});
builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();

// Twitter Helper
builder.Services.Configure<TwitterSettings>(options =>
{
    options.ApiKey = builder.Configuration.GetSection("TwitterAPI:TwitterApiKey").Value;
    options.ApiSecret = builder.Configuration.GetSection("TwitterAPI:TwitterApiSecret").Value;
    options.AccessToken = builder.Configuration.GetSection("TwitterAPI:TwitterAccessToken").Value;
    options.AccessSecret = builder.Configuration.GetSection("TwitterAPI:TwitterAccessTokenSecret").Value;
});

builder.Services.AddScoped<ITwitterHelper, TwitterHelper>();
// Twitter Helper
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "https://twitter-sentiment-analyser.com",
        ValidAudience = "https://twitter-sentiment-analyser.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("turbo-secret-key")),
        ClockSkew = TimeSpan.Zero // remove delay of token when expire
    };
});

var app = builder.Build();

app.UseRouting();
app.UseCors(x =>
{
    x.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true);
});


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(x => x.MapControllers());
// Configure the HTTlP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        
    });
}

app.UseCors();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();