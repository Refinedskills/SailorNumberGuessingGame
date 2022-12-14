using SailorNumberGuessingGame.Server.Data;
using SailorNumberGuessingGame.Server.Services.GameService;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SailorNumberGuessingGame.Server.DAL;
using System.Reflection;
using SailorNumberGuessingGame.Server.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    //options.Issuer = builder.Configuration["Jwt:Issuer"];
    options.Audience = builder.Configuration["Jwt:Audience"];
  });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  // add a custom operation filter which sets default values
  options.OperationFilter<SwaggerDefaultValues>();
  options.CustomSchemaIds(x => x.FullName);
  options.EnableAnnotations();
  //options.OperationFilter<AddResponseHeadersFilter>();
  options.OrderActionsBy((apiDescr) => apiDescr.GroupName);

  options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
  {
    Description = "Standard Authorization header using the Bearer scheme",
    In = ParameterLocation.Header,
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
  });

  // using System.Reflection;
  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

  //options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
  app.UseSwagger();
  app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
