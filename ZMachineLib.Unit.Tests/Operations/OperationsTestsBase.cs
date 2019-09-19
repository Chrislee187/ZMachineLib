using System.Collections.Generic;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Shouldly;
using ZMachineLib.Operations;

namespace ZMachineLib.Unit.Tests.Operations
{
    public class OperationsTestsBase
    {
        protected VariableManagerMockery VariableManagerMockery;
        protected ObjectManagerMockery ObjectManagerMockery;
        protected ZMachine2 ZMachine2;
        private bool? _jumped;
        protected List<ushort> AnyArgs = new OpArgBuilder().Build();
        protected const ushort AnyValue = 1;

        protected void Setup()
        {
            ZMachine2 = new ZMachine2(null, null);
            VariableManagerMockery = new VariableManagerMockery();
            ObjectManagerMockery = new ObjectManagerMockery();
        }
        protected void MockJump(IOperation op)
        {
            op.Jump = b => _jumped = b;
        }
        protected void MockPeekNextByte(IOperation op)
        {
            op.PeekNextByte = () => 0;
        }
        protected void JumpedWith(bool value)
        {
            _jumped.HasValue.ShouldBeTrue();
            _jumped.Value.ShouldBe(value);
        }

    }
}