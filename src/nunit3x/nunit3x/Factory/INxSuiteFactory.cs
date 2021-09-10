﻿using nunit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nunit3x.Factory
{
    public interface INxSuiteFactory
    {
        /// <summary>
        /// Gets the <see cref="INxTestSuite"/>
        /// </summary>
        INxTestSuite Suite { get; }
    }

    public interface INxSuiteFactory<TSuite> : INxSuiteFactory
        where TSuite : class, INxTestSuite
    {
        /// <summary>
        /// Gets the <see cref="TSuite"/>
        /// </summary>
        new TSuite Suite { get; }
    }
}