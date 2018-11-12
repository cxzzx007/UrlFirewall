using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace UrlFirewall.AspNetCore
{
    //UrlFirewall中间件，对中间件传来的httpcontext做处理
    //如果通过，则调用下面的middleware
    //若不通过，则终止管道，返回UrlFirewallOptions中定义的状态码 NotFound
    public class UrlFirewallMiddleware
    {   
        //下一个middleware
        private readonly RequestDelegate _next;

        //注入的UrlFirewallValidator服务，进行UrlFirewall的验证
        private readonly IUrlFirewallValidator _validator;

        //注入的日志
        private readonly ILogger<UrlFirewallMiddleware> _logger;

        //构造函数注入，接收一些服务
        public UrlFirewallMiddleware(RequestDelegate next,
            IUrlFirewallValidator validator,
            ILogger<UrlFirewallMiddleware> logger)
        {
            _next = next;
            _validator = validator;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            //获取httpcontext请求的url
            string path = context.Request.Path.ToString().ToLower();

            //获取httpcontext请求的http方法
            string method = context.Request.Method.ToLower();

            //使用UrlFirewallValidator服务进行验证
            //验证通过，调用下一个middleware
            if (_validator.Validate(path, method))
            {
                _logger.LogInformation($"The path {path} valid.");
                return this._next(context);
            }
            else//验证未通过，终止middleware 返回错误状态码
            {
                _logger.LogInformation($"The path {path} invalid.");
                context.Response.StatusCode = (int) _validator.Options.StatusCode;
                return Task.CompletedTask;;
            }
        }
    }
}