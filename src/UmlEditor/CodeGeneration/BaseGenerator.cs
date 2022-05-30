using System.Collections.Generic;
using System.IO;

namespace UmlEditor.CodeGeneration
{
    public abstract class BaseGenerator
    {
        public virtual List<string> Lines { get; set; } = new List<string>();

        public virtual void InsertLines(string file)
        {
            using (StreamWriter sw = File.AppendText(file))
            {
                foreach (string line in Lines)
                {
                    sw.WriteLine(line);
                }
                sw.WriteLine();
            }
        }
    }
}
