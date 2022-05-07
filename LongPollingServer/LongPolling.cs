using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LongPollingServer
{
    public class LongPolling
    {
        const int Timeout = 20000; // TO DO: let client set this value!

        private static List<LongPolling> s_Subscribers = new List<LongPolling>();

        private TaskCompletionSource<bool> _taskCompletion = new TaskCompletionSource<bool>();

        private string OrderNumber { get; set; }
        private string Status { get; set; }

        public LongPolling(string orderNumber)
        {
            OrderNumber = orderNumber;
            lock (s_Subscribers)
            {
                s_Subscribers.Add(this);
            }
        }

        private void Notify(string status)
        {
            Status = status;
            _taskCompletion.SetResult(true);
        }

        public async Task<string> GetStatusAsync()
        {
            await Task.WhenAny(_taskCompletion.Task, Task.Delay(Timeout));
            lock (s_Subscribers)
            {
                s_Subscribers.Remove(this);
            }
            return Status;
        }

        public static void SetStatus(string orderNumber, string status)
        {
            lock (s_Subscribers)
            {
                var subscriber = s_Subscribers.ToList();
                foreach (var poll in subscriber)
                {
                    if (poll.OrderNumber == orderNumber)
                        poll.Notify(status);
                }
            }
        }
    }
}
