﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Database.Common;

namespace Lynex.Common.Service
{
    public abstract class BaseService
    {
        protected BaseService(IDatabaseService dbService)
        {
            DatabaseService = dbService;
        }
        protected IDatabaseService DatabaseService { get; }
    }
}