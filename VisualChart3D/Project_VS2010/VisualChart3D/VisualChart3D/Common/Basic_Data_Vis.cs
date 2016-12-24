// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
namespace VisualChart3D.Common
{
    class Basic_Data_Vis
    {
        bool Is_Picture;
        //bool isPictureTakenByObjectName;
        //bool isPictureTakenByClassStartObjects;
        //bool isPictureTakenByClassInterval;
        List<string> Pictures;
        int cur_index;
        public Basic_Data_Vis()
        {
            //Is_Picture=false;
            //isPictureTakenByObjectName = false;
            //isPictureTakenByClassStartObjects = false;
            //isPictureTakenByClassInterval = false;
            Pictures=null;
            cur_index = -1; ;
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
                { 
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
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();


            bi3.UriSource = new Uri(adress, UriKind.RelativeOrAbsolute);  //urisource тупит и картинка не показывается дефолтная
            bi3.EndInit();
            return bi3;

        }
        private string Get_Picture_Adress(List<string> Pics, string name_of_picture, Image Picture)
        {
            string result = Pics.Find(pic => (Is_Picture_Exist(pic, name_of_picture))) ?? "Resources / empty_picture.jpg";
            Picture.Source = Add_Picture_On_Screen(result);
            return result;
        }
        private void Paining(Image Picture, int SelectedIndex, SettingsFiles SettFile)
        {
            Is_Picture = SettFile.isPictureTakenByClassInterval || SettFile.isPictureTakenByClassStartObjects || SettFile.isPictureTakenByObjectID || SettFile.isPictureTakenByObjectName;
            if ((SelectedIndex != -1) && (Is_Picture))
            {
                if ((SettFile.isPictureTakenByObjectID) || (SettFile.isPictureTakenByObjectName))
                {

                    Pictures = Get_Pictures_Files_List(Directory.GetFiles(SettFile.Pic_Folder_Adress));
                    if (SettFile.isPictureTakenByObjectName)
                    {
                        Picture.Source = Add_Picture_On_Screen(Get_Picture_Adress(Pictures, SettFile.NamesObjects[SelectedIndex], Picture));
                    }

                }

                if ((SettFile.isPictureTakenByClassStartObjects) || (SettFile.isPictureTakenByClassInterval))
                {
                    //List<String> SubFolders = new List<String>();
                    String substring;
                    int k;
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
                                substring = Pictures[cur_index - 1];
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            else
                            {
                                substring = Get_Picture_Adress(Pictures, cur_index.ToString(), Picture);
                                Picture.Source = Add_Picture_On_Screen(substring);
                            }
                            break;
                        }
                }
            }

        }
    }
}
