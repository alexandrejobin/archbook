using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ArchBook.Services
{
    class ArchBookServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // register every services in this assembly
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"));
        }
    }
}
