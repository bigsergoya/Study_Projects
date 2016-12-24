// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using System.IO;
//using DialogResult = System.Windows.Forms.DialogResult;
namespace VisualChart3D
{
   
    //public class Pictures
    //{
    //    string[] Adresses;
        
    //    //string Dir_adr;
    //    public Pictures()
    //    {
    //        Adresses = null;
    //        // Dir_adr = string.Empty;
    //    }
    //    // public void AddAdresses(DirectoryInfo dir, string add_of_dir)
    //    public Pictures AddAdresses(DirectoryInfo dir, ref bool flag_of_empty_dir)
    //    {
    //        // Dir_adr = add_of_dir;
    //        FileInfo[] imageFiles = dir.GetFiles("*.jpg");
    //        if (imageFiles.Length != 0)
    //        {
    //            //  FileInfo[] imageFiles = dir.GetFiles("*.jpg", Dir_adr);
    //            flag_of_empty_dir = false;
    //            Adresses = new string[imageFiles.Length];
    //            int i = 0;
    //            foreach (FileInfo file in imageFiles)
    //            {
    //                //трассировать все, возможны ошибки счетчиков
    //                Adresses[i] = file.FullName;
    //                i++;
    //            }
    //            return this;
    //        }
    //        else
    //        {
    //            flag_of_empty_dir = true;
    //            return this;
    //        }

    //    }
    //    public string GetAdress(int i)
    //    {
    //        if (i < Adresses.Length)
    //            return Adresses[i];
    //        else
    //            return string.Empty;
    //    }
    //}
    
	/// <summary>
	/// Interaction logic for ConfigWindow.xaml
	/// </summary>
	public partial class ConfigWindow : Window
	{
        private int[] numberOfObjectsOfClass;
        protected string adressOfPictureLogFile = "PictureAdressLog.txt";
        /// <summary>
        /// Получить настроку исходных данных
        /// </summary>
        public System.Collections.Generic.List<string> uniqueClassesNames;
		public SettingsFiles SettFiles { get; private set; }

