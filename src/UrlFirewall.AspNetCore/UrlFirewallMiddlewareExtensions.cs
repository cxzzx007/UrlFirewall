using Microsoft.AspNetCore.Builder;

namespace UrlFirewall.AspNetCore
{
    /// <summary>
    /// IApplicationBuilder扩展方法，用于添加中间件UrlFirewallMiddleware
    /// </summary>
    public static class UrlFirewallMiddlewareExtensions
    {
        public static IApplicationBuilder UseUrlFirewall(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UrlFirewallMiddleware>();
        }
    }
}