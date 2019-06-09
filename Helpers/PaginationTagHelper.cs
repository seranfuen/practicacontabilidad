using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PracticaContabilidad.Model;

namespace PracticaContabilidad.Helpers
{
    /// <summary>
    ///     This helper is used to include bootstrap pagination.
    ///     We use the html tag <pagination></pagination> from inside our razor views.
    ///     Setting the property Pagination will give the information needed to generate the links.
    ///     Setting the property Action and Controller is used to generate the html link to the desired
    ///     controller and action. The controller should have a view get method that accepts an integer named page
    ///     with the page to display
    /// </summary>
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private IUrlHelper _urlHelper;

        public PaginationTagHelper(IUrlHelperFactory urlHelper)
        {
            _urlHelperFactory = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

        public Pagination Pagination { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext == null) throw new InvalidOperationException("ViewContext was expected to be injected");
            if (Pagination == null || Action == null || Controller == null)
                throw new InvalidOperationException("The necessary properties, Pagination, Action and Controller, " +
                                                    "haven't been set. Unable to generate the pagination component");

            _urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "row justify-content-center col-md-8");

            var listBuilder = new TagBuilder("ul");
            listBuilder.AddCssClass("pagination");


            for (var page = 1; page <= Pagination.TotalPages; page++) AddPageLink(page, listBuilder);

            output.Content.AppendHtml(listBuilder);
        }

        private void AddPageLink(int page, TagBuilder listBuilder)
        {
            var listElementBuilder = new TagBuilder("li");
            listElementBuilder.AddCssClass("page-item");

            if (page == Pagination.CurrentPage)
                listElementBuilder.AddCssClass("active");

            listElementBuilder.InnerHtml.AppendHtml(GenerateHtmlLinkForPage(page));
            listBuilder.InnerHtml.AppendHtml(listElementBuilder);
        }

        private TagBuilder GenerateHtmlLinkForPage(int page)
        {
            var linkBuilder = new TagBuilder("a");
            linkBuilder.Attributes["href"] = GetUrlForPage(page);
            linkBuilder.AddCssClass("page-link");
            linkBuilder.InnerHtml.Append(page.ToString());
            return linkBuilder;
        }

        private string GetUrlForPage(int page)
        {
            var url = _urlHelper.Action(new UrlActionContext
            {
                Action = Action, Controller = Controller, Values = new
                {
                    page
                }
            });
            return url;
        }
    }
}