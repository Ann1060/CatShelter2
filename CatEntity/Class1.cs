using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
