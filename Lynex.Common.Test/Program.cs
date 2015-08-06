using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Service;
using Lynex.Database.Common;

namespace Lynex.Common.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseService("DefaultConnectionString", "Lynex.BillMaster.Model");

            var service = new SystemService(db);

            service.ResetDatabase();

            Console.ReadLine();
        }
    }
}
