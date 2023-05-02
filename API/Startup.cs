namespace API
{
    using System.Net;
    using System.Text.Json;
    using DB.models;
    using DB.UnitOfWork;
    using Helper.Helpers;
    using Helper.ObjectsToApi;

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
                    "/api/login",
                    async context =>
                    {
                        var requestBody = await new StreamReader(
                            context.Request.Body
                        ).ReadToEndAsync();

                        var requestBodyJson = JsonSerializer.Deserialize<LoginObject>(requestBody);

                        if (
                            requestBody == null
                            || requestBodyJson!.Username == null
                            || requestBodyJson.PasswordHash == null
                        )
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync("Empty body");
                        }
                        else
                        {
                            Person? person;

                            using (var uow = new UnitOfWork())
                            {
                                person = await uow.Login(
                                    requestBodyJson.Username,
                                    requestBodyJson.PasswordHash
                                );
                            }

                            if (person != null)
                            {
                                Console.WriteLine($"{person.Username}, {person.Id}, {person.Role}");

                                string resToFe = JsonSerializer.Serialize(person);
                                context.Response.StatusCode = (int)HttpStatusCode.OK;
                                await context.Response.WriteAsync(resToFe);
                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                await context.Response.WriteAsync("Incorrect login credentials!");
                            }
                        }
                    }
                );
            });
        }
    }
}
