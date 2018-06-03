using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace WEBAPI_VENDINGMACHINE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "ActionDivertedApi",
            //    routeTemplate: "api/{controller}/{action}/{id}/{od}",
            //    defaults: new {id = RouteParameter.Optional , od = RouteParameter.Optional}
            //);

            config.Routes.MapHttpRoute(
                name: "NewSale",
                routeTemplate: "api/{controller}/{action}/{idMachine}/{tokenKey}/{idProduct}/{slotNumber}/{oneCent}-{twoCents}-{fiveCents}-{tenCents}-{twentyCents}-{fiftyCents}-{oneEur}-{twoEur}",
                defaults: new { }
            );

            config.Routes.MapHttpRoute(
               name: "Getprice",
               routeTemplate: "api/{controller}/{action}/{idMachine}/{tokenId}/{idSlot}/{idProduct}",
               defaults: new {}
           );

            config.Routes.MapHttpRoute(
              name: "GetUpdateSlots",
              routeTemplate: "api/{controller}/{action}/{idMachine}/{tokenId}",
              defaults: new { }
          );
        }
    }
}
