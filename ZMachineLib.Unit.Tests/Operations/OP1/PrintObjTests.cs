using NUnit.Framework;
using ZMachineLib.Operations.OP1;

namespace ZMachineLib.Unit.Tests.Operations.OP1
{
    /// <summary>
    /// 1OP:138 A print_obj object
    /// Print short name of object (the Z-encoded string in the object header,
    /// not a property). If the object number is invalid, the interpreter should
    /// halt with a suitable error message.
    /// </summary>
    public class PrintObjTests : OperationsTestsBase<PrintObj>
    {
        [SetUp]
        public void SetUp()
        {
            Setup();
        }

        [Test]
        public void Should_print_the_object_name()
        {
            var args = new OperandBuilder()
                .WithArg(0)
                .Build();

            var objectName = "My Object";
            Mockery
                .SetNextObject(new ZMachineObjectBuilder()
                    .WithName(objectName).Build());

            Operation.Execute(args);




            Mockery
                // TODO: Check that a packed address was used
                .Printed(objectName)
                ;
        }
    }
}