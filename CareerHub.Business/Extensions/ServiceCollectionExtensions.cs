using AutoMapper;
using CareerHub.Business.Mappings;
using CareerHub.Business.Services.Abstract;
using CareerHub.Business.Services.Concrete;
using CareerHub.DataAccess.Repositories.Abstract;
using CareerHub.DataAccess.Repositories.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using StackExchange.Redis;

namespace CareerHub.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>   
        /// IServiceCollection referanslarının eklenmesini sağlar.     
        /// </summary>      
        /// <param name="services"></param>   
        /// <returns></returns>   
        public static IServiceCollection AddBusinessRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            //AutoMapper Configurations 
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CareerHubMappingProfile());
            }).CreateMapper());

            var redisConnection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);
            //Service DI Configurations
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Elasticsearch settings
            var settings = new ConnectionSettings(new Uri(configuration["Elasticsearch:Url"]))
           .DefaultIndex(configuration["Elasticsearch:DefaultIndex"]).DisableDirectStreaming();

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
            services.AddScoped<IElasticsearchService, ElasticsearchService>();
            return services;
        }
    }
}
