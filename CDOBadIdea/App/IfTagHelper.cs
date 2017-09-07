using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDOBadIdea.App
{
    [HtmlTargetElement(Attributes = "asp-if")]
    public class IfTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-if")]
        public bool Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Value)
            {
                output.Content.SetContent("");
            }
        }
    }
}
