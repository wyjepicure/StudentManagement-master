using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StudentManagement.Middlewares;
using StudentManagement.Models;

namespace StudentManagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //注册服务
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config => {
                var plicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(plicy));

             }).AddXmlSerializerFormatters(); ;//包含了依赖于mvc core 以及常用的第三方服务和方法。
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));
            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireNonAlphanumeric = false;
            }
           
            ) ;
            services.AddIdentity<ApplicationUser, IdentityRole>().AddErrorDescriber<CustomIdentityErrorDescriber>(). AddEntityFrameworkStores<AppDbContext>();
            //  services.AddMvcCore();只包含了核心的mvc功能
            //依赖注入1：singleton,2:transient,3:scoped,低耦合，使用单元测试更方便
            services.AddScoped<IStudentRepository, SQLStudentRepository>();//不会改变
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");

                app.UseExceptionHandler("/Error");//拦截异常

                app.UseStatusCodePagesWithReExecute("/Error/{0}");//拦截处理没有的页面
            }

            /* DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
             defaultFilesOptions.DefaultFileNames.Clear();
             defaultFilesOptions.DefaultFileNames.Add("52abp.html");*/
            /* FileServerOptions fileServerOptions = new FileServerOptions();
             fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
             fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("53abp.html");*/
            /*    app.UseFileServer();*/

            /*    //添加默认文件中间件
                app.UseDefaultFiles(defaultFilesOptions);
                //index.html index.htm 默认 default.htm
                //静态文件中间件*/
            app.UseStaticFiles();
            app.UseAuthentication();
            /*app.UseMvcWithDefaultRoute();*/
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}