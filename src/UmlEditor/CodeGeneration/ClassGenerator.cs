using System.Collections.Generic;
using System.IO;
using UmlEditor.ProjectStructure;

namespace UmlEditor.CodeGeneration
{
    public class ClassGenerator : BaseGenerator
    {
        public ClassStructure ClassStructure { get; set; }
        public ConstructorGenerator Constructor { get; set; }
        public List<PropertyGenerator> Properties { get; set; }
        public List<MethodGenerator> Methods { get; set; }

        public ClassGenerator(ClassStructure structure)
        {
            ClassStructure = structure;
            Constructor = new ConstructorGenerator(structure, structure.Properties);
            Methods = new List<MethodGenerator>();
            foreach (MethodStructure method in structure.Methods)
                Methods.Add(new MethodGenerator(method));
            Properties = new List<PropertyGenerator>();
            foreach (PropertyStructure prop in structure.Properties)
                Properties.Add(new PropertyGenerator(prop));
        }

        public override void InsertLines(string file)
        {
            string line = "    public class " + ClassStructure.Name;
            using (StreamWriter sw = File.CreateText(file))
            {
                sw.WriteLine("using System;");
                sw.WriteLine();
                sw.WriteLine("namespace TestingGen");
                sw.WriteLine("{");
                sw.WriteLine(line);
                sw.WriteLine("    {");
            }
            Properties.ForEach(x => x.InsertLines(file));
            Constructor.InsertLines(file);
            Methods.ForEach(x => x.InsertLines(file));
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine("    }");
                sw.WriteLine("}");
            }
        }
    }
}
