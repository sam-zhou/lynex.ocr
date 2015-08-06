namespace Lynex.BillMaster.Model.Enum
{
    public enum UserChallengeStatus
    {
        Mismatch = 1,
        Expired = 2,
        OverLimit = 3,
        NotFound = 4,
        Successed = 5,
        AlreadyVerified = 6,
        TryAgain = 7,
        UnknownException = 0,
    }
}
