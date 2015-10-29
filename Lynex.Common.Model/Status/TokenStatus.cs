using System.ComponentModel;

namespace Lynex.Common.Model.Status
{
    public enum TokenStatus
    {
        [Description("Token Expired")]
        Expired = 0,

        [Description("Unknown")]
        Unknown = 1,

        [Description("Invalid Username Or Password")]
        Invalid = 2,

        [Description("User Does Not Exist")]
        InvalidUser = 3,

        [Description("Invalid Password")]
        InvalidPassword = 4,

        [Description("User Is Disabled")]
        Disabled = 5,

        [Description("Email Is Not Verified")]
        Unverified = 6,

        [Description("Client Id is not registered in the system")]
        InvalidClientId = 7,

        [Description("Client secret should be sent")]
        MissingClientSecret = 8,

        [Description("Client secret is invalid")]
        InvalidClientSecret = 9,

        [Description("Client is inactive")]
        DisabledClient = 10,
    }
}
