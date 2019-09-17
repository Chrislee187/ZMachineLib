using Shouldly;
using ZMachineLib.Operations;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase
    {
        protected VariableManagerMocks VariableManagerMocks;
        protected ObjectManagerMocks ObjectManagerMocks;
        protected ZMachine2 ZMachine2;
        private bool? _jumped;
        
        protected const ushort AnyValue = 1;

        protected void Setup()
        {
            ZMachine2 = new ZMachine2(null, null);
            VariableManagerMocks = new VariableManagerMocks();
            ObjectManagerMocks = new ObjectManagerMocks();
        }
        protected void MockJump(IOperation op)
        {
            op.Jump = b => _jumped = b;
        }

        protected void JumpedWith(bool value)
        {
            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBe(value);
        }

    }
}