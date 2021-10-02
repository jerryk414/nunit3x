using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit3x.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Tests.DependencyInjection
{
    [TestOf(typeof(RequiresDependencyAttribute))]
    class RequiresDependencyAttributeTests : TestSuite
    {
        [RequiresDependency(typeof(RequiresDependencyAttributeTests))]
        class Parent : NxInjectionTestSuite<NxInjectionSuiteFactory, Parent> { }

        [RequiresDependency(typeof(Parent))]
        class Child : Parent { }

        [Test]
        public void EnsureInherited()
        {
            // Arrange
            Parent parent = new Parent();
            Child child = new Child();

            // Act
            var parentResult = parent.GetDependents();
            var childResult = child.GetDependents();

            // Assert
            Assert.That(parentResult, Is.EquivalentTo(new[]{
                new RequiresDependencyAttribute(typeof(RequiresDependencyAttributeTests))
            }));
            Assert.That(childResult, Is.EquivalentTo(new[]{
                new RequiresDependencyAttribute(typeof(RequiresDependencyAttributeTests)),
                new RequiresDependencyAttribute(typeof(Parent))
            }));
        }

        public T GetRequiredService<T>()
        {
            throw new NotImplementedException();
        }
    }
}
