using PFDLOCFINDER.Services;

var builder = WebApplication.CreateBuilder(args);

//Config IP
builder.WebHost.UseUrls("http://*:80");

//Config Controller
builder.Services.AddControllers();

//Config Service
builder.Services.AddSingleton<IPDFService, PDFService>();

//Config Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Run Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Run Controller
app.MapControllers();

app.Run();
