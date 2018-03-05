using AsIKnow.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTest
{
    public class ULabelledEntity
    {
        public static TypeManager SetUp(TypeManagerOptions options = null)
        {
            options = options ?? new TypeManagerOptions();
            return new ReflectionTypeManager(options);
        }

        #region nested type

        public class Entity : LabelledEntity
        {
            public Entity(TypeManager typeManager) : base(typeManager)
            {
            }

            public Entity(TypeManager typeManager, object obj) : base(typeManager, obj)
            {
            }
        }

        public interface IFirst
        {
            string PropFirst { get; set; }
        }

        public class ClassA : IFirst
        {
            public string PropFirst { get; set; }
        }

        public class ClassB : ClassA
        {
            public int PropN { get; set; }
        }

        #endregion

        [Trait("Category", nameof(ULabelledEntity))]
        [Fact(DisplayName = nameof(Creation))]
        public void Creation()
        {
            var mgr = SetUp();

            var ent = new Entity(mgr, new ClassB() { PropFirst = "test", PropN = 1 });

            Assert.Equal(new string[] { "PropN", "PropFirst" }, ent.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB", "IFirst","ClassA" }, ent.Labels);

            var ent2 = new Entity(mgr).AddLabels<ClassB>().AddProperties<ClassB>();

            Assert.Equal(new string[] { "PropN", "PropFirst" }, ent2.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB", "IFirst", "ClassA" }, ent2.Labels);
        }

        [Trait("Category", nameof(ULabelledEntity))]
        [Fact(DisplayName = nameof(ManipulateLabels))]
        public void ManipulateLabels()
        {
            var mgr = SetUp();

            var ent = new Entity(mgr).AddLabels<ClassB>().AddProperties<ClassB>();

            Assert.Equal(new string[] { "PropN", "PropFirst" }, ent.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB", "IFirst", "ClassA" }, ent.Labels);

            ent.AddLabels<ClassB>();

            Assert.Equal(new string[] { "PropN", "PropFirst" }, ent.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB", "IFirst", "ClassA" }, ent.Labels);

            ent.RemoveLabels<ClassA>();

            Assert.Equal(new string[] { "PropN", "PropFirst" }, ent.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB" }, ent.Labels);

            ent.RemoveProperties<ClassB>();

            Assert.Equal(new string[] { }, ent.PropertiesKeys);
            Assert.Equal(new string[] { "ClassB" }, ent.Labels);
        }


        [Trait("Category", nameof(ULabelledEntity))]
        [Fact(DisplayName = nameof(ManageData))]
        public void ManageData()
        {
            var mgr = SetUp();

            var ent = new Entity(mgr, new ClassB() { PropFirst = "test", PropN = 1 });

            foreach (var kv in ent)
            {
                Assert.Contains(kv.Key, ent.PropertiesKeys);
                Assert.Equal(ent[kv.Key], kv.Value);
            }

            ent["PropN"] = 3;

            Assert.Equal(3, ent.FillObject<ClassB>().PropN);

            ent.SetProps<ClassA>(p => new { p.PropFirst }, new { PropFirst = "bau" });
            Assert.Equal("bau", ent.FillObject<ClassB>().PropFirst);

            ent.SetProps<ClassA>(p=>p.PropFirst, "ciao" );

            Assert.Equal("ciao", ent.FillObject<ClassB>().PropFirst);

            ClassA ca = ent.FillObject<ClassA>();

            Assert.Equal("ciao", ca.PropFirst);
            ca.PropFirst = "pippo";

            ent.FillObject<ClassA>(ca);

            Assert.Equal("ciao", ca.PropFirst);
        }
    }
}
