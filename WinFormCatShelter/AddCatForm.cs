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
    public partial class AddCatForm : Form
    {
        public Cat NewCat { get; private set; }
        public AddCatForm()
        {
            InitializeComponent();
            NewCat = new Cat();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text) ||
                string.IsNullOrEmpty(textBoxBreed.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            NewCat.Name = textBoxName.Text;
            NewCat.Breed = textBoxBreed.Text;
            NewCat.Age = int.Parse(numericAge.Value.ToString());

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
