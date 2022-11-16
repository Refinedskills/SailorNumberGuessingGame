using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SailorNumberGuessingGame.Server.Swagger;


/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
  readonly IApiVersionDescriptionProvider provider;
  readonly IConfiguration configuration;

  /// <summary>
  /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
  /// </summary>
  /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
  /// <param name="configuration">The current configuration.</param>
  public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
  {
    this.provider = provider;
    this.configuration = configuration;
  }

  /// <inheritdoc />
  public void Configure(SwaggerGenOptions options)
  {
    // add a swagger document for each discovered API version
    // note: you might choose to skip or document deprecated API versions differently
    foreach (var description in provider.ApiVersionDescriptions)
    {
      options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, configuration));
    }
  }

  static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IConfiguration configuration)
  {
    var info = new OpenApiInfo()
    {
      Title = configuration.GetValue<string>("ApiInfo:Title"),
      Version = description.ApiVersion.ToString(),
      Description = configuration.GetValue<string>("ApiInfo:Description"),
      Contact = new OpenApiContact()
      {
        Name = configuration.GetValue<string>("ApiInfo:Contact:Name"),
        Email = configuration.GetValue<string>("ApiInfo:Contact:Email")
      },
      License = new OpenApiLicense()
      {
        Name = configuration.GetValue<string>("ApiInfo:License:Name"),
        Url = new Uri(configuration.GetValue<string>("ApiInfo:License:Url"))
      }
    };

    if (description.IsDeprecated)
    {
      info.Description += configuration.GetValue<string>("ApiInfo:Depricated");
    }

    return info;
  }
}
