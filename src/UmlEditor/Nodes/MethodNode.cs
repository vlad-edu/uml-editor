using System;
using UmlEditor.Geometry;
using UmlEditor.NodeStructure;
using UmlEditor.ProjectStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;

namespace UmlEditor.Nodes
{
    public class MethodNode : PropertyNode
    {
        public new MethodStructure CodeStructure { get; set; }

        public TextBoxNode ArgumentsTextBox { get; set; }
        public LabelNode LeftBracket { get; set; }
        public LabelNode RightBracket { get; set; }

        public MethodNode(MethodStructure codeStructure, BasicNodeStructure structure, RectangleRenderElementStyle border_style)
            : base(new PropertyStructure(codeStructure.Position, codeStructure.Name, codeStructure.Type, codeStructure.AccessModifier, codeStructure.Modifier), structure, border_style)
        {
            CodeStructure = codeStructure;
            LeftBracket = new LabelNode(new BasicTextNodeStructure(Position + new Vector(AccessModifierButton.Width + NameTextBox.Width, 0),
                Renderer.SingleTextWidth, Height, "("),
                TextRenderElementStyle.Default, RectangleRenderElementStyle.Textbox);
            string argum = codeStructure.Arguments;
            ArgumentsTextBox = new TextBoxNode(new BasicTextNodeStructure(Position + new Vector(AccessModifierButton.Width + NameTextBox.Width + LeftBracket.Width, 0),
                Renderer.GetTextWidth(argum.Length), Height, argum),
                TextRenderElementStyle.Default, RectangleRenderElementStyle.Textbox);
            RightBracket = new LabelNode(new BasicTextNodeStructure(Position + new Vector(NameTextBox.Width + AccessModifierButton.Width + ArgumentsTextBox.Width + LeftBracket.Width, 0),
                    Renderer.SingleTextWidth, Height, ")"),
                TextRenderElementStyle.Default, RectangleRenderElementStyle.Textbox);
            Children.Add(LeftBracket);
            Children.Add(RightBracket);
            Children.Add(ArgumentsTextBox);
            GenerateMenu();
            GenerateOptions();
        }

        public override void UpdateStructure(object sender, EventArgs e)
        {
            CodeStructure.Name = NameTextBox.Text;
            CodeStructure.Type = TypeTextBox.Text;
            CodeStructure.Arguments = ArgumentsTextBox.Text;
        }

        public override float GetWidth()
        {
            return AccessModifierButton.Width + NameTextBox.Width + Separator.Width + TypeTextBox.Width + LeftBracket.Width + RightBracket.Width + ArgumentsTextBox.Width;
        }

        public override void RepositionChildren()
        {
            Width = GetWidth();
            AccessModifierButton.Position = new Vector(Position.X, Position.Y);
            NameTextBox.Position = Position + new Vector(AccessModifierButton.Width, 0);
            ArgumentsTextBox.Position = Position + new Vector(AccessModifierButton.Width + NameTextBox.Width + LeftBracket.Width, 0);
            TypeTextBox.Position = new Vector(Position.X + AccessModifierButton.Width + NameTextBox.Width + ArgumentsTextBox.Width + LeftBracket.Width + RightBracket.Width + Separator.Width, Position.Y);
            LeftBracket.Position = Position + new Vector(NameTextBox.Width + AccessModifierButton.Width, 0);
            RightBracket.Position = Position + new Vector(NameTextBox.Width + AccessModifierButton.Width + ArgumentsTextBox.Width + LeftBracket.Width, 0);
            Separator.Position = Position + new Vector(NameTextBox.Width + AccessModifierButton.Width + ArgumentsTextBox.Width + LeftBracket.Width + RightBracket.Width, 0);
        }
    }
}
