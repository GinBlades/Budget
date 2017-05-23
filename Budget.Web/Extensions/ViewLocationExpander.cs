﻿using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Web.Extensions
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string[] locations = new string[] { "../Budget.Identity/Views/{1}/{0}.cshtml" };
            return locations.Union(viewLocations);
        }
        
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customViewLocation"] = nameof(ViewLocationExpander);
        }
    }
}
