using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualChart3D.Common.DataBinding
{
    public class ColumnDataViewModel : BaseViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<string> _activeItems;
        private ObservableCollection<string> _ignoredItems;

        public ObservableCollection<string> ActiveItems {
            get {
                if (_activeItems == null)
                {
                    _activeItems = new ObservableCollection<string>();
                }

                return _activeItems;
            }

            set {
                _activeItems = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> IgnoredItems {
            get {
                if (_ignoredItems == null)
                {
                    _ignoredItems = new ObservableCollection<string>();
                }

                return _ignoredItems;
            }

            set {
                _ignoredItems = value;
                NotifyPropertyChanged();
            }
        }
    }
}
