namespace ZMachineLib.Operations.Kind2
{
    public enum Kind2OpCodes : ushort
    {
        Je = 0x01,
        Jl = 0x02,
        Jg = 0x03,
        DecCheck = 0x04,
        IncCheck = 0x05,
        Jin = 0x06,
        Test = 0x07,
        Or = 0x08,
        And = 0x09,
        TestAttribute = 0x0a,
        SetAttribute = 0x0b,
        ClearAttribute = 0x0c,
        Store = 0x0d,
        InsertObj = 0x0e,
        LoadW = 0x0f,
        LoadB = 0x10,
        GetProp = 0x11,
        GetPropAddr = 0x12,
        GetNextProp = 0x13,
        Add = 0x14,
        Sub = 0x15,
        Mul = 0x16,
        Div = 0x17,
        Mod = 0x18,
        Call2S = 0x19,
        Call2N = 0x1a,
        SetColor = 0x1b



    }
}