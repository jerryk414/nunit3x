using NUnit3x.DependencyInjection;
using NUnit3x.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests.DependencyInjection
{
    class NxInjectionSuiteFactory : NxSuiteFactory<INxInjectionTestSuite>
    {
        public NxInjectionSuiteFactory(INxInjectionTestSuite suite) : base(suite)
        {
        }
    }
}
