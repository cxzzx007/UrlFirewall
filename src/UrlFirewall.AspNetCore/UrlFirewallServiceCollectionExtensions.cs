using System;
using Microsoft.Extensions.DependencyInjection;

namespace UrlFirewall.AspNetCore
{
    //IServiceCollection扩展方法，进行UrlFirewall服务注入，以及相关服务注入
    public static class UrlFirewallServiceCollectionExtensions
    {
        public static IServiceCollection AddUrlFirewall(this IServiceCollection services, Action<UrlFirewallOptions> options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            //添加针对Options模型的服务
            services.AddOptions();

            //将配置绑定到Options模型上
            services.Configure(options);

            //将UrlFirewall 我们这个服务作为单例注入到DI容器中
            services.AddSingleton<IUrlFirewallValidator, DefaultUrlFirewallValidator>();

            return services;
        }

        public static IServiceCollection AddUrlFirewallValidator<T>(this IServiceCollection services) where T : IUrlFirewallValidator
        {
            services.AddSingleton(typeof(IUrlFirewallValidator),typeof(T));
            return services;
        }
    }
}