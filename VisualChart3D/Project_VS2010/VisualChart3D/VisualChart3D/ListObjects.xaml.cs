// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using VisualChart3D.Common;

namespace VisualChart3D
{
    /// <summary>
    /// Interaction logic for ListObjects.xaml
    /// </summary>
    /// 
    //-----------------------------------------------------
    public partial class ListObjects : Window
    {
        private const string GettingFileByNameErrorMessage = "Ошибка при выборке и проверке имени файла по имени объекта.";
        private const string PicturesLogFileName = "PictureAdressLog.txt";
        private const string PicturesFolderNotFoundErrorMessage = "Не найдена папка с картинками по адресу '{0}'.{1} Возможно, удаление лог-файла '{2}' в папке с исходными данными и перезапуск программы поможет решить проблему.";

        private string _adressPictureDirectory = null;
        private List<int> _numberOfObjects; //все кроме переменных - с большой буквой
        private List<string> _namesOfObjects;
        private List<string> _namesOfClasses;
        private List<string> countOfClasses;
        private List<SavedPictureInfo> _savedPathsToPictures;
        private double[,] _coords;
        private bool _isDisSpaceMode = false;
        private bool _isInformationLoaded; // Поймали ли в фокус объекты и будет ли отражена инфа о них
        private bool _isPicturesByID;
        private bool _isPicturesByName;
        private bool _isPicturesByClassInterval; //- 3 пункт меню, для файла с только номерами начал классов
        private bool _isPicturesByClassStartObject; // - 4 пункт меню, для файла с номерами начал и названиями классов
        private bool _currentIndexFlag = false; // - для того, чтобы компенсировать баг сброса индекса при обновлении бокса

        /// <summary>
        /// Переменная-буфер
        /// </summary>
        public event EventHandler CloseEvent;
        //------------------------------------
        public bool IsClosing = false;

        struct SavedPictureInfo
        {
            public string label;
            public string path;
            public SavedPictureInfo(string newLabel, string newPath)
            {
                label = newLabel;
                path = newPath;
            }
        }

        public ListObjects()
        {
            InitializeComponent();
        }
        private bool Is_Picture_Exist(string adress, int number_of_picture)
        {
            try
            {
                int end = adress.IndexOf('.');
                if (end == -1)
                    return false;
                int start = end - 1;
                // char.IsDigit -  в рефракторинг
                while (adress[start] != '\\')
                {
                    if (((byte)adress[start] < 48) && ((byte)adress[start] > 57))  // если в имени файла есть символы, то ошибка.
                        return false;                                              // либо запилить остановку до символов и считывать такое - игрок99
                    start--;
                }
                start++;
                String name_of_file = adress.Substring(start, end - start);
                if (number_of_picture == Int32.Parse(name_of_file))
                    return true;
                else
                    return false;
            }
            catch (Exception Exp)
            {
                Utils.ShowErrorMessage(GettingFileByNameErrorMessage);
                //возможно стоит просто не информировать а просто по буллу выводить пустую картинку при ошибке
                return false;
            }
        }
        public void setCoords(double[,] newCoords)
        {
            _coords = newCoords;
        }

