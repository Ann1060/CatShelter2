using CatShelter.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Presenter
{
    public abstract class ViewManager
    {
        public abstract bool? ShowAddCatDialog(MainViewModel mainViewModel);
        public abstract bool? CloseAddCatDialog();

        public abstract bool? ShowEditCatDialog(MainViewModel mainViewModel);
        public abstract bool? CloseEditCatDialog();

        public abstract bool? ShowDeleteConfirmDialog(MainViewModel mainViewModel);
        public abstract bool? CloseDeleteConfirmDialog();

        public abstract bool? ShowStatisticsDialog(MainViewModel mainViewModel);
        public abstract bool? CloseStatisticsDialog();
    }
}
