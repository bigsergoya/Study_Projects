using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VisualChart3D.Common;
using VisualChart3D.Common.DataReader;
using System.Linq;
using System.Collections.ObjectModel;

namespace VisualChart3D.InputDataWindows
{
    /// <summary>
    /// Interaction logic for SCVInputWindow.xaml
    /// </summary>
    public partial class SCVInputWindow : Window
    {
        private const string NotImplementedSourceMatrixType = "Ошибка типа входной матрицы. ";
        private const string BadColumnChoiseMessage = "Ошибка выбора стобцов в качестве имен классов и объектов.";
        private const string BadParsingMessage = "Ошибка преобразования в числовой формат. Среди читаемых столбцов имеется символьный. Чтение невозможно.";
        private const string CannotIgnoreColumn = "Нельзя игнорировать уже выбранный в качестве имен объектов или имен классов столбец";

        private const int SelectedElementIndex = 0;
        private const InputFileType WindowFileType = InputFileType.CSV;

        private IUniversalReader _reader;
        private SourceFileMatrixType _inputMatrixType;

        private Common.DataBinding.ColumnDataViewModel _columnDataViewModel;

        public SCVInputWindow(IUniversalReader reader = null)
        {
            InitializeComponent();
            _columnDataViewModel = new Common.DataBinding.ColumnDataViewModel();

            DataContext = _columnDataViewModel;

            if (reader == null)
            {
                _reader = InitializeReader();
            }
            else
            {
                _reader = reader.InputFileType == WindowFileType ? reader : InitializeReader();
                InitializeColumnData();
            }

            CheckFileVisibility(_reader);
            FillFileValues();
            InitializeColumnsData(_reader);
        }

        private void InitializeColumnData()
        {
            _columnDataViewModel.ActiveItems = new ObservableCollection<string>(_reader.FirstLine.Where(p => !_reader.IgnoredColumns.Contains(p)));
            _columnDataViewModel.IgnoredItems = new ObservableCollection<string>(_reader.IgnoredColumns);
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
            const int StartElement = 0;
            const int SecondElement = 1;
            if (universalReader.FirstLine == null)
            {
                return;
            }

            if (universalReader.FirstLine.Count == 0)
            {
                return;
            }

            _columnDataViewModel.ActiveItems = new ObservableCollection<string>(universalReader.FirstLine);

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
                cmbCLassNumberColumn.SelectedIndex = StartElement;
            }

