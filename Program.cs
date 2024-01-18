using MongoDB.Driver;
using poc.api.mongodb.Configuration;
using poc.api.mongodb.EndPoints;
using poc.api.mongodb.Service;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);

// Configure MongoDB
var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(settings.ConnectionString));
builder.Services.AddScoped(sp => new MongoDatabaseFactory(sp.GetRequiredService<IMongoClient>(), settings.Database));
builder.Services.AddScoped(sp => sp.GetRequiredService<MongoDatabaseFactory>().GetDatabase());

// Service
builder.Services.AddScoped<IProdutoService, ProdutoService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.RegisterProdutosEndpoints();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();