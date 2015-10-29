using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.Common.Service.Interface
{
    public interface IChallengeProvider
    {
        string GenerateNewChallenge();
    }
}
