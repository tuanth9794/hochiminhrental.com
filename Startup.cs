using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreCnice.UtilsCs;
using Microsoft.Net.Http.Headers;
using WebMarkupMin.AspNetCore1;

namespace CoreCnice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("cookie").AddCookie("cookie", options =>
        {
            options.LoginPath = new PathString("/quantriweb");
            options.AccessDeniedPath = new PathString("/quantriweb");
        });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddMvc();
            services.AddMvcCore();
            services.AddWebMarkupMin(
       options =>
       {
           options.AllowMinificationInDevelopmentEnvironment = true;
           options.AllowCompressionInDevelopmentEnvironment = true;
       })
       .AddHtmlMinification(
           options =>
           {
               options.MinificationSettings.RemoveRedundantAttributes = true;
               options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
               options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
           })
       .AddHttpCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //env.EnvironmentName = EnvironmentName.Production;

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    //app.UseHsts();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseSession();
            //app.UseAuthentication();
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".xml"] = "text/xml";
            contentTypeProvider.Mappings[".txt"] = "text/plain";
            contentTypeProvider.Mappings[".bmp"] = "image/bmp";
            contentTypeProvider.Mappings[".gif"] = "image/gif";
            contentTypeProvider.Mappings[".jpeg"] = "image/jpeg";
            contentTypeProvider.Mappings[".jpg"] = "image/jpeg";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = contentTypeProvider
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
      name: "areaRoute",
      template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                // Admin
                routes.MapRoute(
    name: "Admin",
    template: "quantriweb",
    defaults: new { controller = "Account", action = "Login" });

                routes.MapRoute(
name: "KH",
template: "tong-quan-khach-hang",
defaults: new { controller = "AdminCus", action = "Index" });

                routes.MapRoute(
name: "dangtin",
template: "dang-tim-can-ho",
defaults: new { controller = "AdminCus", action = "PostNewsCus" });

                routes.MapRoute(
name: "capnhatkhachhang",
template: "cap-nhat-thong-tin",
defaults: new { controller = "AdminCus", action = "CustomerSetting" });

                routes.MapRoute(
name: "danhsachyeucau",
template: "danh-sach-gui-yeu-cau",
defaults: new { controller = "AdminCus", action = "CustomerListRequest" });

                routes.MapRoute(
name: "danhsachproject",
template: "danh-sach-cho-thue",
defaults: new { controller = "AdminCus", action = "ProjectListAprove" });

                routes.MapRoute(
name: "danhsachseller",
template: "danh-sach-seller",
defaults: new { controller = "AdminCus", action = "SellerList" });

                routes.MapRoute(
name: "dangnhaptaikhoan",
template: "sellernet/trang-dang-nhap",
defaults: new { controller = "AdminCus", action = "Singin" });

                routes.MapRoute(
name: "dangkytaikkhoan",
template: "sellernet/trang-dang-ky",
defaults: new { controller = "AdminCus", action = "RegisterSeller" });
                routes.MapRoute(
name: "quenmatkhaukhach",
template: "sellernet/quen-mat-khau",
defaults: new { controller = "AdminCus", action = "ForgotPassword" });
                
                routes.MapRoute(
  name: "TimKiem",
  template: "ket-qua-tim-kiem",
  defaults: new { controller = "Search", action = "index" });
                routes.MapRoute(
name: "LoiTrang",
template: "thong-bao-loi",
defaults: new { controller = "Catalog", action = "Eror404" });

                routes.MapRoute(
    name: "CatUrl",
    template: "{resource?}/{resource1?}",
    defaults: new { controller = "Catalog", action = "UrlReturn" }
);

 
            });

            app.UseDefaultFiles();
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });
            app.UseWebMarkupMin();
            app.UseSession();
            app.UseMiddleware();
            app.UseMvc();
        }
    }
}
