using InteractiveDataDisplay.WPF;
using System;
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
        private ISammon _sammonProjection;

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

            ddCriteria.ValueChanged += ddAll_ValueChanged;
            ddLowerBound.ValueChanged += ddAll_ValueChanged;
            ddUpperBound.ValueChanged += ddAll_ValueChanged;
            ddStep.ValueChanged += ddAll_ValueChanged;

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
            ddCriteria.Value = SamProjection.Criteria;
            ddLowerBound.Value = SamProjection.MinStep;
            ddUpperBound.Value = SamProjection.MaxStep;
            ddStep.Value = SamProjection.IterationStep;
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
                labels[i] = i;
            }

            circles.PlotColorSize(labels, SamProjection.CalculatedCriteria.ToArray(), 1d, 1d);

        }

        private void Recalculate()
        {
            SamProjection.Criteria = (double)ddCriteria.Value;
            SamProjection.MinStep = (double)ddLowerBound.Value;
            SamProjection.MaxStep = (double)ddUpperBound.Value;
            SamProjection.IterationStep = (double)ddStep.Value;

            SamProjection.ToProject();
        }

        private void ddAll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SamProjection.Criteria != ddCriteria.Value)
            {
                SwitchButtons();
                return;
            }

            if (SamProjection.MinStep != ddLowerBound.Value)
            {
                SwitchButtons();
                return;
            }

            if (SamProjection.MaxStep != ddUpperBound.Value)
            {
                SwitchButtons();
                return;
            }

            if (SamProjection.IterationStep != ddStep.Value)
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
