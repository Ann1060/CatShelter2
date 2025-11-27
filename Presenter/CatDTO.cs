using CatEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CatShelter.Presenter
{
    public class CatDTO : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string breed;
        private int age;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        public string Breed
        {
            get => breed;
            set { breed = value; OnPropertyChanged(); }
        }

        public int Age
        {
            get => age;
            set { age = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Cat ToDomainModel()
        {
            return new Cat
            {
                Id = this.Id,
                Name = this.Name,
                Breed = this.Breed,
                Age = this.Age
            };
        }

        public static CatDTO FromDomainModel(Cat cat)
        {
            return new CatDTO
            {
                Id = cat.Id,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age
            };
        }
    }
}
