using System.Collections.Generic;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;
using WCC.Repositories.Interface.Repositories;
using WCC.Services.Interface;

namespace WCC.Services
{
    public class TestResultService : BaseService, ITestResultService
    {
        public TestResultService(IWCCMainRepository wccMainRepository) : base(wccMainRepository)
        {
        }

        public IEnumerable<TestResult> GetTestResultsByUser(User user)
        {
            return WCCMainRepository.GetTestResultsByUser(user);
        }

        public TestResult GetCurrentTestResultByUser(User user)
        {
            return WCCMainRepository.GetCurrentTestResultByUser(user);
        }

        public TestResult CreateTestResult(User user, double pt, double inr, int labId)
        {
            return WCCMainRepository.CreateTestResult(user, pt, inr, labId);
        }

        public void ViewTestResult(TestResult testResult)
        {
            WCCMainRepository.ViewTestResult(testResult);
        }
    }
}
