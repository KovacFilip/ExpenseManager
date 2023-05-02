namespace API
{
    using System.Net;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(async endpoints =>
            {
                endpoints.MapGet(
                    "/api/home",
                    () => "Sorry Mario. The princess is in another castle"
                );

                endpoints.MapPost(
                    "/api/postTry",
                    async context =>
                    {
                        var requestBody = await new StreamReader(
                            context.Request.Body
                        ).ReadToEndAsync();
                        Console.WriteLine(requestBody); // write the body to console

                        // Return a response to the client
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await context.Response.WriteAsync("Request received!");
                    }
                );
            });
        }
    }
}
