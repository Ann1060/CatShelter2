using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace CatEntity
{
    public interface IDomainObject
    {
        int Id { get; set; }
    }

    public class Cat : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public DateTime LastFeeding { get; set; } = DateTime.Today.AddDays(-1);
        public double HungryLevel { get; set; } = 0.0;
    }
}