using ZMachineLib.Content;

namespace ZMachineLib.Unit.Tests
{
    public class ZMachineObjectBuilder
    {
        private ushort _address;
        private ushort _parent;
        private ushort _child;
        private ushort _objectNumber;
        private ushort _sibling;

        public ZMachineObject Build()
        {
            return new TestZMachineObject
            {
                ObjectNumber = _objectNumber,
                Address = _address,
                Parent = _parent,
                Child = _child,
                Sibling = _sibling,
            };
        }

        public ZMachineObjectBuilder WithAddress(ushort address)
        {
            _address = address;
            return this;
        }
        public ZMachineObjectBuilder WithParent(ushort parent)
        {
            _parent = parent;
            return this;
        }
        public ZMachineObjectBuilder WithChild(ushort child)
        {
            _child = child;
            return this;
        }
        public ZMachineObjectBuilder WithObjectNumber(ushort number)
        {
            _objectNumber = number;
            return this;
        }

        public ZMachineObjectBuilder WithSibling(in ushort sibling)
        {
            _sibling = sibling;
            return this;
        }
    }

    public class TestZMachineObject : ZMachineObject
    {
        public override ZMachineObject RefreshFromMemory()
        {
            return this;
        }
    }

}