// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using System.IO;
using VisualChart3D.Common;

namespace VisualChart3D.ConfigWindow
{

    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class StartSettings : Window
    {
        private const string AdressOfPictureLogFile = "PictureAdressLog.txt";
        private const string GettingClassDataErrorMessage = "Ошибка при выборке данных о классе! {0}";
        private const string DisplayingClassListErrorMessage = "Ошибка при выводе списка классов.";
        private const string ClassFilesChooseErrorMessage = "Ошибка! Файл с классами объектов не выбран";
        private const string ClassDirNotFoundWarningMessage = "Внимание, не найдены директории для классов: {0}. \n Для данных классов выведение изображений объектов недоступно.";
        private const string PictureDirAdressReadingWarningMessage = "Не удалось чтение адреса директории с изображениями из лог-файла";
        private const string PictureDirLogFileWarningMessage = "Не удалось создать лог-файл для сохранения выбранного путя к директории с изображениями.";
        const string NotImplementedMessage = "Some algorithm has not been implemented";

        private int[] numberOfObjectsOfClass;

        /// <summary>
        /// Получить настроку исходных данных
        /// </summary>
        public System.Collections.Generic.List<string> uniqueClassesNames;
        public Engine SettFiles { get; private set; }

        /// <summary>
        /// Успешность принятия настройки
        /// </summary>
        /// 
		public bool ResultDialog = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settFiles">настрока исходных данных</param>
		public StartSettings(Engine settFiles)
        {

            InitializeComponent();

            if (settFiles == null)
            {
                settFiles = new Engine();
            }

            SettFiles = settFiles;
            StartForm();
            UpdateInterface();
        }

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
                    {
                        cbNamesPictures.IsEnabled = true;
                    }

                    break;

                case SourceFileMatrix.ObjectAttribute:
                    rbObjectAttribute.IsChecked = true;
                    tbObjectAttributePath.Text = SettFiles.SourceMatrixFile;

                    if (tbMatrixDistancePath.Text != null)
                    {
                        cbNamesPictures.IsEnabled = true;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (SettFiles.AlgorithmType)
            {
                case AlgorithmType.FastMap:
                    cbFastMapAlg.IsChecked = true;
                    break;

                case AlgorithmType.DisSpace:
                    cbDisSpaceMod.IsChecked = true;
                    break;

                case AlgorithmType.SammonsMap:
                    cbSammonsAlg.IsChecked = true;
                    break;

                case AlgorithmType.KohonenMap:
                    cbKohonenAlg.IsChecked = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(NotImplementedMessage);
            }

            if (SettFiles.isPictureTakenByObjectID)
            {
                cbNamesPictures.IsChecked = true;
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

            if (SettFiles.isPictureTakenByClassInterval || SettFiles.isPictureTakenByClassStartObjects)
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
                    case ClassInfoType.OneToOne:
                        rbClassObjectOneToOne.IsChecked = true;
                        break;
                    case ClassInfoType.CountObj:
                        rbClassObjectCountObj.IsChecked = true;
                        break;
                    case ClassInfoType.StartObjects:
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
                {
                    cbClassEqual.IsChecked = false;
                }
            }

            if (SettFiles.NamesObjectSelected)
            {
                cbNamesObject.IsChecked = true;
                tbNamesObjectPath.Text = SettFiles.NamesObjectFile;
            }

            switch (SettFiles.Metrics)
            {
                case FastMapMetric.Euclidean:
                    rbMetricEuclidean.IsChecked = true;
                    break;

                case FastMapMetric.NonEuclidean:
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
                    case ClassInfoType.OneToOne:
                        rbClassObjectOneToOne.IsChecked = true;
                        break;

                    case ClassInfoType.CountObj:
                        rbClassObjectCountObj.IsChecked = true;
                        break;

                    case ClassInfoType.StartObjects:
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
                {
                    cbClassEqual_Unchecked(cbClassEqual, new RoutedEventArgs());
                }
            }

            if (SettFiles.NamesObjectSelected)
            {
                cbNamesObject_Checked(cbClassObject, new RoutedEventArgs());
            }
            else
            {
                cbNamesObject_Unchecked(cbClassObject, new RoutedEventArgs());
            }

            switch (SettFiles.AlgorithmType)
            {
                case AlgorithmType.FastMap:
                    cbFastMapAlg.IsChecked = true;
                    break;

                case AlgorithmType.DisSpace:
                    cbFastMapAlg.IsChecked = true;
                    break;

                case AlgorithmType.KohonenMap:
                    cbFastMapAlg.IsChecked = true;
                    break;
            }

            tbMinkovskiDegree.Value = SettFiles.MinkovskiDegree;

            switch (SettFiles.Metrics)
            {
                case FastMapMetric.Euclidean:
                    rbMetricEuclidean.IsChecked = true;
                    break;

                case FastMapMetric.NonEuclidean:
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
        private Engine ReadValues()
        {
            Engine result = new Engine(); // тут происходит запись параметров с формы

            if (cbDisSpaceMod.IsChecked.Value)
            {
                result.AlgorithmType = AlgorithmType.DisSpace;
            }

            else if (cbFastMapAlg.IsChecked.Value)
            {
                result.AlgorithmType = AlgorithmType.FastMap;
            }

            else if (cbSammonsAlg.IsChecked.Value)
            {
                result.AlgorithmType = AlgorithmType.SammonsMap;
            }

            else if (cbKohonenAlg.IsChecked.Value)
            {
                result.AlgorithmType = AlgorithmType.KohonenMap;
            }

            result.MinkovskiDegree = (int)tbMinkovskiDegree.Value;

            if (rbMatrixDistance.IsChecked.Value)
            {
                result.SourceMatrix = SourceFileMatrix.MatrixDistance;
                result.SourceMatrixFile = tbMatrixDistancePath.Text != null
                    ? tbMatrixDistancePath.Text.ToString()
                    : string.Empty;
            }

            if ((tbPictureDirectoryPath.Text != null) && (cbNamesPictures.IsChecked == true))
            {
                if (((rbPicturesById.IsChecked == true)
                    || (rbPicturesByObjectsName.IsChecked == true)
                    || (rbPicturesByClassName.IsChecked == true))
                        && (!String.IsNullOrEmpty(tbPictureDirectoryPath.Text)))
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
                {
                    result.ClassObjectType = ClassInfoType.OneToOne;
                }

                if (rbClassObjectCountObj.IsChecked.Value)
                {
                    result.ClassObjectType = ClassInfoType.CountObj;
                }

                if (rbClassObjectStartObjects.IsChecked.Value)
                {
                    result.ClassObjectType = ClassInfoType.StartObjects;
                }
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
            {
                result.NamesObjectSelected = false;
            }

            if (rbMetricEuclidean.IsChecked.Value)
            {
                result.Metrics = FastMapMetric.Euclidean;
            }
            else
            {
                if (rbMetricNonEuclidean.IsChecked.Value)
                {
                    result.Metrics = FastMapMetric.NonEuclidean;
                }
            }

            return result;
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="lb">отображение расположения</param>
        private void OpenFile(System.Windows.Controls.TextBox lb, Boolean switchModeToClassInputChecking)
        {
            OpenFileDialog ofDlg = new OpenFileDialog
            {
                Multiselect = false
            };

            if (lb.Text != null && !String.IsNullOrEmpty(lb.Text.ToString()))
            {
                ofDlg.InitialDirectory = lb.Text.ToString();
            }

            ofDlg.RestoreDirectory = true;
            if (ofDlg.ShowDialog().Value)
            {
                lb.Text = ofDlg.FileName;

                if (switchModeToClassInputChecking)
                {
                    Engine temp = ReadValues();

                    string errors = temp.Validation();

                    if ((!String.IsNullOrEmpty(errors)) || (temp.UniqClassesName == null))
                    {
                        Utils.ShowErrorMessage(String.Format(GettingClassDataErrorMessage, errors));
                        lb.Text = String.Empty;
                        return;
                    }

                    lbUniqueClasses.Items.Clear();

                    uniqueClassesNames = new System.Collections.Generic.List<string>();
                    uniqueClassesNames.AddRange(temp.UniqClassesName.ToArray());
                    getNumberOfObjectsOfClass(temp);
                    lbUniqueClasses.IsEnabled = true;
                    numberOfObjectsOfClass = getNumberOfObjectsOfClass(temp);

                    int k = 0;
                    foreach (string className in uniqueClassesNames)
                    {
                        lbUniqueClasses.Items.Add("Имя класса - " + className + " ; Количество: " + numberOfObjectsOfClass[k]);
                        k++;
                    }
                }
            }
        }
        private int[] getNumberOfObjectsOfClass(Engine temp)
        {
            int countOfClasses = temp.UniqClassesName.Count; //переделать лейблы в эдиты
            switch (temp.ClassObjectType)
            {
                case ClassInfoType.OneToOne:
                    numberOfObjectsOfClass = temp.GetClassPositionsOnOneToOneMode(uniqueClassesNames); // получаем длинну каждого класса 
                    break;
                case ClassInfoType.CountObj:
                    numberOfObjectsOfClass = new int[countOfClasses];

                    //uniqueClassesNames
                    for (int i = 0; i < countOfClasses; i++)
                    {
                        numberOfObjectsOfClass[i] = Int32.Parse(temp.ArrayClassesCountObj[i, 0]);
                    }

                    break;
                case ClassInfoType.StartObjects:
                    numberOfObjectsOfClass = new int[countOfClasses];

                    //uniqueClassesNames
                    for (int i = 1; i < countOfClasses + 1; i++)
                    {
                        numberOfObjectsOfClass[i - 1] = Int32.Parse(temp.Class_Start_Position[i]) - Int32.Parse(temp.Class_Start_Position[i - 1]);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(DisplayingClassListErrorMessage);
            }

            return numberOfObjectsOfClass;
        }
        private void rbMatrixDistance_Checked(object sender, RoutedEventArgs e)
        {
            tbMatrixDistancePath.IsEnabled = true;
            btMatrixDistanceBrowse.IsEnabled = true;
            tbMinkovskiDegree.IsEnabled = false;
        }

        private void rbMatrixDistance_Unchecked(object sender, RoutedEventArgs e)
        {
            tbMatrixDistancePath.Text = String.Empty;
            tbMatrixDistancePath.IsEnabled = false;
            btMatrixDistanceBrowse.IsEnabled = false;
            tbMinkovskiDegree.IsEnabled = true;
        }

        private void rbObjectAttribute_Checked(object sender, RoutedEventArgs e)
        {
            tbObjectAttributePath.IsEnabled = true;
            btObjectAttributeBrowse.IsEnabled = true;

            cbDisSpaceMod.IsEnabled = false;
            cbDisSpaceMod.IsChecked = false;
        }

        private void rbObjectAttribute_Unchecked(object sender, RoutedEventArgs e)
        {
            tbObjectAttributePath.Text = String.Empty;
            tbObjectAttributePath.IsEnabled = false;

            btObjectAttributeBrowse.IsEnabled = false;
            cbDisSpaceMod.IsEnabled = true;
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
                case ClassInfoType.OneToOne:
                    rbClassObjectOneToOne.IsChecked = true;
                    break;
                case ClassInfoType.CountObj:
                    rbClassObjectCountObj.IsChecked = true;
                    break;
                case ClassInfoType.StartObjects:
                    rbClassObjectStartObjects.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void cbClassObject_Unchecked(object sender, RoutedEventArgs e)
        {
            tbClassObjectPath.Text = String.Empty;
            tbClassObjectPath.IsEnabled = false;

            btClassObjectBrowse.IsEnabled = false;

            rbClassObjectOneToOne.IsEnabled = false;
            rbClassObjectCountObj.IsEnabled = false;
            rbClassObjectStartObjects.IsEnabled = false;

            cbClassEqual.IsEnabled = true;

            rbPicturesByClassName.IsEnabled = false;
            rbPicturesByClassName.IsChecked = false;

            tbPictureDirectoryPath.Text = String.Empty;
        }

        private void cbNamesObject_Checked(object sender, RoutedEventArgs e)
        {
            tbNamesObjectPath.IsEnabled = true;
            btNamesObjectBrowse.IsEnabled = true;

            if (cbNamesPictures.IsChecked == true)
            {
                rbPicturesByObjectsName.IsEnabled = true;
            }
        }

        private void cbNamesObject_Unchecked(object sender, RoutedEventArgs e)
        {
            tbNamesObjectPath.Text = String.Empty;
            tbNamesObjectPath.IsEnabled = false;
            btNamesObjectBrowse.IsEnabled = false;
            rbPicturesByClassName.IsEnabled = false;

            if (cbNamesPictures.IsChecked == true)
            {
                rbPicturesByObjectsName.IsEnabled = false;
            }
        }

        private void rbClassObject_Checked(object sender, RoutedEventArgs e)
        {

            tbClassObjectPath.Text = String.Empty;
            lbUniqueClasses.Items.Clear();

            if ((cbNamesPictures.IsChecked == true) && ((rbClassObjectCountObj.IsChecked == true) || (rbClassObjectStartObjects.IsChecked == true)))
            {
                rbPicturesByClassName.IsEnabled = true;
                rbPicturesByClassName.IsChecked = true;
            }
            if (rbClassObjectOneToOne.IsChecked.Value)
            {
                SettFiles.ClassObjectType = ClassInfoType.OneToOne;
            }
            else if (rbClassObjectCountObj.IsChecked.Value)
            {
                SettFiles.ClassObjectType = ClassInfoType.CountObj;
            }
            else if (rbClassObjectStartObjects.IsChecked.Value)
            {
                SettFiles.ClassObjectType = ClassInfoType.StartObjects;
            }
        }

        private void btMatrixDistanceBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFile(tbMatrixDistancePath, false);

            //if (tbMatrixDistancePath.Text != "")
            if (!String.IsNullOrEmpty(tbMatrixDistancePath.Text))
            {
                cbNamesPictures.IsEnabled = true;
                cbClassObject.IsChecked = false;
                rbClassObjectOneToOne.IsChecked = false;
                rbClassObjectCountObj.IsChecked = false;
                rbClassObjectStartObjects.IsChecked = false;
                tbClassObjectPath.Text = String.Empty;
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
                            rbPicturesByClassName.IsChecked = true;
                            break;

                        default:
                            throw new ApplicationException();
                    }
                }
                else
                {
                    cbNamesPictures.IsChecked = false;
                    tbPictureDirectoryPath.Text = String.Empty;
                    rbPicturesById.IsChecked = false;
                    rbPicturesByObjectsName.IsChecked = false;
                    rbPicturesByClassName.IsChecked = false;
                }
            }
        }

        private void btObjectAttributeBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFile(tbObjectAttributePath, false);

            if (!String.IsNullOrEmpty(tbObjectAttributePath.Text))
            {
                cbNamesPictures.IsEnabled = true;
                cbClassObject.IsChecked = false;
                rbClassObjectOneToOne.IsChecked = false;
                rbClassObjectCountObj.IsChecked = false;
                rbClassObjectStartObjects.IsChecked = false;
                tbClassObjectPath.Text = String.Empty;
                lbUniqueClasses.Items.Clear();

                string[] logData = GetDataFromPictureLogFile(tbObjectAttributePath.Text.ToString());
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
                            rbPicturesByClassName.IsChecked = true;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    cbNamesPictures.IsChecked = false;
                    tbPictureDirectoryPath.Text = String.Empty;
                    rbPicturesById.IsChecked = false;
                    rbPicturesByObjectsName.IsChecked = false;
                    rbPicturesByClassName.IsChecked = false;
                }
            }
        }

        private void btClassObjectBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFile(tbClassObjectPath, true);
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
            if (((bool)cbClassObject.IsChecked) && (String.IsNullOrEmpty(tbClassObjectPath.Text)))
            {
                Utils.ShowErrorMessage(ClassFilesChooseErrorMessage);
                return;
            }

            Engine temp = ReadValues();
            string settingErrorDescription = temp.Validation();

            if ((bool)cbClassObject.IsChecked)
            {
                numberOfObjectsOfClass = getNumberOfObjectsOfClass(temp);
                temp.numberOfObjectsOfClass = new int[numberOfObjectsOfClass.Length];
                Array.Copy(numberOfObjectsOfClass, temp.numberOfObjectsOfClass, numberOfObjectsOfClass.Length);
            }

            if (temp.UniqClassesName == null)
            {
                Utils.ShowErrorMessage(String.Format(GettingClassDataErrorMessage, settingErrorDescription));
                return;
            }

            if (!string.IsNullOrEmpty(settingErrorDescription))
            {
                Utils.ShowErrorMessage(settingErrorDescription);
            }
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

            if ((rbPicturesByClassName.IsEnabled == true) && (rbPicturesByClassName.IsChecked == true))
            {
                FBD.Description = "Выберете папку с папками изображениями объектов соответствующих классов";
            }
            else
            {
                FBD.Description = "Выберете папку с изображениями объектов";
            }

            FBD.ShowDialog();
            if (FBD.SelectedPath != String.Empty)
            {
                if ((bool)rbPicturesByClassName.IsChecked)
                {
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
            }
        }
        private void checkClassSubfoldersInPictureFolder(String pictureFolderPath, System.Collections.Generic.List<string> uniqueClassesName)
        {
            string notFoundedClassDirs = String.Empty;
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
                {
                    notFoundedClassDirs += className + ", ";
                }
            }

            if (!String.IsNullOrEmpty(notFoundedClassDirs))
            {
                Utils.ShowWarningMessage(String.Format(ClassDirNotFoundWarningMessage, notFoundedClassDirs.Remove(notFoundedClassDirs.Length - 2)));

            }
        }
        private void cbNamesPictures_Unchecked(object sender, RoutedEventArgs e)
        {
            rbPicturesByClassName.IsEnabled = false;
            rbPicturesById.IsEnabled = false;
            rbPicturesById.IsChecked = false;
            rbPicturesByObjectsName.IsEnabled = false;
            rbPicturesByObjectsName.IsChecked = false;

            btNameFolderPicBrowse.IsEnabled = false;
        }

        private void cbNamesPictures_Checked(object sender, RoutedEventArgs e)
        {
            if (cbNamesObject.IsChecked == true)
            {
                rbPicturesByObjectsName.IsEnabled = true;
            }

            rbPicturesById.IsEnabled = true;

            if ((rbClassObjectStartObjects.IsChecked == true) || (rbClassObjectCountObj.IsChecked == true))
            {
                rbPicturesByClassName.IsEnabled = true;
            }
        }

        private void rbPicturesById_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;

            if ((rbPicturesByObjectsName.IsChecked == true) && (rbPicturesByObjectsName.IsEnabled == true))
            {
                rbPicturesByObjectsName.IsChecked = false;
            }

            if ((rbPicturesByClassName.IsChecked == true) && (rbPicturesByClassName.IsEnabled == true))
            {
                rbPicturesByClassName.IsChecked = false;
            }
            //Пометить отсюда в будущий класс с инфой вариант выбора.
        }

