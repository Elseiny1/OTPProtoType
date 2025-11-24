using Microsoft.AspNetCore.Builder;
using OTPNumberPrototype.Hub;
using OTPNumberPrototype.IRepository;
using OTPNumberPrototype.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.Services.AddMemoryCache();

#region enable CORS
var policy = "allowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy,
        p =>
        {
            p.WithOrigins(
            "http://localhost:5500",
            "http://127.0.0.1:5500"
        )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});
#endregion

builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddHttpClient<ISendOTP, SendOTP>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(static options =>
    options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));

    app.MapScalarApiReference();
}

app.UseCors(policy);

app.MapHub<UserDataHub>("/userData");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
