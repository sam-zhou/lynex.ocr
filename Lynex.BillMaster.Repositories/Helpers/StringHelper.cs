using System;

namespace WCC.Repositories.Helpers
{
    internal static class StringHelper
    {
        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
