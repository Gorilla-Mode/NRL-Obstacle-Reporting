using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NRLObstacleReporting.TagHelpers
{
    // Apply to <script> and <style> elements
    [HtmlTargetElement("script")]
    [HtmlTargetElement("style")]
    public class CspNonceTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CspNonceTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var http = _httpContextAccessor.HttpContext;
            if (http == null) return Task.CompletedTask;

            if (http.Items.TryGetValue("CSPNonce", out var raw) && raw is string nonce && !string.IsNullOrEmpty(nonce))
            {
                // Only add if not already present
                if (!output.Attributes.ContainsName("nonce"))
                {
                    output.Attributes.SetAttribute("nonce", nonce);
                }
            }

            return Task.CompletedTask;
        }
    }
}