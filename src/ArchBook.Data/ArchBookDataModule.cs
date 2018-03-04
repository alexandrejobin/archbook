using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ArchBook.Data
{
    class ArchBookDataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BookDbContext>().InstancePerRequest();
        }
    }
}
