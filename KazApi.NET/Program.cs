
using System.Net;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("KazApp is starting...");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel((context, options) => {
                    options.Listen(IPAddress.Any, 5000); // HTTP
                    if (context.HostingEnvironment.IsProduction())
                    {
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps(
                                "/etc/letsencrypt/live/try-the-work.net/certificate.pfx",
                                "kaz_5050");
                        });
                    }
                });
            });
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://try-the-work.net")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            //app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseCors();
        
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapFallbackToFile("/index.html");
        });
    }
}













// ************************************************************************************************
// original (swagger�g�p�\)
// ************************************************************************************************

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// CORS�I���W���ݒ�
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        builder =>
//        {
//            builder.AllowAnyOrigin()  // ���ׂẴI���W������� CORS �v��������
//                   .AllowAnyMethod()  // ���ׂĂ� HTTP ���\�b�h������
//                   .AllowAnyHeader(); // ���ׂĂ̍쐬�җv���w�b�_�[������
//        });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//// CORS �~�h���E�F�A��L���ɂ���
//app.UseCors();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
