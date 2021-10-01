using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using NUnit3x.DependencyInjection;
using NUnit3x.Factory;
using NUnit3x.Suite;
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

    [TestOf(typeof(INxInjectionTestSuite))]
    class NxInjectionTestSuiteTests : NxInjectionTestSuite<NxInjectionSuiteFactory, NxInjectionTestSuiteTests>
    {
        private static INxInjectionTestSuite _injectedService = new NxInjectionTestSuiteTests();
        protected override void AddServices(IServiceCollection coll)
        {
            coll.AddSingleton<INxInjectionTestSuite>(_injectedService);
        }

        [Test]
        public void GetRequiredService()
        {
            Assert.That(this.GetRequiredService<INxInjectionTestSuite>(), Is.EqualTo(_injectedService));
        }

        public void Properties()
        {
            Assert.That(this.Provider, Is.Not.Null);
            Assert.That(this.Collection, Is.Not.Null);
        }
    }


    [TestOf(typeof(INxInjectionTestSuite))]
    [RequiresDependency(typeof(INxInjectionTestSuite))]
    class NxInjectionTestSuiteWithDependencyTests : NxInjectionTestSuite<NxInjectionSuiteFactory, NxInjectionTestSuiteWithDependencyTests>
    {
        private static INxInjectionTestSuite _injectedService = new NxInjectionTestSuiteTests();
        protected override void AddServices(IServiceCollection coll)
        {
            coll.AddSingleton<INxInjectionTestSuite>(_injectedService);
        }

        [Test]
        public void GetRequiredService()
        {
            Assert.That(this.GetRequiredService<INxInjectionTestSuite>(), Is.Not.EqualTo(_injectedService));
            Assert.That(this.GetRequiredService<Mock<INxInjectionTestSuite>>(), Is.Not.Null);
            Assert.That(this.GetRequiredService<Mock<INxInjectionTestSuite>>().Object, Is.EqualTo(this.GetRequiredService<INxInjectionTestSuite>()));
        }
    }
}
