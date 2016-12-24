// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VisualChart3D
{
    /// <summary>
    /// Настройка исходных данный
    /// </summary>
    public class SettingsFiles
    {
        /// <summary>
        /// Получить количество объектов
        /// </summary>
        public virtual int CountObjects { get; private set; }

        /// <summary>
        /// Получить или задать тип исходной матрицы
        /// </summary>
        public SourceFileMatrix SourceMatrix { get; set; }

        /// <summary>
        /// Получить или задать путь к файлу с исходной матрицей
        /// </summary>
        public string SourceMatrixFile { get; set; }
        /// <summary>
        /// Получить массив с количеством объектов каждого класса
        /// </summary>
        public int[] numberOfObjectsOfClass;
        /// <summary>
        /// Получить массив с исходной матрицей
        /// </summary>
        /// 
        public string Pic_Folder_Adress=null;

        public bool DisSpaceMod=false;
        public bool isPictureTakenByObjectID = false;  //добавить обнуление
        public bool isPictureTakenByObjectName = false;  //добавить обнуление
        public bool isPictureTakenByClassInterval = false;  //добавить обнуление
        public bool isPictureTakenByClassStartObjects = false;  //добавить обнуление
        public List<String> Class_Start_Position;

        public int[] GetClassPositionsOnOneToOneMode(List<String> uniqueClassesNames)
        {
            int[] end_positions = new int[uniqueClassesNames.Count + 1];
            int[] count = new int[uniqueClassesNames.Count];
            end_positions[0]=0;
            int i = 1;
            foreach (string className in uniqueClassesNames)
            {
                end_positions[i] = _classesName.LastIndexOf(className)+1;
                i++;
            }
            for (int k = 1; k < uniqueClassesNames.Count + 1; k++)
            {
                count[k - 1] = end_positions[k] - end_positions[k - 1];
            }
            return count;
        }

        public virtual double[,] ArraySource { get; private set; }

        /// <summary>
        /// Получить или задать сущестовование названий классов. Если есть файл с настройкой классов - true, в противном случае false
        /// </summary>
        public bool ClassObjectSelected { get; set; }

        /// <summary>
        ///  Получить массив данных, если файл классов задан "Один-к-одному"
        /// </summary>
        protected string[] ArrayClassesOneToOne { get; set; }

        /// <summary>
        /// Получить массив данных, если файл классов задан "Число объектов класса" или "Начало класса"
        /// </summary>
        public virtual string[,] ArrayClassesCountObj { get; private set; }

        /// <summary>
        /// Получить или задать тип классов объектов
        /// </summary>
        public ClassObjecEnum ClassObjectType { get; set; }

        /// <summary>
        ///  Получить или задать путь к файлу с настройками классов объектов
        /// </summary>
        public string ClassObjectFile { get; set; }

        /// <summary>
        ///  Получить или задать сущестовование имён объектов. Если есть файл с именем объектов - true, в противном случае false
        /// </summary>
        public bool NamesObjectSelected { get; set; }

        /// <summary>
        ///  Получить или задать путь к файлу с иенем объектов
        /// </summary>
        public string NamesObjectFile { get; set; }

        /// <summary>
        /// Получить или задать задани файлов числом объектов класса
        /// </summary>
        public bool ClassEqualSelected { get; set; }


        /// <summary>
        /// Получить или задать строковое прдеставление число объектов в классе
        /// </summary>
        public string ClassEqualCountStr { get; set; }


        /// <summary>
        /// Получить список названий классов
        /// </summary>
        public virtual List<String> ClassesName
        {
            get
            {
                if (_classesName == null)
                {
                    _classesName = GetListNamesClass();
                    return _classesName;
                }
                return _classesName;
            }
        }

        /// <summary>
        /// Получить список неповторяющихся названий классов
        /// </summary>
        public List<String> UniqClassesName
        {
            get
            {
                if (_uniqClassesName == null)
                {
                    _uniqClassesName = GetUniqClass();
                    return _uniqClassesName;
                }
                return _uniqClassesName;
            }
        }

        /// <summary>
        /// Получить список имён объектов
        /// </summary>
        public virtual List<String> NamesObjects
        {
            get
            {
                if (_namesObjects == null)
                {
                    _namesObjects = GetNamesObject();
                    return _namesObjects;
                }
                return _namesObjects;
            }
        }

		/// <summary>
		///  Получить или задать тип метрики для построения
		/// </summary>
	    public MetricsEnum Metrics { get; set; }

	    /// <summary>
        /// число объектов в классе
        /// </summary>
        private int _classEqualCount;

        /// <summary>
        /// список имен объектов
        /// </summary>
        private List<string> _namesObjects;

        /// <summary>
        /// Уникальные имена классов
        /// </summary>
        private List<string> _uniqClassesName;

        /// <summary>
        /// массив имён объектов
        /// </summary>
        private string[] _arrayNames;

        /// <summary>
        /// список названий классов
        /// </summary>
        private List<string> _classesName;

        /*SettingsFiles SetClassesName(List<string> classesName)
        {
            _classesName.Clear();
            _classesName.AddRange(classesName);
            return this;
        }*/
        public SettingsFiles()
        {
            SourceMatrix = SourceFileMatrix.MatrixDistance;
            ClassObjectSelected = false;
            NamesObjectSelected = false;
        }

        public SettingsFiles(SettingsFiles sf)
        {
            DisSpaceMod = sf.DisSpaceMod;
            isPictureTakenByObjectID = sf.isPictureTakenByObjectID;  
            isPictureTakenByObjectName = sf.isPictureTakenByObjectName;  
            isPictureTakenByClassInterval = sf.isPictureTakenByClassInterval;
            isPictureTakenByClassStartObjects = sf.isPictureTakenByClassStartObjects;
            Class_Start_Position = sf.Class_Start_Position;
           // Pic_Folder_Adress = sf.Pic_Folder_Adress;
            CountObjects = sf.CountObjects;
            SourceMatrix = sf.SourceMatrix;
            Pic_Folder_Adress = sf.Pic_Folder_Adress;
            SourceMatrixFile = sf.SourceMatrixFile;
            ArraySource = sf.ArraySource;
            ClassObjectSelected = sf.ClassObjectSelected;
            ArrayClassesOneToOne = sf.ArrayClassesOneToOne;
            ArrayClassesCountObj = sf.ArrayClassesCountObj;
            ClassObjectType = sf.ClassObjectType;
            ClassObjectFile = sf.ClassObjectFile;
            NamesObjectSelected = sf.NamesObjectSelected;
            NamesObjectFile = sf.NamesObjectFile;
	        Metrics = sf.Metrics;
            _namesObjects = sf._namesObjects;
            _uniqClassesName = sf._uniqClassesName;
            _arrayNames = sf._arrayNames;
            _classesName = sf._classesName;
            numberOfObjectsOfClass = sf.numberOfObjectsOfClass;
        }

        /// <summary>
        /// Проверка на корректность исходной матрицы
        /// </summary>
        /// <returns></returns>
        public bool CheckSourceMatrix()
        {
            if (string.IsNullOrEmpty(SourceMatrixFile) || !File.Exists(SourceMatrixFile))
                return false;
            try
            {
                if (SourceMatrix == SourceFileMatrix.MatrixDistance)
                {
                    ArraySource = CommonMatrix.ReadMatrixDistance(SourceMatrixFile);
                    CountObjects = (int) Math.Sqrt(ArraySource.Length);
                }
                if (SourceMatrix == SourceFileMatrix.ObjectAttribute)
                {
                    ArraySource = CommonMatrix.ReadMatrixAttribute(SourceMatrixFile);
                    CountObjects = File.ReadAllLines(SourceMatrixFile).Length;
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на корректоности файла с классами
        /// </summary>
        /// <returns></returns>
        public bool CheckClassObject()
        {
            if (!ClassObjectSelected || string.IsNullOrEmpty(ClassObjectFile) || !File.Exists(ClassObjectFile))
                return false;
            try
            {
                switch (ClassObjectType)
                {
                    case ClassObjecEnum.OneToOne:
                        ArrayClassesOneToOne = CommonMatrix.ReadMatrixOneToOne(ClassObjectFile);
                        break;
                    case ClassObjecEnum.CountObj:
                    case ClassObjecEnum.StartObjects:
                        ArrayClassesCountObj = CommonMatrix.ReadMatrixCountObj(ClassObjectFile);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка на корректность файла с названиями объектов
        /// </summary>
        /// <returns></returns>
        public bool CheckNamesObject()
        {
            if (!NamesObjectSelected || string.IsNullOrEmpty(NamesObjectFile) || !File.Exists(NamesObjectFile))
                return false;
            try
            {
                _arrayNames = CommonMatrix.ReadMatrixOneToOne(NamesObjectFile);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка настроек
        /// </summary>
        /// <returns></returns>
        public string Validation()
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(SourceMatrixFile))
                errors.AppendLine("Не указан файл \"Матрица расстояний\" или \"Матрица объект-признак\"");
            else if (!CheckSourceMatrix())
                errors.AppendLine("Некорректная структура файла \"Матрица расстояний\" или \"Матрица объект-признак\"");
            if (ClassObjectSelected)
            {
                if (string.IsNullOrEmpty(ClassObjectFile))
                    errors.AppendLine("Не указан файл с классами объектов");
                else if (!CheckClassObject())
                    errors.AppendLine("Некорректная структура файла с классами объектов");
            }
            else
            {
                if (ClassEqualSelected)
                {
                    if (string.IsNullOrEmpty(ClassEqualCountStr))
                        errors.AppendLine("Не указано число объектов в классе");
                    else
                    {
                        if (!int.TryParse(ClassEqualCountStr, out _classEqualCount) || _classEqualCount <= 0)
                            errors.AppendLine("Некорректное число объектов в классе");
                        else if (CountObjects%_classEqualCount != 0)
                            errors.AppendLine("Некорректное число объектов в классе. Должно быть кратное числу объектов");
                    }
                }
            }
            if (NamesObjectSelected)
                if (string.IsNullOrEmpty(NamesObjectFile))
                    errors.AppendLine("Не указан файл с именами объектов");
                else if (!CheckNamesObject())
                    errors.AppendLine("Некорректная структура файла с именами объектов");
            return errors.ToString();
        }

        /// <summary>
        /// Получить имена классов
        /// </summary>
        /// <returns>список имён классов</returns>
        private List<string> GetListNamesClass()
        {
            Class_Start_Position = new List<String>();
            int class_border = 0;
            Class_Start_Position.Add("0");
            List<string> result = new List<string>();
            if (!ClassObjectSelected)
            {
                if (ClassEqualSelected)
                {

                    for (int i = 0; i < CountObjects/_classEqualCount; i++)
                        for (int j = 0; j < _classEqualCount; j++)
                            result.Add((i + 1).ToString());
                }
                else
                    for (int i = 0; i < CountObjects; i++)
                    {
                        result.Add("1");
                    }
                return result;
            }
            try
            {
                switch (ClassObjectType)
                {
                    case ClassObjecEnum.OneToOne:
                        result = ArrayClassesOneToOne.ToList();
                        break;
                    case ClassObjecEnum.CountObj:
                        for (int i = 0; i < ArrayClassesCountObj.Length / 2; i++)
                        {
                            int count = int.Parse(ArrayClassesCountObj[i, 0]);
                            class_border += count;
                            Class_Start_Position.Add(class_border.ToString());
                            string name = string.IsNullOrEmpty(ArrayClassesCountObj[i, 1])
                                ? (i + 1).ToString()
                                : ArrayClassesCountObj[i, 1];
                            for (int j = 0; j < count; j++)
                                result.Add(name);
                        }
                        break;
                    case ClassObjecEnum.StartObjects:
                        int oldCount = int.Parse(ArrayClassesCountObj[0, 0]);
                        for (int i = 1; i < ArrayClassesCountObj.Length / 2; i++)
                        {
                            int count = int.Parse(ArrayClassesCountObj[i, 0]);
                            Class_Start_Position.Add(count.ToString());
                            string name = string.IsNullOrEmpty(ArrayClassesCountObj[i - 1, 1])
                                ? (i).ToString()
                                : ArrayClassesCountObj[i - 1, 1];
                            for (int j = oldCount; j < count; j++)
                                result.Add(name);
                            oldCount = count;
                        }
                        int idx = ArrayClassesCountObj.Length / 2 - 1;
                        string nme = string.IsNullOrEmpty(ArrayClassesCountObj[idx, 1])
                            ? (idx + 1).ToString()
                            : ArrayClassesCountObj[idx, 1];

                        for (int j = oldCount; j < CountObjects; j++)
                            result.Add(nme);
                        Class_Start_Position.Add((CountObjects).ToString()); //конец не важен, главное - границы
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (FormatException)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// Получить список уникальных имён класссов
        /// </summary>
        /// <returns>список уникальных имён классов</returns>
        private List<string> GetUniqClass()
        {
            if (ClassesName!=null)
            {
                HashSet<string> result = new HashSet<string>();
                foreach (string s in ClassesName.Where(s => !result.Contains(s)))
                {
                    result.Add(s);
                }
                return result.ToList();
            }
            else
                return null;
        }

        /// <summary>
        /// Получитьс имена объектов
        /// </summary>
        /// <returns>список имён объектов</returns>
        protected List<string> GetNamesObject()
        {
            List<string> result = new List<string>();
            if (!NamesObjectSelected)
            {
                for (int i = 0; i < CountObjects; i++)
                {
                    result.Add((i + 1).ToString());
                }
                return result;
            }
            return _arrayNames.ToList();
        }
    }
}
