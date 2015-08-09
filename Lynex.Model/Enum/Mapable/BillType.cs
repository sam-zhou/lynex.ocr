using System;

namespace Lynex.BillMaster.Model.Enum.Mapable
{
    [Flags]
    public enum BillType
    {
        Unknown = 0,
        Electricity = 1,
        Gas = 2,
        Solar = 4,
        Water = 8,
        LandLine = 16,
        Mobile = 32,
        Internet = 64,
        CouncilRate = 128,
        Mortgage = 256,
    }
}
