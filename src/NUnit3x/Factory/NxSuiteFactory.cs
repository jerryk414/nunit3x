using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Factory
{
    public abstract class NxSuiteFactory<TSuite> : INxSuiteFactory<TSuite>
        where TSuite: INxTestSuite
    {
        #region Construction

        public NxSuiteFactory(TSuite suite)
        {
            this.Suite = suite;
        }

        #endregion

        #region Properties

        public TSuite Suite { get; }

        INxTestSuite INxSuiteFactory.Suite => this.Suite;

        #endregion
    }
}
