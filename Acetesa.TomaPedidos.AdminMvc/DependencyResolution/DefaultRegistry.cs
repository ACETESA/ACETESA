// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Web;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Web;

namespace Acetesa.TomaPedidos.AdminMvc.DependencyResolution
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<IProductoService>();
                    scan.With(new ControllerConvention());
                    scan.WithDefaultConventions();
                    scan.Assembly("Acetesa.TomaPedidos.DataEntityFramework");
                    scan.Assembly("Acetesa.TomaPedidos.Repository");
                    scan.LookForRegistries();
                });

            //For<ApplicationDbContext>().HttpContextScoped();
            //For<BundleCollection>().Use(BundleTable.Bundles);
            //For<RouteCollection>().Use(RouteTable.Routes);
            //For<IIdentity>().Use(() => HttpContext.Current.User.Identity);
            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            //For<HttpSessionStateBase>()
            //    .Use(() => new HttpSessionStateWrapper(HttpContext.Current.Session));
            //For<HttpContextBase>()
            //    .Use(() => new HttpContextWrapper(HttpContext.Current));
            //For<HttpServerUtilityBase>()
            //    .Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            For<IAuthenticationManager>()
                .Use(() => HttpContext.Current.GetOwinContext().Authentication);

            //Policies.SetAllProperties(prop =>
            //    prop.NameMatches(name => name.EndsWith("Service")
            //        ));
            Policies.SetAllProperties(prop =>
                prop.WithAnyTypeFromNamespaceContainingType<IProductoService>());
            

        }

        #endregion
    }
}