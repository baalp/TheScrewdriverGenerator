namespace ScrewdriverGenerator.View
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonBuild = new System.Windows.Forms.Button();
            this.StatusStripError = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabelError = new System.Windows.Forms.ToolStripStatusLabel();
            this.RadioButtonTypeOfTipFlat = new System.Windows.Forms.RadioButton();
            this.TextBoxLengthOfInnerPartOfRod = new System.Windows.Forms.TextBox();
            this.GroupBoxLengthOfInnerPartOfRod = new System.Windows.Forms.GroupBox();
            this.TextBoxLengthOfHandle = new System.Windows.Forms.TextBox();
            this.GroupBoxLengthOfHandle = new System.Windows.Forms.GroupBox();
            this.TextBoxLengthOfOuterPartOfRod = new System.Windows.Forms.TextBox();
            this.GroupBoxLengthOfOuterPartOfRod = new System.Windows.Forms.GroupBox();
            this.TextBoxWidestPartOfHandle = new System.Windows.Forms.TextBox();
            this.GroupBoxWidestPartOfHandle = new System.Windows.Forms.GroupBox();
            this.TextBoxTipRodHeight = new System.Windows.Forms.TextBox();
            this.GroupBoxTipRodHeight = new System.Windows.Forms.GroupBox();
            this.GroupBoxTypeOfTip = new System.Windows.Forms.GroupBox();
            this.RadioButtonTypeOfTipTriangular = new System.Windows.Forms.RadioButton();
            this.RadioButtonTypeOfTipCross = new System.Windows.Forms.RadioButton();
            this.PictureBoxParameterInformation = new System.Windows.Forms.PictureBox();
            this.StatusStripError.SuspendLayout();
            this.GroupBoxLengthOfInnerPartOfRod.SuspendLayout();
            this.GroupBoxLengthOfHandle.SuspendLayout();
            this.GroupBoxLengthOfOuterPartOfRod.SuspendLayout();
            this.GroupBoxWidestPartOfHandle.SuspendLayout();
            this.GroupBoxTipRodHeight.SuspendLayout();
            this.GroupBoxTypeOfTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxParameterInformation)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonBuild
            // 
            this.ButtonBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBuild.Enabled = false;
            this.ButtonBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonBuild.Location = new System.Drawing.Point(14, 474);
            this.ButtonBuild.Name = "ButtonBuild";
            this.ButtonBuild.Size = new System.Drawing.Size(400, 45);
            this.ButtonBuild.TabIndex = 8;
            this.ButtonBuild.Text = "Build";
            this.ButtonBuild.UseVisualStyleBackColor = true;
            // 
            // StatusStripError
            // 
            this.StatusStripError.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusStripError.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabelError});
            this.StatusStripError.Location = new System.Drawing.Point(0, 532);
            this.StatusStripError.Name = "StatusStripError";
            this.StatusStripError.Size = new System.Drawing.Size(941, 26);
            this.StatusStripError.SizingGrip = false;
            this.StatusStripError.TabIndex = 1;
            // 
            // ToolStripStatusLabelError
            // 
            this.ToolStripStatusLabelError.Name = "ToolStripStatusLabelError";
            this.ToolStripStatusLabelError.Size = new System.Drawing.Size(207, 21);
            this.ToolStripStatusLabelError.Text = "Error: Some fields are empty";
            // 
            // RadioButtonTypeOfTipFlat
            // 
            this.RadioButtonTypeOfTipFlat.AutoSize = true;
            this.RadioButtonTypeOfTipFlat.Checked = true;
            this.RadioButtonTypeOfTipFlat.Location = new System.Drawing.Point(6, 21);
            this.RadioButtonTypeOfTipFlat.Name = "RadioButtonTypeOfTipFlat";
            this.RadioButtonTypeOfTipFlat.Size = new System.Drawing.Size(48, 20);
            this.RadioButtonTypeOfTipFlat.TabIndex = 0;
            this.RadioButtonTypeOfTipFlat.TabStop = true;
            this.RadioButtonTypeOfTipFlat.Text = "Flat";
            this.RadioButtonTypeOfTipFlat.UseVisualStyleBackColor = true;
            this.RadioButtonTypeOfTipFlat.CheckedChanged += new System.EventHandler(this.RadioButtonTypeOfTipFlat_CheckedChanged);
            // 
            // TextBoxLengthOfInnerPartOfRod
            // 
            this.TextBoxLengthOfInnerPartOfRod.Enabled = false;
            this.TextBoxLengthOfInnerPartOfRod.Location = new System.Drawing.Point(6, 23);
            this.TextBoxLengthOfInnerPartOfRod.Name = "TextBoxLengthOfInnerPartOfRod";
            this.TextBoxLengthOfInnerPartOfRod.Size = new System.Drawing.Size(388, 22);
            this.TextBoxLengthOfInnerPartOfRod.TabIndex = 7;
            // 
            // GroupBoxLengthOfInnerPartOfRod
            // 
            this.GroupBoxLengthOfInnerPartOfRod.Controls.Add(this.TextBoxLengthOfInnerPartOfRod);
            this.GroupBoxLengthOfInnerPartOfRod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxLengthOfInnerPartOfRod.Location = new System.Drawing.Point(14, 333);
            this.GroupBoxLengthOfInnerPartOfRod.Name = "GroupBoxLengthOfInnerPartOfRod";
            this.GroupBoxLengthOfInnerPartOfRod.Size = new System.Drawing.Size(400, 55);
            this.GroupBoxLengthOfInnerPartOfRod.TabIndex = 5;
            this.GroupBoxLengthOfInnerPartOfRod.TabStop = false;
            this.GroupBoxLengthOfInnerPartOfRod.Text = "Length of inner part of rod: (Li, in mm, # - # mm)";
            // 
            // TextBoxLengthOfHandle
            // 
            this.TextBoxLengthOfHandle.Enabled = false;
            this.TextBoxLengthOfHandle.Location = new System.Drawing.Point(6, 23);
            this.TextBoxLengthOfHandle.Name = "TextBoxLengthOfHandle";
            this.TextBoxLengthOfHandle.Size = new System.Drawing.Size(388, 22);
            this.TextBoxLengthOfHandle.TabIndex = 6;
            // 
            // GroupBoxLengthOfHandle
            // 
            this.GroupBoxLengthOfHandle.Controls.Add(this.TextBoxLengthOfHandle);
            this.GroupBoxLengthOfHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxLengthOfHandle.Location = new System.Drawing.Point(14, 267);
            this.GroupBoxLengthOfHandle.Name = "GroupBoxLengthOfHandle";
            this.GroupBoxLengthOfHandle.Size = new System.Drawing.Size(400, 55);
            this.GroupBoxLengthOfHandle.TabIndex = 4;
            this.GroupBoxLengthOfHandle.TabStop = false;
            this.GroupBoxLengthOfHandle.Text = "Length of handle: (Lh, in mm, # - # mm)";
            // 
            // TextBoxLengthOfOuterPartOfRod
            // 
            this.TextBoxLengthOfOuterPartOfRod.Enabled = false;
            this.TextBoxLengthOfOuterPartOfRod.Location = new System.Drawing.Point(6, 23);
            this.TextBoxLengthOfOuterPartOfRod.Name = "TextBoxLengthOfOuterPartOfRod";
            this.TextBoxLengthOfOuterPartOfRod.Size = new System.Drawing.Size(388, 22);
            this.TextBoxLengthOfOuterPartOfRod.TabIndex = 5;
            // 
            // GroupBoxLengthOfOuterPartOfRod
            // 
            this.GroupBoxLengthOfOuterPartOfRod.Controls.Add(this.TextBoxLengthOfOuterPartOfRod);
            this.GroupBoxLengthOfOuterPartOfRod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxLengthOfOuterPartOfRod.Location = new System.Drawing.Point(14, 201);
            this.GroupBoxLengthOfOuterPartOfRod.Name = "GroupBoxLengthOfOuterPartOfRod";
            this.GroupBoxLengthOfOuterPartOfRod.Size = new System.Drawing.Size(400, 55);
            this.GroupBoxLengthOfOuterPartOfRod.TabIndex = 3;
            this.GroupBoxLengthOfOuterPartOfRod.TabStop = false;
            this.GroupBoxLengthOfOuterPartOfRod.Text = "Length of outer part of rod: (Lo, in mm, # - # mm)";
            // 
            // TextBoxWidestPartOfHandle
            // 
            this.TextBoxWidestPartOfHandle.Enabled = false;
            this.TextBoxWidestPartOfHandle.Location = new System.Drawing.Point(6, 23);
            this.TextBoxWidestPartOfHandle.Name = "TextBoxWidestPartOfHandle";
            this.TextBoxWidestPartOfHandle.Size = new System.Drawing.Size(388, 22);
            this.TextBoxWidestPartOfHandle.TabIndex = 4;
            // 
            // GroupBoxWidestPartOfHandle
            // 
            this.GroupBoxWidestPartOfHandle.Controls.Add(this.TextBoxWidestPartOfHandle);
            this.GroupBoxWidestPartOfHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxWidestPartOfHandle.Location = new System.Drawing.Point(14, 135);
            this.GroupBoxWidestPartOfHandle.Name = "GroupBoxWidestPartOfHandle";
            this.GroupBoxWidestPartOfHandle.Size = new System.Drawing.Size(400, 55);
            this.GroupBoxWidestPartOfHandle.TabIndex = 2;
            this.GroupBoxWidestPartOfHandle.TabStop = false;
            this.GroupBoxWidestPartOfHandle.Text = "Widest part of handle: (D, in mm, # - # mm)";
            // 
            // TextBoxTipRodHeight
            // 
            this.TextBoxTipRodHeight.BackColor = System.Drawing.Color.White;
            this.TextBoxTipRodHeight.Location = new System.Drawing.Point(6, 23);
            this.TextBoxTipRodHeight.Name = "TextBoxTipRodHeight";
            this.TextBoxTipRodHeight.Size = new System.Drawing.Size(388, 22);
            this.TextBoxTipRodHeight.TabIndex = 3;
            // 
            // GroupBoxTipRodHeight
            // 
            this.GroupBoxTipRodHeight.Controls.Add(this.TextBoxTipRodHeight);
            this.GroupBoxTipRodHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxTipRodHeight.Location = new System.Drawing.Point(14, 69);
            this.GroupBoxTipRodHeight.Name = "GroupBoxTipRodHeight";
            this.GroupBoxTipRodHeight.Size = new System.Drawing.Size(400, 55);
            this.GroupBoxTipRodHeight.TabIndex = 1;
            this.GroupBoxTipRodHeight.TabStop = false;
            this.GroupBoxTipRodHeight.Text = "Tip rod height: (H, in mm, 0.1 - 10 mm)";
            // 
            // GroupBoxTypeOfTip
            // 
            this.GroupBoxTypeOfTip.Controls.Add(this.RadioButtonTypeOfTipTriangular);
            this.GroupBoxTypeOfTip.Controls.Add(this.RadioButtonTypeOfTipFlat);
            this.GroupBoxTypeOfTip.Controls.Add(this.RadioButtonTypeOfTipCross);
            this.GroupBoxTypeOfTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GroupBoxTypeOfTip.Location = new System.Drawing.Point(14, 12);
            this.GroupBoxTypeOfTip.Name = "GroupBoxTypeOfTip";
            this.GroupBoxTypeOfTip.Size = new System.Drawing.Size(400, 46);
            this.GroupBoxTypeOfTip.TabIndex = 0;
            this.GroupBoxTypeOfTip.TabStop = false;
            this.GroupBoxTypeOfTip.Text = "Type of tip:";
            // 
            // RadioButtonTypeOfTipTriangular
            // 
            this.RadioButtonTypeOfTipTriangular.AutoSize = true;
            this.RadioButtonTypeOfTipTriangular.Location = new System.Drawing.Point(127, 21);
            this.RadioButtonTypeOfTipTriangular.Name = "RadioButtonTypeOfTipTriangular";
            this.RadioButtonTypeOfTipTriangular.Size = new System.Drawing.Size(87, 20);
            this.RadioButtonTypeOfTipTriangular.TabIndex = 2;
            this.RadioButtonTypeOfTipTriangular.Text = "Triangular";
            this.RadioButtonTypeOfTipTriangular.UseVisualStyleBackColor = true;
            this.RadioButtonTypeOfTipTriangular.CheckedChanged += new System.EventHandler(this.RadioButtonTypeOfTipTriangular_CheckedChanged);
            // 
            // RadioButtonTypeOfTipCross
            // 
            this.RadioButtonTypeOfTipCross.AutoSize = true;
            this.RadioButtonTypeOfTipCross.Location = new System.Drawing.Point(60, 21);
            this.RadioButtonTypeOfTipCross.Name = "RadioButtonTypeOfTipCross";
            this.RadioButtonTypeOfTipCross.Size = new System.Drawing.Size(61, 20);
            this.RadioButtonTypeOfTipCross.TabIndex = 1;
            this.RadioButtonTypeOfTipCross.Text = "Cross";
            this.RadioButtonTypeOfTipCross.UseVisualStyleBackColor = true;
            this.RadioButtonTypeOfTipCross.CheckedChanged += new System.EventHandler(this.RadioButtonTypeOfTipCross_CheckedChanged);
            // 
            // PictureBoxParameterInformation
            // 
            this.PictureBoxParameterInformation.BackColor = System.Drawing.SystemColors.Control;
            this.PictureBoxParameterInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBoxParameterInformation.Image = global::ScrewdriverGenerator.View.Properties.Resources.ParameterInformation;
            this.PictureBoxParameterInformation.InitialImage = global::ScrewdriverGenerator.View.Properties.Resources.ParameterInformation;
            this.PictureBoxParameterInformation.Location = new System.Drawing.Point(428, 19);
            this.PictureBoxParameterInformation.Name = "PictureBoxParameterInformation";
            this.PictureBoxParameterInformation.Size = new System.Drawing.Size(500, 500);
            this.PictureBoxParameterInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxParameterInformation.TabIndex = 8;
            this.PictureBoxParameterInformation.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 558);
            this.Controls.Add(this.GroupBoxTypeOfTip);
            this.Controls.Add(this.GroupBoxTipRodHeight);
            this.Controls.Add(this.GroupBoxWidestPartOfHandle);
            this.Controls.Add(this.GroupBoxLengthOfOuterPartOfRod);
            this.Controls.Add(this.GroupBoxLengthOfHandle);
            this.Controls.Add(this.PictureBoxParameterInformation);
            this.Controls.Add(this.GroupBoxLengthOfInnerPartOfRod);
            this.Controls.Add(this.StatusStripError);
            this.Controls.Add(this.ButtonBuild);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Screwdriver Generator";
            this.StatusStripError.ResumeLayout(false);
            this.StatusStripError.PerformLayout();
            this.GroupBoxLengthOfInnerPartOfRod.ResumeLayout(false);
            this.GroupBoxLengthOfInnerPartOfRod.PerformLayout();
            this.GroupBoxLengthOfHandle.ResumeLayout(false);
            this.GroupBoxLengthOfHandle.PerformLayout();
            this.GroupBoxLengthOfOuterPartOfRod.ResumeLayout(false);
            this.GroupBoxLengthOfOuterPartOfRod.PerformLayout();
            this.GroupBoxWidestPartOfHandle.ResumeLayout(false);
            this.GroupBoxWidestPartOfHandle.PerformLayout();
            this.GroupBoxTipRodHeight.ResumeLayout(false);
            this.GroupBoxTipRodHeight.PerformLayout();
            this.GroupBoxTypeOfTip.ResumeLayout(false);
            this.GroupBoxTypeOfTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxParameterInformation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonBuild;
        private System.Windows.Forms.StatusStrip StatusStripError;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabelError;
        private System.Windows.Forms.RadioButton RadioButtonTypeOfTipFlat;
        private System.Windows.Forms.TextBox TextBoxLengthOfInnerPartOfRod;
        private System.Windows.Forms.GroupBox GroupBoxLengthOfInnerPartOfRod;
        private System.Windows.Forms.PictureBox PictureBoxParameterInformation;
        private System.Windows.Forms.TextBox TextBoxLengthOfHandle;
        private System.Windows.Forms.GroupBox GroupBoxLengthOfHandle;
        private System.Windows.Forms.TextBox TextBoxLengthOfOuterPartOfRod;
        private System.Windows.Forms.GroupBox GroupBoxLengthOfOuterPartOfRod;
        private System.Windows.Forms.TextBox TextBoxWidestPartOfHandle;
        private System.Windows.Forms.GroupBox GroupBoxWidestPartOfHandle;
        private System.Windows.Forms.TextBox TextBoxTipRodHeight;
        private System.Windows.Forms.GroupBox GroupBoxTipRodHeight;
        private System.Windows.Forms.GroupBox GroupBoxTypeOfTip;
        private System.Windows.Forms.RadioButton RadioButtonTypeOfTipTriangular;
        private System.Windows.Forms.RadioButton RadioButtonTypeOfTipCross;
    }
}

