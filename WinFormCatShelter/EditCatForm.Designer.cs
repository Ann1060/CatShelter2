namespace WinFormCatShelter
{
    partial class EditCatForm
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

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxBreed;
        private System.Windows.Forms.NumericUpDown numericAge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;

        private void InitializeComponent()
        {
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxBreed = new System.Windows.Forms.TextBox();
            this.numericAge = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericAge)).BeginInit();
            this.SuspendLayout();

            // textBoxName
            this.textBoxName.Location = new System.Drawing.Point(80, 20);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(200, 20);
            this.textBoxName.TabIndex = 0;

            // textBoxBreed
            this.textBoxBreed.Location = new System.Drawing.Point(80, 60);
            this.textBoxBreed.Name = "textBoxBreed";
            this.textBoxBreed.Size = new System.Drawing.Size(200, 20);
            this.textBoxBreed.TabIndex = 1;

            // numericAge
            this.numericAge.Location = new System.Drawing.Point(80, 100);
            this.numericAge.Name = "numericAge";
            this.numericAge.Size = new System.Drawing.Size(60, 20);
            this.numericAge.TabIndex = 2;
            this.numericAge.Minimum = 0;
            this.numericAge.Maximum = 30;

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Имя:";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Порода:";

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Возраст:";

            // buttonSave
            this.buttonSave.Location = new System.Drawing.Point(80, 140);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);

            // buttonCancel
            this.buttonCancel.Location = new System.Drawing.Point(161, 140);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);

            // EditCatForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 181);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericAge);
            this.Controls.Add(this.textBoxBreed);
            this.Controls.Add(this.textBoxName);
            this.Name = "EditCatForm";
            this.Text = "Редактировать кота";
            ((System.ComponentModel.ISupportInitialize)(this.numericAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #region Windows Form Designer generated code 
        #endregion
    }
}