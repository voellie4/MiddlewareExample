using Middleware_Example.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//without extension
app.UseMiddleware<ExceptionHandlingMiddleware>();

//with extension
//MapWhen: if true then the middleware in inner block is used otherwise use middleware in the next code
//UseWhen: no matter true or false, the middleware in next code is used

//app.MapWhen(context => context.Request.Path.ToString().Contains("DivideByZero"), appBuilder =>
//{
//    appBuilder.UseSecondMiddleware();
//});

app.UseWhen(context => context.Request.Path.ToString().Contains("DivideByZero"), appBuilder =>
{
    appBuilder.UseSecondMiddleware();
});

app.UseThirdMiddleware();


app.MapControllers();

app.Run();
