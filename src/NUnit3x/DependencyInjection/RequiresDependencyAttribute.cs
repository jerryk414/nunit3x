using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.DependencyInjection
{
    /// <summary>
    /// When using <see cref=""/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RequiresDependencyAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// Marks a test suite as requiring the specific dependency. The dependency will be added to the <see cref="IServiceProvider"/>
        /// for the test suite.
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="ServiceType"/> is null</exception>
        public RequiresDependencyAttribute(Type type, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceType = type ?? throw new ArgumentNullException("Type must not be null");
            Lifetime = serviceLifetime;
        }

        #endregion

        #region Properties

        public Type ServiceType { get; }

        public ServiceLifetime Lifetime { get; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return obj != null &&
                obj is RequiresDependencyAttribute second &&
                second.ServiceType == this.ServiceType &&
                second.Lifetime == this.Lifetime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ServiceType, this.Lifetime);
        }

        #endregion
    }
}
