using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Reciever
{
    /// <summary>
    /// Extension methods for configuring RabbitMQ receiver in ASP.NET Core applications.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private static RabbitMQExchangeReciever Reciever;

        /// <summary>
        /// Configures the RabbitMQ receiver to start and stop with the application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseRabbitMQReciever(this IApplicationBuilder app)
        {
            // Retrieve the RabbitMQExchangeReciever instance from the service provider
            Reciever = app.ApplicationServices.GetService<RabbitMQExchangeReciever>();

            // Retrieve the host application lifetime
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            // Register event handlers to start and stop the receiver when the application starts and stops
            hostApplicationLifetime.ApplicationStarted.Register(OnStart);
            hostApplicationLifetime.ApplicationStopping.Register(OnStop);

            return app;
        }

        /// <summary>
        /// Stops the RabbitMQ receiver when the application is stopping.
        /// </summary>
        private static void OnStop()
        {
            Reciever.Stop();
        }

        /// <summary>
        /// Starts the RabbitMQ receiver when the application starts.
        /// </summary>
        private static void OnStart()
        {
            Reciever.Start();
        }
    }
}
