// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualChart3D
{

    public partial class MainWindow
    {
        //public int ssds;
        //public int first_object_to_dis_space = 1;
        //public int second_object_to_dis_space = 2;
        private DisSpace DissimiliaritySpace;
        //private bool AutomaticlySettingUpOfReference=false;
        private bool referencedObjectsMode = false;
        /// <summary>
        /// Тип визуализации
        /// </summary>
        private TypePlot _typePlot = TypePlot.Main;

        /// <summary>
        /// Настройки классов
        /// </summary>
        private SettingsClasses _settingsClasses; //private

        /// <summary>
        /// Настройки исходных данных, используемых в данный момент
        /// </summary>
        private SettingsFiles _settFilesCurrent;
        /// <summary>
        /// Текущие координаты объектов
        /// </summary>
        private double[,] currentCoords;
        /// <summary>
        /// Настройки исходных данных, при визуализации основных компонентов
        /// </summary>
        private SettingsFiles _settFilesMain;

        /// <summary>
        /// Размер объектов, при визуализации основных компонентов
        /// </summary>
        private double _sizeMain;

        /// <summary>
        /// Расширение для файла настроек класса
        /// </summary>
        private const string ExtColorSchema = ".clrSchema";


        /// <summary>
        /// Трансформация модели
        /// </summary>
        private readonly TransformMatrix _transformMatrix = new TransformMatrix();

        /// <summary>
        /// Данные текущего 3d графика 
        /// </summary>
        private ScatterChart3D _3DChartCurrent;

        /// <summary>
        /// Прямоугольник выделения
        /// </summary>
        private readonly ViewportRect _selectRect = new ViewportRect();

        /// <summary>
        /// Координаты точек текущего графика
        /// </summary>
        private Vertex3D[] _coordCurrent;

        /// <summary>
        /// Координаты точек главного графика
        /// </summary>
        private Vertex3D[] _coordMain;

        /// <summary>
        /// Индекс для прямоугольника выделения
        /// </summary>
        private readonly int _nRectModelIndex = -1;

        /// <summary>
        /// индекс для Viewport3d
        /// </summary>
        private int _nChartModelIndex = -1;

        /// <summary>
        /// Массив выделенных индексов
        /// </summary>
        private int[] _selectedIndex = new int[0];

        /// <summary>
        /// Окно, для отображения информации о выделенных объектов
        /// </summary>
        private ListObjects _listObjectWindow;


        public MainWindow()
        {
            CallBackPoint.callbackEventHandler = new CallBackPoint.callbackEvent(this.LightCurrentObject);
            InitializeComponent();
            _selectRect.SetRect(new Point(-0.5, -0.5), new Point(-0.5, -0.5));
            Model3D model3D = new Model3D();
            List<Mesh3D> meshs = _selectRect.GetMeshes();
            _nRectModelIndex = model3D.UpdateModel(meshs, null, _nRectModelIndex, MainViewport);
        }
        void LightCurrentObject(int objectNumber)
        {
            //MessageBox.Show("Номер объекта - " + objectNumber);
        }

        /// <summary>
        ///  Отображение точек на графике
        /// </summary>
        public void DrawScatterPlot()
        {
            if (_coordCurrent == null || _coordCurrent.Length == 0)
                return;
            if (_settFilesCurrent != null)
                MnDisSpaceSett.IsEnabled = _settFilesCurrent.DisSpaceMod;
            else
            {
                MessageBox.Show("Исключительная ситуация. Код 1");
                return;
            }
            // Установить данные разброса графика
            _3DChartCurrent = new ScatterChart3D(true, _settingsClasses.CountPoligon);
            _3DChartCurrent.SetDataNo(_coordCurrent.Length);

            // установить свойство каждой точки (размер, положение, форма, цвет)
            for (int i = 0; i < _coordCurrent.Length; i++)
            {
                try
                {
                    AloneSettClass classSet =
                        _settingsClasses.GetSettingClass(_settFilesCurrent.ClassesName[i]);
                    //StatusPane.Tag = _settFilesCurrent.NamesObjects[i]; Условие укорочить
                    //if ((classSet == null) || (classSet.isLiquid))
                    if (classSet != null)
                        if (classSet.isLiquid)
                        continue;
                    ScatterPlotItem plotItem = new ScatterPlotItem
                    {
                        ShapeType = (classSet == null) ? VisualChart3D.Shapes.Ellipse3D : classSet.Shape,
                        Color = (classSet == null) ? Color.FromRgb(255, 0, 13) : classSet.ColorObject,   //classSet.ColorObject Color.FromRgb(255,0,13)
                        
                        W = _settingsClasses.SizeObject,
                        H = _settingsClasses.SizeObject,
                        X = _coordCurrent[i].X,
                        Y = _coordCurrent[i].Y,
                        Z = _coordCurrent[i].Z
                    };


                    //if (_settFilesCurrent.DisSpaceMod && Array.Cont)
                    //{
                    //    int[] basicArray = DissimiliaritySpace.getBasicObjectsArray();
                    //    for (int i = 0; i < DissimiliaritySpace.getBasicObjectsNumber(); i++)
                    //    {
                    //        _3DChartCurrent.Get(basicArray[i]-1).Color = Color.FromRgb((byte)(255 - _3DChartCurrent.Get(basicArray[i]-1).Color.R), // тут косяк, три разных объекта, что-то нулевое
                    //       (byte)(255 - _3DChartCurrent.Get(basicArray[i]-1).Color.G), (byte)(255 - _3DChartCurrent.Get(basicArray[i]-1).Color.B));
                    //    }
                    //   
                    //}
                    _3DChartCurrent.SetVertex(i, plotItem);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Не удалось построить график. Возможно, некорректо задан тип файла классов");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при построении графика\n" + ex.Message + "\n" + ex + "\n" + ex.StackTrace);
                    return;
                }
            }
            if (_settFilesCurrent.DisSpaceMod)
            {
                if (DissimiliaritySpace.getBasicObjectsColorMode())
                {
                    int[] basicArray = DissimiliaritySpace.getBasicObjectsArray();
                    for (int i = 0; i < DissimiliaritySpace.getBasicObjectsNumber(); i++)
                    {
                        if (_3DChartCurrent.Get(basicArray[i] - 1) != null)
                            _3DChartCurrent.Get(basicArray[i] - 1).Color = Color.FromRgb((byte)(255 - _3DChartCurrent.Get(basicArray[i] - 1).Color.R), // тут косяк, три разных объекта, что-то нулевое
                        (byte)(255 - _3DChartCurrent.Get(basicArray[i] - 1).Color.G), (byte)(255 - _3DChartCurrent.Get(basicArray[i] - 1).Color.B));
                    }
                }
               
            }

            // Задать оси
            _3DChartCurrent.SetAxes(Colors.LightSkyBlue,_settFilesCurrent.DisSpaceMod);

            // Получить массив трёхмерных точек на графике
            List<Mesh3D> meshs = _3DChartCurrent.GetMeshes();

            // вывести график в ViewPoint3D
            Model3D model3D = new Model3D();
            _nChartModelIndex = model3D.UpdateModel(meshs, null, _nChartModelIndex, MainViewport);

            // Задать проекцию
            double viewRange = _3DChartCurrent.ViewRange;
            _transformMatrix.CalculateProjectionMatrix(new Point3D(0, 0, 0),
                new Point3D(viewRange, viewRange, viewRange), 0.5);
            TransformChart();
        }

        /// <summary>
        /// Вращения/Перемещение и Увеличения 3d графика
        /// </summary>
        private void TransformChart()
        {
            if (_nChartModelIndex == -1)
                return;
            ModelVisual3D visual3D = (ModelVisual3D) (MainViewport.Children[_nChartModelIndex]);
            if (visual3D.Content == null)
                return;
            Transform3DGroup group1 = visual3D.Content.Transform as Transform3DGroup;
            if (group1 != null)
            {
                group1.Children.Clear();
                group1.Children.Add(new MatrixTransform3D(_transformMatrix.TotalMatrix));
            }
        }

        /// <summary>
        /// Генерация координат
        /// </summary>
        /// 
        /*private double[,] DissimilitarySpaceGetCoords(SettingsFiles SetFile, int FirstObj, int SecondObj)
        {
            
            double[,] Cords = new double[2, SetFile.CountObjects];
            for (int j = 0; j < SetFile.CountObjects; j++)
            {
                Cords[0,j]=SetFile.ArraySource[FirstObj-1,j];
                Cords[1, j] = SetFile.ArraySource[SecondObj-1, j];
            }
            return Cords;

        }*/
        private void DissimilitarySpaceGeneration(DisSpace DissimiliaritySpace,bool firstGeneration = false)
        {
            //double[,] currentCoords = DissimilitarySpaceGetCoords(_settFilesCurrent, FirstObject, SecondObject); //,123,143
            currentCoords = DissimiliaritySpace.GetCoords(_settFilesCurrent);
            int countCords = currentCoords.Length / DissimiliaritySpace.getBasicObjectsNumber();
            _coordCurrent = new Vertex3D[countCords];
            if(firstGeneration)
              _selectRect.OnMouseDown(new Point(0, 0), MainViewport, _nRectModelIndex);
            if (DissimiliaritySpace.getSpace().Equals("2D"))
              {
                  for (int i = 0; i < countCords; i++)
                  {
                      _coordCurrent[i] = new Vertex3D
                      {
                          X = currentCoords[0, i],
                          Y = currentCoords[1, i],
                          Z = 0.1
                      };
                  }
              }
              else
                  for (int i = 0; i < countCords; i++)
                  {
                      _coordCurrent[i] = new Vertex3D
                      {
                          X = currentCoords[0, i],
                          Y = currentCoords[1, i],
                          Z = currentCoords[2, i]
                      };
                  }
        }



        private void FastMapGeneration(bool firstGeneration = false)
        {
            FastMap fastMap = _settFilesCurrent.SourceMatrix == SourceFileMatrix.MatrixDistance
                ? new FastMap(_settFilesCurrent.ArraySource, _settFilesCurrent.Metrics)
                : new FastMap(CommonMatrix.ObjectAttributeToDistance(_settFilesCurrent.ArraySource, _settFilesCurrent.CountObjects), _settFilesCurrent.Metrics);
            const int countProj = 3;
            currentCoords = fastMap.GetCoordinates(countProj);
            int countCords = currentCoords.Length/countProj;
            _coordCurrent = new Vertex3D[countCords];
            if (!firstGeneration)
            {
                for (int i = 0; i < countCords; i++)
                {
                    _coordCurrent[i] = new Vertex3D
                    {
                        X = currentCoords[i, 0],
                        Y = currentCoords[i, 1],
                        Z = currentCoords[i, 2]
                    };
                }
            }
            else
            {
                _selectRect.OnMouseDown(new Point(0, 0), MainViewport, _nRectModelIndex);
                string fileName =_settFilesCurrent.SourceMatrixFile + ".3D";
                try
                {
                    using (WriteTextToFile file = new WriteTextToFile(fileName))
                    {
                        for (int i = 0; i < countCords; i++)
                        {
                            file.WriteLineFormat("{0}\t{1}\t{2}\t{3}", _settFilesCurrent.ClassesName[i], currentCoords[i, 0],
                                currentCoords[i, 1], currentCoords[i, 2]);
                            _coordCurrent[i] = new Vertex3D
                            {
                                X = currentCoords[i, 0],
                                Y = currentCoords[i, 1],
                                Z = currentCoords[i, 2]
                            };
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Не удалось создать файл " + fileName);
                    for (int i = 0; i < countCords; i++)
                    {
                        _coordCurrent[i] = new Vertex3D
                        {
                            X = currentCoords[i, 0],
                            Y = currentCoords[i, 1],
                            Z = currentCoords[i, 2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Определение опримального размера объектов
        /// </summary>
        private void SizeDetection()
        {
            double nDataRange =  _coordCurrent.Max(d => d.X);
            _settingsClasses.SizeObject = nDataRange/100;
        }

        private void canvasOn3D_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(MainViewport);
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
	            if (Math.Abs(_selectRect.XMax - _selectRect.XMin) > 0.01)
	            {
		            _selectRect.OnMouseDown(new Point(0, 0), MainViewport, _nRectModelIndex);
					_selectedIndex = _3DChartCurrent.Select(_selectRect, _transformMatrix, MainViewport);
					MeshGeometry3D meshGeometry = Model3D.GetGeometry(MainViewport, _nChartModelIndex);
					_3DChartCurrent.HighlightSelection(meshGeometry, Color.FromRgb(200, 200, 200));
	            }
	            _transformMatrix.OnMouseMove(pt, MainViewport);

                TransformChart();
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                _selectRect.OnMouseMove(pt, MainViewport, _nRectModelIndex);
            }
            else
            {
                Point pt2 = _transformMatrix.VertexToScreenPt(new Point3D(0.5, 0.5, 0.3), MainViewport);
                string s1 = string.Format("Screen:({0:d},{1:d}), Predicated: ({2:d}, H:{3:d}), Window: {4}",
                    (int) pt.X, (int) pt.Y, (int) pt2.X, (int) pt2.Y, _typePlot.ToString());
                StatusPane.Text = s1;

            }
        }

        private void canvasOn3D_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _transformMatrix.OnLBtnUp();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (_nChartModelIndex == -1) return;

                MeshGeometry3D meshGeometry = Model3D.GetGeometry(MainViewport, _nChartModelIndex);
                if (meshGeometry == null) return;

                if (_typePlot == TypePlot.Main)
                {
                    _selectedIndex = _3DChartCurrent.Select(_selectRect, _transformMatrix, MainViewport);
                    _3DChartCurrent.HighlightSelection(meshGeometry, Color.FromRgb(200, 200, 200));
                    if (_listObjectWindow != null)
                        _listObjectWindow.SetListObjects(_settFilesCurrent, _selectedIndex, currentCoords);
                }
                else
                {
                    int[] selectedIndex = _3DChartCurrent.Select(_selectRect, _transformMatrix, MainViewport);
                    _3DChartCurrent.HighlightSelection(meshGeometry, Color.FromRgb(200, 200, 200));
                    if (_listObjectWindow != null)
                        _listObjectWindow.SetListObjects(_settFilesCurrent, selectedIndex, currentCoords);
                }
            }
        }

        private void canvasOn3D_MouseWheel(object sender, MouseWheelEventArgs e)
        {
	        if (Math.Abs(_selectRect.XMax - _selectRect.XMin) > 0.01)
	        {
				_selectRect.OnMouseDown(new Point(0, 0), MainViewport, _nRectModelIndex);
				_selectedIndex = _3DChartCurrent.Select(_selectRect, _transformMatrix, MainViewport);
				MeshGeometry3D meshGeometry = Model3D.GetGeometry(MainViewport, _nChartModelIndex);
				_3DChartCurrent.HighlightSelection(meshGeometry, Color.FromRgb(200, 200, 200));
	        }
	        _transformMatrix.OnWheel(e);
            TransformChart();
        }

        private void canvasOn3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ClickCount == 2) && (!_settFilesCurrent.DisSpaceMod))
            {
                if (_typePlot == TypePlot.Main && _selectedIndex.Length > 0)
                {
                     _selectRect.OnMouseDown(new Point(0,0), MainViewport, _nRectModelIndex);
                    _typePlot = TypePlot.Subsidiary;
                    _settFilesMain = _settFilesCurrent;
                    _coordMain = _coordCurrent;
                    _sizeMain = _settingsClasses.SizeObject;

                    SettingsFilesSubsidiary settTemp = new SettingsFilesSubsidiary(_settFilesCurrent, _selectedIndex);
                    _settFilesCurrent = settTemp;
                    FastMapGeneration();
                        //DissimilitarySpaceGeneration(first_object_to_dis_space, second_object_to_dis_space,false);
                    SizeDetection();


                    DrawScatterPlot();
                    return;
                }
            }

            Point pt = e.GetPosition(MainViewport);
            if (e.ChangedButton == MouseButton.Left)
            {
                _transformMatrix.OnLBtnDown(pt);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                _selectRect.OnMouseDown(pt, MainViewport, _nRectModelIndex);
            }
        }
        private void MnDisSpaceSett_Click(object sender, RoutedEventArgs e)
        {
            if (_settFilesCurrent == null)
            {
                MessageBox.Show("Не заданы исходные данные!");
                return;
            }
            DisSpaceWin DisSpcWin = new DisSpaceWin(_settFilesCurrent, DissimiliaritySpace.getFirstBasicObject(),
                DissimiliaritySpace.getSecondBasicObject(), DissimiliaritySpace.getThirdBasicObject(), DissimiliaritySpace.getSpace(), DissimiliaritySpace.getBasicObjectsColorMode(), referencedObjectsMode);
            bool? showDialog = DisSpcWin.ShowDialog();
            DissimiliaritySpace.setBasicObjects(DisSpcWin.firstСhoosedОbject, DisSpcWin.secondChoosedObject, DisSpcWin.thirdChoosedObject)
                .setSizeOfSpace(DisSpcWin.sizeOfSpace);
            DissimiliaritySpace.setBasicObjectsMode(DisSpcWin.basisObjectColorMode);
            referencedObjectsMode = (bool)DisSpcWin.cbAutomaticlySettingUpOfReference.IsChecked;
                // создаем окошко и передаем туда сеттингфайлз, получаем -
            DissimilitarySpaceGeneration(DissimiliaritySpace, true);
            DrawScatterPlot();
            if (MnListObjects.IsChecked)
            {
                _listObjectWindow.setCoords(currentCoords);
                _listObjectWindow.displayObjectCoords(_listObjectWindow.ListBoxObjects.SelectedIndex);
                //MnListObjects.IsChecked = false; //допилить сохранение координат и размера. А в теории -
                //запилить просто без обновления перерасчет координат!
            }
            //MnListObjects.IsChecked = true;
        }

        private void MnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (_settFilesCurrent == null)
            {
                MessageBox.Show("Не заданы исходные данные!");
                return;
            }
            SettingsWindow stWin = new SettingsWindow(_settingsClasses);
            bool? showDialog = stWin.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                _settingsClasses = stWin.SettingsClassesForms;
                if (_settFilesCurrent.ClassObjectSelected)
                {
                    System.Xml.Serialization.XmlSerializer writer =
                        new System.Xml.Serialization.XmlSerializer(typeof (SettingsClasses));
                    var path = _settFilesCurrent.ClassObjectFile + ExtColorSchema;
                    FileStream file = File.Create(path);
                    writer.Serialize(file, _settingsClasses);
                    file.Close();
                }
                DrawScatterPlot();
            }

        }

        private void MnOpen_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow stWin = new ConfigWindow(_settFilesCurrent);
            bool? showDialog = stWin.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                _typePlot = TypePlot.Main;
                _settFilesCurrent = stWin.SettFiles;
                if (_settFilesCurrent.DisSpaceMod)
                {
                    DissimiliaritySpace = new DisSpace();
                    DissimilitarySpaceGeneration(DissimiliaritySpace, true);
                }
                else
                    FastMapGeneration(true);
                try
                {
                    string pathXml = _settFilesCurrent.ClassObjectSelected
                        ? _settFilesCurrent.ClassObjectFile + ExtColorSchema
                        : String.Empty;
                    if (File.Exists(pathXml))
                    {
                        System.Xml.Serialization.XmlSerializer reader =
                            new System.Xml.Serialization.XmlSerializer(typeof (SettingsClasses));
                        StreamReader file = new StreamReader(
                            pathXml);
                        _settingsClasses = new SettingsClasses();
                        _settingsClasses = (SettingsClasses) reader.Deserialize(file);
                        file.Close();

                    }
                    else
                    {
                        _settingsClasses = new SettingsClasses(_settFilesCurrent.UniqClassesName);
                        SizeDetection();
                    }
                }
                catch
                {
                    _settingsClasses = new SettingsClasses(_settFilesCurrent.UniqClassesName);
                    SizeDetection();
                }
                Title = "Visual Chart 3D " + Path.GetFileName(_settFilesCurrent.SourceMatrixFile);
                DrawScatterPlot();
            }
            else
                _settFilesCurrent = null;
        }

        private void MnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseListObjects(object sender, EventArgs e)
        {
            MnListObjects.IsChecked = false;
        }

        private void MnListObjects_Checked(object sender, RoutedEventArgs e)
        {
            //if (_listObjectWindow == null)
            {
                _listObjectWindow = new ListObjects();
                _listObjectWindow.CloseEvent += CloseListObjects;
                _listObjectWindow.Show();
            }
        }

        private void MnListObjects_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_listObjectWindow != null && !_listObjectWindow.IsClosing)
            {
                _listObjectWindow.CloseEvent -= CloseListObjects;
                _listObjectWindow.Close();
            }
            _listObjectWindow = null;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MnListObjects.IsChecked = false;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_typePlot == TypePlot.Subsidiary && e.Key == Key.Back)
            {
                _selectRect.OnMouseDown(new Point(0, 0), MainViewport, _nRectModelIndex);
                _typePlot = TypePlot.Main;
                _settFilesCurrent = _settFilesMain;
                _coordCurrent = _coordMain;
                if(_settFilesCurrent.DisSpaceMod)
                    DissimilitarySpaceGeneration(DissimiliaritySpace, false);
                else
                    FastMapGeneration(); //_settingsClasses _settingsClasses
                _settingsClasses.SizeObject = _sizeMain;
                DrawScatterPlot();
            }
            else
            {
                _transformMatrix.OnKeyDown(e);
                TransformChart();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Keyboard.Focus(this);
        }

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			const string pathHelp = @"help\index.htm";
			if (File.Exists(pathHelp))
				Process.Start(@"help\index.htm");
			else
				MessageBox.Show("Файл \"" + pathHelp + "\" не найден");
		}


    }
}