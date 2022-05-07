using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongPollingClientConsoleApp
{
    public interface ITestLongPolling
    {
        Task<string> GetStatusAsync(string orderNumber);

        //Task SetStatusAsync(string orderNumber, string status);

    }
}
