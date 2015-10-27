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
    public class GetUserByEmail : IGetItemQuery<ApplicationUser>
    {
        private readonly string _email;

        public GetUserByEmail(string email)
        {
            _email = email;
        }

        public ApplicationUser Execute(ISession session)
        {
            return session.Query<ApplicationUser>().FirstOrDefault(x => x.UserName == _email);
        }
    }
}
