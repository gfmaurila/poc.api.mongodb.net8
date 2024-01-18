using Microsoft.OpenApi.Models;

namespace poc.api.mongodb.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, IConfiguration conf)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Cadastro de produtos",
                    Version = "v1"
                }
            );
        });
        return services;
    }
}
