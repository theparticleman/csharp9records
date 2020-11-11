using NUnit.Framework;

namespace csharp9records
{
    public class Tests
    {
        [Test]
        public void BasicRecordProperties()
        {
            var myRecord = new MyRecordType { IntProperty = 42, StringProperty = "foo" };

            Assert.That(myRecord.IntProperty, Is.EqualTo(42));
            Assert.That(myRecord.StringProperty, Is.EqualTo("foo"));
        }

        [Test]
        public void RecordToString()
        {
            var myRecord = new MyRecordType { IntProperty = 42, StringProperty = "foo" };

            Assert.That(myRecord.ToString(), Is.EqualTo("MyRecordType { IntProperty = 42, StringProperty = foo }"));
        }

        [Test]
        public void ShallowPropertyEquality()
        {
            var firstRecord = new MyRecordType { IntProperty = 42, StringProperty = "foo" };
            var secondRecord = new MyRecordType { IntProperty = 42, StringProperty = "foo" };

            Assert.That(firstRecord.Equals(secondRecord), Is.EqualTo(true));
            Assert.That(firstRecord == secondRecord, Is.True);
            Assert.That(firstRecord.GetHashCode(), Is.EqualTo(secondRecord.GetHashCode()));
            //You can mess up Equality and GetHashCode if you try, but
            //by default they will work like your record type is a value type.
        }

        [Test]
        public void RecordsDoNotUseDuckTyping()
        {
            var recordType1 = new MyRecordType { IntProperty = 42, StringProperty = "foo" };
            var recordType2 = new ImmutableRecordType { IntProperty = 42, StringProperty = "foo" };

            Assert.That(recordType1.Equals(recordType2), Is.False);

            //The compiler already knows that these two records are different types,
            //so the following line results in a compiler error.
            //Assert.That(recordType1 == recordType2, Is.False);
        }

        [Test]
        public void ImmutableRecords()
        {
            var mutableRecord = new MyRecordType { IntProperty = 42 };
            var immutableRecord = new ImmutableRecordType { IntProperty = 42 };

            mutableRecord.IntProperty = 43;

            //The following line results in a compiler error.
            //immutableRecord.IntProperty = 43;

            Assert.That(mutableRecord.IntProperty, Is.EqualTo(43));
            Assert.That(immutableRecord.IntProperty, Is.EqualTo(42));
        }

        [Test]
        public void AlternateRecordDeclarationSyntax()
        {
            var autoPropertySyntaxRecord = new MyRecordType { IntProperty = 42 };
            //All properties declared in the constructor are required.
            var constructorSyntaxRecord = new ConstructorSyntaxRecordType(42, "foo");

            //Properties declared with the constructor syntax are readonly,
            //so the following line results in a compiler error.
            //constructorSyntaxRecord.IntProperty = 43;

            Assert.That(constructorSyntaxRecord.IntProperty, Is.EqualTo(42));
        }

        [Test]
        public void InitPropertiesOnClasses()
        {
            var immutableNonRecord = new ImmutableNonRecordType { IntProperty = 42 };
            
            //Init properties can only be set in an object initializer.
            // immutableNonRecord.IntProperty = 43;

            Assert.That(immutableNonRecord.IntProperty, Is.EqualTo(42));
        }
    }

    //At the time of this writing, the syntax highlighting in VS Code
    //for this record syntax isn't quite right.
    public record MyRecordType
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }

    public record ImmutableRecordType
    {
        public int IntProperty { get; init; }
        public string StringProperty { get; init; }
    }

    public record ConstructorSyntaxRecordType(int IntProperty, string StringProperty);

    public class ImmutableNonRecordType
    {
        public int IntProperty { get; init; }
    }
}

//This class is necessary to use init properties if you
//are using .NET Core 3.1. If you are using .NET 5
//this class is not necessary.
namespace System.Runtime.CompilerServices
{
    public sealed class IsExternalInit { }
}

