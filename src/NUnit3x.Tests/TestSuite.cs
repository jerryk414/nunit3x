using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests
{
    public class TestSuite : NxTestSuite<SuiteFactory, TestSuite>
    {
        #region Methods

        protected override IEnumerable<IConcurrentProperty> GetConcurrentPropertyDefaults()
        {
            yield break;
        }

        #endregion
    }
}
