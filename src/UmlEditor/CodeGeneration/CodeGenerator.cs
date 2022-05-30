using System.Collections.Generic;
using System.IO;
using UmlEditor.ProjectStructure;

namespace UmlEditor.CodeGeneration
{
    public class CodeGenerator
    {
        public Project Project { get; set; }
        public string DirectoryPath { get; set; }
        List<ClassGenerator> Classes = new();

        public CodeGenerator(Project project, string dir)
        {
            Project = project;
            DirectoryPath = dir;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            foreach (ClassStructure klass in Project.Classes)
            {
                Classes.Add(new ClassGenerator(klass));
            }
        }

        public void Generate()
        {
            foreach (ClassGenerator gen in Classes)
            {
                string file = DirectoryPath + "\\" + gen.ClassStructure.Name + ".cs";
                gen.InsertLines(file);
            }
        }
    }
}
