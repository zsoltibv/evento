using Evento.Application;
using Evento.Email;
using Evento.Endpoints;
using Evento.Endpoints.Endpoints;
using Evento.Endpoints.Hubs;
using Evento.Infrastructure;
using Evento.Jobs;
using Evento.Payments;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Register OpenApi
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// Add SignalR
builder.Services.AddSignalR();

// Add all Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add all Application Services
builder.Services.AddApplicationServices();

// Add email services
builder.Services.AddEmailServices(builder.Configuration);

// Add payment services
builder.Services.AddPaymentServices(builder.Configuration);

// Add quartz jobs
builder.Services.AddQuartzJobs(builder.Configuration);

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // Angular dev server URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

// Map Endpoints
app.MapPaymentEndpoints();
app.MapAuthEndpoints();
app.MapVenueEndpoints();
app.MapBookingEndpoints();
app.MapRoleRequestEndpoints();
app.MapEmailEndpoints();
app.MapChatEndpoints();
app.MapUserEndpoints();
app.MapHub<ChatNotificationHub>("/hubs/chat");

app.Run();