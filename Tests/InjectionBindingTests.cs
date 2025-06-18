using NUnit.Framework;
using strange.extensions.injector.impl;
using System;

namespace StrangeIoCTests
{
    public class InjectionBindingTests
    {
        private class TestInjectionBinding : InjectionBinding
        {
            public TestInjectionBinding() : base(null) { }
            public bool CallHasGenericAssignableFrom(Type keyType, Type objType)
            {
                return base.HasGenericAssignableFrom(keyType, objType);
            }
        }

        [Test]
        public void HasGenericAssignableFrom_ReturnsTrue_ForOpenGenericInterface()
        {
            var binding = new TestInjectionBinding();
            bool result = binding.CallHasGenericAssignableFrom(typeof(System.Collections.Generic.IEnumerable<>), typeof(System.Collections.Generic.List<int>));
            Assert.IsTrue(result);
        }

        [Test]
        public void HasGenericAssignableFrom_ReturnsFalse_WhenNotImplemented()
        {
            var binding = new TestInjectionBinding();
            bool result = binding.CallHasGenericAssignableFrom(typeof(System.IComparable<>), typeof(System.Collections.Generic.List<int>));
            Assert.IsFalse(result);
        }

        [Test]
        public void SetValue_AllowsOpenGenericAssignment()
        {
            var binding = new TestInjectionBinding();
            binding.Bind(typeof(System.Collections.Generic.IEnumerable<>));
            Assert.DoesNotThrow(() => binding.SetValue(new System.Collections.Generic.List<int>()));
        }

        [Test]
        public void SetValue_Throws_WhenGenericNotImplemented()
        {
            var binding = new TestInjectionBinding();
            binding.Bind(typeof(System.IComparable<>));
            Assert.Throws<strange.extensions.injector.impl.InjectionException>(() => binding.SetValue(new System.Collections.Generic.List<int>()));
        }
    }
}
