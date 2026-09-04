using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<GlickoContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("GlickoContext")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "GlickShow", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "GlickShow v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Call EnsureCreated() to create the database and tables  
using (var scope = app.Services.CreateScope())  
{  
    var services = scope.ServiceProvider;  
    try  
    {  
        var dbContext = services.GetRequiredService<GlickoContext>();  
        dbContext.Database.EnsureCreated(); // Creates database/tables if missing  
    }  
    catch (Exception ex)  
    {  
        var logger = services.GetRequiredService<ILogger<Program>>();  
        logger.LogError(ex, "An error occurred creating the database.");  
    }  
}  

app.Run();
