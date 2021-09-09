using nunit3x.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nunit3x.Suite
{
    public interface INxTestSuite
    {
        INxSuiteFactory Factory { get; }
    }

    public interface INxTestSuite<TFactory> : INxTestSuite
        where TFactory: INxSuiteFactory
    {
        new TFactory Factory { get; }
    }
}
