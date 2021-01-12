namespace ApplicationMaintenance
{
    partial class FrmGenerateSlips
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenerateSlips));
            this.buttonProceed = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCompanyCode = new System.Windows.Forms.TextBox();
            this.dateTimePickerBeginDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.radioButtonSlipdate1 = new System.Windows.Forms.RadioButton();
            this.groupBoxSlipDates = new System.Windows.Forms.GroupBox();
            this.radioButtonSlipdate6 = new System.Windows.Forms.RadioButton();
            this.radioButtonSlipdate5 = new System.Windows.Forms.RadioButton();
            this.radioButtonSlipdate4 = new System.Windows.Forms.RadioButton();
            this.radioButtonSlipdate3 = new System.Windows.Forms.RadioButton();
            this.radioButtonSlipdate2 = new System.Windows.Forms.RadioButton();
            this.textBoxDriver = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxStore = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBarSlips = new System.Windows.Forms.ProgressBar();
            this.groupBoxSlipDates.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(360, 31);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 1;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Store";
            // 
            // textBoxCompanyCode
            // 
            this.textBoxCompanyCode.Location = new System.Drawing.Point(89, 242);
            this.textBoxCompanyCode.Name = "textBoxCompanyCode";
            this.textBoxCompanyCode.Size = new System.Drawing.Size(43, 20);
            this.textBoxCompanyCode.TabIndex = 3;
            // 
            // dateTimePickerBeginDate
            // 
            this.dateTimePickerBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerBeginDate.Location = new System.Drawing.Point(89, 215);
            this.dateTimePickerBeginDate.Name = "dateTimePickerBeginDate";
            this.dateTimePickerBeginDate.Size = new System.Drawing.Size(108, 20);
            this.dateTimePickerBeginDate.TabIndex = 4;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(212, 216);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(108, 20);
            this.dateTimePickerEndDate.TabIndex = 5;
            // 
            // radioButtonSlipdate1
            // 
            this.radioButtonSlipdate1.AutoSize = true;
            this.radioButtonSlipdate1.Location = new System.Drawing.Point(16, 25);
            this.radioButtonSlipdate1.Name = "radioButtonSlipdate1";
            this.radioButtonSlipdate1.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate1.TabIndex = 6;
            this.radioButtonSlipdate1.TabStop = true;
            this.radioButtonSlipdate1.Text = "radioButton1";
            this.radioButtonSlipdate1.UseVisualStyleBackColor = true;
            // 
            // groupBoxSlipDates
            // 
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate6);
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate5);
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate4);
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate3);
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate2);
            this.groupBoxSlipDates.Controls.Add(this.radioButtonSlipdate1);
            this.groupBoxSlipDates.Location = new System.Drawing.Point(89, 12);
            this.groupBoxSlipDates.Name = "groupBoxSlipDates";
            this.groupBoxSlipDates.Size = new System.Drawing.Size(226, 175);
            this.groupBoxSlipDates.TabIndex = 7;
            this.groupBoxSlipDates.TabStop = false;
            this.groupBoxSlipDates.Text = "Slip Dates";
            // 
            // radioButtonSlipdate6
            // 
            this.radioButtonSlipdate6.AutoSize = true;
            this.radioButtonSlipdate6.Location = new System.Drawing.Point(16, 144);
            this.radioButtonSlipdate6.Name = "radioButtonSlipdate6";
            this.radioButtonSlipdate6.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate6.TabIndex = 11;
            this.radioButtonSlipdate6.TabStop = true;
            this.radioButtonSlipdate6.Text = "radioButton6";
            this.radioButtonSlipdate6.UseVisualStyleBackColor = true;
            // 
            // radioButtonSlipdate5
            // 
            this.radioButtonSlipdate5.AutoSize = true;
            this.radioButtonSlipdate5.Location = new System.Drawing.Point(16, 118);
            this.radioButtonSlipdate5.Name = "radioButtonSlipdate5";
            this.radioButtonSlipdate5.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate5.TabIndex = 10;
            this.radioButtonSlipdate5.TabStop = true;
            this.radioButtonSlipdate5.Text = "radioButton5";
            this.radioButtonSlipdate5.UseVisualStyleBackColor = true;
            // 
            // radioButtonSlipdate4
            // 
            this.radioButtonSlipdate4.AutoSize = true;
            this.radioButtonSlipdate4.Location = new System.Drawing.Point(16, 94);
            this.radioButtonSlipdate4.Name = "radioButtonSlipdate4";
            this.radioButtonSlipdate4.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate4.TabIndex = 9;
            this.radioButtonSlipdate4.TabStop = true;
            this.radioButtonSlipdate4.Text = "radioButton4";
            this.radioButtonSlipdate4.UseVisualStyleBackColor = true;
            // 
            // radioButtonSlipdate3
            // 
            this.radioButtonSlipdate3.AutoSize = true;
            this.radioButtonSlipdate3.Location = new System.Drawing.Point(16, 71);
            this.radioButtonSlipdate3.Name = "radioButtonSlipdate3";
            this.radioButtonSlipdate3.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate3.TabIndex = 8;
            this.radioButtonSlipdate3.TabStop = true;
            this.radioButtonSlipdate3.Text = "radioButton3";
            this.radioButtonSlipdate3.UseVisualStyleBackColor = true;
            // 
            // radioButtonSlipdate2
            // 
            this.radioButtonSlipdate2.AutoSize = true;
            this.radioButtonSlipdate2.Location = new System.Drawing.Point(16, 48);
            this.radioButtonSlipdate2.Name = "radioButtonSlipdate2";
            this.radioButtonSlipdate2.Size = new System.Drawing.Size(85, 17);
            this.radioButtonSlipdate2.TabIndex = 7;
            this.radioButtonSlipdate2.TabStop = true;
            this.radioButtonSlipdate2.Text = "radioButton2";
            this.radioButtonSlipdate2.UseVisualStyleBackColor = true;
            // 
            // textBoxDriver
            // 
            this.textBoxDriver.Location = new System.Drawing.Point(90, 268);
            this.textBoxDriver.Name = "textBoxDriver";
            this.textBoxDriver.Size = new System.Drawing.Size(27, 20);
            this.textBoxDriver.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Driver";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(360, 60);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxStore
            // 
            this.textBoxStore.Location = new System.Drawing.Point(157, 242);
            this.textBoxStore.Name = "textBoxStore";
            this.textBoxStore.Size = new System.Drawing.Size(63, 20);
            this.textBoxStore.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Dates";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(139, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "-";
            // 
            // progressBarSlips
            // 
            this.progressBarSlips.Location = new System.Drawing.Point(32, 349);
            this.progressBarSlips.Name = "progressBarSlips";
            this.progressBarSlips.Size = new System.Drawing.Size(309, 26);
            this.progressBarSlips.TabIndex = 14;
            // 
            // FrmGenerateSlips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 430);
            this.Controls.Add(this.progressBarSlips);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStore);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxDriver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxSlipDates);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.dateTimePickerBeginDate);
            this.Controls.Add(this.textBoxCompanyCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonProceed);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGenerateSlips";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Slips";
            this.groupBoxSlipDates.ResumeLayout(false);
            this.groupBoxSlipDates.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dateTimePickerBeginDate;
        public System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        public System.Windows.Forms.RadioButton radioButtonSlipdate1;
        public System.Windows.Forms.GroupBox groupBoxSlipDates;
        public System.Windows.Forms.RadioButton radioButtonSlipdate6;
        public System.Windows.Forms.RadioButton radioButtonSlipdate5;
        public System.Windows.Forms.RadioButton radioButtonSlipdate4;
        public System.Windows.Forms.RadioButton radioButtonSlipdate3;
        public System.Windows.Forms.RadioButton radioButtonSlipdate2;
        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.TextBox textBoxCompanyCode;
        public System.Windows.Forms.TextBox textBoxDriver;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.TextBox textBoxStore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ProgressBar progressBarSlips;
    }
}