using SimpleOpenCap.Data;

var env = SLSAK.Utilities.Environment.GetEnvironmentVariables(false);
var disableCors = bool.Parse(env["DISABLE_CORS"]);
var enableSwagger = bool.Parse(env["ENABLE_SWAGGER"]);

var builder = WebApplication.CreateBuilder(args);

if (disableCors)
{
    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(policy =>
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods("get"))
    );
}

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DataReader>();

var app = builder.Build();

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();