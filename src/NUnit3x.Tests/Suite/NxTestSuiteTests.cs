using Moq;
using NUnit.Framework;
using NUnit3x.Suite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests.Suite
{
    [TestOf(typeof(INxTestSuite))]
    [Parallelizable(ParallelScope.All)]
    public class NxTestSuiteTests : TestSuite
    {
        [Test]
        public void VerifyInitialization()
        {
            // Assert
            Assert.That(this.Factory, Is.Not.Null);
            Assert.That(this.Factory, Is.InstanceOf<SuiteFactory>());
            Assert.That(this.Factory.Suite, Is.EqualTo(this));
        }

        #region TestCaseSource

        protected static IEnumerable<TestCaseData> VerifyLazyMock_TestCases
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
        [Test, TestCaseSource(nameof(VerifyLazyMock_TestCases))]
        public void VerifyLazyMock(int testCase)
        {
            // Arrange
            Mock<INxTestSuite> mock1 = LazyMock<INxTestSuite>();

            // Assert
            Assert.That(mock1, Is.Not.Null);

            // Re-retrieve the mock later in the test to ensure that the object returned is equal to the original instance
            Mock<INxTestSuite> mock2 = LazyMock<INxTestSuite>();
            Assert.That(mock1, Is.EqualTo(mock2));
        }

        [Test]
        public void VerifyLazyMockIndexer()
        {
            // Arrange
            Mock<INxTestSuite> mock1 = LazyMock<INxTestSuite>();
            Mock<INxTestSuite> mock2 = LazyMock<INxTestSuite>(0);
            Mock<INxTestSuite> mock3 = LazyMock<INxTestSuite>(1);
            Mock<INxTestSuite> mock4 = LazyMock<INxTestSuite>(2);

            // Assert
            Assert.That(mock1, Is.Not.Null);
            Assert.That(mock2, Is.Not.Null);
            Assert.That(mock3, Is.Not.Null);
            Assert.That(mock4, Is.Not.Null);

            Assert.That(mock1, Is.EqualTo(mock2));
            Assert.That(mock2, Is.Not.EqualTo(mock3));
            Assert.That(mock2, Is.Not.EqualTo(mock4));
            Assert.That(mock3, Is.Not.EqualTo(mock4));
        }
    }
}