            //Если есть ферст лайн при инициализации и имена столбцов равны нулю, то нот чекед поставить.
            if (universalReader.ObjectNameColumn != null)
            {
                cmbObjectNameColumn.SelectedIndex = universalReader.FirstLine.FindIndex(p => p == universalReader.ObjectNameColumn);
                cbObjectNameColumn.IsChecked = true;
            }
            else
            {
                cmbObjectNameColumn.SelectedIndex = SecondElement;
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

        private IUniversalReader InitializeReader()
        {            
            IUniversalReader reader = new FastSCVDataReader(WindowFileType);
            //IUniversalReader reader = new SCVDataReader(WindowFileType);
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

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            //bool isBadChoise = CheckSelectedIndexes();
            bool isIndexNotInitialize = CheckIInitialzedIndexes();

            if (isIndexNotInitialize)
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

            Reader.IgnoredColumns = _columnDataViewModel.IgnoredItems.ToList<string>();

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

        private bool CheckIdenticalSelection()
        {
            if (!IsBothColumnsChecked())
            {
                return false;
            }

            return (cmbCLassNumberColumn.SelectedIndex == cmbObjectNameColumn.SelectedIndex);
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void IgnoreSelectedItem(object sender, RoutedEventArgs e)
        {
            if (lbAllColumns.SelectedIndex == -1)
            {
                return;
            }

            string transferedColumn = _columnDataViewModel.ActiveItems[lbAllColumns.SelectedIndex];

            if ((bool)cbClassNumberColumn.IsChecked)
            {
                if (transferedColumn.CompareTo(cmbCLassNumberColumn.SelectedValue.ToString()) == 0)
                {
                    Utils.ShowWarningMessage(CannotIgnoreColumn);
                    return;
                }
            }

            if ((bool)cbObjectNameColumn.IsChecked)
            {
                if (transferedColumn.CompareTo(cmbCLassNumberColumn.SelectedValue.ToString()) == 0)
                {
                    Utils.ShowWarningMessage(CannotIgnoreColumn);
                    return;
                }
            }

            _columnDataViewModel.ActiveItems.Remove(transferedColumn);
            _columnDataViewModel.IgnoredItems.Add(transferedColumn);

            return;
        }

        private void ActivateSelectedItem(object sender, RoutedEventArgs e)
        {
            if (lbIgnoredColumns.SelectedIndex == -1)
            {
                return;
            }

            string transferedColumn = _columnDataViewModel.IgnoredItems[lbIgnoredColumns.SelectedIndex];

            _columnDataViewModel.IgnoredItems.Remove(transferedColumn);
            _columnDataViewModel.ActiveItems.Add(transferedColumn);

            return;
        }

        public IUniversalReader Reader {
            get {
                return _reader;
            }
        }

        private void Column_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == SelectedElementIndex)
            {
                return;
            }

            if (e.AddedItems.Count == SelectedElementIndex)
            {
                return;
            }

            string removedItem = e.RemovedItems[SelectedElementIndex].ToString();
            bool isIdenticalColumns = CheckIdenticalSelection();

            if (isIdenticalColumns)
            {
                Utils.ShowErrorMessage(BadColumnChoiseMessage);
                RestoreLastValue(sender as ComboBox, removedItem);

                return;
            }

            string addedItem = e.AddedItems[SelectedElementIndex].ToString();

            RestoreItem(removedItem);
            DeleteItemFromItems(addedItem);
            SetToolTip(sender, addedItem);

            return;
        }

        private void RestoreLastValue(ComboBox comboBox, string lastItem)
        {            
            comboBox.SelectedItem = lastItem;
        }

        private void AddItem(ObservableCollection<string> observableCollection, string item)
        {
            observableCollection.Add(item);
        }

        private void DeleteItemFromItems(string deletedItem)
        {
            RemoveItem(_columnDataViewModel.ActiveItems, deletedItem);
            RemoveItem(_columnDataViewModel.IgnoredItems, deletedItem);
        }

        private void RemoveItem(ObservableCollection<string> observableCollection, string item)
        {
            if (observableCollection.Contains(item))
            {
                observableCollection.Remove(item);
            }
        }

        private void ToolTip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            string toolTipText = e.AddedItems[0].ToString();
            SetToolTip(sender, toolTipText);
        }

        private void SetToolTip(object uiObject, string toolTip)
        {
            Control uiControl = uiObject as Control;
            uiControl.ToolTip = toolTip;
        }

        private void cbClassNumberColumn_Checked(object sender, RoutedEventArgs e)
        {
            DeleteItemFromItems(cmbCLassNumberColumn.SelectedItem.ToString());
            cmbCLassNumberColumn.IsEnabled = true;
        }

        private void cbClassNumberColumn_Unchecked(object sender, RoutedEventArgs e)
        {
            string selectedItem = cmbCLassNumberColumn.SelectedItem.ToString();
            RestoreItem(selectedItem);
            cmbCLassNumberColumn.IsEnabled = false;
        }

        private void RestoreItem(string item)
        {
            if (!_columnDataViewModel.ActiveItems.Contains(item))
            {
                AddItem(_columnDataViewModel.ActiveItems, item);
            }
        }

        private void cbObjectNameColumn_Checked(object sender, RoutedEventArgs e)
        {
            DeleteItemFromItems(cmbObjectNameColumn.SelectedItem.ToString());
            cmbObjectNameColumn.IsEnabled = true;
        }

        private void cbObjectNameColumn_Unchecked(object sender, RoutedEventArgs e)
        {
            string selectedItem = cmbObjectNameColumn.SelectedItem.ToString();
            RestoreItem(selectedItem);
            cmbObjectNameColumn.IsEnabled = false;
        }
    }
}
