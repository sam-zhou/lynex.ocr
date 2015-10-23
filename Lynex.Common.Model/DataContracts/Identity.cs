using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.Common.Model.DataContracts
{
    public class Identity : IIdentity
    {
        public Identity(bool isAuthenticated, string name)
        {
            IsAuthenticated = isAuthenticated;
            Name = name;
        }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }
    }
}
