namespace ZMachineLib.Operations.KindVar
{
    public enum KindVarOpCodes : ushort
    {
        Call = 0x00,
        StoreW = 0x01,
        StoreB = 0x02,
        PutProp = 0x03,
        Read = 0x04,
        PrintChar = 0x05,
        PrintNum = 0x06,
        Random = 0x07,
        Push = 0x08,
        Pull = 0x09,
        SplitWindow = 0x0a,
        SetWindow = 0x0b,
        CallVs2 = 0x0c,
        EraseWindow = 0x0d,
        // 0x0f ???
        SetCursor = 0x0f,
        SetTextStyle = 0x11,
        BufferMode = 0x12,
        OutputStream = 0x13,
        SoundEffect = 0x15,
        ReadChar = 0x16,
        ScanTable = 0x17,
        Not = 0x18,
        CallVn = 0x19,
        CallVn2 = 0x1a,
        CopyTable = 0x1d,
        PrintTable = 0x1e,
        CheckArgCount = 0x1f
    }
}