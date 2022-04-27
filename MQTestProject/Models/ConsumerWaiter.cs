using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTestProject.Models
{
    public static class ConsumerWaiter
    {
        static AutoResetEvent semaphore = new AutoResetEvent(false);

        public static async Task Run()
        {
            Task.Run(Exit);
            semaphore.WaitOne();
        }
        private static async Task Exit()
        {
            try
            {
                Console.WriteLine("Press enter to stop server");

                await Task.Run(() => Console.ReadLine());
                semaphore.Set();
            }
            finally
            {
                semaphore.Close();
            }
        }
    }
}
