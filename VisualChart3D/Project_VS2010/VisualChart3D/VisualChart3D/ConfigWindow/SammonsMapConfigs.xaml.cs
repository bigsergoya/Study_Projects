using InteractiveDataDisplay.WPF;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using VisualChart3D.Common.Visualization;

namespace VisualChart3D.ConfigWindow
{
    /// <summary>
    /// Interaction logic for SammonsMapConfigs.xaml
    /// Логика взаимодействия - если изменил начальные данные, то пока не перерисуешь график - не сохранишь их.
    /// </summary>
    public partial class SammonsMapConfigs : Window
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ISammon _sammonProjection;
        private double _maxHeight;

        /*public double PlotHeight {
            get {
                return _maxHeight;
            }
            set {
                _maxHeight = value;
                OnPropertyChanged("PlotHeight");
            }
        }*/

        public ISammon SamProjection {
            get {
                return _sammonProjection;
            }

            private set {
                _sammonProjection = value;
                SetValues();
            }
        }

        public SammonsMapConfigs(ISammon sammonProjection)
        {
            InitializeComponent();

            idIterationNumber.ValueChanged += ddAll_ValueChanged;
            ddUpperBound.ValueChanged += ddAll_ValueChanged;
            //ddStep.ValueChanged += ddAll_ValueChanged;

            SamProjection = sammonProjection;
            RepaintChart();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnRecalculate_Click(object sender, RoutedEventArgs e)
        {
            Recalculate();
            RepaintChart();

            if (BtnSave.IsEnabled == false)
            {
                BtnSave.IsEnabled = true;
                BtnRecalculate.IsEnabled = false;
            }
        }

        private void SetValues()
        {
            ddUpperBound.Value = SamProjection.IterationStep;
            idIterationNumber.Value = SamProjection.IterationNumber;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void RepaintChart()
        {
            double[] labels = new double[SamProjection.CalculatedCriteria.Count];

            for (int i = 0; i < SamProjection.CalculatedCriteria.Count; i++)
            {
                labels[i] = i+1;
            }
            
            circles.PlotColorSize(labels, SamProjection.CalculatedCriteria.ToArray(), 10d, 10d);
        }

        private void Recalculate()
        {
            SamProjection.IterationStep = (double)ddUpperBound.Value;
            SamProjection.IterationNumber = (int)idIterationNumber.Value;

            SamProjection.ToProject();
        }

        private void ddAll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SamProjection.IterationStep != ddUpperBound.Value)
            {
                SwitchButtons();
                return;
            }

            if (SamProjection.IterationNumber != idIterationNumber.Value)
            {
                SwitchButtons();
                return;
            }

            BtnSave.IsEnabled = true;
        }

        private void SwitchButtons()
        {
            BtnSave.IsEnabled = false;
            BtnRecalculate.IsEnabled = true;
        }
    }
}
