namespace ENGyn.Nodes.Appearance
{
    partial class MainForm
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
            this.LoadButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.colorButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.transparencySlider = new System.Windows.Forms.TrackBar();
            this.Transparency = new System.Windows.Forms.Label();
            this.labelTransparency = new System.Windows.Forms.Label();
            this.Reset_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.transparencySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(374, 53);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(130, 40);
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.Load_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(25, 53);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(312, 335);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(374, 141);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(130, 40);
            this.colorButton.TabIndex = 2;
            this.colorButton.Text = "Select Color";
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler(this.colorbutton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(374, 385);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(130, 40);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save Profile";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // transparencySlider
            // 
            this.transparencySlider.Location = new System.Drawing.Point(374, 244);
            this.transparencySlider.Maximum = 100;
            this.transparencySlider.Minimum = -1;
            this.transparencySlider.Name = "transparencySlider";
            this.transparencySlider.Size = new System.Drawing.Size(130, 45);
            this.transparencySlider.TabIndex = 20;
            this.transparencySlider.Scroll += new System.EventHandler(this.transparencySlider_Scroll);
            // 
            // Transparency
            // 
            this.Transparency.AutoSize = true;
            this.Transparency.Location = new System.Drawing.Point(432, 228);
            this.Transparency.Name = "Transparency";
            this.Transparency.Size = new System.Drawing.Size(10, 13);
            this.Transparency.TabIndex = 5;
            this.Transparency.Text = "-";
            // 
            // labelTransparency
            // 
            this.labelTransparency.AutoSize = true;
            this.labelTransparency.Location = new System.Drawing.Point(398, 206);
            this.labelTransparency.Name = "labelTransparency";
            this.labelTransparency.Size = new System.Drawing.Size(72, 13);
            this.labelTransparency.TabIndex = 5;
            this.labelTransparency.Text = "Transparency";
            // 
            // Reset_Button
            // 
            this.Reset_Button.Location = new System.Drawing.Point(374, 295);
            this.Reset_Button.Name = "Reset_Button";
            this.Reset_Button.Size = new System.Drawing.Size(130, 40);
            this.Reset_Button.TabIndex = 21;
            this.Reset_Button.Text = "Reset";
            this.Reset_Button.UseVisualStyleBackColor = true;
            this.Reset_Button.Click += new System.EventHandler(this.Reset_Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(516, 450);
            this.Controls.Add(this.Reset_Button);
            this.Controls.Add(this.labelTransparency);
            this.Controls.Add(this.Transparency);
            this.Controls.Add(this.transparencySlider);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.LoadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "Appearance Profiler";
            ((System.ComponentModel.ISupportInitialize)(this.transparencySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TrackBar transparencySlider;
        private System.Windows.Forms.Label Transparency;
        private System.Windows.Forms.Label labelTransparency;
        private System.Windows.Forms.Button Reset_Button;
    }
}

