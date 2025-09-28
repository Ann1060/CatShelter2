using CatEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCatShelter
{
    public partial class EditCatForm : Form
    {
        public Cat UpdatedCat { get; private set; }
        public EditCatForm(Cat cat)
        {
            InitializeComponent();
            UpdatedCat = cat;

            // Заполняем поля данными кота
            textBoxName.Text = cat.Name;
            textBoxBreed.Text = cat.Breed;
            numericAge.Value = cat.Age;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text) ||
                string.IsNullOrEmpty(textBoxBreed.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            // Обновляем данные кота
            UpdatedCat.Name = textBoxName.Text;
            UpdatedCat.Breed = textBoxBreed.Text;
            UpdatedCat.Age = (int)numericAge.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
