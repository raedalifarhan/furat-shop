using API.Data;
using API.DTOs.CategoryDTOs;
using API.Interfaces;
using API.Mappings;
using API.Models;
using API.Services;
using API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddMvc();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("app-conn"));
            });

            // Validator
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

            // services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
            // services.AddValidatorsFromAssemblyContaining<ProductValidator>();

            // Auto Mappers
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddAutoMapper(typeof(CategoryProfile).Assembly);

            // Services
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));

            return services;
        }
    }
}