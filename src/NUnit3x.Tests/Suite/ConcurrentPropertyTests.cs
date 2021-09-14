using NUnit.Framework;
using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NUnit3x.Tests.Suite
{
    [TestOf(typeof(ConcurrentProperty<>))]
    [Parallelizable(ParallelScope.All)]
    [TestFixtureSource(typeof(ConcurrentPropertyTests), nameof(ConcurrentPropertyTests_FixtureData))]
    class ConcurrentPropertyTests : TestSuite
    {
        #region Construction

        #region TestFixtureSource

        protected static IEnumerable<TestFixtureData> ConcurrentPropertyTests_FixtureData
        {
            get
            {
                return NxTestFixtureData.Combinatorial(new[] { -100, 0, 100 });
            }
        }

        #endregion
        public ConcurrentPropertyTests(int concurrentPropDefault)
        {
            _concurrentPropDefault = concurrentPropDefault;
        }

        #endregion

        #region Fields

        private static Guid PROP_GUID = Guid.NewGuid();

        private readonly int _concurrentPropDefault;

        #endregion

        public int ConcurrentProperty
        {
            get => this.ConcurrentProperties[PROP_GUID].GetValue<int>();
            set => this.ConcurrentProperties[PROP_GUID].SetValue(value);
        }

        #region TestCaseSource

        protected static IEnumerable<TestCaseData> OverloadConcurrentProperties_TestCases
        {
            get
            {
                for (int i = 0; i < 1000; i++)
                {
                    yield return new NxTestCaseData(i);
                }
            }
        }

        #endregion
        [Test, TestCaseSource(nameof(OverloadConcurrentProperties_TestCases))]
        public void OverloadConcurrentProperties(int testCase)
        {
            Assert.That(this.ConcurrentProperty, Is.EqualTo(_concurrentPropDefault));

            this.ConcurrentProperty = testCase;

            Assert.That(this.ConcurrentProperty, Is.EqualTo(testCase));
        }

        protected override IEnumerable<IConcurrentProperty> GetConcurrentPropertyDefaults()
        {
            List<IConcurrentProperty> result = new List<IConcurrentProperty>(base.GetConcurrentPropertyDefaults());
            result.Add(new ConcurrentProperty<int>(PROP_GUID, _concurrentPropDefault));

            return result;
        }
    }
}
