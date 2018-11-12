namespace UrlFirewall.AspNetCore
{
    /// <summary>
    /// UrlFirewall 管理规则
    /// 控制Url 和http请求的方法
    /// </summary>
    public class UrlFirewalllRule
    {
        public string Url { get; set; }

        public string Method { get; set; }
    }
}