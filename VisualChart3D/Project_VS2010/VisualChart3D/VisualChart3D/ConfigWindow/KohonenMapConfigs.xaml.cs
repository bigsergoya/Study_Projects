using System.Windows;

namespace VisualChart3D.ConfigWindow
{
    /// <summary>
    /// Interaction logic for SammonsMapConfigWindow.xaml
    /// </summary>
    public partial class KohonenMapConfigs : Window
    {
        //private const int MaxIterationUpperLimit = 1000;
        //private const int MaxIterationLowerLimit = 1;
        //private string WarningMessageTitle = "Недопустимое значение";
        //private readonly string WarningMessageDescrtiption = String.Format("Внимание, вы задали значение вне максимальных пределов. Допустимые пределы (от {0} до {1})", MaxIterationLowerLimit, MaxIterationUpperLimit);

        private int _maxIteration;

        public KohonenMapConfigs(int maxIteration)
        {
            InitializeComponent();
            _maxIteration = maxIteration;
            this.tbCountOfIterations.Value = _maxIteration;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _maxIteration = this.tbCountOfIterations.Value.Value;
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public int MaxIteration { get => _maxIteration; }
    }
}
