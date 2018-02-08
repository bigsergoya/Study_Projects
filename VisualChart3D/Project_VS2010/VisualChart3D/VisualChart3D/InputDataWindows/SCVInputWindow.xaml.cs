using System;
using System.Windows;
using VisualChart3D.Common;
using VisualChart3D.Common.DataReader;

namespace VisualChart3D.InputDataWindows
{
    /// <summary>
    /// Interaction logic for SCVInputWindow.xaml
    /// </summary>
    public partial class SCVInputWindow : Window
    {
        private const string NotImplementedSourceMatrixType = "Ошибка типа входной матрицы. ";
        private const string BadColumnChoiseMessage = "Выбор двух одинаковых стобцов в качестве имен классов и объектов.";
        private const string BadParsingMessage = "Ошибка преобразования в числовой формат. Среди читаемых столбцов имеется символьный. Чтение невозможно.";

        private IUniversalReader _reader;
        private SourceFileMatrixType _inputMatrixType;
        private const InputFileType WindowFileType = InputFileType.CSV;

        public SCVInputWindow(IUniversalReader reader = null)
        {
            InitializeComponent();

            if (reader == null)
            {
                _reader = InitializeReader(WindowFileType);
            }
            else
            {
                _reader = reader;
            }

            CheckFileVisibility(_reader);
            FillFileValues();
            InitializeColumnsData(_reader);
        }

        private void btChooseFile_Click(object sender, RoutedEventArgs e)
        {
            bool result = Utils.OpenFile(tbDataMatrixPath);

            if (!result)
            {
                return;
            }

            _inputMatrixType = GetChoosedMatrixType();
            if (_reader.CheckSourceMatrix(tbDataMatrixPath.Text, _inputMatrixType))
            {
                CheckFileVisibility(_reader);
                InitializeColumnsData(_reader);
            }

        }

        private SourceFileMatrixType GetChoosedMatrixType()
        {
            if (rbDistanceMatrix.IsChecked == true)
            {
                return SourceFileMatrixType.MatrixDistance;
            }
            else if (rbObjectAttributeMatrix.IsChecked == true)
            {
                return SourceFileMatrixType.ObjectAttribute;
            }

            throw new NotImplementedException(NotImplementedSourceMatrixType);
        }

        private void CheckFileVisibility(IUniversalReader universalReader)
        {
            if (universalReader.SourceMatrixType != SourceFileMatrixType.ObjectAttribute)
            {
                return;
            }

            if (String.IsNullOrEmpty(universalReader.SourceMatrixFile))
            {
                return;
            }

            tbMinkovskiDegree.IsEnabled = true;
        }

        private void InitializeColumnsData(IUniversalReader universalReader)
        {
            const int compensation = -1;

            if (universalReader.FirstLine == null)
            {
                return;
            }

            if (universalReader.FirstLine.Count == 0)
            {
                return;
            }

            cmbCLassNumberColumn.ItemsSource = cmbObjectNameColumn.ItemsSource = universalReader.FirstLine;

            cbClassNumberColumn.IsEnabled = true;
            //cmbCLassNumberColumn.IsEnabled = true;

            cbObjectNameColumn.IsEnabled = true;
            //cmbObjectNameColumn.IsEnabled = true;

            if (universalReader.ClassNameColumn != null)
            {
                cmbCLassNumberColumn.SelectedIndex = universalReader.FirstLine.FindIndex(p => p == universalReader.ClassNameColumn);
                cbClassNumberColumn.IsChecked = true;
            }
            else
            {
                cmbCLassNumberColumn.SelectedIndex = 0;
            }

            //Если есть ферст лайн при инициализации и имена столбцов равны нулю, то нот чекед поставить.
            if (universalReader.ObjectNameColumn != null)
            {
                cmbObjectNameColumn.SelectedIndex = universalReader.FirstLine.FindIndex(p => p == universalReader.ObjectNameColumn);
                cbObjectNameColumn.IsChecked = true;
            }
            else
            {
                cmbObjectNameColumn.SelectedIndex = 0;
            }
        }

        private void FillFileValues()
        {
            if (_reader.SourceMatrixType == SourceFileMatrixType.MatrixDistance)
            {
                rbDistanceMatrix.IsChecked = true;
            }
            else if (_reader.SourceMatrixType == SourceFileMatrixType.ObjectAttribute)
            {
                rbObjectAttributeMatrix.IsChecked = true;
            }

            if (!String.IsNullOrEmpty(_reader.SourceMatrixFile))
            {
                tbDataMatrixPath.Text = _reader.SourceMatrixFile;
            }

            tbMinkovskiDegree.Value = _reader.MinkovskiDegree;
        }

        private IUniversalReader InitializeReader(InputFileType windowFileType)
        {
            IUniversalReader reader = new SCVDataReader(windowFileType);
            return reader;
        }

        private void rbDistanceMatrix_Checked(object sender, RoutedEventArgs e)
        {
            if (tbMinkovskiDegree == null)
            {
                return;
            }

            if (tbMinkovskiDegree.IsEnabled)
            {
                tbMinkovskiDegree.IsEnabled = false;
            }
        }

        public IUniversalReader Reader {
            get {
                return _reader;
            }
        }

        private void cbClassNumberColumn_Click(object sender, RoutedEventArgs e)
        {
            cmbCLassNumberColumn.IsEnabled = !(cmbCLassNumberColumn.IsEnabled);
        }

        private void cbObjectNameColumn_Click(object sender, RoutedEventArgs e)
        {
            cmbObjectNameColumn.IsEnabled = !(cmbObjectNameColumn.IsEnabled);
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            bool isBadChoise = CheckSelectedIndexes();
            bool isIndexNotInitialize = CheckIInitialzedIndexes();

            if (isBadChoise || isIndexNotInitialize)
            {
                Utils.ShowErrorMessage(BadColumnChoiseMessage);
                return;
            }

            if (cbClassNumberColumn.IsChecked == true)
            {
                Reader.ClassNameColumn = (string)cmbCLassNumberColumn.SelectedItem;
            }

            if (cbObjectNameColumn.IsChecked == true)
            {
                Reader.ObjectNameColumn = (string)cmbObjectNameColumn.SelectedItem;
            }

            if (Reader.ArraySource == null)
            {                
                Utils.ShowErrorMessage(BadParsingMessage);
                return;
            }

            if (Utils.CheckSourceMatrix(Reader.ArraySource, Reader.SourceMatrixType))
            {
                Utils.ShowErrorMessage(Utils.BadMatrixType);
                return;
            }

            Reader.MinkovskiDegree = (int)tbMinkovskiDegree.Value;
            DialogResult = true;
        }

        private bool IsBothColumnsChecked()
        {
            if (cbClassNumberColumn.IsChecked == false)
            {
                return false;
            }

            if (cbObjectNameColumn.IsChecked == false)
            {
                return false;
            }

            return true;
        }

        private bool CheckIInitialzedIndexes()
        {
            if (!IsBothColumnsChecked())
            {
                return false;
            }

            return (cmbCLassNumberColumn.SelectedIndex == -1) || (cmbObjectNameColumn.SelectedIndex == -1);
        }

        private bool CheckSelectedIndexes()
        {
            if(!IsBothColumnsChecked())
            {
                return false;
            }

            return (cmbCLassNumberColumn.SelectedIndex == cmbObjectNameColumn.SelectedIndex);
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
