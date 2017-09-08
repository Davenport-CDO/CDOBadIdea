using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDOBadIdea.App
{
    [HtmlTargetElement("asp-if")]
    public class IfTagHelper : TagHelper
    {
        [HtmlAttributeName("condition")]
        public bool Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items["if_condition"] = Value;

            if (!Value && context.AllAttributes.ContainsName("noelse"))
            {
                output.Content.SetContent("");
            }
        }
    }

    [HtmlTargetElement("asp-true", ParentTag = "asp-if")]
    public class TrueTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((bool)context.Items["if_condition"] == false)
            {
                output.Content.SetContent("");
            }
        }
    }

    [HtmlTargetElement("asp-false", ParentTag = "asp-if")]
    public class FalseTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((bool)context.Items["if_condition"] == true)
            {
                output.Content.SetContent("");
            }
        }
    }
}
