using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace UrlFirewall.AspNetCore
{
    /// <summary>
    /// UrlFirewall选项
    /// </summary>
    public class UrlFirewallOptions
    {
        /// <summary>
        /// White List or Black List.Default is Black.
        /// 默认黑名单
        /// </summary>
        public UrlFirewallRuleType RuleType { get; set; } = UrlFirewallRuleType.Black;

        /// <summary>
        /// Standard Rule.String complete matching
        /// 规则的列表  控制Url和Http请求方法
        /// </summary>
        public List<UrlFirewalllRule> StandardRuleList { get; set; }=new List<UrlFirewalllRule>();

        /// <summary>
        /// Regex Rule.Regex matching.The two rule list is set up to speed up the matching.
        /// 正则表达式 规则列表
        /// </summary>
        public List<UrlFirewalllRule> RegexRuleList { get; set; }=new List<UrlFirewalllRule>();

        //http状态码 默认404
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;

        public void SetRuleList(IConfigurationSection section)
        {
            if (section == null)
            {
                throw new ArgumentException(nameof(section));
            }

            var list = section.Get<List<UrlFirewalllRule>>();

            if (list == null)
            {
                throw new UrlFirewallException("The section key is invalid.");
            }

            foreach (var t in list)
            {
                t.Method = t.Method.ToLower();

                //Whether it is a special rule or not
                if (t.Url.Contains("/*") || t.Url.Contains("/?"))
                {
                    //convert to regex 
                    t.Url = t.Url.Replace("/*", "/.*").Replace("/?", "/.?").ToLower();
                    RegexRuleList.Add(t);
                }
                else
                {
                    t.Url = t.Url.ToLower();
                    StandardRuleList.Add(t);
                }
            }
        }
    }
}