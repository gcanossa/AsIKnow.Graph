using AsIKnow.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class UTypeManager
    {
        public static TypeManager SetUp(TypeManagerOptions options = null)
        {
            options = options ?? new TypeManagerOptions();
            return new ReflectionTypeManager(options);
        }

        #region nested type

        public interface IInterfaceA
        {
            string PropA { get; set; }
            string ShadowPropA { get; }
        }
        public interface IInterfaceB { }
        public interface IInterfaceC : IInterfaceA, IInterfaceB { }

        public abstract class AbstractClass
        {
            public int PropAbstract { get; set; }
        }

        public class ClassA
        {
            public string PropA { get; set; }
        }
        public class ClassNotA
        {
            public int PropA { get; set; }
        }

        public class ClassB : AbstractClass, IInterfaceA
        {
            public string PropA { get; set; }
            public string ShadowPropA { get; }
        }

        public class ClassC : AbstractClass, IInterfaceC
        {
            public string PropA { get; set; }
            public string ShadowPropA { get; }
            public ClassA PropC { get; set; }
        }

        #endregion

        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(GetLablel))]
        public void GetLablel()
        {
            var mgr = SetUp();

            Assert.Equal("ClassC", mgr.GetLabel<ClassC>());

            mgr = SetUp(new TypeManagerOptions() { UseFullNameInLables = true });

            Assert.Equal("UnitTest.UTypeManager", mgr.GetLabel<UTypeManager>());
            Assert.Equal("UnitTest.UTypeManager+ClassC", mgr.GetLabel<ClassC>());
        }

        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(GetLablels))]
        public void GetLablels()
        {
            var mgr = SetUp();

            Assert.Equal(new string[] 
            {
                "ClassC",
                "IInterfaceC",
                "IInterfaceA",
                "IInterfaceB",
                "AbstractClass"
            }, mgr.GetLabels<ClassC>());

            mgr = SetUp(new TypeManagerOptions() { UseFullNameInLables = true });

            Assert.Equal(new string[] 
            {
                "UnitTest.UTypeManager+ClassC",
                "UnitTest.UTypeManager+IInterfaceC",
                "UnitTest.UTypeManager+IInterfaceA",
                "UnitTest.UTypeManager+IInterfaceB",
                "UnitTest.UTypeManager+AbstractClass"
            }, mgr.GetLabels<ClassC>());
        }

        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(GetTypesFromLabels))]
        public void GetTypesFromLabels()
        {
            var mgr = SetUp();

            Assert.Equal(new Type[]
            {
                typeof(ClassC),
                typeof(IInterfaceC),
                typeof(IInterfaceA),
                typeof(IInterfaceB),
                typeof(AbstractClass)
            }, mgr.GetTypesFromLabels(mgr.GetLabels<ClassC>()));

            mgr = SetUp(new TypeManagerOptions() { UseFullNameInLables = true });

            Assert.Equal(new Type[]
            {
                typeof(ClassC),
                typeof(IInterfaceC),
                typeof(IInterfaceA),
                typeof(IInterfaceB),
                typeof(AbstractClass)
            }, mgr.GetTypesFromLabels(mgr.GetLabels<ClassC>()));
        }
        
        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(GetPropertyNames))]
        public void GetPropertyNames()
        {
            var mgr = SetUp();

            Assert.Equal(new string[]
           {
                "PropA",
                "PropC",
                "PropAbstract"
           }, mgr.GetPropertyNames<ClassC>());
        }

        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(GetInstanceOfMostSpecific))]
        public void GetInstanceOfMostSpecific()
        {
            var mgr = SetUp();

            var types = mgr.GetTypesFromLabels(mgr.GetLabels<ClassC>());

            Assert.True(mgr.GetInstanceOfMostSpecific(types) is ClassC);

            Assert.Throws<InvalidOperationException>(()=>mgr.GetInstanceOfMostSpecific(types.Where(p=>p!=typeof(ClassC))));
        }
        
        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(FromType_AsType))]
        public void FromType_AsType()
        {
            var mgr = SetUp();
            ClassA test = new ClassA();
            ClassC obj = new ClassC() { PropA = "test", PropAbstract=2, PropC = test };

            Dictionary<string, object> res = mgr.FromType(obj);
            Assert.Equal(new Dictionary<string, object>() { { "PropA", "test" },{ "PropAbstract", 2 }, { "PropC", test } }, res);

            ClassC obj2 = mgr.AsType<ClassC>(new Dictionary<string, object>() { { "PropA", "test--" }, { "PropAbstract", 20 }, { "PropC", test } });

            Assert.Equal("test--", obj2.PropA);
            Assert.Equal(20, obj2.PropAbstract);
            Assert.Equal(test, obj2.PropC);
        }

        [Trait("Category", nameof(UTypeManager))]
        [Fact(DisplayName = nameof(CheckObjectInclusion))]
        public void CheckObjectInclusion()
        {
            var mgr = SetUp();

            ClassC c = new ClassC();
            ClassA a = new ClassA();
            ClassNotA na = new ClassNotA();
            IInterfaceA ia = null;

            Assert.True(mgr.CheckObjectInclusion(c, a));
            Assert.False(mgr.CheckObjectInclusion(c, ia));
            Assert.False(mgr.CheckObjectInclusion(c, na));
        }
    }
}
