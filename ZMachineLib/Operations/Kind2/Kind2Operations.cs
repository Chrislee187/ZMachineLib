using System.Collections;
using System.Collections.Generic;

namespace ZMachineLib.Operations.Kind2
{
    public class Kind2Operations : IReadOnlyDictionary<Kind2OpCodes, IOperation>
    {
        private readonly IDictionary<Kind2OpCodes, IOperation> _operations = new Dictionary<Kind2OpCodes, IOperation>();

        public Kind2Operations(ZMachine2 machine,
            IUserIo io)
        {
            _operations.Add(Kind2OpCodes.Je, new Je(machine));
            _operations.Add(Kind2OpCodes.Jl, new Jl(machine));
            _operations.Add(Kind2OpCodes.Jg, new Jg(machine));
            _operations.Add(Kind2OpCodes.DecCheck, new DecCheck(machine));
            _operations.Add(Kind2OpCodes.IncCheck, new IncCheck(machine));
            _operations.Add(Kind2OpCodes.Jin, new Jin(machine));
            _operations.Add(Kind2OpCodes.Test, new Test(machine));
            _operations.Add(Kind2OpCodes.Or, new Or(machine));
            _operations.Add(Kind2OpCodes.And, new And(machine));
            _operations.Add(Kind2OpCodes.TestAttribute, new TestAttribute(machine));
            _operations.Add(Kind2OpCodes.SetAttribute, new SetAttribute(machine));
            _operations.Add(Kind2OpCodes.ClearAttribute, new ClearAttribute(machine));
            _operations.Add(Kind2OpCodes.Store, new Store(machine));
            _operations.Add(Kind2OpCodes.InsertObj, new InsertObj(machine));
            _operations.Add(Kind2OpCodes.LoadW, new LoadW(machine));
            _operations.Add(Kind2OpCodes.LoadB, new LoadB(machine));
            _operations.Add(Kind2OpCodes.GetProp, new GetProp(machine));
            _operations.Add(Kind2OpCodes.GetPropAddr, new GetPropAddr(machine));
            _operations.Add(Kind2OpCodes.GetNextProp, new GetNextProp(machine));
            _operations.Add(Kind2OpCodes.Add, new Add(machine));
            _operations.Add(Kind2OpCodes.Sub, new Sub(machine));
            _operations.Add(Kind2OpCodes.Mul, new Mul(machine));
            _operations.Add(Kind2OpCodes.Div, new Div(machine));
            _operations.Add(Kind2OpCodes.Mod, new Mod(machine));
            _operations.Add(Kind2OpCodes.Call2S, new Call2S(machine));
            _operations.Add(Kind2OpCodes.Call2N, new Call2N(machine));
            _operations.Add(Kind2OpCodes.SetColor, new SetColor(machine, io));
        }

        #region IReadOnlyDictionary<>
        public IEnumerator<KeyValuePair<Kind2OpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(Kind2OpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(Kind2OpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[Kind2OpCodes key] => _operations[key];

        public IEnumerable<Kind2OpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values; 
        #endregion
    }
}