        private bool Is_Picture_Exist(string adress, string name_of_object)
        {
            try
            {
                int end = adress.IndexOf('.');
                if (end == -1)
                    return false;
                int start = end - 1;
                while (adress[start] != '\\')
                {

                    start--;
                }
                start++;
                String name_of_file = adress.Substring(start, end - start);
                //if (name_of_object == (name_of_file))
                if (string.Compare(name_of_object, name_of_file, true) == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception Exp)
            {
                Utils.ShowErrorMessage(GettingFileByNameErrorMessage);
                //возможно стоит просто не информировать а просто по буллу выводить пустую картинку при ошибке
                return false;
            }
        }
        public void SetListObjects(Engine settFiles, int[] idxArr, double[,] coordsCurrent)
        {
            _coords = coordsCurrent;
            Picture.Source = new BitmapImage(new Uri("Resources/empty_picture.jpg", UriKind.Relative));
            _isInformationLoaded = false;
            _isPicturesByID = false;
            _isPicturesByName = false;
            _isPicturesByClassInterval = false;
            _isPicturesByClassStartObject = false;
            ListBoxObjects.Items.Clear();
            //-------------------------------
            _adressPictureDirectory = settFiles.Pic_Folder_Adress;

            //-------------------------------
            if (idxArr == null)
            {
                _isInformationLoaded = false;
                return;
            }
            else
                _isInformationLoaded = true;
            _isPicturesByID = settFiles.isPictureTakenByObjectID;
            _numberOfObjects = new List<int>();
            _namesOfObjects = new List<string>();
            _namesOfClasses = new List<string>();
            _isPicturesByName = settFiles.isPictureTakenByObjectName;
            _isPicturesByClassInterval = settFiles.isPictureTakenByClassInterval;
            countOfClasses = settFiles.Class_Start_Position;
            _isPicturesByClassStartObject = settFiles.isPictureTakenByClassStartObjects;
            Title = "Список объектов. Количество:" + idxArr.Length;
            _numberOfObjects.AddRange(idxArr);
            _isDisSpaceMode = settFiles.AlgorithmType == AlgorithmType.DisSpace;
            _savedPathsToPictures = new List<SavedPictureInfo>();

            if (settFiles.NamesObjectSelected)
            {
                foreach (int i in idxArr)
                {
                    _namesOfObjects.Add(settFiles.NamesObjects[i]);
                    _namesOfClasses.Add(settFiles.ClassesName[i]);
                    ListBoxObjects.Items.Add(string.Format("Объект №{0}\tКласс: {1}\tИмя: {2}", i, settFiles.ClassesName[i],
                        settFiles.NamesObjects[i]));
                    //даже если у нас есть имена объектов по текущей логике всеравно можно загрузить посредством айди
                }
            }
            else
            {
                foreach (int i in idxArr)
                {
                    _namesOfObjects.Add(settFiles.NamesObjects[i]);
                    _namesOfClasses.Add(settFiles.ClassesName[i]);
                    ListBoxObjects.Items.Add(string.Format("Объект №{0}\tКласс: {1}", settFiles.NamesObjects[i], settFiles.ClassesName[i]));

                }
            }

            ListBoxObjects.SelectedIndex = 0;
            ListBoxObjects.ScrollIntoView(ListBoxObjects.SelectedItem);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsClosing = true;
            CloseEvent?.Invoke(this, new EventArgs());
        }

        private List<string> Get_Pictures_Files_List(string[] All_Files)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".bmp", ".png" };
            List<string> List_of_Files = new List<string>();

            foreach (string file in All_Files)
            {
                foreach (string extension in imageExtensions)
                { //рефракторинг
                    if (file.EndsWith(extension))
                    {
                        List_of_Files.Add(file);
                        break;
                    }
                }
            }
            return List_of_Files;
        }

        private BitmapImage Add_Picture_On_Screen(string adress)
        {
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri(adress, UriKind.RelativeOrAbsolute);
            bitImage.EndInit();
            return bitImage;

        }

