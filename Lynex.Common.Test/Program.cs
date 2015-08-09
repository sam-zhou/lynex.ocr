﻿using System;
using Lynex.BillMaster.Admin.Service;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.BillMaster.Service;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DatabaseService("DefaultConnectionString", "Lynex.BillMaster"))
            {
                var service = new SystemService(db);

                service.ResetDatabase();

                //var user = new User { Email = "samzhou.it@gmail.com", Mobile = "0430501022" };

                //var challenge = new UserChallenge("test") {User = user};



                //var wallet = new Wallet {User = user};

               
                
                
                //db.Save(user);
                //db.Save(challenge);
                //db.Save(wallet);
                var billService = new BillService(db);
                var addressService = new AddressService(db);
                var userService = new UserService(db, addressService);
                //userService.RegisterUser("samzhou.it@gmail.com", "jukfrg", "0430501022", "zhou", "sam");
                var user = db.Get<User>(1);


                

                var adminBillService = new AdminBillService(db, addressService);
                var billCompany = db.Get<BillingCompany>(1);
                //userService.CreateAddress(user, new Address
                //{
                //    AddressLine1 = "8 Arklow Glen",
                //    AddressLine2 = "",
                //    AddressLine3 = "",
                //    Country = "Australia",
                //    PostCode = "6155",
                //    State = "Western Australia",
                //    Suburb = "Canning Vale"
                //});

                billService.CreateBill(new Bill
                {
                    Amount = 100,
                    BillType = BillType.Gas,
                    Company = billCompany,
                    DueDate = new DateTime(2015,10, 1),
                    IssueDate = new DateTime(2015,8,8),
                    User = user
                });
                //billCompany.BillTypes = BillType.LandLine | BillType.Internet;
                //adminBillService.UpdateBillCompany(billCompany);
                var t = billCompany;
                //var billCompany = adminBillService.CreateBillCompany("Telestra", BillType.Gas | BillType.Electricity | BillType.Mortgage, new Address
                //{
                //    AddressLine1 = "8 Arklow Glen",
                //    AddressLine2 = "",
                //    AddressLine3 = "",
                //    Country = "Australia",
                //    PostCode = "6155",
                //    State = "Western Australia",
                //    Suburb = "Canning Vale"
                //});


                //billCompany.BillTypes = BillType.Gas | BillType.Electricity;
                //billCompany.Name = "haha";

                //adminBillService.UpdateBillCompany(billCompany);



                //userService.CreateNewChallenge(1);

                var challengeStatus = userService.ChallengeUser(1, "51BC9AD3F3C69A3511AC930BA6493587E3A738EEBF207A14912463286FAF06540D4F620E085AFB975B7FCDD7D1436E75C69FBBC7B6E444D2BD5F6CBA2A9B979A");
                System.Console.WriteLine("Challenge 1:" + challengeStatus);

                challengeStatus = userService.ChallengeUser(1, "849749A7F94C35401EE233757095201B150998DF7A49A8335653B533E7A0A87EBA80260831391E754A67AE4058A4274D9A2837ADD22646535D5ACDFA0FC72070");
                System.Console.WriteLine("Challenge 2:" + challengeStatus);

                challengeStatus = userService.ChallengeUser(1, "30A44230602E1E0EFC31493E1E480B10A432C327D3E92FE903986D3EA45B4DC6C8601563AB1013EE8C4369152064656B96E5074F8100AD1B94B7AF7DE4C5AB53");
                System.Console.WriteLine("Challenge 3:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 4:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 5:" + challengeStatus);

                //challengeStatus = userService.ChallengeUser(1, "jukfrg");
                //Console.WriteLine("Challenge 6:" + challengeStatus);


                var loginstatus = userService.Login("samzhou.it@gmail.com", "jukfrg");
                System.Console.WriteLine("Login:" + loginstatus);

                //var newUser = db.Get<User>(1);

                //Console.WriteLine(newUser.Email);

                //var newChallenge = newUser.UserChallenge;
            }

                

            System.Console.ReadLine();
        }
    }
}
