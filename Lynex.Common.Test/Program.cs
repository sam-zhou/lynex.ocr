using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Admin.Service;
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

                var userService = new UserService(db);
                userService.RegisterUser("samzhou.it@gmail.com", "jukfrg", "0430501022", "zhou", "sam");
                var user = db.Get<User>(1);

                //db.Save(user);
                //db.Save(challenge);
                //db.Save(wallet);
                var billService = new BillService(db);
                var addressService = new AddressService(db);

                var adminBillService = new AdminBillService(db, addressService);

                var billCompany = adminBillService.CreateBillCompany("Telestra", new Address
                {
                    AddressLine1 = "8 Arklow Glen",
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Country = "Australia",
                    PostCode = "6155",
                    State = "Western Australia",
                    Suburb = "Canning Vale"
                });

                




                //userService.CreateNewChallenge(1);

                var challengeStatus = userService.ChallengeUser(1, "51BC9AD3F3C69A3511AC930BA6493587E3A738EEBF207A14912463286FAF06540D4F620E085AFB975B7FCDD7D1436E75C69FBBC7B6E444D2BD5F6CBA2A9B979A");
                Console.WriteLine("Challenge 1:" + challengeStatus);

                challengeStatus = userService.ChallengeUser(1, "849749A7F94C35401EE233757095201B150998DF7A49A8335653B533E7A0A87EBA80260831391E754A67AE4058A4274D9A2837ADD22646535D5ACDFA0FC72070");
                Console.WriteLine("Challenge 2:" + challengeStatus);

                challengeStatus = userService.ChallengeUser(1, "30A44230602E1E0EFC31493E1E480B10A432C327D3E92FE903986D3EA45B4DC6C8601563AB1013EE8C4369152064656B96E5074F8100AD1B94B7AF7DE4C5AB53");
                Console.WriteLine("Challenge 3:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 4:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 5:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 6:" + challengeStatus);


                var loginstatus = userService.Login("samzhou.it@gmail.com", "jukfrg");
                Console.WriteLine("Login:" + loginstatus);

                //var newUser = db.Get<User>(1);

                //Console.WriteLine(newUser.Email);

                //var newChallenge = newUser.UserChallenge;
            }

                

            Console.ReadLine();
        }
    }
}
