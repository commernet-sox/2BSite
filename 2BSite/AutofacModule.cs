using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2BSite
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(typeof(PrivilegeManagement.Controllers.HomeController).GetTypeInfo().Assembly)
            //                .Where(
            //                    t =>
            //                        typeof(Controller).IsAssignableFrom(t) &&
            //                        t.Name.EndsWith("Controller", StringComparison.Ordinal)).PropertiesAutowired();
        }
    }
}
