using Moq;
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
        /// <summary>
        /// Determines whether logging is enabled (<see cref="true"/> by default)
        /// </summary>
        bool LoggingEnabled { get; }

        INxSuiteFactory Factory { get; }

        IImmutableDictionary<Guid, IConcurrentProperty> ConcurrentProperties { get; }

        /// <summary>
        /// Lazily creates or retrieves the default singleton instance of <see cref="Mock{TType}"/>
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns>The singleton instance of <see cref="Mock{TType}"/> for the current test case</returns>
        Mock<TType> LazyMock<TType>() where TType : class;

        /// <summary>
        /// Lazily creates or retrieves a singleton instance of <see cref="Mock{TType}"/>
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="key">Specify the key of the mock to retrieve to allow for multiple instances of the same type to be used in the same test case</param>
        /// <returns>The singleton instance of <see cref="Mock{TType}"/> for the current test case</returns>
        Mock<TType> LazyMock<TType>(int key) where TType: class;

        /// <summary>
        /// Logs the given <paramref name="message"/> to the console
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);
    }

    public interface INxTestSuite<TFactory> : INxTestSuite
        where TFactory: INxSuiteFactory
    {
        new TFactory Factory { get; }
    }
}
