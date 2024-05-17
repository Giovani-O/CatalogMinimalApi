using CatalogMinimalApi.AppServicesExtensions;
using CatalogMinimalApi.Context;
using CatalogMinimalApi.Endpoints;
using CatalogMinimalApi.Models;
using CatalogMinimalApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddJwtAuthentication();

var app = builder.Build();

#region API Endpoints

app.MapAuthenticationEndpoints();

app.MapCategoryEndpoints();

app.MapProductEndpoints();

#endregion

var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

// Ativa autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.Run();