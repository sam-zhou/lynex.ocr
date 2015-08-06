using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using NHibernate;
using NHibernate.Linq;

namespace Lynex.BillMaster.Repository.UserRepo
{
    public class IsEmailUnique: IGetItemQuery<bool>
    {
        private readonly string _email;

        public IsEmailUnique(string email)
        {
            _email = email;
        }

        public bool Execute(ISession session)
        {
            return !session.Query<User>().Any(x => x.Email == _email);
        }
    }
}
