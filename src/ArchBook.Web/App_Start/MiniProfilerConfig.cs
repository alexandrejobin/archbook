using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchBook.Web
{
    public static class MiniProfilerConfig
    {
        public static void RegisterMiniProfiler()
        {
            MiniProfiler.Configure(new MiniProfilerOptions()
            {
                PopupShowTrivial = true,
                PopupShowTimeWithChildren = true
            }).AddViewPofiling();

            MiniProfilerEF6.Initialize();
        }
    }
}