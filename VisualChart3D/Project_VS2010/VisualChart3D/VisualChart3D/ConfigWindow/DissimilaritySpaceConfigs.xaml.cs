// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VisualChart3D.Common;
using VisualChart3D.Common.Visualization;

namespace VisualChart3D.ConfigWindow
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class DissimilaritySpaceConfigs : Window
    {
        private readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".bmp", ".png" };
        private const string ObjectNameErrorMessage = "Ошибка при выборке и проверке имени файла по имени объекта.";
        private const string ReadingDirAdressErrorMessage = "Не удалось чтение адреса директории с изображениями из лог-файла";
        private const string CreatingLogFileErrorMessage = "Не удалось создать лог-файл для сохранения выбранного путя к директории с изображениями.";
        private const string BadFirstIndexWarningMessage = "Неккоректный выбор первого объекта. Пожалуйста, повторите.";
        private const string BadSecondIndexWarningMessage = "Неккоректный выбор второго объекта. Пожалуйста, повторите.";
        private const string BadThirdIndexWarningMessage = "Неккоректный выбор третьего объекта. Пожалуйста, повторите.";

        private DisSpace _dissimilaritySpace;
        private Engine settFile;
        private Common.DataBinding.ReferencedObjectsViewModel _referencedObjectsViewModel;

        public DisSpace DissimilaritySpace { get => _dissimilaritySpace; }

        public DissimilaritySpaceConfigs(Engine CurrentSetFile, DisSpace disSpace)
        {
            InitializeComponent();

            _referencedObjectsViewModel = new Common.DataBinding.ReferencedObjectsViewModel();
            DataContext = _referencedObjectsViewModel;

            settFile = CurrentSetFile;
            _dissimilaritySpace = disSpace;
            cbThirdClassChoose.IsEnabled = false;
            pcThirdSelectedObject.Visibility = Visibility.Hidden;

            cbFirstClassChoose.ItemsSource = settFile.NamesObjects;
            //firstСhoosedОbject = firstObject;
            cbFirstClassChoose.SelectedIndex = disSpace.FirstBasisObject - 1;


            cbSecondClassChoose.ItemsSource = settFile.NamesObjects;
            //secondChoosedObject = secondObject;
            cbSecondClassChoose.SelectedIndex = disSpace.SecondBasisObject - 1;

            cbThirdClassChoose.ItemsSource = settFile.NamesObjects;
            //thirdChoosedObject = thirdObject;
            cbThirdClassChoose.SelectedIndex = disSpace.ThirdBasisObject - 1;

            //sizeOfSpace = spaceSize;

            if (disSpace.Space == Space.ThreeDimensional)
            {
                Is3DSpace.IsChecked = true;
            }

            cbBasisObjectColorMode.IsChecked = disSpace.BasicObjectsColorMode;
            //cbAutomaticlySettingUpOfReference.IsChecked = referencedObjectsMode;

            InitializeReferencedObjects();
        }

        private void btBackAndSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbFirstClassChoose.SelectedIndex.Equals(-1))
            {
                Utils.ShowWarningMessage(BadFirstIndexWarningMessage);
                return;
            }

            if (cbSecondClassChoose.SelectedIndex.Equals(-1))
            {
                Utils.ShowWarningMessage(BadSecondIndexWarningMessage);
                return;
            }

            if (cbThirdClassChoose.SelectedIndex.Equals(-1))
            {
                Utils.ShowWarningMessage(BadThirdIndexWarningMessage);
                return;
            }

            if ((!cbFirstClassChoose.SelectedIndex.Equals(-1)) && (!cbSecondClassChoose.SelectedIndex.Equals(-1)))
            {
                _dissimilaritySpace.FirstBasisObject = cbFirstClassChoose.SelectedIndex + 1;
                _dissimilaritySpace.SecondBasisObject = cbSecondClassChoose.SelectedIndex + 1;
                _dissimilaritySpace.ThirdBasisObject = cbThirdClassChoose.SelectedIndex + 1;
                _dissimilaritySpace.BasicObjectsColorMode = (bool)cbBasisObjectColorMode.IsChecked;
                this.Close();
            }
        }

        private void btBackWithoutSaving_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Paining(Image Picture, int SelectedIndex)
        {
            bool is_Picture = settFile.isPictureTakenByClassInterval || settFile.isPictureTakenByClassStartObjects || settFile.isPictureTakenByObjectID || settFile.isPictureTakenByObjectName;
            if ((SelectedIndex != -1) && (is_Picture))
            {
                if ((settFile.isPictureTakenByObjectID) || (settFile.isPictureTakenByObjectName))
                {
                    List<string> Pictures = Get_Pictures_Files_List(Directory.GetFiles(settFile.picFolderAdress));
                    if (settFile.isPictureTakenByObjectID)
                    {  //эксперимент - проверка надобности отдельной функции типа инт для выборки картинки. Вместо инт посылаем стринг
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, settFile.NamesObjects[SelectedIndex], Picture));
                    }

                    if (settFile.isPictureTakenByObjectName)
                    {
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, settFile.NamesObjects[SelectedIndex], Picture));
                    }
                }

                if ((settFile.isPictureTakenByClassStartObjects) || (settFile.isPictureTakenByClassInterval))
                {
                    String substring;
                    int k;
                    List<string> Pictures;
                    int cur_index;
                    DirectoryInfo[] directories = new DirectoryInfo(settFile.picFolderAdress).GetDirectories();

                    foreach (var directory in directories)
                    {
                        if (directory.Name.Equals(settFile.ClassesName[SelectedIndex]))
                        {
                            Pictures = Get_Pictures_Files_List(Directory.GetFiles(directory.FullName));
                            cur_index = Int32.Parse(settFile.NamesObjects[SelectedIndex]);

                            for (k = 0; k < settFile.classStartPosition.Count - 1; k++)
                            {
                                if ((cur_index >= Int32.Parse(settFile.classStartPosition[k])) && (cur_index <= Int32.Parse(settFile.classStartPosition[k + 1])))
                                {
                                    cur_index -= Int32.Parse(settFile.classStartPosition[k]);
                                    break;
                                }
                            }

                            if ((Pictures.Count == (Int32.Parse(settFile.classStartPosition[k + 1]) - Int32.Parse(settFile.classStartPosition[k]))))
                            {
                                substring = Pictures[cur_index - 1];
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            else
                            {
                                substring = Get_Picture_Adress(Pictures, cur_index, Picture);
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void cb_FirstClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFirstClassChoose.SelectedIndex != -1)
            {
                Paining(pcFirstSelectedObject, cbFirstClassChoose.SelectedIndex);
            }
        }

        private void cb_SecondClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSecondClassChoose.SelectedIndex != -1)
            {
                Paining(pcSecondSelectedObject, cbSecondClassChoose.SelectedIndex);
            }
        }

        private void cbThirdClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbThirdClassChoose.SelectedIndex != -1)
            {
                Paining(pcThirdSelectedObject, cbThirdClassChoose.SelectedIndex);
            }
        }

        private bool Is_Picture_Exist(string adress, int number_of_picture)
        {
            try
            {
                int end = adress.IndexOf('.');

                if (end == -1)
                {
                    return false;
                }

                int start = end - 1;

                // char.IsDigit -  в рефракторинг
                while (adress[start] != '\\')
                {
                    //if (((byte)adress[start] < 48) && ((byte)adress[start] > 57))  // если в имени файла есть символы, то ошибка.
                    if (char.IsDigit(adress[start]))
                    {  // если в имени файла есть символы, то ошибка.
                        return false;
                    }// либо запилить остановку до символов и считывать такое - игрок99
                    start--;
                }

                start++;
                String name_of_file = adress.Substring(start, end - start);

                if (number_of_picture == Int32.Parse(name_of_file))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Is_Picture_Exist(string adress, string name_of_object)
        {
            try
            {
                int end = adress.IndexOf('.');

                if (end == -1)
                {
                    return false;
                }

                int start = end - 1;

                while (adress[start] != '\\')
                {

                    start--;
                }

                start++;
                String name_of_file = adress.Substring(start, end - start);

                if (string.Compare(name_of_object, name_of_file, true) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                Utils.ShowErrorMessage(ObjectNameErrorMessage);
                return false;
            }
        }

        private List<string> Get_Pictures_Files_List(string[] allFiles)
        {

            List<string> pictureFiles = new List<string>();

            foreach (string file in allFiles)
            {
                foreach (string extension in ImageExtensions)
                { //рефракторинг
                    if (file.EndsWith(extension))
                    {
                        pictureFiles.Add(file);
                        break;
                    }
                }
            }

            return pictureFiles;
        }

        private BitmapImage Add_Picture_On_Screen(string adress)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(adress, UriKind.RelativeOrAbsolute);  //urisource тупит и картинка не показывается дефолтная
            bi.EndInit();
            return bi;
        }

        private string Get_Picture_Adress(List<string> Pics, int order_of_picture, Image Picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, order_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }

        private string Get_Picture_Adress(List<string> Pics, string name_of_picture, Image Picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, name_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }

        private void Is3DSpace_Checked(object sender, RoutedEventArgs e)
        {
            //sizeOfSpace = "3D";
            _dissimilaritySpace.Space = Space.ThreeDimensional;
            cbThirdClassChoose.IsEnabled = true;
            pcThirdSelectedObject.Visibility = Visibility.Visible;
        }

        private void Is3DSpace_Unchecked(object sender, RoutedEventArgs e)
        {
            //sizeOfSpace = "2D";
            _dissimilaritySpace.Space = Space.TwoDimensional;
            cbThirdClassChoose.IsEnabled = false;
            pcThirdSelectedObject.Visibility = Visibility.Hidden;
        }

       /* private void cbAutomaticlySettingUpOfReference_Checked(object sender, RoutedEventArgs e)
        {
            //обратно отправлять флажок с режимом центровки и с списком описания центровых объектов
            InitializeReferencedObjects();
            
        }*/

        private void InitializeReferencedObjects()
        {
            List<string> referedObjectsSavedInfo; //SettFile.SourceMatrixFile
            referedObjectsSavedInfo = GetDataFromReferenceLogFile(settFile.UniversalReader.SourceMatrixFile);

            if (referedObjectsSavedInfo == null)
            {
                referedObjectsSavedInfo = _dissimilaritySpace.GetReferencedObjectsWithClassNames(settFile);

                WriteDataToReferenceLog(settFile.UniversalReader.SourceMatrixFile, referedObjectsSavedInfo);
                btRefreshReferenceObjects.IsEnabled = true;
            }

            _referencedObjectsViewModel.ReferedObjectsInfo = new ObservableCollection<string>(referedObjectsSavedInfo);
        }

        private List<string> GetDataFromReferenceLogFile(string pathToAnyMatrix)
        {
            List<string> data = new List<string>();
            string pathToLogFile = pathToAnyMatrix
                    .Remove(pathToAnyMatrix.LastIndexOf('\\') + 1) + "adressOfReferenceLogFile.txt";
            try
            {
                if (File.Exists(pathToLogFile))
                {
                    data.AddRange(File.ReadAllLines(pathToLogFile));
                    return data;
                }
            }
            catch
            {
                Utils.ShowErrorMessage(ReadingDirAdressErrorMessage);
            }
            return null;
        }

        private void WriteDataToReferenceLog(string pathToAnyMatrix, List<string> data)
        {
            string pathToPictureContentAdressLog = pathToAnyMatrix.Remove(pathToAnyMatrix.LastIndexOf('\\') + 1)
                + "adressOfReferenceLogFile.txt";
            try
            {
                using (WriteTextToFile file = new WriteTextToFile(pathToPictureContentAdressLog))
                {
                    foreach (string line in data)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch
            {
                Utils.ShowErrorMessage(CreatingLogFileErrorMessage);
            }

        }

        /*private void cbAutomaticlySettingUpOfReference_Unchecked(object sender, RoutedEventArgs e)
        {
            btRefreshReferenceObjects.IsEnabled = false;
            cbAutomaticlySettingUpOfReference.IsChecked = false;
            lbReferencedObjects.Items.Clear();
        }*/

        private void btRefreshReferenceObjects_Click(object sender, RoutedEventArgs e)
        {
            List<string> referedObjectsSavedInfo = _dissimilaritySpace.GetReferencedObjectsWithClassNames(settFile);

            _referencedObjectsViewModel.ReferedObjectsInfo = new ObservableCollection<string>(referedObjectsSavedInfo);

            WriteDataToReferenceLog(settFile.UniversalReader.SourceMatrixFile, referedObjectsSavedInfo);
        }

        private void lbReferencedObjects_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
