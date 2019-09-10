using System.Collections.Generic;

namespace ZMachineLib.Operations.KindVar
{
    public class KindVarOperations : Dictionary<KindVarOpCodes, IOperation>
    {
        public KindVarOperations(ZMachine2 machine,
            IZMachineIo io)
        {
            Add(KindVarOpCodes.Call, new Call(machine));
            Add(KindVarOpCodes.StoreB, new StoreB(machine));
            Add(KindVarOpCodes.StoreW, new StoreW(machine));
            Add(KindVarOpCodes.PutProp, new PutProp(machine));
            Add(KindVarOpCodes.Read, new Read(machine, io));
            Add(KindVarOpCodes.PrintChar, new PrintChar(machine, io));
            Add(KindVarOpCodes.PrintNum, new PrintNum(machine, io));
            Add(KindVarOpCodes.Random, new Random(machine));
            Add(KindVarOpCodes.Push, new Push(machine));
            Add(KindVarOpCodes.Pull, new Pull(machine));
            Add(KindVarOpCodes.SplitWindow, new SplitWindow(machine, io));
            Add(KindVarOpCodes.SetWindow, new SetWindow(machine, io));
            Add(KindVarOpCodes.CallVs2, new CallVs2(machine));
            Add(KindVarOpCodes.EraseWindow, new EraseWindow(machine, io));
            Add(KindVarOpCodes.SetCursor, new SetCursor(machine, io));
            Add(KindVarOpCodes.SetTextStyle, new SetTextStyle(machine, io));
            Add(KindVarOpCodes.BufferMode, new BufferMode(machine, io));
            Add(KindVarOpCodes.OutputStream, new OutputStream(machine));
            Add(KindVarOpCodes.SoundEffect, new SoundEffect(machine, io));
            Add(KindVarOpCodes.ReadChar, new ReadChar(machine, io));
            Add(KindVarOpCodes.ScanTable, new ScanTable(machine));
            Add(KindVarOpCodes.Not, new Not(machine));
            Add(KindVarOpCodes.CallVn, new CallVn(machine));
            Add(KindVarOpCodes.CallVn2, new CallVn2(machine));
            Add(KindVarOpCodes.CopyTable, new CopyTable(machine));
            Add(KindVarOpCodes.PrintTable, new PrintTable(machine, io));
            Add(KindVarOpCodes.CheckArgCount, new CheckArgCount(machine));
        }
    }
}