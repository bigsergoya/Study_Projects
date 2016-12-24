// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
namespace VisualChart3D
{
	/// <summary>
	/// Interaction logic for ListObjects.xaml
	/// </summary>
    /// 
    //-----------------------------------------------------
	public partial class ListObjects : Window
	{
        //место объявления переменной-буфера
		public event EventHandler CloseEvent;
        //вытаскиваем это - --------------------------------
        private string adressPictureDirectory = null;
        //private List<List<int>> Pictures = new List<List<int>>();
        List<int> numberOfObjects; //все кроме переменных - с большой буквой
        List<string> namesOfObjects;
        List<string> namesOfClasses;
        List<string> countOfClasses;
        double[,] coords;
        bool isDisSpaceMode = false;
        bool isInformationLoaded; // Поймали ли в фокус объекты и будет ли отражена инфа о них
        bool isPicturesByID;
        bool isPicturesByName;
        bool isPicturesByClassInterval; //- 3 пункт меню, для файла с только номерами начал классов
        bool isPicturesByClassStartObject; // - 4 пункт меню, для файла с номерами начал и названиями классов
        //------------------------------------
		public bool IsClosing = false;
        
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
		public void SetListObjects(SettingsFiles settFiles, int[] idxArr, double[,] coordsCurrent)
		{
            coords = coordsCurrent;
            Picture.Source = new BitmapImage(new Uri("Resources/empty_picture.jpg", UriKind.Relative));
            isInformationLoaded = false; 
            isPicturesByID = false;
            isPicturesByName = false;
            isPicturesByClassInterval = false; 
            isPicturesByClassStartObject = false;
			ListBoxObjects.Items.Clear();
            //-------------------------------
            adressPictureDirectory = settFiles.Pic_Folder_Adress;  
            
            //-------------------------------
            if (idxArr == null)
            {
                isInformationLoaded = false;
                return;
            }
            else
                isInformationLoaded = true;
            isPicturesByID = settFiles.isPictureTakenByObjectID;
            numberOfObjects = new List<int>();
            namesOfObjects = new List<string>();
            namesOfClasses = new List<string>();
            //countOfClasses = new List<string>();
            isPicturesByName = settFiles.isPictureTakenByObjectName;
            isPicturesByClassInterval = settFiles.isPictureTakenByClassInterval;
            countOfClasses = settFiles.Class_Start_Position;
            isPicturesByClassStartObject=settFiles.isPictureTakenByClassStartObjects;
			Title = "Список объектов. Количество:" + idxArr.Length;
            numberOfObjects.AddRange(idxArr);
            isDisSpaceMode = settFiles.DisSpaceMod;
			if (settFiles.NamesObjectSelected) 
				foreach (int i in idxArr)
				{	
                    namesOfObjects.Add(settFiles.NamesObjects[i]);
                    namesOfClasses.Add(settFiles.ClassesName[i]);
                    ListBoxObjects.Items.Add(string.Format("Объект №{0}\tИмя: {1}\tКласс: {2}", i, settFiles.NamesObjects[i],
                        settFiles.ClassesName[i]));
                    //даже если у нас есть имена объектов по текущей логике всеравно можно загрузить посредством айди
				}
			else
				foreach (int i in idxArr)
				{
                    namesOfObjects.Add(settFiles.NamesObjects[i]);
                    namesOfClasses.Add(settFiles.ClassesName[i]);
                    ListBoxObjects.Items.Add(string.Format("Объект №{0}\tКласс: {1}", settFiles.NamesObjects[i], settFiles.ClassesName[i]));
                    
				}
            ListBoxObjects.SelectedIndex = 0;
            ListBoxObjects.ScrollIntoView(ListBoxObjects.SelectedItem);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			IsClosing = true;
            EventHandler handler = CloseEvent;
            if (handler != null)
                handler(this, new EventArgs());
		}
        private List<string> Get_Pictures_Files_List(string[] All_Files)
        {
            string[] imageExtensions = {".jpg", ".jpeg", ".bmp", ".png"};
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
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, order_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }
        private string Get_Picture_Adress(List<string> Pics, string name_of_picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, name_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }
        private void ListBoxObjects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            /*
Пофиксить вывод координат, не все оси соответствуют изображенным.
             */
            if ((ListBoxObjects.SelectedIndex != -1) && ((string)ListBoxObjects.Items[ListBoxObjects.SelectedIndex] != String.Empty) && (isInformationLoaded)){
                if (!isDisSpaceMode)
                {
                    tbCurrentObjectCoords.Text = 
                        "Координаты: x=" + coords[(numberOfObjects[ListBoxObjects.SelectedIndex]), 0] +
                        " y=" + coords[(numberOfObjects[ListBoxObjects.SelectedIndex]), 1] +
                        " z=" + coords[(numberOfObjects[ListBoxObjects.SelectedIndex]), 2];
                }
                else
                {
                    if (coords.GetLength(0) == 2)
                    {
                        tbCurrentObjectCoords.Text =
                            "Координаты: x=" + coords[0, (numberOfObjects[ListBoxObjects.SelectedIndex])] +
                            " y=" + coords[1, (numberOfObjects[ListBoxObjects.SelectedIndex])] +
                            " z=0";
                    }
                    else
                    {
                        tbCurrentObjectCoords.Text =
                            "Координаты: x=" + coords[0, (numberOfObjects[ListBoxObjects.SelectedIndex])] +
                            " y=" + coords[1, (numberOfObjects[ListBoxObjects.SelectedIndex])] +
                            " z=" + coords[2, (numberOfObjects[ListBoxObjects.SelectedIndex])];
                    }
                }
                if ((isPicturesByID) || (isPicturesByName))
                { 
                    List<string> Pictures = Get_Pictures_Files_List(Directory.GetFiles(adressPictureDirectory));
                        if (isPicturesByID)
                        {
                            Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, numberOfObjects[ListBoxObjects.SelectedIndex]));
                        }
                    //нужен рефакторинг массивов и флаговой структуры, все устарело.
                        if (isPicturesByName)
                        {
                            Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, namesOfObjects[ListBoxObjects.SelectedIndex]));
                        }

                }

                if ((isPicturesByClassStartObject) || (isPicturesByClassInterval))
                {
                    String substring="";
                    int k;
                    List<string> Pictures;
                    int cur_index;
                    DirectoryInfo[] dirs = new DirectoryInfo(adressPictureDirectory).GetDirectories();
                    foreach (var item in dirs)
                        if (item.Name.Equals(namesOfClasses[ListBoxObjects.SelectedIndex]))
                        {
                            Pictures = Get_Pictures_Files_List(Directory.GetFiles(item.FullName));
                            cur_index = Int32.Parse(namesOfObjects[ListBoxObjects.SelectedIndex])-1;
                            for(k=0;k<countOfClasses.Count-1;k++)
                                if((cur_index>=Int32.Parse(countOfClasses[k]))&&(cur_index<Int32.Parse(countOfClasses[k+1]))){
                                    cur_index -= Int32.Parse(countOfClasses[k]);
                                    break;
                                }
                            if((Pictures.Count == (Int32.Parse(countOfClasses[k + 1]) - Int32.Parse(countOfClasses[k])))){
                                substring = Pictures[cur_index];                              
                                Picture.Source = Add_Picture_On_Screen(substring);                             
                            }
                            else{
                                substring = Get_Picture_Adress(Pictures, cur_index + 1);                              
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            
                            break;
                        }
                    if ((!ListBoxObjects.SelectedValue.ToString().Contains("\tФайл:")) && (ListBoxObjects.SelectedIndex != -1))
                    {
                        int current_index = ListBoxObjects.SelectedIndex;         
                        ListBoxObjects.Items[ListBoxObjects.SelectedIndex] += string.Format(" \tФайл: " + substring);
                        ListBoxObjects.SelectedIndex = current_index;
                    }
                }
            }
        }
	}
}
