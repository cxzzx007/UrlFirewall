using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UrlFirewall.AspNetCore
{
    //实现了UrlFirewall接口，作为一个默认的处理方式，实现Validate方法
    public class DefaultUrlFirewallValidator: IUrlFirewallValidator
    {
        private readonly ILogger<DefaultUrlFirewallValidator> _logger;

        //接收DI注入时，Service.Configure(options)绑定的用户传入的配置
        public UrlFirewallOptions Options { get; set; }

        public DefaultUrlFirewallValidator(ILogger<DefaultUrlFirewallValidator> logger,IOptions<UrlFirewallOptions> options)
        {
            _logger = logger;
            Options = options.Value;

        }

        //实现的Validate方法   验证url和method是否在选项配置要求里
        public bool Validate(string url,string method)
        {
            string valUrl = url;

            //如果url结尾包含/  则截取去除
            if (valUrl.Length > 1 && valUrl.Last() == '/')
            {
                valUrl = valUrl.Substring(0, valUrl.Length - 1);
            }
            //标准规则列表中是否有匹配当前httpcontext中url的规则的记录
            var rule = Options.StandardRuleList.FirstOrDefault(m => m.Url == valUrl);

            //如果标准规则匹配的url记录为空，则从正则规则中进行匹配
            if (rule == null)
            {
                foreach (var item in Options.RegexRuleList)
                {
                    if (Regex.IsMatch(url, item.Url, RegexOptions.IgnoreCase))
                    {
                        rule = item;
                        break;
                    }
                }
            }

            //如果是黑名单规则
            if (Options.RuleType == UrlFirewallRuleType.Black)
            {

                //如果没有匹配到 返回true
                if (rule == null)
                {
                    return true;
                }
                else
                {
                    //in balck list,next step is match method.
                    //匹配到了，匹配所有方法  返回false
                    if (rule.Method == "all")
                    {
                        return false;
                    }
                    else if (rule.Method == method)//并且匹配到对应方法  返回false
                    {
                        //if path & method are matched,not allow this request.
                        return false;
                    }
                    else
                    {
                        // if path & method are not matched,allow this request.
                        //否则返回false
                        return true;
                    }
                }

                
            }
            else
            {
                //如果是白名单，没有匹配到  则返回false
                if (rule == null)
                {
                    return false;
                }
                else
                {
                    //in white list,next step is match method.

                    if (rule.Method == "all")
                    {
                        return true;
                    }
                    else if (rule.Method == method)
                    {
                        //if path & method are matched,allow this request.
                        return true;
                    }
                    else
                    {
                        // if path & method are not matched,not allow this request.
                        return false;
                    }
                }
            }

            
            
        }
    }
}