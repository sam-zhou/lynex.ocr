using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class PermissionRole: BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual bool AccessMyDosage { get; set; }

        public virtual bool AccessMyHistory { get; set; }

        public virtual bool ManageMyAccount { get; set; }

        public virtual bool ManageUsers { get; set; }

        public virtual bool AlterPreRequisites { get; set; }

        public static PermissionRole User = new PermissionRole
        {
            Id = 2,
            Name = "User",
            AccessMyDosage = true,
            AccessMyHistory = true,
            ManageMyAccount = true,
        };

        public static PermissionRole Admin = new PermissionRole
        {
            Id = 1,
            Name = "Admin",
            //AccessMyDosage = true,
            //AccessMyHistory = true,
            ManageMyAccount = true,
            ManageUsers = true,
            AlterPreRequisites = true,
        };
    }
}
