using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Service;
using Lynex.Common.Database;
using Lynex.Common.Service;
using Microsoft.Win32;

namespace Lynex.Common.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DatabaseService("DefaultConnectionString", "Lynex.BillMaster.Model"))
            {
                var service = new SystemService(db);

                service.ResetDatabase();

                //var user = new User { Email = "samzhou.it@gmail.com", Mobile = "0430501022" };

                //var challenge = new UserChallenge("test") {User = user};

                

                //var wallet = new Wallet {User = user};



                //db.Save(user);
                //db.Save(challenge);
                //db.Save(wallet);


                var userService = new UserService(db);
                userService.RegisterUser("samzhou.it@gmail.com", "jukfrg", "0430501022");


                //var newUser = db.Get<User>(1);

                //Console.WriteLine(newUser.Email);

                //var newChallenge = newUser.UserChallenge;
            }

                

            Console.ReadLine();
        }
    }
}
