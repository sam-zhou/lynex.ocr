using System.Collections.Generic;
using WCC.Model.Domain.DbModels;

namespace WCC.Repositories.Interface.Repositories
{
    public interface ITestResultRepository
    {
        IEnumerable<TestResult> GetTestResultsByUser(User user);

        TestResult GetCurrentTestResultByUser(User user);

        TestResult CreateTestResult(User user, double pt, double inr, int labId);

        void ViewTestResult(TestResult testResult);
    }
}
