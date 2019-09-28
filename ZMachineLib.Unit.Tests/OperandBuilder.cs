using System.Collections.Generic;

namespace ZMachineLib.Unit.Tests
{
    public class OperandBuilder
    {
        private readonly List<ushort> _values = new List<ushort>();
        public List<ushort> Build()
        {
            return _values;
        }

        public OperandBuilder WithArg(ushort value)
        {
            _values.Add(value);
            return this;
        }
        public OperandBuilder WithAnyArg()
        {
            _values.Add(0);
            return this;
        }
        public OperandBuilder WithArgs(params ushort[] values)
        {
            _values.AddRange(values);
            return this;
        }
    }
}