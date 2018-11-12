using Microsoft.AspNetCore.Builder;

namespace UrlFirewall.AspNetCore
{
    //IApplicationBuilder扩展方法 UrlFirewallMiddleware的扩展
    public static class UrlFirewallMiddlewareExtensions
    {
        public static IApplicationBuilder UseUrlFirewall(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UrlFirewallMiddleware>();
        }
    }
}