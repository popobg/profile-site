using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using UploadImageMVCTest.Repositories;
using UploadImageMVCTest.Service;

namespace UploadImageMVCTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set the Cloudinary Credentials
            // ========================

            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton(cloudinary);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=List}/{id?}");

            app.Run();
        }
    }
}
