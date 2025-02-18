var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
    options.Version = "v1";
    options.Title = "My XML Parser API";
    options.DocumentName = "XMLParser-v1";
    options.Description = "This API is used to parse the XML files and return the data in JSON format";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.Path = "/swagger";
        options.DocumentPath = "/swagger/{documentName}/swagger.json";
        options.DocumentTitle = "My XML Parser API";
        options.DocExpansion = "list";
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();