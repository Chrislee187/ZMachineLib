using System.Collections.Generic;
using System.IO;

namespace ZTest
{
    public class ZMachineTestScript
    {
        public string ScriptFile { get; }
        public IReadOnlyList<CommandExpects> Lines { get; }


        public ZMachineTestScript(string scriptFile)
        {
            ScriptFile = scriptFile;
            var text = File.OpenText(scriptFile).ReadToEnd();
            var lines = new StringReader(text);
            var line = lines.ReadLine();
            var cmd = "";

            var list = new List<CommandExpects>();
            var lineNo = 1;
            while (line != null)
            {
                if (!line.StartsWith('#'))
                {
                    if (line.Trim().StartsWith(">"))
                    {
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            list.Add(new CommandExpects(cmd, "", lineNo));
                        }

                        cmd = line.Substring(1).Trim();
                    }
                    else
                    {
                        var result = line.Trim();

                        list.Add(new CommandExpects(cmd, result, lineNo));

                        cmd = string.Empty;
                    }
                }

                lineNo++;
                line = lines.ReadLine();
            }

            Lines = list;
        }
    }
}