using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using CatShelterDaL;
using CatEntity;
using Ninject.Modules;

namespace BisnessLogic
{
    public class SimpleConfigModule : NinjectModule 
    {
        public override void Load()
        {
            Bind<ICatRepository>().To<CatRepository>().InSingletonScope();
        }
    }
}
