using System;

namespace WCC.Services.Helpers
{
    public static class StringHelper
    {
        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
