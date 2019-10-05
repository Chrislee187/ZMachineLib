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
        private string _name;

        public ZMachineObject Build()
        {
            return new TestZMachineObject(_name, _address, _objectNumber, _parent, _sibling, _child);
        }

        public ZMachineObjectBuilder WithRelations(ushort parent, ushort child, ushort sibling)
        {
            _parent = parent;
            _child = child;
            _sibling = sibling;
            return this;
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

        public ZMachineObjectBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public static (ZMachineObject childZobj, ZMachineObject parentZobj, ZMachineObject siblingZobj)
            BuildSimpleRelationship(ushort parent, ushort child, ushort sibling, ushort child2)
        {
            var parentZObj = new ZMachineObjectBuilder()
                .WithObjectNumber(parent)
                .WithChild(child)
                .Build();

            var childZObj = new ZMachineObjectBuilder()
                .WithObjectNumber(child)
                .WithRelations(parent, child2, sibling)
                .Build();

            var siblingZObj = new ZMachineObjectBuilder()
                .WithObjectNumber(sibling)
                .WithParent(child)
                .Build();

            return (childZObj, parentZObj, siblingZObj);
        }
    }

    public class TestZMachineObject : ZMachineObject
    {
        public TestZMachineObject(in string name, in ushort address, in ushort objectNumber, in ushort parent, in ushort sibling, in ushort child) 
            : base(name, address, objectNumber, parent, sibling, child)
        {
        }

        public override ZMachineObject RefreshFromMemory()
        {
            return this;
        }

        public override ushort Parent { get; set; }
        public override ushort Sibling { get; set; }
        public override ushort Child { get; set; }
    }
}