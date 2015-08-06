namespace Lynex.BillMaster.Model.Enum
{
    public enum UserLoginStatus
    {
        UnknownException = 0,
        PasswordMismatch = 1,
        UserDoesNotExist = 2,
        OverTryLimit = 3,
        Unverified = 4,
        UserDisabled = 5,
        Successed = 6,
    }
}