        /// <summary>
        /// Успешность принятия настройки
        /// </summary>
        /// 
		public bool ResultDialog = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settFiles">настрока исходных данных</param>
		public ConfigWindow(SettingsFiles settFiles)
		{
            
			InitializeComponent();
			if (settFiles == null)
				settFiles = new SettingsFiles();
			SettFiles = settFiles;
			StartForm();
			UpdateInterface();
		}
        /*
        public bool CheckClassObject()
        {
            if (!ClassObjectSelected || string.IsNullOrEmpty(ClassObjectFile) || !File.Exists(ClassObjectFile))
                return false;
            try
            {
               if((bool)rbClassObjectOneToOne.IsChecked)
                  ArrayClassesOneToOne = CommonMatrix.ReadMatrixOneToOne(ClassObjectFile);
               if(){

               }
                    case ClassObjecEnum.StartObjects:
                        ArrayClassesCountObj = CommonMatrix.ReadMatrixCountObj(ClassObjectFile);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
          */
        /// <summary>
        /// Обновить интерфейс в соответвии с настройками
        /// </summary>
        private void UpdateInterface()
		{
			switch (SettFiles.SourceMatrix)
			{
				case SourceFileMatrix.MatrixDistance:
					rbMatrixDistance.IsChecked = true;
					tbMatrixDistancePath.Text = SettFiles.SourceMatrixFile;
                    if (tbMatrixDistancePath.Text != null)
                        cbNamesPictures.IsEnabled = true;
					break;
				case SourceFileMatrix.ObjectAttribute:
					rbObjectAttribute.IsChecked = true;
                    tbObjectAttributePath.Text = SettFiles.SourceMatrixFile;
                    if (tbMatrixDistancePath.Text != null)
                        cbNamesPictures.IsEnabled = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
            if(SettFiles.DisSpaceMod)
            {
                cbDisSpaceMod.IsChecked = true;
            }
            if (SettFiles.isPictureTakenByObjectID)
            {
                cbNamesPictures.IsChecked=true;
                rbPicturesById.IsChecked = true;
                tbPictureDirectoryPath.IsEnabled = true;
                tbPictureDirectoryPath.Text = SettFiles.Pic_Folder_Adress;
            }
            if (SettFiles.isPictureTakenByObjectName)
            {
                cbNamesPictures.IsChecked = true;
                rbPicturesByObjectsName.IsChecked = true;
                tbPictureDirectoryPath.IsEnabled = true;
                tbPictureDirectoryPath.Text = SettFiles.Pic_Folder_Adress;
            }
            if (SettFiles.isPictureTakenByClassInterval||SettFiles.isPictureTakenByClassStartObjects)
            {
                cbNamesPictures.IsChecked = true;
                rbPicturesByClassName.IsChecked = true;
                tbPictureDirectoryPath.IsEnabled = true;
                tbPictureDirectoryPath.Text = SettFiles.Pic_Folder_Adress;
            }

             
			if (SettFiles.ClassObjectSelected)
			{
				cbClassObject.IsChecked = true;
                tbClassObjectPath.Text = SettFiles.ClassObjectFile;
				switch (SettFiles.ClassObjectType)
				{
					case ClassObjecEnum.OneToOne:
						rbClassObjectOneToOne.IsChecked = true;
						break;
					case ClassObjecEnum.CountObj:
						rbClassObjectCountObj.IsChecked = true;
						break;
					case ClassObjecEnum.StartObjects:
						rbClassObjectStartObjects.IsChecked = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				if (SettFiles.ClassEqualSelected && !string.IsNullOrEmpty(SettFiles.ClassEqualCountStr))
				{
					cbClassEqual.IsChecked = true;
					tbcbClassEqual.Text = SettFiles.ClassEqualCountStr;
				}
				else
					cbClassEqual.IsChecked = false;
			}
			if (SettFiles.NamesObjectSelected)
			{
				cbNamesObject.IsChecked = true;

				tbNamesObjectPath.Text = SettFiles.NamesObjectFile;
			}
	        switch (SettFiles.Metrics)
	        {
		        case MetricsEnum.Euclidean:
			        rbMetricEuclidean.IsChecked = true;
			        break;
		        case MetricsEnum.NonEuclidean:
			        rbMetricNonEuclidean.IsChecked = true;
			        break;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
		}

        /// <summary>
        /// Обновление интерфейса при первом запуске
        /// </summary>
		private void StartForm()
		{
			switch (SettFiles.SourceMatrix)
			{
				case SourceFileMatrix.MatrixDistance:
					rbMatrixDistance_Checked(rbMatrixDistance, new RoutedEventArgs());
					rbObjectAttribute_Unchecked(rbObjectAttribute, new RoutedEventArgs());
					break;
				case SourceFileMatrix.ObjectAttribute:
					rbObjectAttribute_Checked(rbObjectAttribute, new RoutedEventArgs());
					rbMatrixDistance_Unchecked(rbMatrixDistance, new RoutedEventArgs());
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (SettFiles.ClassObjectSelected)
			{
				switch (SettFiles.ClassObjectType)
				{
					case ClassObjecEnum.OneToOne:
						rbClassObjectOneToOne.IsChecked = true;
						break;
					case ClassObjecEnum.CountObj:
						rbClassObjectCountObj.IsChecked = true;
						break;
					case ClassObjecEnum.StartObjects:
						rbClassObjectStartObjects.IsChecked = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				rbClassObject_Checked(rbClassObjectOneToOne, new RoutedEventArgs());
			}
			else
			{
				cbClassObject_Unchecked(cbClassObject, new RoutedEventArgs());
				if (SettFiles.ClassEqualSelected && !string.IsNullOrEmpty(SettFiles.ClassEqualCountStr))
				{
					cbClassEqual.IsChecked = true;
					tbcbClassEqual.Text = SettFiles.ClassEqualCountStr;
				}
				else
					cbClassEqual_Unchecked(cbClassEqual, new RoutedEventArgs());
			}
			if (SettFiles.NamesObjectSelected)
				cbNamesObject_Checked(cbClassObject, new RoutedEventArgs());
			else
				cbNamesObject_Unchecked(cbClassObject, new RoutedEventArgs());
			switch (SettFiles.Metrics)
			{
				case MetricsEnum.Euclidean:
					rbMetricEuclidean.IsChecked = true;
					break;
				case MetricsEnum.NonEuclidean:
					rbMetricNonEuclidean.IsChecked = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

		}

        /// <summary>
        /// Считывание значений из формы в класс настроек
        /// </summary>
        /// <returns></returns>
        private SettingsFiles ReadValues()
        {
            SettingsFiles result = new SettingsFiles(); // тут происходит запись параметров с формы
            if (rbMatrixDistance.IsChecked.Value)
            {
                result.DisSpaceMod = cbDisSpaceMod.IsChecked.Value;
                result.SourceMatrix = SourceFileMatrix.MatrixDistance;
                result.SourceMatrixFile = tbMatrixDistancePath.Text != null
                    ? tbMatrixDistancePath.Text.ToString()
                    : string.Empty;
            }
            if (tbPictureDirectoryPath.Text != null)
            {
                if (((rbPicturesById.IsChecked == true) || (rbPicturesByObjectsName.IsChecked == true) || (rbPicturesByClassName.IsChecked == true)) && (!tbPictureDirectoryPath.Text.Equals("")))
                {
                    result.Pic_Folder_Adress = tbPictureDirectoryPath.Text.ToString();
                    result.isPictureTakenByObjectID = (bool)rbPicturesById.IsChecked;
                    result.isPictureTakenByObjectName = (bool)rbPicturesByObjectsName.IsChecked;
                    if (rbPicturesByClassName.IsChecked == true)
                    {
                        result.isPictureTakenByClassInterval = (bool)rbClassObjectCountObj.IsChecked;
                        result.isPictureTakenByClassStartObjects = (bool)rbClassObjectStartObjects.IsChecked;
                    }
                }
            }
            if (rbObjectAttribute.IsChecked.Value)
            {
                result.SourceMatrix = SourceFileMatrix.ObjectAttribute;
                result.SourceMatrixFile = tbObjectAttributePath.Text != null
                    ? tbObjectAttributePath.Text.ToString()
                    : string.Empty;
            }
            if (cbClassObject.IsChecked.Value)
            {
                result.ClassObjectSelected = true;
                result.ClassObjectFile = tbClassObjectPath.Text != null
                    ? tbClassObjectPath.Text.ToString()
                    : string.Empty;
                if (rbClassObjectOneToOne.IsChecked.Value)
                    result.ClassObjectType = ClassObjecEnum.OneToOne;
                if (rbClassObjectCountObj.IsChecked.Value)
                    result.ClassObjectType = ClassObjecEnum.CountObj;
                if (rbClassObjectStartObjects.IsChecked.Value)
                    result.ClassObjectType = ClassObjecEnum.StartObjects;
            }
            else
            {
                result.ClassObjectSelected = false;
                if (cbClassEqual.IsChecked.Value)
                {
                    result.ClassEqualSelected = true;
                    result.ClassEqualCountStr = tbcbClassEqual.Text;
                }
            }
            if (cbNamesObject.IsChecked.Value)
            {
                result.NamesObjectSelected = true;
                result.NamesObjectFile = tbNamesObjectPath.Text != null
                    ? tbNamesObjectPath.Text.ToString()
                    : string.Empty;
            }
            else
                result.NamesObjectSelected = false;

			if (rbMetricEuclidean.IsChecked.Value)
				result.Metrics = MetricsEnum.Euclidean;
			else
				if (rbMetricNonEuclidean.IsChecked.Value)
					result.Metrics = MetricsEnum.NonEuclidean;


            return result;
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="lb">отображение расположения</param>
        private void OpenFile(System.Windows.Controls.TextBox lb,Boolean switchModeToClassInputChecking)
        {
            OpenFileDialog ofDlg = new OpenFileDialog
            {
                Multiselect = false
            };
            if (lb.Text != null && !String.IsNullOrEmpty(lb.Text.ToString()))
                ofDlg.InitialDirectory = lb.Text.ToString();
            ofDlg.RestoreDirectory = true;
            if (ofDlg.ShowDialog().Value)
            {
                lb.Text = ofDlg.FileName;
                if (switchModeToClassInputChecking)
                {
                    SettingsFiles temp = ReadValues();
                    string errors = temp.Validation();
                    if ((!errors.Equals("")) || (temp.UniqClassesName==null))
                    {
                        MessageBox.Show("Ошибка при выборке данных о классе! "+errors);
                        lb.Text = "";
                        return;
                    }
                    //1. Добавлены всплывающие подсказки для повышения корректности выбора типа файла описания классов.

//2. Добавлен вывод о числе объектов каждого класса.

//2. Устранены дополнительные баги.
                    lbUniqueClasses.Items.Clear();
                    
                    uniqueClassesNames = new System.Collections.Generic.List<string>();
                    uniqueClassesNames.AddRange(temp.UniqClassesName.ToArray());
                    getNumberOfObjectsOfClass(temp);
                    lbUniqueClasses.IsEnabled = true;
                    numberOfObjectsOfClass = getNumberOfObjectsOfClass(temp);
                    
                    //if(temp.ClassObjectType.Equals(
                    int k=0;
                    foreach (string className in uniqueClassesNames)
                    {
                        lbUniqueClasses.Items.Add("Имя класса - " + className + " ; Количество: " + numberOfObjectsOfClass[k]);
                        k++;
                    }

                    
                }
            }
        }
        private int[] getNumberOfObjectsOfClass(SettingsFiles temp)
        {
            int countOfClasses = temp.UniqClassesName.Count; //переделать лейблы в эдиты
            switch (temp.ClassObjectType)
            {
                case ClassObjecEnum.OneToOne:
                    numberOfObjectsOfClass = temp.GetClassPositionsOnOneToOneMode(uniqueClassesNames); // получаем длинну каждого класса 
                    break;
                case ClassObjecEnum.CountObj:
                    numberOfObjectsOfClass = new int[countOfClasses];
                    //uniqueClassesNames
                    for (int i = 0; i < countOfClasses; i++)
                    {
                        numberOfObjectsOfClass[i] = Int32.Parse(temp.ArrayClassesCountObj[i, 0]);
                    }
                    break;
                case ClassObjecEnum.StartObjects:
                    numberOfObjectsOfClass = new int[countOfClasses];
                    //uniqueClassesNames
                    for (int i = 1; i < countOfClasses + 1; i++)
                    {
                        numberOfObjectsOfClass[i - 1] = Int32.Parse(temp.Class_Start_Position[i]) - Int32.Parse(temp.Class_Start_Position[i - 1]);
                    }
                    break;
                default:
                    MessageBox.Show("Ошибка при выводе списка классов.");
                    throw new ArgumentOutOfRangeException();
            }
            return numberOfObjectsOfClass;

        }
		private void rbMatrixDistance_Checked(object sender, RoutedEventArgs e)
		{
			tbMatrixDistancePath.IsEnabled = true;
			btMatrixDistanceBrowse.IsEnabled = true;
		}

		private void rbMatrixDistance_Unchecked(object sender, RoutedEventArgs e)
		{
            tbMatrixDistancePath.Text = "";
			tbMatrixDistancePath.IsEnabled = false;
			btMatrixDistanceBrowse.IsEnabled = false;
		}

		private void rbObjectAttribute_Checked(object sender, RoutedEventArgs e)
		{
			tbObjectAttributePath.IsEnabled = true;
			btObjectAttributeBrowse.IsEnabled = true;
		}

		private void rbObjectAttribute_Unchecked(object sender, RoutedEventArgs e)
		{
            tbObjectAttributePath.Text = "";
			tbObjectAttributePath.IsEnabled = false;
			btObjectAttributeBrowse.IsEnabled = false;
		}

		private void cbClassObject_Checked(object sender, RoutedEventArgs e)
		{
			tbClassObjectPath.IsEnabled = true;
			btClassObjectBrowse.IsEnabled = true;
			rbClassObjectOneToOne.IsEnabled = true;
			rbClassObjectCountObj.IsEnabled = true;
			rbClassObjectStartObjects.IsEnabled = true;

			cbClassEqual.IsChecked = false;
			cbClassEqual.IsEnabled = false;

			switch (SettFiles.ClassObjectType)
			{
				case ClassObjecEnum.OneToOne:
					rbClassObjectOneToOne.IsChecked = true;
					break;
				case ClassObjecEnum.CountObj:
					rbClassObjectCountObj.IsChecked = true;
					break;
				case ClassObjecEnum.StartObjects:
					rbClassObjectStartObjects.IsChecked = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void cbClassObject_Unchecked(object sender, RoutedEventArgs e)
		{
            tbClassObjectPath.Text = "";
			tbClassObjectPath.IsEnabled = false;
			btClassObjectBrowse.IsEnabled = false;
			rbClassObjectOneToOne.IsEnabled = false;
			rbClassObjectCountObj.IsEnabled = false;
			rbClassObjectStartObjects.IsEnabled = false;
			cbClassEqual.IsEnabled = true;
		}

		private void cbNamesObject_Checked(object sender, RoutedEventArgs e)
		{
			tbNamesObjectPath.IsEnabled = true;
			btNamesObjectBrowse.IsEnabled = true;
            if (cbNamesPictures.IsChecked == true)
                rbPicturesByObjectsName.IsEnabled = true;
		}

		private void cbNamesObject_Unchecked(object sender, RoutedEventArgs e)
		{
			tbNamesObjectPath.Text = "";
			tbNamesObjectPath.IsEnabled = false;
			btNamesObjectBrowse.IsEnabled = false;
            rbPicturesByClassName.IsEnabled = false;
            if (cbNamesPictures.IsChecked == true)
                rbPicturesByObjectsName.IsEnabled = false;
		}

		private void rbClassObject_Checked(object sender, RoutedEventArgs e)
		{
            if ((cbNamesPictures.IsChecked == true) && ((rbClassObjectCountObj.IsChecked == true) || (rbClassObjectStartObjects.IsChecked == true)))
            {
                rbPicturesByClassName.IsEnabled = true;
                rbPicturesByClassName.IsChecked = true;
            }
			if (rbClassObjectOneToOne.IsChecked.Value)
				SettFiles.ClassObjectType = ClassObjecEnum.OneToOne;
			else
				if (rbClassObjectCountObj.IsChecked.Value)
					SettFiles.ClassObjectType = ClassObjecEnum.CountObj;
				else
					if (rbClassObjectStartObjects.IsChecked.Value)
						SettFiles.ClassObjectType = ClassObjecEnum.StartObjects;
			
		}

		private void btMatrixDistanceBrowse_Click(object sender, RoutedEventArgs e)
		{
            OpenFile(tbMatrixDistancePath, false);
            if (tbMatrixDistancePath.Text != "")
            {
                cbNamesPictures.IsEnabled = true;
                cbClassObject.IsChecked = false;
                rbClassObjectOneToOne.IsChecked = false;
                rbClassObjectCountObj.IsChecked = false;
                rbClassObjectStartObjects.IsChecked = false;
                tbClassObjectPath.Text = "";
                lbUniqueClasses.Items.Clear();
                string[] logData = GetDataFromPictureLogFile(tbMatrixDistancePath.Text.ToString());
                if (logData != null)
                {
                    cbNamesPictures.IsEnabled = true;
                    cbNamesPictures.IsChecked = true;
                    tbPictureDirectoryPath.Text = logData[0];
                    switch (logData[1])
                    {
                        case "PicturesById":
                            rbPicturesById.IsChecked = true;
                            break;
                        case "PicturesByObjectsName":
                            rbPicturesByObjectsName.IsChecked = true;
                            break;
                        case "PicturesByClassName":
                            //rbPicturesByClassName.IsEnabled = true;
                            rbPicturesByClassName.IsChecked = true;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    cbNamesPictures.IsChecked = false;
                    tbPictureDirectoryPath.Text = "";
                    rbPicturesById.IsChecked = false;
                    rbPicturesByObjectsName.IsChecked = false;
                    rbPicturesByClassName.IsChecked = false;
                }
            }
		}

		private void btObjectAttributeBrowse_Click(object sender, RoutedEventArgs e)
		{
			OpenFile(tbObjectAttributePath,false);
		}

		private void btClassObjectBrowse_Click(object sender, RoutedEventArgs e)
		{
			OpenFile(tbClassObjectPath,true);
		}

		private void btNamesObjectBrowse_Click(object sender, RoutedEventArgs e)
		{
            OpenFile(tbNamesObjectPath, false);
		}

		private void btCancel_Click(object sender, RoutedEventArgs e)
		{
			ResultDialog = false;
			this.Close();
		}

		private void btSave_Click(object sender, RoutedEventArgs e)
		{
		    SettingsFiles temp = ReadValues(); 
			string errors = temp.Validation();
            if ((bool)cbClassObject.IsChecked)
            {
                numberOfObjectsOfClass = getNumberOfObjectsOfClass(temp);
                temp.numberOfObjectsOfClass = new int[numberOfObjectsOfClass.Length]; 
                Array.Copy(numberOfObjectsOfClass, temp.numberOfObjectsOfClass, numberOfObjectsOfClass.Length);
            }
            if (temp.UniqClassesName == null)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при выборке данных о классе! " + errors);
                return;
            }
			if (!string.IsNullOrEmpty(errors))
				System.Windows.Forms.MessageBox.Show(errors);
			else
			{
				SettFiles = temp;
				ResultDialog = true;
				DialogResult = true;
				Close();
			}
		}

		private void cbClassEqual_Checked(object sender, RoutedEventArgs e)
		{
			tbcbClassEqual.IsEnabled = true;
		}

		private void cbClassEqual_Unchecked(object sender, RoutedEventArgs e)
		{
			tbcbClassEqual.IsEnabled = false;
			tbcbClassEqual.Text = string.Empty;
		}

        private void cbDirPic_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;
        }

        private void cbDirPic_Unchecked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = false;
        }

        private void btNameFolderPicBrowse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if((rbPicturesByClassName.IsEnabled==true)&&(rbPicturesByClassName.IsChecked==true))
                FBD.Description="Выберете папку с папками изображениями объектов соответствующих классов";
            else
                FBD.Description="Выберете папку с изображениями объектов";
           // if (lbMatrixDistancePath.Content != null && !String.IsNullOrEmpty(lbMatrixDistancePath.Content.ToString()))
           //     FBD. = lbMatrixDistancePath.Content.ToString();
            //if ((!lbObjectAttributePath.Content.Equals("")) || (!lbMatrixDistancePath.Content.Equals("")))
                //FBD.RootFolder = Environment.;
            FBD.ShowDialog();
            if (FBD.SelectedPath != String.Empty)
            {
                if((bool)rbPicturesByClassName.IsChecked){
                    checkClassSubfoldersInPictureFolder(FBD.SelectedPath, uniqueClassesNames);
                }

                //на событие загрузки матрицы расстояний или попарных сравнений запилить обновление пути к картинкам.
                tbPictureDirectoryPath.Text = FBD.SelectedPath;
                if (rbPicturesById.IsChecked == true)
                {
                    WriteDataToPictureLog(tbMatrixDistancePath.Text.ToString(), FBD.SelectedPath, "PicturesById");
                    return;
                }
                if (rbPicturesByObjectsName.IsChecked == true)
                {
                    WriteDataToPictureLog(tbMatrixDistancePath.Text.ToString(), FBD.SelectedPath, "PicturesByObjectsName");
                    return;
                }
                if (rbPicturesByClassName.IsChecked == true)
                {
                    WriteDataToPictureLog(tbMatrixDistancePath.Text.ToString(), FBD.SelectedPath, "PicturesByClassName");
                    return;
                }


            }  /*
            using (FolderBrowserDialog fb = new FolderBrowserDialog())
            {
                fb.ShowNewFolderButton = false;
                string matrixFolderPath = lbMatrixDistancePath.Content.ToString();
                int end = matrixFolderPath.IndexOf('.');
                int start = end - 1;
                // char.IsDigit -  в рефракторинг
                while (matrixFolderPath[start] != '\\')
                {
                    start--;
                }
                start++;
                
                fb.SelectedPath = matrixFolderPath.Remove(start);
                if ((rbPicturesByClassName.IsEnabled == true) && (rbPicturesByClassName.IsChecked == true))
                    fb.Description = "Выберете папку с папками изображениями объектов соответствующих классов";
                else
                    fb.Description = "Выберете папку с изображениями объектов";
                fb.ShowDialog();
                if (fb.SelectedPath != String.Empty)
                {
                    if ((bool)rbPicturesByClassName.IsChecked)
                    {
                        checkClassSubfoldersInPictureFolder(fb.SelectedPath, uniqueClassesNames);
                    }

                    pictureDirectoryPath.Content = fb.SelectedPath;
                }
            }
                */

        }
        private void checkClassSubfoldersInPictureFolder(String pictureFolderPath, System.Collections.Generic.List<string> uniqueClassesName)
        {
            string notFoundedClassDirs="";
            bool isDirectoryFound;
            DirectoryInfo[] dirs = new DirectoryInfo(pictureFolderPath).GetDirectories();
            foreach (var className in uniqueClassesName)
            {
                isDirectoryFound = false;
                for (int i = 0; i < dirs.Length; i++)
                {
                    if (string.Compare(dirs[i].Name, className, true) == 0)
                    {
                        isDirectoryFound = true;
                        break;
                    }
                }
                if (!isDirectoryFound)
                    notFoundedClassDirs += className + ", ";   
            }
            if (!notFoundedClassDirs.Equals(""))
            {

                MessageBox.Show("Внимание, не найдены директории для классов: " + notFoundedClassDirs.Remove(notFoundedClassDirs.Length - 2) + ". \n Для данных классов не будут выводиться изображения");
            }
            //uniqueClassesNames
            //foreach (var item in dirs)
            //if (item.Name.Equals(namesOfClasses[ListBoxObjects.SelectedIndex]))
        }
        private void cbNamesPictures_Unchecked(object sender, RoutedEventArgs e)
        {
            rbPicturesByClassName.IsEnabled = false;
            btNameFolderPicBrowse.IsEnabled = false;
            rbPicturesById.IsEnabled = false;            
            rbPicturesById.IsChecked = false;
            rbPicturesByObjectsName.IsEnabled = false;
            rbPicturesByObjectsName.IsChecked = false;
        }

        private void cbNamesPictures_Checked(object sender, RoutedEventArgs e)
        {
            if(cbNamesObject.IsChecked==true)
                rbPicturesByObjectsName.IsEnabled = true;
            rbPicturesById.IsEnabled = true;
            if ((rbClassObjectStartObjects.IsChecked == true) || (rbClassObjectCountObj.IsChecked == true))
                rbPicturesByClassName.IsEnabled = true;
            //btNameFolderPicBrowse.IsEnabled = true;
        }

        private void rbPicturesById_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;
            if ((rbPicturesByObjectsName.IsChecked == true) && (rbPicturesByObjectsName.IsEnabled==true))
                rbPicturesByObjectsName.IsChecked = false;
            if ((rbPicturesByClassName.IsChecked == true) && (rbPicturesByClassName.IsEnabled == true))
                rbPicturesByClassName.IsChecked = false;
            //Пометить отсюда в будущий класс с инфой вариант выбора.
        }

        private void rbPicturesByObjectsName_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;
            if ((rbPicturesById.IsChecked == true) && (rbPicturesById.IsEnabled==true))
                rbPicturesById.IsChecked = false;
            if ((rbPicturesByClassName.IsChecked == true) && (rbPicturesByClassName.IsEnabled == true))
                rbPicturesByClassName.IsChecked = false;
            //Пометить отсюда в будущий класс с инфой вариант выбора.
        }

        private void rbPicturesById_Unchecked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = false;
        }

        private void rbPicturesByObjectsName_Unchecked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = false;
        }

        private void rbPicturesByClassName_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;
        }

        private void rbPicturesByClassName_Unchecked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = false;
        }

