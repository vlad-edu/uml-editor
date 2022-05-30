using UmlEditor.ProjectStructure;

namespace UmlEditor.CodeGeneration
{
    public class PropertyGenerator : BaseGenerator
    {
        public PropertyStructure Structure { get; set; }

        public PropertyGenerator(PropertyStructure structure)
        {
            Structure = structure;
            GenerateLines();
        }

        private void GenerateLines()
        {
            string line = "        public " + Structure.Type + " " + Structure.Name + " { get; set; }";
            Lines.Add(line);
        }
    }
}
