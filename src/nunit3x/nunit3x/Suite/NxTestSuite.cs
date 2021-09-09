using NUnit.Framework;
using nunit3x.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace nunit3x.Suite
{
    public abstract class NxTestSuite<TFactory> : INxTestSuite<TFactory>
        where TFactory : class, INxSuiteFactory
    {
        /// <summary>
        /// Gets the <see cref="TFactory"/>
        /// </summary>
        public abstract TFactory Factory { get; }
        INxSuiteFactory INxTestSuite.Factory => this.Factory;

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
        protected virtual void OnTestCaseSetup() => Expression.Empty();

        /// <summary>
        /// Test case teardown method called after each individual test case.
        /// </summary>
        protected virtual void OnTestCaseTeardown() => Expression.Empty();
    }

    public abstract class NxTestSuite<TFactory, TSuite> : NxTestSuite<TFactory>
        where TFactory : NxSuiteFactory<TSuite>
        where TSuite : class, INxTestSuite
    {
        #region Construction

        public NxTestSuite()
        {
            if (!(this is TSuite)) 
            {
                throw new ArgumentException($"Suite '{ this.GetType() }' must be an instance of '{ typeof(TSuite) }' in order to be used");
            }

            this.Factory = (TFactory)Activator.CreateInstance(typeof(TFactory), this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="TFactory"/>
        /// </summary>
        public override TFactory Factory { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Test case setup decorated with the <see cref="SetUpAttribute"/>
        /// </summary>
        [SetUp]
        protected void TestCaseSetup()
        {
            this.OnTestCaseSetup();
        }

        /// <summary>
        /// Test case setup decorated with the <see cref="TearDownAttribute"/>
        /// </summary>
        [TearDown]
        protected void TestCaseTeardown()
        {
            this.OnTestCaseTeardown();
        }

        /// <summary>
        /// One time setup decorated with the <see cref="OneTimeSetUpAttribute"/>
        /// </summary>
        [OneTimeSetUp]
        protected async Task OneTimeSetupAsync()
        {
            await this.OnOneTimeSetupAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// One time teardown decorated with the <see cref="OneTimeTearDownAttribute"/>
        /// </summary>
        [OneTimeTearDown]
        protected async Task OneTimeTeardownAsync()
        {
            await this.OnOneTimeTeardownAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
