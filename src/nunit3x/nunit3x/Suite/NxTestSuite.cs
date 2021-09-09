using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace nunit3x.Suite
{
    public abstract class NxTestSuite
    {
        /// <summary>
        /// Asynchronous one time setup method called before any tests are executed for the current suite.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnOneTimeSetupAsync() => Task.CompletedTask;

        /// <summary>
        /// Asynchronous one time teardown method called before any tests are executed for the current suite.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnOneTimeTeardownAsync() => Task.CompletedTask;

        /// <summary>
        /// Test case setup method called prior to each individual test case.
        /// </summary>
        protected abstract void OnTestCaseSetup();

        /// <summary>
        /// Test case teardown method called after each individual test case.
        /// </summary>
        protected abstract void OnTestCaseTeardown();
    }

    public abstract class NxTestSuite<TType> : NxTestSuite, INxTestSuite
    {
        #region Construction

        #endregion

        #region Methods

        [SetUp]
        protected void TestCaseSetup()
        {
            this.OnTestCaseSetup();
        }

        [TearDown]
        protected void TestCaseTeardown()
        {
            this.OnTestCaseTeardown();
        }

        [OneTimeSetUp]
        protected async Task OneTimeSetupAsync()
        {
            await this.OnOneTimeSetupAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        protected async Task OneTimeTeardownAsync()
        {
            await this.OnOneTimeTeardownAsync().ConfigureAwait(false);
        }

        protected override void OnTestCaseSetup() => Expression.Empty();
        protected override void OnTestCaseTeardown() => Expression.Empty();

        #endregion
    }

    public class Test : NxTestSuite<object>
    {

    }
}
