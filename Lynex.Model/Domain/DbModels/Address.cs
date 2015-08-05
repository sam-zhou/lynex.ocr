﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.Model.Domain.DbModels
{
    public class Address : BaseEntity
    {
        public virtual string AddressLine1 { get; set; }

        public virtual string AddressLine2 { get; set; }

        public virtual string AddressLine3 { get; set; }

        public virtual string PostCode { get; set; }

        public virtual string Suburb { get; set; }

        public virtual string State { get; set; }

        public virtual string Country { get; set; }
    }
}