using ZMachineLib.Content;

namespace ZMachineLib.Unit.Tests
{
    public class ZMachineObjectBuilder
    {
        private ushort _address;
        private ushort _parent;

        public ZMachineObject Build()
        {
            return new ZMachineObject
            {
                Address = _address,
                Parent = _parent
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

    }
}