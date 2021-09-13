using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x
{
    public class NxTestFixtureData : TestFixtureData
    {
        #region Construction

        public NxTestFixtureData(params object[] args)
            : base(args)
        {

        }

        #endregion

        #region Methods

        public static IEnumerable<NxTestFixtureData> Combinatorial(params IEnumerable[] args)
        {
            if (args != null)
            {
                CombinatorialStrategy strategy = new CombinatorialStrategy();
                foreach (ITestCaseData data in strategy.GetTestCases(args))
                {
                    yield return new NxTestFixtureData(data.Arguments);
                }
            }
        }

        #endregion
    }
}
