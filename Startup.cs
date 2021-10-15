using Hazelcast;
using HazelcastCaching.Caching;
using HazelcastCaching.Models;
using HazelcastCaching.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace HazelcastCaching
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HazelcastCaching", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });
            var connectionString = ServiceSettings.MongoConnectionString;
            var client = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(client);
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IHazelcastCaching<string, Product>, HazelcastCaching<string, Product>>();

            var options = new HazelcastOptionsBuilder().Build();
            var factory = new ProductSerializableFactory();
            options.Serialization.AddDataSerializableFactory(ProductSerializableFactory.FactoryId, factory);
            options.ClusterName = ServiceSettings.HazelcastClusterName;
            options.ClientName = ServiceSettings.HazelcastClientName;
            /*foreach (var networkingAddress in ServiceSettings.HazelcastNetworkingAddresses)
            {
                options.Networking.Addresses.Add(networkingAddress);
            }*/
            var hzclient = HazelcastClientFactory.StartNewClientAsync(options).Result;
            services.AddSingleton<IHazelcastClient>(hzclient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HazelcastCaching v1"));
            }

            app.UseRouting();

            app.UseCors("CORS");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