        private void rbClassObjectCountObj_Unchecked(object sender, RoutedEventArgs e)
        {
            rbPicturesByClassName.IsEnabled = false;
        }

        private void rbClassObjectStartObjects_Unchecked(object sender, RoutedEventArgs e)
        {
            rbPicturesByClassName.IsEnabled = false;
        }

        private void cbDisSpaceMod_Checked(object sender, RoutedEventArgs e)
        {
            rbObjectAttribute.IsEnabled = false;
            rbMetricEuclidean.IsEnabled = false;
            rbMetricNonEuclidean.IsEnabled = false;
        }

        private void cbDisSpaceMod_Unchecked(object sender, RoutedEventArgs e)
        {
            rbObjectAttribute.IsEnabled = true;
            rbMetricEuclidean.IsEnabled = true;
            rbMetricNonEuclidean.IsEnabled = true;
            
        }

        private void btClassObjectBrowse_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        private string[] GetDataFromPictureLogFile(string pathToAnyMatrix)
        {
            string[] data;
            string pathToLogFile = pathToAnyMatrix
                    .Remove(pathToAnyMatrix.LastIndexOf('\\') + 1) + adressOfPictureLogFile;
            try
            {
                if (File.Exists(pathToLogFile))
                {
                    data = File.ReadAllLines(pathToLogFile);
                    return data;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось чтение адреса директории с изображениями из лог-файла");
            }
            return null;
        }
        private void WriteDataToPictureLog(string pathToAnyMatrix, string pathToPictureFolder, string pictureLoadingType)
        {
            string pathToPictureContentAdressLog = pathToAnyMatrix.Remove(pathToAnyMatrix.LastIndexOf('\\') + 1) 
                + adressOfPictureLogFile;
            try
            {
                using (WriteTextToFile file = new WriteTextToFile(pathToPictureContentAdressLog))
                {
                    file.WriteLine(pathToPictureFolder);
                    file.WriteLine(pictureLoadingType);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось создать лог-файл для сохранения выбранного путя к директории с изображениями.");
            }

        }
    }
}
