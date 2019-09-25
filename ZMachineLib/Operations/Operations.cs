using System.Collections;
using System.Collections.Generic;
using ZMachineLib.Operations.OP0;
using ZMachineLib.Operations.OP1;
using ZMachineLib.Operations.OP2;
using ZMachineLib.Operations.OPVAR;
using Not = ZMachineLib.Operations.OP1.Not;

namespace ZMachineLib.Operations
{
    public class Operations : IReadOnlyDictionary<OpCodes, IOperation>
    {
        private readonly IDictionary<OpCodes, IOperation> _operations = new Dictionary<OpCodes, IOperation>();

        public Operations(ZMachine2 machine,
            IUserIo io, IFileIo fileIo)
        {
            // OP2
            _operations.Add(OpCodes.Je, new Je(machine.Memory));
            _operations.Add(OpCodes.Jl, new Jl(machine.Memory));
            _operations.Add(OpCodes.Jg, new Jg(machine.Memory));
            _operations.Add(OpCodes.DecCheck, new DecCheck(machine.Memory));
            _operations.Add(OpCodes.IncCheck, new IncCheck(machine.Memory));
            _operations.Add(OpCodes.Jin, new Jin(machine.Memory));
            _operations.Add(OpCodes.Test, new Test(machine.Memory));
            _operations.Add(OpCodes.Or, new Or(machine.Memory));
            _operations.Add(OpCodes.And, new And(machine.Memory));
            _operations.Add(OpCodes.TestAttribute, new TestAttribute(machine.Memory));
            _operations.Add(OpCodes.SetAttribute, new SetAttribute(machine.Memory));
            _operations.Add(OpCodes.ClearAttribute, new ClearAttribute(machine.Memory));
            _operations.Add(OpCodes.Store, new Store(machine.Memory));
            _operations.Add(OpCodes.InsertObj, new InsertObj(machine.Memory));
            _operations.Add(OpCodes.LoadW, new LoadW(machine.Memory));
            _operations.Add(OpCodes.LoadB, new LoadB(machine.Memory));
            _operations.Add(OpCodes.GetProp, new GetProp(machine.Memory));
            _operations.Add(OpCodes.GetPropAddr, new GetPropAddr(machine.Memory));
            _operations.Add(OpCodes.GetNextProp, new GetNextProp(machine.Memory));
            _operations.Add(OpCodes.Add, new Add(machine.Memory));
            _operations.Add(OpCodes.Sub, new Sub(machine.Memory));
            _operations.Add(OpCodes.Mul, new Mul(machine.Memory));
            _operations.Add(OpCodes.Div, new Div(machine.Memory));
            _operations.Add(OpCodes.Mod, new Mod(machine.Memory));
            _operations.Add(OpCodes.Call2S, new Call2S(machine.Memory));
            _operations.Add(OpCodes.Call2N, new Call2N(machine.Memory));
            _operations.Add(OpCodes.SetColor, new SetColor(machine.Memory, io));

            // OP1
            _operations.Add(OpCodes.Jz, new Jz(machine.Memory));
            _operations.Add(OpCodes.GetSibling, new GetSibling(machine.Memory));
            _operations.Add(OpCodes.GetChild, new GetChild(machine.Memory));
            _operations.Add(OpCodes.GetParent, new GetParent(machine.Memory));
            _operations.Add(OpCodes.GetPropLen, new GetPropLen(machine.Memory));
            _operations.Add(OpCodes.Inc, new Inc(machine.Memory));
            _operations.Add(OpCodes.Dec, new Dec(machine.Memory));
            _operations.Add(OpCodes.PrintAddr, new PrintAddr(machine.Memory, io));
            _operations.Add(OpCodes.Call1S, new Call1S(machine.Memory));
            _operations.Add(OpCodes.RemoveObj, new RemoveObj(machine.Memory));
            _operations.Add(OpCodes.PrintObj, new PrintObj(machine.Memory, io));
            _operations.Add(OpCodes.Ret, new Ret(machine.Memory));
            _operations.Add(OpCodes.Jump, new Jump(machine.Memory));
            _operations.Add(OpCodes.PrintPAddr, new PrintPAddr(machine.Memory, io));
            _operations.Add(OpCodes.Load, new Load(machine.Memory));

            if (machine.Memory.Header.Version <= 4)
            {
                _operations.Add(OpCodes.Not, new Not(machine.Memory));
            }
            else
            {
                _operations.Add(OpCodes.Call1N, new Call1N(machine.Memory));
            }

            // OP0
            _operations.Add(OpCodes.RTrue, new RTrue(machine.Memory));
            _operations.Add(OpCodes.RFalse, new RFalse(machine.Memory));
            _operations.Add(OpCodes.Print, new Print(machine.Memory, io));
            _operations.Add(OpCodes.PrintRet, new PrintRet(machine.Memory, io, (RTrue)_operations[OpCodes.RTrue]));
            _operations.Add(OpCodes.Nop, new Nop());
            _operations.Add(OpCodes.Save, new Save(machine.Memory, fileIo)); 
            _operations.Add(OpCodes.Restore, new Restore(machine.Memory, fileIo));
            _operations.Add(OpCodes.Restart, new Restart(machine.Memory));
            _operations.Add(OpCodes.RetPopped, new RetPopped(machine.Memory));
            _operations.Add(OpCodes.Pop, new Pop(machine.Memory));
            _operations.Add(OpCodes.Quit, new Quit(machine.Memory, io));// TODO: Remove machine dependency
            _operations.Add(OpCodes.NewLine, new Newline(machine.Memory, io));
            _operations.Add(OpCodes.ShowStatus, new ShowStatus(machine.Memory, io));
            _operations.Add(OpCodes.Verify, new Verify(machine.Memory));
            _operations.Add(OpCodes.Piracy, new Piracy(machine.Memory));

            // OPVAR
            _operations.Add(OpCodes.Call, new Call(machine.Memory));
            _operations.Add(OpCodes.StoreB, new StoreB(machine.Memory));
            _operations.Add(OpCodes.StoreW, new StoreW(machine.Memory));
            _operations.Add(OpCodes.PutProp, new PutProp(machine.Memory)); 
            _operations.Add(OpCodes.Read, new Read(machine, machine.Memory, io));// TODO: Remove machine dependency
            _operations.Add(OpCodes.PrintChar, new PrintChar(machine.Memory, io));
            _operations.Add(OpCodes.PrintNum, new PrintNum(machine.Memory, io));
            _operations.Add(OpCodes.Random, new Random(machine.Memory));
            _operations.Add(OpCodes.Push, new Push(machine.Memory));
            _operations.Add(OpCodes.Pull, new Pull(machine.Memory));
            _operations.Add(OpCodes.SplitWindow, new SplitWindow(machine.Memory, io));
            _operations.Add(OpCodes.SetWindow, new SetWindow(machine.Memory, io));
            _operations.Add(OpCodes.CallVs2, new CallVs2(machine.Memory));
            _operations.Add(OpCodes.EraseWindow, new EraseWindow(machine.Memory, io));
            _operations.Add(OpCodes.SetCursor, new SetCursor(machine.Memory, io));
            _operations.Add(OpCodes.SetTextStyle, new SetTextStyle(machine.Memory, io));
            _operations.Add(OpCodes.BufferMode, new BufferMode(machine.Memory, io));
            _operations.Add(OpCodes.OutputStream, new OutputStream(machine.Memory));
            _operations.Add(OpCodes.SoundEffect, new SoundEffect(machine.Memory, io));
            _operations.Add(OpCodes.ReadChar, new ReadChar(machine.Memory, io));
            _operations.Add(OpCodes.ScanTable, new ScanTable(machine.Memory));
            _operations.Add(OpCodes.NotVar, new Not(machine.Memory));
            _operations.Add(OpCodes.CallVn, new CallVn(machine.Memory));
            _operations.Add(OpCodes.CallVn2, new CallVn2(machine.Memory));
            _operations.Add(OpCodes.CopyTable, new CopyTable(machine.Memory));
            _operations.Add(OpCodes.PrintTable, new PrintTable(machine.Memory, io));
            _operations.Add(OpCodes.CheckArgCount, new CheckArgCount(machine.Memory));
        }

        #region IReadOnlyDictionary<>
        public IEnumerator<KeyValuePair<OpCodes, IOperation>> GetEnumerator() => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_operations).GetEnumerator();

        public int Count => _operations.Count;

        public bool ContainsKey(OpCodes key) => _operations.ContainsKey(key);

        public bool TryGetValue(OpCodes key, out IOperation value) => _operations.TryGetValue(key, out value);

        public IOperation this[OpCodes key] => _operations[key];

        public IEnumerable<OpCodes> Keys => _operations.Keys;

        public IEnumerable<IOperation> Values => _operations.Values;
        #endregion
    }
}