﻿using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using static System.Formats.Asn1.AsnWriter;

namespace TatBlog.WebApp.Extensions
{
    public static class WebApplicationExtensions
    {
        // Them cac dich vu duoc yeu cau boi MVC Framework
        public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddResponseCompression();
            return builder;
        }

        // Dang ky cac dich vu voi DI Container
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(
            builder.Configuration
            .GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IBlogRepository, BlogRepository>(); builder.Services.AddScoped<IDataSeeder, DataSeeder>();
            return builder;
        }

        // Cau hinh HTTP Request pipeline
        public static WebApplication UseRequestPipeline(this WebApplication app)
        {
            // Them middleware de hien thi thong bao loi
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Blog/Error");

                // Them middleware cho viec ap dung HSTS
                // (them header Strict-Transport-Security vao HTTP Response)
                app.UseHsts();
            }

            // Them middleware de tu dong nen HTTP response
            app.UseResponseCompression();

            // Them middleware de chuyen huong HTTP sang HTTPS
            app.UseHttpsRedirection();

            // Them middleware phuc vu cac yeu cau lien quan
            // toi cac tap tin noi dung tinh nhu hinh anh, css, ...
            app.UseStaticFiles();

            // Them middleware lua chon endpoint phu hop nhat
            // de xu ly mot HTTP request
            app.UseRouting();

            return app;
        }

        public static IApplicationBuilder UseDateSeeder(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            try
            {
                scope.ServiceProvider.GetRequiredService<IDataSeeder>()
                    .Initialize();
            }
            catch (Exception ex)
            {
                scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
                    .LogError(ex, "Could not insert data into database");
            }

            return app;
        }
    }
}