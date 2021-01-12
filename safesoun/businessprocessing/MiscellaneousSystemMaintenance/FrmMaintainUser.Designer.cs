namespace MiscellaneousSystemMaintenance
 {
   partial class FrmMaintainUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMaintainUser));
            this.labelPassword = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.labeUserid = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.buttonSelectUser = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonUserstatus = new System.Windows.Forms.Button();
            this.labelUserrole = new System.Windows.Forms.Label();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.labelUserStatus = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxEmailAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUserid = new System.Windows.Forms.TextBox();
            this.buttonSelectApprole = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(134, 152);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(61, 13);
            this.labelPassword.TabIndex = 11;
            this.labelPassword.Text = "Password";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(174, 13);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 26);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(336, 13);
            this.buttonInsert.Margin = new System.Windows.Forms.Padding(2);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(72, 26);
            this.buttonInsert.TabIndex = 9;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // labeUserid
            // 
            this.labeUserid.AutoSize = true;
            this.labeUserid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeUserid.Location = new System.Drawing.Point(146, 98);
            this.labeUserid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labeUserid.Name = "labeUserid";
            this.labeUserid.Size = new System.Drawing.Size(50, 13);
            this.labeUserid.TabIndex = 6;
            this.labeUserid.Text = "User ID";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.Location = new System.Drawing.Point(126, 124);
            this.labelUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(69, 13);
            this.labelUsername.TabIndex = 13;
            this.labelUsername.Text = "User Name";
            // 
            // buttonSelectUser
            // 
            this.buttonSelectUser.Location = new System.Drawing.Point(12, 13);
            this.buttonSelectUser.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectUser.Name = "buttonSelectUser";
            this.buttonSelectUser.Size = new System.Drawing.Size(72, 26);
            this.buttonSelectUser.TabIndex = 14;
            this.buttonSelectUser.Text = "Select User";
            this.buttonSelectUser.UseVisualStyleBackColor = true;
            this.buttonSelectUser.Click += new System.EventHandler(this.buttonSelectUser_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(417, 13);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(72, 26);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonUserstatus
            // 
            this.buttonUserstatus.Location = new System.Drawing.Point(255, 13);
            this.buttonUserstatus.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUserstatus.Name = "buttonUserstatus";
            this.buttonUserstatus.Size = new System.Drawing.Size(72, 26);
            this.buttonUserstatus.TabIndex = 16;
            this.buttonUserstatus.Text = "Deactivate";
            this.buttonUserstatus.UseVisualStyleBackColor = true;
            this.buttonUserstatus.Click += new System.EventHandler(this.buttonUserstatus_Click);
            // 
            // labelUserrole
            // 
            this.labelUserrole.AutoSize = true;
            this.labelUserrole.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserrole.Location = new System.Drawing.Point(303, 204);
            this.labelUserrole.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUserrole.Name = "labelUserrole";
            this.labelUserrole.Size = new System.Drawing.Size(63, 13);
            this.labelUserrole.TabIndex = 18;
            this.labelUserrole.Text = "User Role";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(93, 13);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(72, 26);
            this.buttonEdit.TabIndex = 20;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // labelUserStatus
            // 
            this.labelUserStatus.AutoSize = true;
            this.labelUserStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserStatus.Location = new System.Drawing.Point(174, 58);
            this.labelUserStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUserStatus.Name = "labelUserStatus";
            this.labelUserStatus.Size = new System.Drawing.Size(73, 13);
            this.labelUserStatus.TabIndex = 21;
            this.labelUserStatus.Text = "User Status";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPassword.Location = new System.Drawing.Point(203, 150);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPassword.MaxLength = 50;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(218, 20);
            this.textBoxPassword.TabIndex = 23;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(203, 123);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxUsername.MaxLength = 35;
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(220, 20);
            this.textBoxUsername.TabIndex = 24;
            // 
            // textBoxEmailAddress
            // 
            this.textBoxEmailAddress.Location = new System.Drawing.Point(203, 174);
            this.textBoxEmailAddress.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEmailAddress.MaxLength = 150;
            this.textBoxEmailAddress.Name = "textBoxEmailAddress";
            this.textBoxEmailAddress.Size = new System.Drawing.Size(220, 20);
            this.textBoxEmailAddress.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(106, 175);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Email Address";
            // 
            // textBoxUserid
            // 
            this.textBoxUserid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxUserid.Location = new System.Drawing.Point(203, 95);
            this.textBoxUserid.MaxLength = 4;
            this.textBoxUserid.Name = "textBoxUserid";
            this.textBoxUserid.Size = new System.Drawing.Size(59, 20);
            this.textBoxUserid.TabIndex = 27;
            // 
            // buttonSelectApprole
            // 
            this.buttonSelectApprole.Location = new System.Drawing.Point(203, 199);
            this.buttonSelectApprole.Name = "buttonSelectApprole";
            this.buttonSelectApprole.Size = new System.Drawing.Size(86, 23);
            this.buttonSelectApprole.TabIndex = 28;
            this.buttonSelectApprole.Text = "User Role";
            this.buttonSelectApprole.UseVisualStyleBackColor = true;
            this.buttonSelectApprole.Click += new System.EventHandler(this.buttonSelectApprole_Click);
            // 
            // FrmMaintainUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 347);
            this.Controls.Add(this.buttonSelectApprole);
            this.Controls.Add(this.textBoxUserid);
            this.Controls.Add(this.textBoxEmailAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelUserStatus);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.labelUserrole);
            this.Controls.Add(this.buttonUserstatus);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonSelectUser);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.labeUserid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmMaintainUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Maintenance";
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label labelPassword;
      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.Button buttonInsert;
      private System.Windows.Forms.Label labeUserid;
      private System.Windows.Forms.Label labelUsername;
      private System.Windows.Forms.Button buttonSelectUser;
      private System.Windows.Forms.Button buttonSave;
      private System.Windows.Forms.Button buttonUserstatus;
      private System.Windows.Forms.Label labelUserrole;
      private System.Windows.Forms.Button buttonEdit;
      private System.Windows.Forms.Label labelUserStatus;
      private System.Windows.Forms.TextBox textBoxPassword;
      private System.Windows.Forms.TextBox textBoxUsername;
      private System.Windows.Forms.TextBox textBoxEmailAddress;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox textBoxUserid;
      private System.Windows.Forms.Button buttonSelectApprole;
   }
}