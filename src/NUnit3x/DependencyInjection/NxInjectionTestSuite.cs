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
        where TFactory: INxSuiteFactory
        where TSuite: INxInjectionTestSuite
    {
        #region Fields

        private readonly object _lock = new object();

        private IServiceCollection _root;
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
        protected abstract void AddServices(IServiceCollection coll);

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
                collection.Add(rootDependency);
            }

            Type baseType = typeof(Mock<>);
            foreach (RequiresDependencyAttribute attrib in GetDependents())
            {
                Type type = baseType.MakeGenericType(attrib.Type);
                Mock dependency = (Mock)Activator.CreateInstance(type);

                this.LazyMocks.Add(type, new Dictionary<int, Mock>()
                {
                    { 0, dependency }
                });

                switch (attrib.ServiceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        collection.AddSingleton(type, dependency);
                        collection.AddSingleton(attrib.Type, dependency.Object);
                        break;
                    case ServiceLifetime.Scoped:
                        collection.AddScoped(type, i => dependency);
                        collection.AddScoped(attrib.Type, i => dependency.Object);
                        break;
                    case ServiceLifetime.Transient:
                        collection.AddTransient(type, i => dependency);
                        collection.AddTransient(attrib.Type, i => dependency.Object);
                        break;
                    default:
                        throw new ArgumentException($"Invalid lifetime '{ attrib.ServiceLifetime }' specified for type '{ attrib.Type }'registered using { typeof(RequiresDependencyAttribute)}");
                }
            }

            this.Collection = collection;
            this.Provider = collection.BuildServiceProvider();
        }

        private IServiceCollection GetRoot()
        {
            if (_root == null)
            {
                lock (_lock)
                {
                    if (_root == null)
                    {
                        ServiceCollection coll = new ServiceCollection();
                        this.AddServices(coll);

                        _root = coll;
                    }
                }
            }

            return _root;
        }

        private IEnumerable<RequiresDependencyAttribute> GetDependents()
        {
            if (_dependents == null)
            {
                lock (_lock)
                {
                    if (_dependents == null)
                    {
                        _dependents = this.GetType().GetCustomAttributes<RequiresDependencyAttribute>();
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
