using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RBUtils.PaginatedList.Core
{
    public static class GenerateUrl
    {
        public static string FromArea(string area, string page, object values)
        {
            return FromUri("/" + area + page, values.ToDictionary());
        }
        public static string FromUri(string uri, object values)
        {
            return GetUrl(uri, values.ToDictionary());
        }
        private static string GetUrl(string uri, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    if ((parameter.Value as string == null) && parameter.Value is IEnumerable values)
                    {
                        foreach (var value in values)
                        {
                            if (value.ToString().Trim() != "")
                            {
                                uri = QueryHelpers.AddQueryString(uri, parameter.Key, value.ToString());
                            }
                        }
                    }
                    else if (parameter.Value.ToString().Trim() != "")
                    {
                        uri = QueryHelpers.AddQueryString(uri, parameter.Key, parameter.Value.ToString());
                    }
                }
            }

            return uri;
        }
        private static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(r => r.Name, r => r.GetValue(source));
        }
    }
}
