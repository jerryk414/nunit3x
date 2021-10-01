using Microsoft.Extensions.DependencyInjection;
using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.DependencyInjection
{
    /// <summary>
    /// Used to provide helper functionality for <see cref="Microsoft.Extensions.DependencyInjection"/> to <see cref="INxTestSuite"/>
    /// </summary>
    public interface INxInjectionTestSuite : INxTestSuite
    {
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for the current test of the current test suite
        /// </summary>
        IServiceProvider Provider { get; }

        /// <summary>
        /// Gets the base collection used to generate this <see cref="IServiceProvider"/>
        /// </summary>
        IServiceCollection Collection { get; }

        /// <summary>
        /// Shortcut method to executing <see cref="IServiceProvider.GetRequiredService{T}()"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetRequiredService<T>();
    }

}
