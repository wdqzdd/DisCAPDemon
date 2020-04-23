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
                //�����ʹ�õ� EF �������ݲ���������Ҫ����������ã�
                //x.UseEntityFramework<BloggingContext>();  //��ѡ��㲻��Ҫ�ٴ����� x.UseSqlServer ��

                x.UseMySql("Server=localhost;Port=3307;Database=capdemon; User=root;Password=123456;");

                //CAP֧�� RabbitMQ��Kafka��AzureServiceBus ����ΪMQ������ʹ��ѡ�����ã�
                x.UseRabbitMQ(config => {
                    config.HostName = "192.168.31.42";
                    config.Port = 5672;
                    //config.ConnectionFactoryOptions = opt => {
                    //};
                });


                //ָ����������
                //x.DefaultGroup
                //����Ϣ���͵�ʱ���������ʧ�ܣ�CAP�������Ϣ�������ԣ�����������������ÿ�����Եļ��ʱ�䡣Ĭ��ֵ��60 ��
                //x.FailedRetryInterval
                //����Ϣ���͹����У������� Broker 崻���������ʧ�ܵ��������߳����쳣������£����ʱ�� CAP ��Է��͵����ԣ���һ�����Դ���Ϊ 3��4���Ӻ��Ժ�ÿ��������һ�Σ����д��� +1�����ܴ����ﵽ50�κ�CAP��������������ԡ�
                //x.FailedRetryCount
                //�ɹ���Ϣ�Ĺ���ʱ�䣨�룩�� ����Ϣ���ͻ������ѳɹ�ʱ����ʱ��ﵽ SucceedMessageExpiredAfter ��ʱ�򽫻�� Persistent ��ɾ���������ͨ��ָ����ֵ�����ù��ڵ�ʱ�䡣
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
