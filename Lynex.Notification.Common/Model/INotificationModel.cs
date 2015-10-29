using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Notification.Common.Model
{
    public interface INotificationModel
    {
        string Template { get;  }

        string Body { get;  }

        IUser Receiver { get;  }
    }
}
