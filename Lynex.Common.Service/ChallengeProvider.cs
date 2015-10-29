using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Service.Interface;

namespace Lynex.Common.Service
{
    public class ChallengeProvider: IChallengeProvider
    {
        public string GenerateNewChallenge()
        {
            return Guid.NewGuid().ToString("n");
        }
    }
}
