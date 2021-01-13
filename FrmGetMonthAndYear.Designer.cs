namespace CommonAppClasses
{
    partial class FrmGetMonthAndYear
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetMonthAndYear));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.numericUpDownMonth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownYear = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(47, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Month";
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(140, 12);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 3;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // numericUpDownMonth
            // 
            this.numericUpDownMonth.Location = new System.Drawing.Point(98, 71);
            this.numericUpDownMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMonth.Name = "numericUpDownMonth";
            this.numericUpDownMonth.Size = new System.Drawing.Size(44, 20);
            this.numericUpDownMonth.TabIndex = 4;
            this.numericUpDownMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.Location = new System.Drawing.Point(97, 98);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownYear.TabIndex = 6;
            this.numericUpDownYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Year";
            // 
            // FrmGetMonthAndYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 173);
            this.Controls.Add(this.numericUpDownYear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownMonth);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGetMonthAndYear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter Month and Year";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.NumericUpDown numericUpDownMonth;
        public System.Windows.Forms.NumericUpDown numericUpDownYear;
        private System.Windows.Forms.Label label2;
    }
}