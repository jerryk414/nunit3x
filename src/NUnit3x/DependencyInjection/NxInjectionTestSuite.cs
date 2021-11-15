using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit3x.Factory;
using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.DependencyInjection
{
    public abstract class NxInjectionTestSuite<TFactory, TSuite> : NxTestSuite<TFactory, TSuite>, INxInjectionTestSuite
        where TFactory : INxSuiteFactory
        where TSuite : INxInjectionTestSuite
    {
        #region Fields

        private readonly object _lock = new object();

        private IEnumerable<RequiresDependencyAttribute> _dependents;

        #endregion

        #region Properties

        private static Guid SERVICEPROVIDER_KEY = Guid.NewGuid();
        public IServiceProvider Provider
        {
            get
            {
                return this.ConcurrentProperties[SERVICEPROVIDER_KEY].GetValue<IServiceProvider>();
            }
            private set
            {
                this.ConcurrentProperties[SERVICEPROVIDER_KEY].SetValue(value);
            }
        }

        private static Guid SERVICECOLLECTION_KEY = Guid.NewGuid();
        public IServiceCollection Collection
        {
            get
            {
                return this.ConcurrentProperties[SERVICECOLLECTION_KEY].GetValue<IServiceCollection>();
            }
            private set
            {
                this.ConcurrentProperties[SERVICECOLLECTION_KEY].SetValue(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the application services to the given <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="coll"></param>
        protected virtual void AddServices(IServiceCollection coll)
        { }

        protected override void OnTestCaseSetup()
        {
            base.OnTestCaseSetup();
            SetupProvider();
        }

        private void SetupProvider()
        {
            ServiceCollection collection = new ServiceCollection();
            foreach (ServiceDescriptor rootDependency in GetRoot())
            {
                this.Log($"Adding root dependency of type '{ rootDependency.ServiceType }' ('{ rootDependency.Lifetime }')");

                collection.Add(rootDependency);
            }

            Type baseType = typeof(Mock<>);
            foreach (RequiresDependencyAttribute attrib in GetDependents())
            {
                this.Log($"Adding required dependency of type '{ attrib.ServiceType }' ('{ attrib.Lifetime }')");

                if (this.LazyMocks.ContainsKey(attrib.ServiceType))
                {
                    this.Log($"Type '{ attrib.ServiceType }' already registered");
                    continue;
                }

                Type type = baseType.MakeGenericType(attrib.ServiceType);
                Mock dependency = (Mock)Activator.CreateInstance(type);

                this.LazyMocks.Add(attrib.ServiceType, new Dictionary<int, Mock>()
                {
                    { 0, dependency }
                });

                switch (attrib.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        collection.AddSingleton(type, dependency);
                        collection.AddSingleton(attrib.ServiceType, dependency.Object);
                        break;
                    case ServiceLifetime.Scoped:
                        collection.AddScoped(type, i => dependency);
                        collection.AddScoped(attrib.ServiceType, i => dependency.Object);
                        break;
                    case ServiceLifetime.Transient:
                        collection.AddTransient(type, i => dependency);
                        collection.AddTransient(attrib.ServiceType, i => dependency.Object);
                        break;
                    default:
                        throw new ArgumentException($"Invalid lifetime '{ attrib.Lifetime }' specified for type '{ attrib.ServiceType }'registered using { typeof(RequiresDependencyAttribute)}");
                }
            }

            this.Collection = collection;
            this.Provider = collection.BuildServiceProvider();
        }

        internal IServiceCollection GetRoot()
        {
            ServiceCollection coll = new ServiceCollection();
            this.AddServices(coll);

            return coll;
        }

        internal IEnumerable<RequiresDependencyAttribute> GetDependents()
        {
            if (_dependents == null)
            {
                lock (_lock)
                {
                    if (_dependents == null)
                    {
                        _dependents = this.GetType().GetCustomAttributes<RequiresDependencyAttribute>(true);
                    }
                }
            }

            return _dependents;
        }

        internal override IEnumerable<IConcurrentProperty> GetInternalConcurrentPropertyDefaults()
        {
            List<IConcurrentProperty> result = new List<IConcurrentProperty>(base.GetInternalConcurrentPropertyDefaults());
            result.Add(new ConcurrentProperty<IServiceCollection>(SERVICECOLLECTION_KEY, null));
            result.Add(new ConcurrentProperty<IServiceProvider>(SERVICEPROVIDER_KEY, null));

            return result;
        }

        public T GetRequiredService<T>() => this.Provider.GetRequiredService<T>();

        #endregion
    }
}
