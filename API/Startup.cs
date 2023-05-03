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
                endpoints.MapPost("/api/login", Functions.Login);
                endpoints.MapPost("/api/getExpenses", Functions.GetExpenses);
                endpoints.MapPost("/api/createExpense", Functions.CreateExpense);
            });
        }
    }
}
