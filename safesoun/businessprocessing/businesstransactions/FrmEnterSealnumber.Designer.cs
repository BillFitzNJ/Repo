﻿namespace BusinessTransactions
{
    partial class FrmEnterSealnumber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEnterSealnumber));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSealnumber = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelMoneyCenterName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seal Number";
            // 
            // textBoxSealnumber
            // 
            this.textBoxSealnumber.Location = new System.Drawing.Point(102, 70);
            this.textBoxSealnumber.Name = "textBoxSealnumber";
            this.textBoxSealnumber.Size = new System.Drawing.Size(253, 20);
            this.textBoxSealnumber.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(259, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelMoneyCenterName
            // 
            this.labelMoneyCenterName.AutoSize = true;
            this.labelMoneyCenterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMoneyCenterName.Location = new System.Drawing.Point(16, 44);
            this.labelMoneyCenterName.Name = "labelMoneyCenterName";
            this.labelMoneyCenterName.Size = new System.Drawing.Size(80, 13);
            this.labelMoneyCenterName.TabIndex = 3;
            this.labelMoneyCenterName.Text = "Center Name";
            // 
            // FrmEnterSealnumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 160);
            this.Controls.Add(this.labelMoneyCenterName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxSealnumber);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEnterSealnumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter Seal Number";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBoxSealnumber;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label labelMoneyCenterName;
    }
}