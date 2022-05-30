namespace UmlEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Plane = new System.Windows.Forms.PictureBox();
            this.ControlerTab = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Plane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlerTab)).BeginInit();
            this.SuspendLayout();
            // 
            // Plane
            // 
            this.Plane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Plane.Location = new System.Drawing.Point(0, 0);
            this.Plane.Name = "Plane";
            this.Plane.Size = new System.Drawing.Size(1584, 861);
            this.Plane.TabIndex = 0;
            this.Plane.TabStop = false;
            // 
            // ControlerTab
            // 
            this.ControlerTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlerTab.Location = new System.Drawing.Point(0, 0);
            this.ControlerTab.Name = "ControlerTab";
            this.ControlerTab.Size = new System.Drawing.Size(1584, 25);
            this.ControlerTab.TabIndex = 1;
            this.ControlerTab.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.ControlerTab);
            this.Controls.Add(this.Plane);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UML-редактор";
            ((System.ComponentModel.ISupportInitialize)(this.Plane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlerTab)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Plane;
        private System.Windows.Forms.PictureBox ControlerTab;
    }
}