        private void rbPicturesByObjectsName_Checked(object sender, RoutedEventArgs e)
        {
            btNameFolderPicBrowse.IsEnabled = true;
            if ((rbPicturesById.IsChecked == true) && (rbPicturesById.IsEnabled == true))
            {
                rbPicturesById.IsChecked = false;
            }
            if ((rbPicturesByClassName.IsChecked == true) && (rbPicturesByClassName.IsEnabled == true))
            {
                rbPicturesByClassName.IsChecked = false;
            }
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
            tbMinkovskiDegree.IsEnabled = false;
            //rbMetricEuclidean.IsEnabled = false;
            //rbMetricNonEuclidean.IsEnabled = false;
        }

        private void cbDisSpaceMod_Unchecked(object sender, RoutedEventArgs e)
        {
            rbObjectAttribute.IsEnabled = true;
            tbMinkovskiDegree.IsEnabled = true;
            //rbMetricEuclidean.IsEnabled = true;
            //rbMetricNonEuclidean.IsEnabled = true;

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
                    .Remove(pathToAnyMatrix.LastIndexOf('\\') + 1) + AdressOfPictureLogFile;
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
                Utils.ShowWarningMessage(PictureDirAdressReadingWarningMessage);
            }

            return null;
        }

        private void WriteDataToPictureLog(string pathToAnyMatrix, string pathToPictureFolder, string pictureLoadingType)
        {
            string pathToPictureContentAdressLog = pathToAnyMatrix.Remove(pathToAnyMatrix.LastIndexOf('\\') + 1)
                + AdressOfPictureLogFile;
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
                Utils.ShowWarningMessage(PictureDirLogFileWarningMessage);
            }
        }

        private void cbFastMapAlg_Checked(object sender, RoutedEventArgs e)
        {
            gbSpaceType.IsEnabled = true;
        }

        private void cbFastMapAlg_Unchecked(object sender, RoutedEventArgs e)
        {
            gbSpaceType.IsEnabled = false;
        }
    }
}
