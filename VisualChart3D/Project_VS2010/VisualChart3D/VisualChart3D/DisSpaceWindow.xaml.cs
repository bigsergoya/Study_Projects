// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
namespace VisualChart3D
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class DisSpaceWin : Window
    {
        public int firstСhoosedОbject;
        public int secondChoosedObject;
        public int thirdChoosedObject;
        public bool basisObjectColorMode;
        public bool ReferencedObjectsMode;
        public string sizeOfSpace;
        SettingsFiles SettFile;
        public DisSpaceWin(SettingsFiles CurrentSetFile, int firstObject, int secondObject, int thirdObject, string spaceSize, bool objectsColorMode, bool referencedObjectsMode) 
        {
            InitializeComponent();
            SettFile = CurrentSetFile;

            cbThirdClassChoose.IsEnabled = false;
            pcThirdSelectedObject.Visibility = Visibility.Hidden;
            
            cbFirstClassChoose.ItemsSource = SettFile.NamesObjects;
            firstСhoosedОbject = firstObject;
            cbFirstClassChoose.SelectedIndex = firstObject - 1;


            cbSecondClassChoose.ItemsSource = SettFile.NamesObjects;          
            secondChoosedObject = secondObject;            
            cbSecondClassChoose.SelectedIndex = secondObject-1;

            cbThirdClassChoose.ItemsSource = SettFile.NamesObjects;
            thirdChoosedObject = thirdObject;
            cbThirdClassChoose.SelectedIndex = thirdObject - 1;
            sizeOfSpace = spaceSize;
            if (sizeOfSpace.Equals("3D"))
                Is3DSpace.IsChecked = true;
            cbBasisObjectColorMode.IsChecked = objectsColorMode;

            cbAutomaticlySettingUpOfReference.IsChecked = referencedObjectsMode;
            btRefreshReferenceObjects.IsEnabled = referencedObjectsMode;
        }

        private void btBackAndSave_Click(object sender, RoutedEventArgs e)
        {
            if ((!cbFirstClassChoose.SelectedIndex.Equals(-1)) && (!cbSecondClassChoose.SelectedIndex.Equals(-1)))
            {
                firstСhoosedОbject = cbFirstClassChoose.SelectedIndex+1;
                secondChoosedObject = cbSecondClassChoose.SelectedIndex+1;
                thirdChoosedObject = cbThirdClassChoose.SelectedIndex + 1;
                basisObjectColorMode = (bool)cbBasisObjectColorMode.IsChecked;
                this.Close();
            }
        }

        private void btBackWithoutSaving_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Paining(Image Picture, int SelectedIndex)
        {
            bool is_Picture = SettFile.isPictureTakenByClassInterval || SettFile.isPictureTakenByClassStartObjects || SettFile.isPictureTakenByObjectID || SettFile.isPictureTakenByObjectName;
            if ((SelectedIndex != -1) && (is_Picture))
            {
                if ((SettFile.isPictureTakenByObjectID) || (SettFile.isPictureTakenByObjectName))
                {
                    List<string> Pictures = Get_Pictures_Files_List(Directory.GetFiles(SettFile.Pic_Folder_Adress));
                    if (SettFile.isPictureTakenByObjectID)
                    {  //эксперимент - проверка надобности отдельной функции типа инт для выборки картинки. Вместо инт посылаем стринг
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, SettFile.NamesObjects[SelectedIndex], Picture));
                    }

                    if (SettFile.isPictureTakenByObjectName)
                    {
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, SettFile.NamesObjects[SelectedIndex], Picture));
                    }
                }

                if ((SettFile.isPictureTakenByClassStartObjects) || (SettFile.isPictureTakenByClassInterval))
                {
                    String substring;
                    int k;
                    List<string> Pictures;
                    int cur_index;
                    DirectoryInfo[] dirs = new DirectoryInfo(SettFile.Pic_Folder_Adress).GetDirectories();
                    foreach (var item in dirs)
                        if (item.Name.Equals(SettFile.ClassesName[SelectedIndex]))
                        { 
                            Pictures = Get_Pictures_Files_List(Directory.GetFiles(item.FullName));
                            cur_index = Int32.Parse(SettFile.NamesObjects[SelectedIndex]);
                            for (k = 0; k < SettFile.Class_Start_Position.Count - 1; k++)
                                if ((cur_index >= Int32.Parse(SettFile.Class_Start_Position[k])) && (cur_index <= Int32.Parse(SettFile.Class_Start_Position[k + 1])))
                                {
                                    cur_index -= Int32.Parse(SettFile.Class_Start_Position[k]);
                                    break;
                                }
                            if ((Pictures.Count == (Int32.Parse(SettFile.Class_Start_Position[k + 1]) - Int32.Parse(SettFile.Class_Start_Position[k]))))
                            {
                                substring = Pictures[cur_index-1];
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            else
                            {
                                substring = Get_Picture_Adress(Pictures, cur_index,Picture);
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            break;
                        }
                }
            }
        }
        private void cb_FirstClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbFirstClassChoose.SelectedIndex != -1)
                Paining(pcFirstSelectedObject, cbFirstClassChoose.SelectedIndex);
        }

        private void cb_SecondClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSecondClassChoose.SelectedIndex != -1)
                Paining(pcSecondSelectedObject, cbSecondClassChoose.SelectedIndex);
        }
        private void cbThirdClassChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbThirdClassChoose.SelectedIndex != -1)
                Paining(pcThirdSelectedObject, cbThirdClassChoose.SelectedIndex);
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
                    //if (((byte)adress[start] < 48) && ((byte)adress[start] > 57))  // если в имени файла есть символы, то ошибка.
                    if (char.IsDigit(adress[start]))  // если в имени файла есть символы, то ошибка.
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
                //MessageBox.Show("Ошибка! Невозможно преобразовать значение после выборки из адреса папки с картинками.");
                //возможно стоит просто не информировать а просто по буллу выводить пустую картинку при ошибке
                return false;
            }
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
                MessageBox.Show("Ошибка при выборке и проверке имени файла по имени объекта.");
                //возможно стоит просто не информировать а просто по буллу выводить пустую картинку при ошибке
                return false;
            }
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
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(adress, UriKind.RelativeOrAbsolute);  //urisource тупит и картинка не показывается дефолтная
            bi.EndInit();
            return bi;
        }
        private string Get_Picture_Adress(List<string> Pics, int order_of_picture,Image Picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, order_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }
        private string Get_Picture_Adress(List<string> Pics, string name_of_picture,Image Picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, name_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }

        private void Is3DSpace_Checked(object sender, RoutedEventArgs e)
        {
            sizeOfSpace = "3D";
            cbThirdClassChoose.IsEnabled = true;
            pcThirdSelectedObject.Visibility = Visibility.Visible;
        }

        private void Is3DSpace_Unchecked(object sender, RoutedEventArgs e)
        {
            sizeOfSpace = "2D";
            cbThirdClassChoose.IsEnabled = false;
            pcThirdSelectedObject.Visibility = Visibility.Hidden;
        }

        private void cbAutomaticlySettingUpOfReference_Checked(object sender, RoutedEventArgs e)
        {
            //обратно отправлять флажок с режимом центровки и с списком описания центровых объектов
            List<string> referedObjectsSavedInfo; //SettFile.SourceMatrixFile
            if ((referedObjectsSavedInfo =
                GetDataFromReferenceLogFile(SettFile.SourceMatrixFile)) == null)
            {
                ReferencedObjects refObjects = new ReferencedObjects(SettFile.ArraySource, SettFile.numberOfObjectsOfClass);
                ///referedObjectsSavedInfo = new List<string>();
                referedObjectsSavedInfo = (refObjects.getReferencedObjectsWithClassNames(SettFile.UniqClassesName));
                lbReferencedObjects.ItemsSource = referedObjectsSavedInfo;
                WriteDataToReferenceLog(SettFile.SourceMatrixFile, referedObjectsSavedInfo);
                btRefreshReferenceObjects.IsEnabled = true;
            }
            else
            {
                lbReferencedObjects.ItemsSource = (referedObjectsSavedInfo);
            }
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
                MessageBox.Show("Не удалось чтение адреса директории с изображениями из лог-файла");
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
                    foreach(string line in data){
                        file.WriteLine(line);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось создать лог-файл для сохранения выбранного путя к директории с изображениями.");
            }

        }

        private void cbAutomaticlySettingUpOfReference_Unchecked(object sender, RoutedEventArgs e)
        {
            btRefreshReferenceObjects.IsEnabled = false;
            cbAutomaticlySettingUpOfReference.IsChecked = false;
            lbReferencedObjects.Items.Clear();
        }

        private void btRefreshReferenceObjects_Click(object sender, RoutedEventArgs e)
        {
            List<string> referedObjectsSavedInfo;
            ReferencedObjects refObjects = new ReferencedObjects(SettFile.ArraySource, SettFile.numberOfObjectsOfClass);
            //referedObjectsSavedInfo = new List<string>();
            referedObjectsSavedInfo = (refObjects.getReferencedObjectsWithClassNames(SettFile.UniqClassesName));
            lbReferencedObjects.ItemsSource = referedObjectsSavedInfo;
            WriteDataToReferenceLog(SettFile.SourceMatrixFile, referedObjectsSavedInfo);
        }
    }
}
