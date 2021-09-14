using NUnit3x.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests
{
    public class SuiteFactory : NxSuiteFactory<TestSuite>
    {
        #region Construction

        public SuiteFactory(TestSuite suite) 
            : base(suite)
        {
        }

        #endregion
    }
}
