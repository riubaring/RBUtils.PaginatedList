using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RBUtils.PaginatedList.Core
{
    public static class HtmlHelper
    {
        public static IHtmlContent PaginatedListPager(this IHtmlHelper html, IPaginatedList list, string area, string page, IDictionary<string, object> values, string? label = "Items") 
        {
            return SetListPager(list, GetUrl("/" + area + page, values), label);
        }
        public static IHtmlContent PaginatedListPager(this IHtmlHelper html, IPaginatedList list, Func<int, string> generatePageUrl, string? label = "Items")
        {
            return SetListPager(list, GetUrl(generatePageUrl), label);
        }

        private static IHtmlContent SetListPager(IPaginatedList list, string url, string label)
        { 
            if (list.TotalItems > list.PageSize)
            {
                var container = new TagBuilder("nav");
                container.MergeAttribute("aria-label", "Page navigation");
                container.AddCssClass("pagination flex-row");

                var ul = new TagBuilder("ul");
                ul.AddCssClass("pagination");

                // First
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.First, list, url));

                // Previous
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Previous, list, url));

                // Page N
                if (list.TotalPages <= 10)
                {
                    for (var r = 1; r <= list.TotalPages; r++)
                    {
                        ul.InnerHtml.AppendHtml(SetTagPage(r, list, url));
                    }
                }
                else
                {
                    if (list.PageIndex <= 5)
                    {
                        for (var i = 1; i <= 7; i++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(i, list, url));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, url));

                        for (var i = list.TotalPages - 2; i <= list.TotalPages; i++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(i, list, url));
                        }

                    }
                    else if ((list.PageIndex > 5) && (list.PageIndex <= (list.TotalPages - 5)))
                    {
                        ul.InnerHtml.AppendHtml(SetTagPage(1, list, url));
                        ul.InnerHtml.AppendHtml(SetTagPage(2, list, url));
                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, url));

                        for (var u = list.PageIndex - 2; u <= list.PageIndex + 2; u++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(u, list, url));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, url));
                        ul.InnerHtml.AppendHtml(SetTagPage(list.TotalPages - 1, list, url));
                        ul.InnerHtml.AppendHtml(SetTagPage(list.TotalPages, list, url));

                    }
                    else
                    {
                        for (var b = 1; b <= 3; b++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(b, list, url));
                        }

                        ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Ellipsis, list, url));

                        for (var b = list.TotalPages - 6; b <= list.TotalPages; b++)
                        {
                            ul.InnerHtml.AppendHtml(SetTagPage(b, list, url));
                        }
                    }
                }

                // Next
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Next, list, url));

                // Last
                ul.InnerHtml.AppendHtml(SetTagPage(PageTypes.Last, list, url));

                container.InnerHtml.AppendHtml(SetItemPerPage(list, label, url.Split('?')));
                container.InnerHtml.AppendHtml(ul);

                return container;
            }
            else
            {
                return new TagBuilder("span");
            }
        }

        private static TagBuilder SetItemPerPage(IPaginatedList list, string? label, string[] queryString)
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
            paginationText.InnerHtml.SetHtmlContent(label + " per page.");
            //Showing " + list.FirstItemOnPage.ToString("N0") + " to " + 
            //    list.LastItemOnPage.ToString() + " of " + list.TotalItems.ToString("N0") + " " + label + ".");

            // Main container
            var container = new TagBuilder("div");
            container.AddCssClass("pagination flex-grow-1 mb-3");

            // Put them together
            container.InnerHtml.AppendHtml(paginationForm);
            container.InnerHtml.AppendHtml(paginationText);

            return container;
        }
        private static TagBuilder SetTagPage(PageTypes t, IPaginatedList list, string url)
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
                link.MergeAttribute("href", url + "&pageNumber=" + pageNo);
            }

            return (SetTagWrapper(link, (disabled ? "page-item disabled" : "page-item")));
        }
        private static TagBuilder SetTagPage(int i, IPaginatedList list, string url)
        {
            var link = new TagBuilder("a");
            link.AddCssClass("page-link");
            link.MergeAttribute("href", url + "&pageNumber=" + i);
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
        private static string GetUrl(string uri, IDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                if(parameter.Value != null)
                {
                    if((parameter.Value as string == null) && parameter.Value is IEnumerable values)
                    {
                        foreach(var value in values)
                        {
                            if(value.ToString().Trim() != "")
                            {
                                uri = QueryHelpers.AddQueryString(uri, parameter.Key, value.ToString());
                            }
                        }
                    }
                    else if(parameter.Value.ToString().Trim() != "")
                    {
                        uri = QueryHelpers.AddQueryString(uri, parameter.Key, parameter.Value.ToString());
                    }
                }
            }

            return uri;
        }
        private static string GetUrl(Func<int, string> generatePageUrl)
        {
            var uri = generatePageUrl(0).Split('?');
            return uri[0] + "?" + QueryHelpers.ParseQuery(uri[1]).Remove("pageNumber");
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
