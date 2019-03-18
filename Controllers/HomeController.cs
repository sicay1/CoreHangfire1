using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreHangfire1.Models;
using Hangfire;
using System.ComponentModel;
using System.Threading;
using Hangfire.Console;
using Hangfire.Server;

namespace CoreHangfire1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            RecurringJob.AddOrUpdate("tenGi2323Do", () => hamgido(null), Cron.MinuteInterval(1), null, "queue1111");
            return View();
        }

        //[AutomaticRetry(Attempts = 3)]
        //[DisplayName("Super Xayda!")]
        //public void hamgido()
        //{
        //    hamgido(null);
        //}

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("Super Xayda!")]
        public static void hamgido(PerformContext context)
        {
            var pgbar = context.WriteProgressBar();
            //ConsoleExtensions.WriteLine(PerformContext);
            context.WriteLine("Job started");
            context.WriteLine("123123123");
            context.WriteLine("begin sleep23123123");
            Thread.Sleep(2000);
            pgbar.SetValue(20);
            Thread.Sleep(8000);
            pgbar.SetValue(100);
            context.WriteLine("after sleep");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
