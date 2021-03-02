using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace RBUtils.PaginatedList.Core
{
    public static class HtmlHelper
    {
        public static IHtmlContent PaginatedListPager(this IHtmlHelper html, IPaginatedList list, Func<int, string> generatePageUrl, string? label = "Items")
        {
            if (list.TotalItems > list.PageSize)
            {
                var container = new TagBuilder("nav");
                container.MergeAttribute("aria-label", "Page navigation");
                container.AddCssClass("pagination flex-row");

                var ul = new TagBuilder("ul");
                ul.AddCssClass("pagination");

                // First
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.First, list, generatePageUrl));

                // Previous
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Previous, list, generatePageUrl));

                // Page N
                if (list.TotalPages <= 10)
                {
                    for (var r = 1; r <= list.TotalPages; r++)
                    {
                        ul.InnerHtml.AppendHtml(SetTagPage(r, list, generatePageUrl));
                    }
                }
                else
                {
                    if (list.PageIndex <= 5)
                    {
                        for (var i = 1; i <= 7; i++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(i, list, generatePageUrl));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, generatePageUrl));

                        for (var i = list.TotalPages - 2; i <= list.TotalPages; i++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(i, list, generatePageUrl));
                        }

                    }
                    else if ((list.PageIndex > 5) && (list.PageIndex <= (list.TotalPages - 5)))
                    {
                        ul.InnerHtml.AppendHtml(SetTagPage(1, list, generatePageUrl));
                        ul.InnerHtml.AppendHtml(SetTagPage(2, list, generatePageUrl));
                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, generatePageUrl));

                        for (var u = list.PageIndex - 2; u <= list.PageIndex + 2; u++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(u, list, generatePageUrl));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, generatePageUrl));
                        ul.InnerHtml.AppendHtml(SetTagPage(list.TotalPages - 1, list, generatePageUrl));
                        ul.InnerHtml.AppendHtml(SetTagPage(list.TotalPages, list, generatePageUrl));

                    }
                    else
                    {
                        for (var b = 1; b <= 3; b++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(b, list, generatePageUrl));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, generatePageUrl));

                        for (var b = list.TotalPages - 6; b <= list.TotalPages; b++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(b, list, generatePageUrl));
                        }
                    }
                }

                // Next
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Next, list, generatePageUrl));

                // Last
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Last, list, generatePageUrl));

                container.InnerHtml.AppendHtml(SetItemPerPage(list, label, generatePageUrl(0).Split('?'), generatePageUrl(0)));
                container.InnerHtml.AppendHtml(ul);

                return container;
            }
            else
            {
                return new TagBuilder("span");
            }
        }

        private static TagBuilder SetItemPerPage(IPaginatedList list, string? label, string[] queryString, string qString)
        {
            // Pagination Dropdown
            var numberOptions = new int[] { 5, 10, 17, 25, 50, 100, 250, 500, 1000 };

            var dropdown = new TagBuilder("select");
            dropdown.MergeAttribute("name", "pageSize");
            dropdown.MergeAttribute("onChange", "this.form.submit();");
            dropdown.AddCssClass("pagination page-link");

            foreach (var opt in numberOptions)
            {
                var option = new TagBuilder("option");
                option.InnerHtml.SetHtmlContent(opt.ToString());
                option.MergeAttribute("value", opt.ToString());
                if (opt == list.PageSize)
                {
                    option.MergeAttribute("selected", "true");
                }

                dropdown.InnerHtml.AppendHtml(option);
            }

            // Pagination Form
            var paginationForm = new TagBuilder("form");
            if (queryString.Length > 0)
            {
                paginationForm.MergeAttribute("action", queryString[0]);
                paginationForm.MergeAttribute("post", "get");
                foreach (var parameter in QueryHelpers.ParseQuery(queryString[1]))
                {
                    //var p = parameter.Split('=');
                    if (parameter.Key != "pageSize" && parameter.Key != "pageNumber")
                    {
                        var i = new TagBuilder("input");
                        i.MergeAttribute("name", parameter.Key);
                        i.MergeAttribute("type", "hidden");
                        i.MergeAttribute("value", parameter.Value);

                        paginationForm.InnerHtml.AppendHtml(i);
                    }
                }
            }
            paginationForm.InnerHtml.AppendHtml(dropdown);

            // Pagination Text
            label = String.IsNullOrEmpty(label) ? "Items" : label;
            var paginationText = new TagBuilder("div");
            paginationText.AddCssClass("m-2");
            paginationText.InnerHtml.SetHtmlContent(label + " per page. Showing " + list.FirstItemOnPage.ToString("N0") + " to " + 
                list.LastItemOnPage.ToString() + " of " + list.TotalItems.ToString("N0") + " " + label + ".");

            // Main container
            var container = new TagBuilder("div");
            container.AddCssClass("pagination flex-grow-1 mb-3");

            // Put them together
            container.InnerHtml.AppendHtml(paginationForm);
            container.InnerHtml.AppendHtml(paginationText);

            return container;
        }
        private static TagBuilder SetTagPage(PageTypes t, IPaginatedList list, Func<int, string> generatePageUrl)
        {
            int pageNo = 0;
            bool disabled = false;
            var link = new TagBuilder("a");
            link.AddCssClass("page-link");

            if (t == PageTypes.First)
            {
                pageNo = 1;
                link.InnerHtml.SetHtmlContent("<i class=\"fas fa-angle-double-left\"></i>");
                disabled = list.PageIndex == 1;
            }
            else if (t == PageTypes.Previous)
            {
                pageNo = list.HasPreviousPage ? list.PageIndex - 1 : 1;
                link.InnerHtml.SetHtmlContent("<i class=\"fas fa-angle-left\"></i>");
                disabled = list.PageIndex == 1;
            }
            else if (t == PageTypes.Next)
            {
                pageNo = list.HasNextPage ? list.PageIndex + 1 : list.TotalPages;
                link.InnerHtml.SetHtmlContent("<i class=\"fas fa-angle-right\"></i>");
                disabled = list.PageIndex == list.TotalPages;
            }
            else if (t == PageTypes.Last)
            {
                pageNo = list.TotalPages;
                link.InnerHtml.SetHtmlContent("<i class=\"fas fa-angle-double-right\"></i>");
                disabled = list.PageIndex == list.TotalPages;
            }
            else if (t == PageTypes.Ellipsis)
            {
                link.InnerHtml.SetHtmlContent("<i class=\"fas fa-ellipsis-h\"></i>");
                disabled = true;
            }

            if (pageNo > 0)
            {
                link.MergeAttribute("href", generatePageUrl(pageNo));
            }

            return (SetTagWrapper(link, (disabled ? "page-item disabled" : "page-item")));
        }
        private static TagBuilder SetTagPage(int i, IPaginatedList list, Func<int, string> generatePageUrl)
        {
            var link = new TagBuilder("a");
            link.AddCssClass("page-link");
            link.MergeAttribute("href", generatePageUrl(i));
            link.InnerHtml.SetHtmlContent(i.ToString("N0"));

            return (SetTagWrapper(link, (i == list.PageIndex ? "page-item active" : "page-item")));
        }

        private static TagBuilder SetTagWrapper(TagBuilder tag, params string[] classes)
        {
            var li = new TagBuilder("li");

            foreach(var c in classes)
            {
                li.AddCssClass(c);
            }
            li.InnerHtml.AppendHtml(tag);

            return li;
        }
    }

    public enum PageTypes {
        All,
        First,
        Previous,
        Next,
        Last,
        Ellipsis,
        N
    }
}
