namespace API
{
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/api/login", Functions.Login);
                endpoints.MapPost("/api/getExpenses", Functions.GetExpenses);
                endpoints.MapPost("/api/createExpense", Functions.CreateExpense);
                endpoints.MapPost("/api/changePassword", Functions.ChangePassword);
                endpoints.MapPost("/api/getUsers", Functions.GetUsers);
                endpoints.MapPost("/api/createUser", Functions.CreateUser);
            });
        }
    }
}
