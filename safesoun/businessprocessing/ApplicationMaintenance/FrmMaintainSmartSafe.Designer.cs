namespace ApplicationMaintenance
{
    partial class FrmMaintainSmartSafe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMaintainSmartSafe));
            this.buttonSelect = new System.Windows.Forms.Button();
            this.textBoxBankFedid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectStore = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStorecode = new System.Windows.Forms.TextBox();
            this.textBoxStorename = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSerialNumber = new System.Windows.Forms.TextBox();
            this.buttonSelectBank = new System.Windows.Forms.Button();
            this.comboBoxManufacturer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(19, 12);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 0;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            // 
            // textBoxBankFedid
            // 
            this.textBoxBankFedid.Location = new System.Drawing.Point(130, 56);
            this.textBoxBankFedid.Name = "textBoxBankFedid";
            this.textBoxBankFedid.ReadOnly = true;
            this.textBoxBankFedid.Size = new System.Drawing.Size(201, 20);
            this.textBoxBankFedid.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bank";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(113, 12);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(207, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(301, 12);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(75, 23);
            this.buttonInsert.TabIndex = 5;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(395, 12);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(489, 12);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(40, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Manufacturer";
            // 
            // buttonSelectStore
            // 
            this.buttonSelectStore.Location = new System.Drawing.Point(281, 137);
            this.buttonSelectStore.Name = "buttonSelectStore";
            this.buttonSelectStore.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectStore.TabIndex = 13;
            this.buttonSelectStore.Text = "Select Store";
            this.buttonSelectStore.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(85, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Store";
            // 
            // textBoxStorecode
            // 
            this.textBoxStorecode.Location = new System.Drawing.Point(130, 140);
            this.textBoxStorecode.Name = "textBoxStorecode";
            this.textBoxStorecode.ReadOnly = true;
            this.textBoxStorecode.Size = new System.Drawing.Size(132, 20);
            this.textBoxStorecode.TabIndex = 11;
            // 
            // textBoxStorename
            // 
            this.textBoxStorename.Location = new System.Drawing.Point(130, 166);
            this.textBoxStorename.Name = "textBoxStorename";
            this.textBoxStorename.ReadOnly = true;
            this.textBoxStorename.Size = new System.Drawing.Size(226, 20);
            this.textBoxStorename.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(36, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Serial Number";
            // 
            // textBoxSerialNumber
            // 
            this.textBoxSerialNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxSerialNumber.Location = new System.Drawing.Point(130, 111);
            this.textBoxSerialNumber.Name = "textBoxSerialNumber";
            this.textBoxSerialNumber.Size = new System.Drawing.Size(201, 20);
            this.textBoxSerialNumber.TabIndex = 15;
            // 
            // buttonSelectBank
            // 
            this.buttonSelectBank.Location = new System.Drawing.Point(337, 55);
            this.buttonSelectBank.Name = "buttonSelectBank";
            this.buttonSelectBank.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectBank.TabIndex = 18;
            this.buttonSelectBank.Text = "Select Bank";
            this.buttonSelectBank.UseVisualStyleBackColor = true;
            // 
            // comboBoxManufacturer
            // 
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Items.AddRange(new object[] {
            "AMSEC",
            "S and S",
            "TRITON"});
            this.comboBoxManufacturer.Location = new System.Drawing.Point(130, 84);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new System.Drawing.Size(201, 21);
            this.comboBoxManufacturer.TabIndex = 19;
            // 
            // FrmMaintainSmartSafe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 216);
            this.Controls.Add(this.comboBoxManufacturer);
            this.Controls.Add(this.buttonSelectBank);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxSerialNumber);
            this.Controls.Add(this.textBoxStorename);
            this.Controls.Add(this.buttonSelectStore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStorecode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxBankFedid);
            this.Controls.Add(this.buttonSelect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMaintainSmartSafe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maintain SmartSafe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button buttonSelect;
        public System.Windows.Forms.TextBox textBoxBankFedid;
        public System.Windows.Forms.Button buttonEdit;
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Button buttonInsert;
        public System.Windows.Forms.Button buttonDelete;
        public System.Windows.Forms.Button buttonSave;
        public System.Windows.Forms.Button buttonSelectStore;
        public System.Windows.Forms.TextBox textBoxStorename;
        public System.Windows.Forms.TextBox textBoxSerialNumber;
        public System.Windows.Forms.TextBox textBoxStorecode;
        public System.Windows.Forms.Button buttonSelectBank;
        public System.Windows.Forms.ComboBox comboBoxManufacturer;
    }
}