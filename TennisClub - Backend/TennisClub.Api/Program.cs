using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Azure.Cosmos;
using Microsoft.OpenApi.Models;
using TennisClub.Api.Extensions;
using TennisClub.Api.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCosmosDbIdentityProvider(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new InputMappings());
    config.AddProfile(new OutputMappings());
});
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tennis Club Api", Version = "v1" }));

var app = builder.Build();

await InitializeDatabaseAndContainers(app);

app.UseExceptionHandler(configure =>
{
    configure.Run(async context =>
    {
        var exeptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (exeptionHandlerPathFeature != null && exeptionHandlerPathFeature.Error is Exception ex)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(ex.Message);
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();


async static Task InitializeDatabaseAndContainers(WebApplication app)
{
    var client = app.Services.GetService<CosmosClient>();
    Database database = await client.CreateDatabaseIfNotExistsAsync("tennis-club-db");

    await database.DefineContainer("matches", "/id").CreateIfNotExistsAsync();

    await database.DefineContainer("players", "/id").CreateIfNotExistsAsync();

    await database.DefineContainer("tournaments", "/id").CreateIfNotExistsAsync();

    await database.DefineContainer("weeklyRanks", "/id")
        .WithIndexingPolicy()
        .WithCompositeIndex()
            .Path("/Year", CompositePathSortOrder.Descending)
            .Path("/Week", CompositePathSortOrder.Descending)
            .Attach()
        .Attach()
        .CreateIfNotExistsAsync();
}
