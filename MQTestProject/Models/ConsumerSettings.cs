using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTestProject.Models
{
    public class ConsumerSettings
    {
        public string City { get;set; }
        public string Temperature { get;set; }

        public void Init()
        {
            Console.WriteLine("Enter city name for subscribe: ");
            City = Console.ReadLine() ?? String.Empty;
            Console.WriteLine("Enter temperature for subscribe: ");
            Temperature = Console.ReadLine() ?? String.Empty;
        }
    }
}
