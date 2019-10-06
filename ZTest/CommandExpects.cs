namespace ZTest
{
    public class CommandExpects
    {
        public int LineNo { get; }
        public string Command { get; }
        public string Expectation { get; }

        public bool HasCommand => !string.IsNullOrEmpty(Command);
        public bool HasExpectation => !string.IsNullOrEmpty(Expectation);

        public bool MeetsExpectation(string output)
        => !HasExpectation 
           || string.IsNullOrEmpty(output) 
           || output.Contains(Expectation);

        public CommandExpects(string command, string expectation, int lineNo)
        {
            LineNo = lineNo;
            Command = command;
            Expectation = expectation;
        }
    }
}