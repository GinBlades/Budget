using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Web.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string AppendQueryString(this IUrlHelper helper, HttpRequest request, string orderBy)
        {
            var queryDictionary = MakeQueryDictionary(request.QueryString.ToString());
            SetDirection(queryDictionary);
            queryDictionary["OrderBy"] = orderBy;
            string newQuery = string.Join("&", queryDictionary.Select(d => $"{d.Key}={d.Value}"));
            return $"{request.Path}?{newQuery}";
        }

        private static Dictionary<string, string> MakeQueryDictionary(string queryString)
        {
            var parts = queryString.Trim('?').Split('&');
            var queryDictionary = new Dictionary<string, string>();
            if (parts.Length > 1)
            {
                foreach (var part in parts)
                {
                    var keyValue = part.Split('=');
                    queryDictionary[keyValue[0]] = keyValue[1];
                }
            }
            return queryDictionary;
        }

        private static void SetDirection(Dictionary<string, string> queryDictionary)
        {
            var dir = queryDictionary.ContainsKey("OrderDirection") ? queryDictionary["OrderDirection"] : string.Empty;
            if (dir == string.Empty)
            {
                dir = "ASC";
            }
            else if (dir == "ASC")
            {
                dir = "DESC";
            }
            else if (dir == "DESC")
            {
                dir = "ASC";
            }
            queryDictionary["OrderDirection"] = dir;
        }
    }
}