        private string Get_Picture_Adress(List<string> Pics, int order_of_picture)
        {

            string result = _savedPathsToPictures.Find(path => path.label.Equals(order_of_picture.ToString())).path;

            if (!String.IsNullOrEmpty(result))
            {
                Picture.Source = Add_Picture_On_Screen(result);
                return result;
            }//нужен тест!!!!

            result = Pics.Find(pic => (Is_Picture_Exist(pic, order_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            _savedPathsToPictures.Add(new SavedPictureInfo(order_of_picture.ToString(), result));
            return result;
        }

        private string Get_Picture_Adress(List<string> Pics, string name_of_picture)
        {
            string result = _savedPathsToPictures.Find(path => path.label.Equals(name_of_picture)).path;

            if (result != null)
            {
                Picture.Source = Add_Picture_On_Screen(result);
                return result;
            }//нужен тест!!!!

            result = Pics.Find(pic => (Is_Picture_Exist(pic, name_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            _savedPathsToPictures.Add(new SavedPictureInfo(name_of_picture.ToString(), result));
            return result;
        }

        public void displayObjectCoords(int selectedInted)
        {
            if (!_isDisSpaceMode)
            {
                tbCurrentObjectCoords.Text =
                    "x=" + _coords[(_numberOfObjects[selectedInted]), 0] + Environment.NewLine +
                    "y=" + _coords[(_numberOfObjects[selectedInted]), 1] + Environment.NewLine +
                    "z=" + _coords[(_numberOfObjects[selectedInted]), 2];
            }
            else
            {
                if (_coords.GetLength(0) == 2)
                {
                    tbCurrentObjectCoords.Text =
                        "x=" + _coords[0, (_numberOfObjects[selectedInted])] + Environment.NewLine +
                        "y=" + _coords[1, (_numberOfObjects[selectedInted])] + Environment.NewLine +
                        "z=0";
                }
                else
                {
                    tbCurrentObjectCoords.Text =
                        "x=" + _coords[0, (_numberOfObjects[selectedInted])] + Environment.NewLine +
                        "y=" + _coords[1, (_numberOfObjects[selectedInted])] + Environment.NewLine +
                        "z=" + _coords[2, (_numberOfObjects[selectedInted])];
                }
            }
        }

        private void ListBoxObjects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            /*
Пофиксить вывод координат, не все оси соответствуют изображенным.
             */
            if (_currentIndexFlag)
            {
                _currentIndexFlag = false;
                return;
            }

            if ((ListBoxObjects.SelectedIndex != -1) && ((string)ListBoxObjects.Items[ListBoxObjects.SelectedIndex] != String.Empty) && (_isInformationLoaded))
            {
                displayObjectCoords(ListBoxObjects.SelectedIndex);
                CallBackPoint.callbackEventHandler(ListBoxObjects.SelectedIndex);

                if ((_isPicturesByID) || (_isPicturesByName))
                {
                    List<string> Pictures = Get_Pictures_Files_List(Directory.GetFiles(_adressPictureDirectory));
                    if (_isPicturesByID)
                    {
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, _numberOfObjects[ListBoxObjects.SelectedIndex]));
                    }

                    //нужен рефакторинг массивов и флаговой структуры, все устарело.
                    if (_isPicturesByName)
                    {
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, _namesOfObjects[ListBoxObjects.SelectedIndex]));
                    }

                }

                if ((_isPicturesByClassStartObject) || (_isPicturesByClassInterval))
                {
                    string result = _savedPathsToPictures.Find(path => path.label.Equals((Int32.Parse(_namesOfObjects[ListBoxObjects.SelectedIndex]) - 1).ToString())).path;
                    if (result != null)
                    {
                        Picture.Source = Add_Picture_On_Screen(result);
                        return;
                    }

                    String substring = String.Empty;
                    int k;
                    List<string> Pictures;
                    int cur_index;

                    if (Directory.Exists(_adressPictureDirectory))
                    {
                        DirectoryInfo[] dirs = new DirectoryInfo(_adressPictureDirectory).GetDirectories();
                        foreach (var item in dirs)
                        {
                            if (item.Name.Equals(_namesOfClasses[ListBoxObjects.SelectedIndex]))
                            {
                                Pictures = Get_Pictures_Files_List(Directory.GetFiles(item.FullName));
                                cur_index = Int32.Parse(_namesOfObjects[ListBoxObjects.SelectedIndex]) - 1;

                                for (k = 0; k < countOfClasses.Count - 1; k++)
                                {
                                    if ((cur_index >= Int32.Parse(countOfClasses[k])) && (cur_index < Int32.Parse(countOfClasses[k + 1])))
                                    {
                                        cur_index -= Int32.Parse(countOfClasses[k]);
                                        break;
                                    }
                                }

                                if ((Pictures.Count == (Int32.Parse(countOfClasses[k + 1]) - Int32.Parse(countOfClasses[k]))))
                                {
                                    substring = Pictures[cur_index];
                                    _savedPathsToPictures.Add(new SavedPictureInfo(cur_index.ToString(), substring));
                                    Picture.Source = Add_Picture_On_Screen(substring);
                                }
                                else
                                {
                                    substring = Get_Picture_Adress(Pictures, cur_index + 1);
                                    Picture.Source = Add_Picture_On_Screen(substring);
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        Utils.ShowErrorMessage(String.Format(PicturesFolderNotFoundErrorMessage, _adressPictureDirectory, Environment.NewLine, PicturesLogFileName));
                        
                        //MessageBox.Show("Не найдена папка с картинками по адресу '" + _adressPictureDirectory + "'"
                     //+ Environment.NewLine + "Возможно, удаление лог-файла PictureAdressLog.txt в папке с исходными данными и перезапуск программы поможет решить проблему.");
                    }

                    if ((!ListBoxObjects.SelectedValue.ToString().Contains("\tФайл:")) && (ListBoxObjects.SelectedIndex != -1))
                    {
                        _currentIndexFlag = true;
                        int current_index = ListBoxObjects.SelectedIndex;
                        ListBoxObjects.Items[ListBoxObjects.SelectedIndex] += string.Format(" \tФайл: " + substring);
                        _currentIndexFlag = true;
                        ListBoxObjects.SelectedIndex = current_index; //ПЕРЕДЕЛАТЬ КОНСТРУКЦИЮ!!! Двойной список, полностью повторяющий вывод картинки и инфы о ней
                    }
                }
            }
        }
    }
}
