﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace LongPollingClientConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            var testLongPolling = ActivatorUtilities.CreateInstance<TestLongPolling>(host.Services);
            var orderNumber = "123";
            Console.WriteLine(await testLongPolling.GetStatusAsync(orderNumber));
            Console.ReadKey();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<ITestLongPolling, TestLongPolling>();
            });
    }
}
