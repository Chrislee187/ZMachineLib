using System.Collections.Generic;

namespace ZMachineLib.Unit.Tests
{
    public class OpArgBuilder
    {
        private readonly List<ushort> _values = new List<ushort>();
        public List<ushort> Build()
        {
            return _values;
        }

        public OpArgBuilder WithValue(ushort value)
        {
            _values.Add(value);
            return this;
        }
        public OpArgBuilder WithValues(ushort[] values)
        {
            _values.AddRange(values);
            return this;
        }
    }
}