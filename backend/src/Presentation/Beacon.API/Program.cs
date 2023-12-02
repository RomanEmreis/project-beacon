using Beacon.API.Connection;
using Beacon.Application.Features.Users.Queries;
using Beacon.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddBeacon();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/users", async (IMediator mediator) => 
{
    var users = await mediator.Send(new GetActiveUsers());
    return users;
});

app.MapHub<ConnectionHub>("/ConnectionHub");

app.Run();