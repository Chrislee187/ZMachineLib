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
            _operations.Add(OpCodes.Je, new Je(machine.Contents));
            _operations.Add(OpCodes.Jl, new Jl(machine.Contents));
            _operations.Add(OpCodes.Jg, new Jg(machine.Contents));
            _operations.Add(OpCodes.DecCheck, new DecCheck(machine.Contents));
            _operations.Add(OpCodes.IncCheck, new IncCheck(machine.Contents));
            _operations.Add(OpCodes.Jin, new Jin(machine.Contents));
            _operations.Add(OpCodes.Test, new Test(machine.Contents));
            _operations.Add(OpCodes.Or, new Or(machine.Contents));
            _operations.Add(OpCodes.And, new And(machine.Contents));
            _operations.Add(OpCodes.TestAttribute, new TestAttribute(machine.Contents));
            _operations.Add(OpCodes.SetAttribute, new SetAttribute(machine.Contents));
            _operations.Add(OpCodes.ClearAttribute, new ClearAttribute(machine.Contents));
            _operations.Add(OpCodes.Store, new Store(machine.Contents));
            _operations.Add(OpCodes.InsertObj, new InsertObj(machine, machine.Contents));
            _operations.Add(OpCodes.LoadW, new LoadW(machine.Contents));
            _operations.Add(OpCodes.LoadB, new LoadB(machine.Contents));
            _operations.Add(OpCodes.GetProp, new GetProp(machine, machine.Contents));
            _operations.Add(OpCodes.GetPropAddr, new GetPropAddr(machine));
            _operations.Add(OpCodes.GetNextProp, new GetNextProp(machine));
            _operations.Add(OpCodes.Add, new Add(machine.Contents));
            _operations.Add(OpCodes.Sub, new Sub(machine.Contents));
            _operations.Add(OpCodes.Mul, new Mul(machine.Contents));
            _operations.Add(OpCodes.Div, new Div(machine.Contents));
            _operations.Add(OpCodes.Mod, new Mod(machine.Contents));
            _operations.Add(OpCodes.Call2S, new Call2S(machine.Contents));
            _operations.Add(OpCodes.Call2N, new Call2N(machine.Contents));
            _operations.Add(OpCodes.SetColor, new SetColor(machine.Contents, io));

            // OP1
            _operations.Add(OpCodes.Jz, new Jz(machine));
            _operations.Add(OpCodes.GetSibling, new GetSibling(machine));
            _operations.Add(OpCodes.GetChild, new GetChild(machine));
            _operations.Add(OpCodes.GetParent, new GetParent(machine));
            _operations.Add(OpCodes.GetPropLen, new GetPropLen(machine));
            _operations.Add(OpCodes.Inc, new Inc(machine));
            _operations.Add(OpCodes.Dec, new Dec(machine));
            _operations.Add(OpCodes.PrintAddr, new PrintAddr(machine, io));
            _operations.Add(OpCodes.Call1S, new Call1S(machine));
            _operations.Add(OpCodes.RemoveObj, new RemoveObj(machine));
            _operations.Add(OpCodes.PrintObj, new PrintObj(machine, io));
            _operations.Add(OpCodes.Ret, new Ret(machine));
            _operations.Add(OpCodes.Jump, new Jump(machine));
            _operations.Add(OpCodes.PrintPAddr, new PrintPAddr(machine, io));
            _operations.Add(OpCodes.Load, new Load(machine));

            if (machine.Contents.Header.Version <= 4)
            {
                _operations.Add(OpCodes.Not, new Not(machine));
            }
            else
            {
                _operations.Add(OpCodes.Call1N, new Call1N(machine));
            }

            // OP0
            _operations.Add(OpCodes.RTrue, new RTrue(machine));
            _operations.Add(OpCodes.RFalse, new RFalse(machine));
            _operations.Add(OpCodes.Print, new Print(machine, io));
            _operations.Add(OpCodes.PrintRet, new PrintRet(machine, io, (RTrue)_operations[OpCodes.RTrue]));
            _operations.Add(OpCodes.Nop, new Nop());
            _operations.Add(OpCodes.Save, new Save(machine, fileIo));
            _operations.Add(OpCodes.Restore, new Restore(machine, fileIo));
            _operations.Add(OpCodes.Restart, new Restart(machine));
            _operations.Add(OpCodes.RetPopped, new RetPopped(machine));
            _operations.Add(OpCodes.Pop, new Pop(machine.Contents));
            _operations.Add(OpCodes.Quit, new Quit(machine, io));
            _operations.Add(OpCodes.NewLine, new Newline(machine, io));
            _operations.Add(OpCodes.ShowStatus, new ShowStatus(machine, io));
            _operations.Add(OpCodes.Verify, new Verify(machine));
            _operations.Add(OpCodes.Piracy, new Piracy(machine));

            // OPVAR
            _operations.Add(OpCodes.Call, new Call(machine));
            _operations.Add(OpCodes.StoreB, new StoreB(machine));
            _operations.Add(OpCodes.StoreW, new StoreW(machine));
            _operations.Add(OpCodes.PutProp, new PutProp(machine));
            _operations.Add(OpCodes.Read, new Read(machine, io));
            _operations.Add(OpCodes.PrintChar, new PrintChar(machine, io));
            _operations.Add(OpCodes.PrintNum, new PrintNum(machine, io));
            _operations.Add(OpCodes.Random, new Random(machine));
            _operations.Add(OpCodes.Push, new Push(machine));
            _operations.Add(OpCodes.Pull, new Pull(machine));
            _operations.Add(OpCodes.SplitWindow, new SplitWindow(machine, io));
            _operations.Add(OpCodes.SetWindow, new SetWindow(machine, io));
            _operations.Add(OpCodes.CallVs2, new CallVs2(machine));
            _operations.Add(OpCodes.EraseWindow, new EraseWindow(machine, io));
            _operations.Add(OpCodes.SetCursor, new SetCursor(machine, io));
            _operations.Add(OpCodes.SetTextStyle, new SetTextStyle(machine, io));
            _operations.Add(OpCodes.BufferMode, new BufferMode(machine, io));
            _operations.Add(OpCodes.OutputStream, new OutputStream(machine));
            _operations.Add(OpCodes.SoundEffect, new SoundEffect(machine, io));
            _operations.Add(OpCodes.ReadChar, new ReadChar(machine, io));
            _operations.Add(OpCodes.ScanTable, new ScanTable(machine));
            _operations.Add(OpCodes.NotVar, new Not(machine));
            _operations.Add(OpCodes.CallVn, new CallVn(machine));
            _operations.Add(OpCodes.CallVn2, new CallVn2(machine));
            _operations.Add(OpCodes.CopyTable, new CopyTable(machine));
            _operations.Add(OpCodes.PrintTable, new PrintTable(machine, io));
            _operations.Add(OpCodes.CheckArgCount, new CheckArgCount(machine));
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