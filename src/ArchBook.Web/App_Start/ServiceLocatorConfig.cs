using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace ArchBook.Web
{
    public class ServiceLocatorConfig
    {
        public static void RegisterServiceLocator()
        {
            var builder = new Autofac.ContainerBuilder();

            // Registers modules
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            builder.RegisterAssemblyModules(assemblies.ToArray());

            // Set the MVC dependency resolver to use Autofac
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}