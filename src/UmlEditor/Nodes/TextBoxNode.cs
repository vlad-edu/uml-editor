using System;
using System.Drawing;
using System.Windows.Forms;
using UmlEditor.EventArguments;
using UmlEditor.Hitboxes;
using UmlEditor.Nodes.Interfaces;
using UmlEditor.NodeStructure;
using UmlEditor.Rendering;
using UmlEditor.Rendering.ElementStyles;

namespace UmlEditor.Nodes
{
    public class TextBoxNode : LabelNode, IKeyboardFocusableNode
    {
        public TextBoxNode(BasicTextNodeStructure structure, TextRenderElementStyle text_style,
            RectangleRenderElementStyle border_style) : base(structure, text_style, border_style)
        {
            TriggerAreas.Add(new RectangleHitbox(Position, Width, Height));
            OnKeyPress += HandleKey;
            OnFocused += (sender, args) => FillColor = Color.CadetBlue;
            OnUnfocused += (sender, args) => FillColor = Color.White;
        }

        private void HandleKey(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;
            if (key == (char)8 && Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
                Width = Renderer.GetTextWidth(Text.Length);
            }
            else if (key == (char)13)
                OnUnfocused?.Invoke(this, new NodeEventArgs(this));
            else if (Char.IsWhiteSpace(key))
                Text = Text.Insert(Text.Length, " ");
            else
                Text = Text.Insert(Text.Length, key.ToString());
        }

        public EventHandler<KeyPressEventArgs> OnKeyPress { get; set; }
        public EventHandler<NodeEventArgs> OnFocused { get; set; }
        public EventHandler<NodeEventArgs> OnUnfocused { get; set; }
    }
}
