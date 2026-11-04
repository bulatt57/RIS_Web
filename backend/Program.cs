using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Фиксируем HTTP-порт 5000 по умолчанию, если не переопределён внешне
builder.WebHost.UseUrls("http://localhost:5000");

// Подключаем контроллеры и Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация сервиса-заготовки в контейнере Dependency Injection (DI)
builder.Services.AddScoped<IAssignmentService, AssignmentService>();

var app = builder.Build();

// Включаем Swagger в режиме Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
