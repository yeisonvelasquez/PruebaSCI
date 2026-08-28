using System.Reflection;
using PruebaSCI.Application;
using PruebaSCI.Api.Middleware;
using PruebaSCI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	var documentationFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var documentationPath = Path.Combine(AppContext.BaseDirectory, documentationFile);
	options.IncludeXmlComments(documentationPath);
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("LocalClients", policy => policy
		.SetIsOriginAllowed(origin =>
			origin.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase) ||
			origin.StartsWith("https://localhost:", StringComparison.OrdinalIgnoreCase))
		.AllowAnyHeader()
		.AllowAnyMethod());
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("LocalClients");
app.UseAuthorization();

app.MapControllers();

app.Run();
