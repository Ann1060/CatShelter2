namespace WinFormCatShelter
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
        private System.Windows.Forms.DataGridView dataGridViewCats;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonStats;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Label labelTotal;

        private void InitializeComponent()
        {
            this.dataGridViewCats = new System.Windows.Forms.DataGridView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonStats = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCats)).BeginInit();
            this.SuspendLayout();

            // dataGridViewCats
            this.dataGridViewCats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCats.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewCats.Name = "dataGridViewCats";
            this.dataGridViewCats.Size = new System.Drawing.Size(560, 300);
            this.dataGridViewCats.TabIndex = 0;
            this.dataGridViewCats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCats.ReadOnly = true;
            this.dataGridViewCats.AllowUserToAddRows = false;
            this.dataGridViewCats.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCats_CellDoubleClick);

            // buttonAdd
            this.buttonAdd.Location = new System.Drawing.Point(12, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);

            // buttonEdit
            this.buttonEdit.Location = new System.Drawing.Point(93, 12);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "Изменить";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);

            // buttonDelete
            this.buttonDelete.Location = new System.Drawing.Point(174, 12);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);

            // buttonStats
            this.buttonStats.Location = new System.Drawing.Point(255, 12);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(75, 23);
            this.buttonStats.TabIndex = 4;
            this.buttonStats.Text = "Статистика";
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click);

            // buttonRefresh
            this.buttonRefresh.Location = new System.Drawing.Point(336, 12);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 5;
            this.buttonRefresh.Text = "Обновить";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);

            // labelTotal
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(417, 17);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(85, 13);
            this.labelTotal.TabIndex = 6;
            this.labelTotal.Text = "Всего котов: 0";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dataGridViewCats);
            this.Name = "MainForm";
            this.Text = "Приют для кошек";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #region Код, автоматически созданный конструктором форм Windows 
        #endregion
    }
}

