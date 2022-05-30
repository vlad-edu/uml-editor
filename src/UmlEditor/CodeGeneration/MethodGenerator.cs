using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UmlEditor.ProjectStructure;

namespace UmlEditor.CodeGeneration
{
    public class MethodGenerator : BaseGenerator
    {
        public MethodStructure Structure { get; set; }
        public MethodGenerator(MethodStructure structure)
        {
            Structure = structure;
            GenerateLines();
        }

        private void GenerateLines()
        {
            string Arguments = "";
            List<string> parts = Structure.Arguments.Split(',').ToList();
            List<string> names = parts.Select(x => Regex.Replace(x.Split(':')[0], @"\s+", "")).ToList();
            List<string> types = parts.Select(x => Regex.Replace(x.Split(':')[1], @"\s+", "")).ToList();
            for (int i = 0; i < names.Count; i++)
            {
                Arguments += types[0] + " ";
                Arguments += names[0] + ", ";
            }
            if (Arguments.Length > 1)
                Arguments = Arguments.Substring(0, Arguments.Length - 2);
            string line = "        public " + Structure.Type + " " + Structure.Name + "(" + Arguments + ")";
            Lines.Add(line);
            Lines.Add("        {");
            Lines.Add("        }");
        }
    }
}
