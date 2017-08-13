using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PieShop.TagHelpers
{

	public class ItalicTagHelper : TagHelper
	{
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.Attributes.RemoveAll("i");
			output.PreContent.SetHtmlContent("<i>");
			output.PostContent.SetHtmlContent("</i>");
		}
	}
}
