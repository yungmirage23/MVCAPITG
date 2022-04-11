using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using mysecondshop.Models.ViewModels;

namespace mysecondshop.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo pageModel { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string LiClass { get; set; }
        public string AClass { get; set; }
        public string UlClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("ul");
            for (int i = 1; i <= pageModel.TotalPages; i++)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder tag = new TagBuilder("a");
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });
                if (PageClassesEnabled)
                {
                    result.AddCssClass(UlClass);
                    li.AddCssClass(LiClass);
                    tag.AddCssClass(AClass);
                    tag.AddCssClass(i == pageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }

                tag.InnerHtml.AppendHtml(i.ToString());
                li.InnerHtml.AppendHtml(tag);
                result.InnerHtml.AppendHtml(li);
            }
            TagBuilder lil = new TagBuilder("li");
            TagBuilder al = new TagBuilder("a");
            TagBuilder img = new TagBuilder("img");
            al.Attributes["href"] = urlHelper.Action(PageAction, new { itemPage = (pageModel.CurrentPage) + 1 });
            img.MergeAttribute("src", "/img/icon.svg");
            al.InnerHtml.AppendHtml(img);
            lil.InnerHtml.AppendHtml(al);
            result.InnerHtml.AppendHtml(lil);

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}