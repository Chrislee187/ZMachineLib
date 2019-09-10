using System.Collections;
using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind1
{
    public class Kind1Operations : IReadOnlyDictionary<Kind1OpCodes, IOperation>
    {
        private readonly IDictionary<Kind1OpCodes, IOperation> _operations = new Dictionary<Kind1OpCodes, IOperation>();

        public Kind1Operations(ZMachine2 machine,
            IZMachineIo io)
        {
            _operations.Add(Kind1OpCodes.Jz, new Jz(machine));
            _operations.Add(Kind1OpCodes.GetSibling, new GetSibling(machine));
            _operations.Add(Kind1OpCodes.GetChild, new GetChild(machine));
            _operations.Add(Kind1OpCodes.GetParent, new GetParent(machine));
            _operations.Add(Kind1OpCodes.GetPropLen, new GetPropLen(machine));
            _operations.Add(Kind1OpCodes.Inc, new Inc(machine));
            _operations.Add(Kind1OpCodes.Dec, new Dec(machine));
            _operations.Add(Kind1OpCodes.PrintAddr, new PrintAddr(machine, io));
            _operations.Add(Kind1OpCodes.Call1S, new Call1S(machine));
            _operations.Add(Kind1OpCodes.RemoveObj, new RemoveObj(machine));
            _operations.Add(Kind1OpCodes.PrintObj, new PrintObj(machine, io));
            _operations.Add(Kind1OpCodes.Ret, new Ret(machine));
            _operations.Add(Kind1OpCodes.Jump, new Jump(machine));
            _operations.Add(Kind1OpCodes.PrintPAddr, new PrintPAddr(machine, io));
            _operations.Add(Kind1OpCodes.Load, new Load(machine));

            if (machine.Header.Version <= 4)
            {
                _operations.Add(Kind1OpCodes.Not, new Not(machine));
            }
            else
            {
                _operations.Add(Kind1OpCodes.Call1N, new Call1N(machine));
            }
        }

        public IEnumerator<KeyValuePair<Kind1OpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(Kind1OpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(Kind1OpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[Kind1OpCodes key] => _operations[key];

        public IEnumerable<Kind1OpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values;
    }
}