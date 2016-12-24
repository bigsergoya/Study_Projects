// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VisualChart3D
{
    public partial class SettingsWindow
    {

        /// <summary>
        /// Настройки
        /// </summary>
        public SettingsClasses SettingsClassesForms { get; set; }
		
        /// <summary>
        /// Значения <see cref="Shapes"/>
        /// </summary>
        private readonly IEnumerable<Shapes> _arrShape = Enum.GetValues(typeof (Shapes)).Cast<Shapes>();


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stg">начальные настройки</param>
        public SettingsWindow(SettingsClasses stg)
        {
            InitializeComponent();
	        SettingsClassesForms = stg;
            CBshapes.Items.Clear();
	        TbSizeObjects.Text = SettingsClassesForms.SizeObjectStr;
	        NmCountPolig.Value = SettingsClassesForms.CountPoligon;
            foreach (Shapes shape in _arrShape)
            {
                CBshapes.Items.Add(shape.GetRusName());
            }
            foreach (AloneSettClass clss in SettingsClassesForms.ArrayClass)
	        {
				cbClasses.Items.Add(clss.NameClass);
	        }
	        cbClasses.SelectedIndex = 0;
	        CnvColor.Background = new SolidColorBrush(SettingsClassesForms.ArrayClass[0].ColorObject);
			CBshapes.SelectedItem = SettingsClassesForms.ArrayClass[0].Shape.GetRusName();
        }


        private Color? SelectColor(Canvas cnv)
        {
            System.Windows.Forms.ColorDialog clrDlg = new System.Windows.Forms.ColorDialog();
            if (clrDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color clr = clrDlg.Color.ToColor();
                SolidColorBrush b = new SolidColorBrush(clr);
                cnv.Background = b;
                return clr;
            }
            return null;
        }


        private void cnvColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			if (cbClasses.SelectedIndex == -1)
				return;
            Color? clr = SelectColor(CnvColor);
            if (clr.HasValue)
                SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].ColorObject = clr.Value;
        }



        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SettingsClassesForms.CountPoligonStr = NmCountPolig.Value.ToString();
            SettingsClassesForms.SizeObjectStr = TbSizeObjects.Text;
            string errors = SettingsClassesForms.ValidationStrValues();
            if (string.IsNullOrEmpty(errors))
                DialogResult = true;
            else
                MessageBox.Show(errors);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

	    private void CbClasses_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
			if (cbClasses.SelectedIndex == -1)
				return;
			CnvColor.Background = new SolidColorBrush(SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].ColorObject);
			CBshapes.SelectedItem = SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].Shape.GetRusName();
            cbIsClassVisible.IsChecked = SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].isLiquid;
	    }

	    private void CBshapes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	    {
		    if (cbClasses.SelectedIndex == -1)
			    return;
			foreach (Shapes shape in _arrShape)
			{
				if (shape.GetRusName() == CBshapes.SelectedItem.ToString())
				{
					SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].Shape = shape;
					break;
				}
			}
	    }

        private void cbIsClassVisible_Checked(object sender, RoutedEventArgs e)
        {
            SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].isLiquid = true;
        }

        private void cbIsClassVisible_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsClassesForms.ArrayClass[cbClasses.SelectedIndex].isLiquid = false;
        }
    }
}
