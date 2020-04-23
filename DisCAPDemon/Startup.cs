using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisCAPDemon.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DisCAPDemon
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
            var mySqlCon = "Server=localhost;Port=3307;Database=capdemon; User=root;Password=123456;";
            services.AddDbContext<BloggingContext>(options => options.UseMySQL(mySqlCon));


            services.AddCap(x =>
            {
                //如果你使用的 EF 进行数据操作，你需要添加如下配置：
                //x.UseEntityFramework<BloggingContext>();  //可选项，你不需要再次配置 x.UseSqlServer 了

                x.UseMySql("Server=localhost;Port=3307;Database=capdemon; User=root;Password=123456;");

                //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
                x.UseRabbitMQ(config => {
                    config.HostName = "192.168.31.42";
                    config.Port = 5672;
                    //config.ConnectionFactoryOptions = opt => {
                    //};
                });


                //指定队列名称
                //x.DefaultGroup
                //在消息发送的时候，如果发送失败，CAP将会对消息进行重试，此配置项用来配置每次重试的间隔时间。默认值：60 秒
                //x.FailedRetryInterval
                //在消息发送过程中，当出现 Broker 宕机或者连接失败的情况亦或者出现异常的情况下，这个时候 CAP 会对发送的重试，第一次重试次数为 3，4分钟后以后每分钟重试一次，进行次数 +1，当总次数达到50次后，CAP将不对其进行重试。
                //x.FailedRetryCount
                //成功消息的过期时间（秒）。 当消息发送或者消费成功时候，在时间达到 SucceedMessageExpiredAfter 秒时候将会从 Persistent 中删除，你可以通过指定此值来设置过期的时间。
                //x.SucceedMessageExpiredAfter = 1 * 24 * 3600;
                
            });

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
