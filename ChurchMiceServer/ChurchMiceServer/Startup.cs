using ChurchMiceServer.Configuration;
using ChurchMiceServer.Domains;
using ChurchMiceServer.Domains.Proxies;
using ChurchMiceServer.Security;
using ChurchMiceServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace ChurchMiceServer;

public class Startup
{
	public const string DB_CONNECTION_STRING_KEY = "ChurchMiceDatabase";
	public const string APIKEY_STRING_KEY = "Apikey";

	public const string SALT_STRING_KEY = "Salt";
	public const string NONCE_STRING_KEY = "Nonce";
	public const string KEYITERATIONS_VALUE_KEY = "KeyIterations";

	public IConfiguration Configuration { get; }

	private IConfigurationLoader configurationLoader;
	
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
		configurationLoader = new XmlConfigurationLoader(GetPathToXmlConfigurationFile(configuration));
	}

	private string GetPathToXmlConfigurationFile(IConfiguration configuration)
	{
		return configuration.GetValue<string>("XmlConfiguration");
	}

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services.Add(new ServiceDescriptor(typeof(IConfigurationLoader), configurationLoader));

		//services.AddSingleton<IRegisterNonApiService, RegisterNonApiServices>();

		services.AddCors(options =>
		{
			options.AddDefaultPolicy(
				builder =>
				{
					builder.WithOrigins("https://localhost:44351", "http://localhost:4200")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
		});

		// create db contexts for each of the domains
		services.AddDbContext<ChurchMiceContext>(options =>
			options.UseSqlServer(
				configurationLoader.GetKeyValueFor(DB_CONNECTION_STRING_KEY)));

		services.AddScoped<IUserProxy, UserProxy>();
		services.AddScoped<IEmailProxy, EmailProxy>();
		services.AddSingleton<IEmailSenderService, EmailSenderService>();

		services.AddControllers().AddJsonOptions(options =>
		{
			// // Use the default property (Pascal) casing
			// options.SerializerSettings.ContractResolver = new DefaultContractResolver();

			// Configure a custom converter
			//options.JsonSerializerOptions.Converters.Add(new DecimalJSONConverter());
		});

		services.AddScoped<IAuthenticationService, UserAuthenticationService>();

		services.AddControllers();
	}
	

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseHttpsRedirection();
		app.UseRouting();

		app.UseCors(builder =>
		{
			builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
		});

		app.UseMiddleware<JwtMiddleware>();

		app.UseValidateClient();
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
			// endpoints.MapControllerRoute(
			// 	"default",
			// 	"api/{controller}/{action}/{id?}");
		});
	}
}

