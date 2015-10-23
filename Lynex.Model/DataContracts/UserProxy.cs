using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Model.DataContracts;

namespace Lynex.BillMaster.Model.DataContracts
{
    public class UserProxy : IPrincipal
    {
        private IIdentity m_identity;

        internal UserProxy(IPrincipal user)
        {
            
            m_identity = new GenericIdentity(user.Identity.Name, user.Identity.AuthenticationType);
            
        }


        private UserProxy()
        {

        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity
        {
            get { return m_identity; }
        }
    }
}
