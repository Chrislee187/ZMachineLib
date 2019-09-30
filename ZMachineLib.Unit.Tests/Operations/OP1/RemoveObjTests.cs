using NUnit.Framework;
using Shouldly;
using ZMachineLib.Content;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:137 9 remove_obj object
    /// Detach the object from its parent, so that it no longer has any parent.
    /// (Its children remain in its possession.)
    /// </summary>
    public class RemoveObjTestsTests : OperationsTestsBase<RemoveObj>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_remove_immediate_child()
        {
            ushort child = 20;
            ushort child2 = 25;
            ushort sibling = 30;
            ushort parent = 5;
            ushort objectToRemove = child;

            var (childZObj, parentZObj, _) 
                = ZMachineObjectBuilder.BuildSimpleRelationship(parent, child, sibling, child2);

            Mockery
                .SetGetObject(child, childZObj)
                .SetGetObject(parent, parentZObj);

            var args = new OperandBuilder()
                .WithArg(objectToRemove)
                .Build();

            Operation.Execute(args);

            parentZObj.Child.ShouldBe(childZObj.Sibling);
            childZObj.Parent.ShouldBe((ushort) 0);
        }


        [Test]
        public void Should_remove_indirect_child()
        {
            Assert.Inconclusive();
//            ushort child = 20;
//            ushort child2 = 25;
//            ushort sibling = 30;
//            ushort parent = 5;
//            ushort objectToRemove = child;
//
//            var (childZObj, parentZObj, _)
//                = ZMachineObjectBuilder.BuildSimpleRelationship(parent, child, sibling, child2);
//
//            Mockery
//                .SetGetObject(child, childZObj)
//                .SetGetObject(parent, parentZObj);
//
//            var args = new OperandBuilder()
//                .WithArg(objectToRemove)
//                .Build();
//
//            Operation.Execute(args);
//
//            parentZObj.Child.ShouldBe(childZObj.Sibling);
//            childZObj.Parent.ShouldBe((ushort)0);
        }
    }
}