﻿using Moq;
using NUnit3x.Factory;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Suite
{
    public interface INxTestSuite
    {
        INxSuiteFactory Factory { get; }

        IImmutableDictionary<Guid, IConcurrentProperty> ConcurrentProperties { get; }

        /// <summary>
        /// Lazily creates or retrieves a singleton instance of <see cref="Mock{TType}"/>
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="index">Specify the key of the mock to retrieve to allow for multiple instances of the same type to be used in the same test case</param>
        /// <returns>The singleton instance of <see cref="Mock{TType}"/> for the current test case</returns>
        Mock<TType> LazyMock<TType>(int key = 0) where TType: class;
    }

    public interface INxTestSuite<TFactory> : INxTestSuite
        where TFactory: INxSuiteFactory
    {
        new TFactory Factory { get; }
    }
}
