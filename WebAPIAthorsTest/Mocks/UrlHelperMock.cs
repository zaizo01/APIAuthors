﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIAthorsTest.Mocks
{
    class UrlHelperMock : IUrlHelper
    {
        public ActionContext ActionContext => throw new NotImplementedException();

        public string Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        public string Content(string contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string Link(string routeName, object values)
        {
            return "";
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            throw new NotImplementedException();
        }
    }
}
