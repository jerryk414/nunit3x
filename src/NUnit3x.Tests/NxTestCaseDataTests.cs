using NUnit.Framework;
using NUnit3x;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests
{
    [TestOf(typeof(NxTestCaseData))]
    [Parallelizable(ParallelScope.All)]
    public class NxTestCaseDataTests : TestSuite
    {
        #region TestCaseSource

        public static IEnumerable<TestCaseData> Combinatorial_TestCases
        {
            get
            {
                yield return new NxTestCaseData(
                    new[] {
                        new[] { 1, 2 },
                        new[] { 3, 4 }
                    },
                    new[] {
                        new[] { 1, 3 },
                        new[] { 1, 4 },
                        new[] { 2, 3 },
                        new[] { 2, 4 }
                    });

                yield return new NxTestCaseData(
                    new[] {
                        new[] { 1, 2, 3 } 
                    },
                    new[]
                    {
                        new[] { 1 },
                        new[] { 2 },
                        new[] { 3 }
                    });

                yield return new NxTestCaseData(new object[][] { }, new object[][] { new object[] { } });
            }
        }

        #endregion
        [Test, TestCaseSource(nameof(Combinatorial_TestCases))]
        public void Combinatorial(IEnumerable[] input, IEnumerable[] expected)
        {
            // Act
            IEnumerable<NxTestCaseData> result = NxTestCaseData.Combinatorial(input);

            // Assert
            Assert.That(result.Select(i => i.Arguments), Is.EquivalentTo(expected));
        }

        [Test]
        public void Returns()
        {
            // Arrange
            NxTestCaseData data = new NxTestCaseData();

            // Act
            NxTestCaseData result = data.Returns(true);

            // Assert
            Assert.That(data, Is.EqualTo(result));
        }
    }
}
