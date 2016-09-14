namespace WeigandGenerator
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
            this.tbxBadge = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxFacilityCode = new System.Windows.Forms.TextBox();
            this.comboBoxProtocol = new System.Windows.Forms.ComboBox();
            this.btnComputeSequence = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxBinaryOutput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxHexOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbxBadge
            // 
            this.tbxBadge.Location = new System.Drawing.Point(125, 34);
            this.tbxBadge.Name = "tbxBadge";
            this.tbxBadge.Size = new System.Drawing.Size(215, 20);
            this.tbxBadge.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Badge Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Facility Code:";
            // 
            // tbxFacilityCode
            // 
            this.tbxFacilityCode.Location = new System.Drawing.Point(125, 98);
            this.tbxFacilityCode.Name = "tbxFacilityCode";
            this.tbxFacilityCode.Size = new System.Drawing.Size(215, 20);
            this.tbxFacilityCode.TabIndex = 2;
            // 
            // comboBoxProtocol
            // 
            this.comboBoxProtocol.FormattingEnabled = true;
            this.comboBoxProtocol.Items.AddRange(new object[] {
            "Weigand 35 Bits"});
            this.comboBoxProtocol.Location = new System.Drawing.Point(125, 156);
            this.comboBoxProtocol.Name = "comboBoxProtocol";
            this.comboBoxProtocol.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProtocol.TabIndex = 4;
            this.comboBoxProtocol.Text = "Protocol";
            // 
            // btnComputeSequence
            // 
            this.btnComputeSequence.Location = new System.Drawing.Point(125, 218);
            this.btnComputeSequence.Name = "btnComputeSequence";
            this.btnComputeSequence.Size = new System.Drawing.Size(132, 23);
            this.btnComputeSequence.TabIndex = 5;
            this.btnComputeSequence.Text = "Compute Sequence";
            this.btnComputeSequence.UseVisualStyleBackColor = true;
            this.btnComputeSequence.Click += new System.EventHandler(this.btnComputeSequence_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 279);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Binary Output:";
            // 
            // tbxBinaryOutput
            // 
            this.tbxBinaryOutput.Location = new System.Drawing.Point(125, 276);
            this.tbxBinaryOutput.Name = "tbxBinaryOutput";
            this.tbxBinaryOutput.Size = new System.Drawing.Size(381, 20);
            this.tbxBinaryOutput.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Hex Output:";
            // 
            // tbxHexOutput
            // 
            this.tbxHexOutput.Location = new System.Drawing.Point(125, 329);
            this.tbxHexOutput.Name = "tbxHexOutput";
            this.tbxHexOutput.Size = new System.Drawing.Size(381, 20);
            this.tbxHexOutput.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 394);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxHexOutput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxBinaryOutput);
            this.Controls.Add(this.btnComputeSequence);
            this.Controls.Add(this.comboBoxProtocol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxFacilityCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxBadge);
            this.Name = "Form1";
            this.Text = "Weigand Generator - Internal Tool - NOT FOR RELEASE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxBadge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxFacilityCode;
        private System.Windows.Forms.ComboBox comboBoxProtocol;
        private System.Windows.Forms.Button btnComputeSequence;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxBinaryOutput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxHexOutput;
    }
}

