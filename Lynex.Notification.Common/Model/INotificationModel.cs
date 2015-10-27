using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;

namespace Lynex.Notification.Common.Model
{
    public interface INotificationModel
    {
        string Template { get;  }

        string Body { get;  }

        IUser Receiver { get;  }
    }
}
