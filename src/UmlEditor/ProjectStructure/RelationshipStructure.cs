namespace UmlEditor.ProjectStructure
{
    public class RelationshipStructure
    {
        public RelationshipStructure(ClassStructure origin, ClassStructure target)
        {
            Origin = origin;
            Target = target;
        }

        public ClassStructure Origin { get; set; }
        public ClassStructure Target { get; set; }
    }
}
