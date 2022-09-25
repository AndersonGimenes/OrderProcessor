using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProcessor.Domain.Hubs;
using OrderProcessor.Domain.IoC;
using OrderProcessor.Domain.Options;

namespace OrderProcessor.API
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.DomainDependencyInjection();
            services.Configure<RabbitMqOptions>(_configuration.GetSection("RabbitMqOptions"));

            services.AddControllers();

            services.AddCors(opts =>
            {
                opts.AddPolicy
                (
                    "MyPolicy", builder =>
                        builder.SetIsOriginAllowed(host => true)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                );
            });

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("MyPolicy");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OrderProcessorHub>("/orderProcessorWebSocket");
            });
        }
    }
}
