// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;

namespace VisualChart3D.Common.Visualization
{
    public class DisSpace
    {
        private const int DefaultFirstObjectID = 1;
        private const int DefaultSecondObjectID = 2;
        private const int DefaultThirdObjectID = 3;

        private const int FirstObjectArrayIndex = 0;
        private const int SecondObjectArrayIndex = 1;
        private const int ThirdObjectArrayIndex = 2;

        private const int ObjectsCount = 3;

        private const int Compensator = 1;

        private int _firstBasisObject;
        private int _secondBasisObject;
        private int _thirdBasisObject;
        private bool _basicObjectsColorMode;
        private double[,] _arraySource;
        private int _countOfObjects;
        private ITimer _timer;
        private Space _space = Space.TwoDimensional;
        private double[,] _coords;
        private int[] _basicObjects;

        //ОТСУТСТВУЕТ ИНИЦИАЛИЗАЦИЯ ИЗ КОНСТРУКТОРА КЛАССА REFERENCED OBJECT
        private int[] _referencedObjects;

        public int FirstBasisObject { get => _firstBasisObject; set => _firstBasisObject = value; }
        public int SecondBasisObject { get => _secondBasisObject; set => _secondBasisObject = value; }
        public int ThirdBasisObject { get => _thirdBasisObject; set => _thirdBasisObject = value; }
        public Space Space { get => _space; set => _space = value; }
        public bool BasicObjectsColorMode { get => _basicObjectsColorMode; set => _basicObjectsColorMode = value; }
        public double[,] ArraySource { get => _arraySource; }
        public int BasicObjectsNumber { get => (int)_space; }

        public DisSpace(double[,] arraySource, int countOfObjects)
        {
            _timer = new CustomTimer();
            _firstBasisObject = DefaultFirstObjectID;
            _secondBasisObject = DefaultSecondObjectID;
            _thirdBasisObject = DefaultThirdObjectID;
            _coords = null;
            _basicObjectsColorMode = false;
            _arraySource = arraySource;
            _countOfObjects = countOfObjects;
        }

        //ВСПОМНИТЬ ХИТРУЮ ЗАДУМКУ
        private void CalculateReferencedObjects(double[,] SourceArray, int[] countOfClassObjects)
        {

        }

        private double FindSumOfDistances(double[,] SourceArray, int classStartObject, int classEndObject, int currentObject)
        {
            double sumfOfDistances = 0;

            for (int i = classStartObject; i <= classEndObject; i++)
            {
                sumfOfDistances += SourceArray[currentObject, i];
            }

            return sumfOfDistances;
        }

        private static int GetMaxColumnValueIndex(double[,] sourceArray, int column, int columnCount)
        {
            double value = Double.MinValue;
            int index = -1;

            for (int i = 0; i < columnCount; i++)
            {
                if (sourceArray[column, i] > value)
                {
                    value = sourceArray[column, i];
                    index = i;
                }
            }

            return index;
        }

        private static int GetTwoColumnsSumMaxIndex(double[,] sourceArray, int firstIndex, int secondIndex, int objectsCount)
        {
            double value = Double.MinValue;
            int index = -1;

            double iterationSum;

            for (int i = 0; i < objectsCount; i++)
            {
                if (firstIndex == i)
                {
                    continue;
                }

                if (secondIndex == i)
                {
                    continue;
                }

                iterationSum = sourceArray[firstIndex, i] + sourceArray[secondIndex, i];

                if (iterationSum > value)
                {
                    value = iterationSum;
                    index = i;
                }
            }

            return index;
        }

        public static int[] GetMostestThreeRemoteObjects(double[,] sourceArray)
        {
            const int countOfRemoteObjects = 3;
            int objectsCount = sourceArray.GetLength(0);
            int[] returnedIndexes = new int[countOfRemoteObjects];

            //Находим самый далекий объект
            returnedIndexes[0] = GetMostRemoteObject(sourceArray, objectsCount);

            //Затем в его столбце ищем самый далекий от него
            returnedIndexes[1] = GetMaxColumnValueIndex(sourceArray, returnedIndexes[0], objectsCount);

            //Затем суммируем расстояния в двух стобцах и ищем объект с самым большим значением.
            returnedIndexes[2] = GetTwoColumnsSumMaxIndex(sourceArray, returnedIndexes[0], returnedIndexes[1], objectsCount);

            return returnedIndexes;
        }

        /// <summary>
        /// ПЕРЕПИСАТЬ ВЫШЕЛЕЖАЩУЮ ФУНКЦИЮ ТАК, ЧТОБЫ БЫЛ ЗНАК БОЛЬШЕ В ЦИКЛЕ!!!1111
        /// </summary>
        /// <param name="SourceArray"></param>
        /// <param name="classStartObject"></param>
        /// <param name="classEndObject"></param>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        public static double FindSumOfDistancesWORKING(double[,] SourceArray, int classStartObject, int classEndObject, int currentObject)
        {
            double sumfOfDistances = 0;

            for (int i = classStartObject; i < classEndObject; i++)
            {
                sumfOfDistances += SourceArray[currentObject, i];
            }

            return sumfOfDistances;
        }

        public int[] BasicObjectsArray
        {
            get
            {
                _basicObjects = new int[ObjectsCount];
                _basicObjects[FirstObjectArrayIndex] = _firstBasisObject;
                _basicObjects[SecondObjectArrayIndex] = _secondBasisObject;
                _basicObjects[ThirdObjectArrayIndex] = _thirdBasisObject;

                return _basicObjects;
            }

            set
            {
                _basicObjects = value;
            }
        }

        public int[] ReferencedObjects { get => _referencedObjects; }

        public static int GetMostRemoteObject(double[,] sourceArray, int objectsCount)
        {
            double[] mostRemoteObjects = new double[objectsCount];
            int[] mostRemoteObjectIndexes = new int[objectsCount];

            for (int i = 0; i < objectsCount; i++)
            {
                mostRemoteObjectIndexes[i] = i;
                mostRemoteObjects[i] = FindSumOfDistancesWORKING(sourceArray, 0, objectsCount, i);
            }

            ReverseComparer reverseComparer = new ReverseComparer();
            System.Array.Sort(mostRemoteObjects, mostRemoteObjectIndexes, reverseComparer);

            return mostRemoteObjectIndexes[0];
        }

        public List<string> getReferencedObjectsWithClassNames(List<string> ClassesNames)
        {
            List<string> ReferencedObjectsWithClassNames = new List<string>();

            for (int i = 0; i < _referencedObjects.Length; i++)
            {
                ReferencedObjectsWithClassNames.Add("Класс - " + ClassesNames[i] + ", № Эталона - " + _referencedObjects[i] + ".");
            }

            return ReferencedObjectsWithClassNames;
        }

        public DisSpace SetBasicObjects(int firstObject, int secondObject, int thirdObject)
        {
            _firstBasisObject = firstObject;
            _secondBasisObject = secondObject;
            _thirdBasisObject = thirdObject;
            return this;
        }

        public double[,] ToProject()
        {
            _timer.Start();

            _coords = new double[_countOfObjects, BasicObjectsNumber];

            if (_space == Space.TwoDimensional)
            {
                for (int j = 0; j < _countOfObjects; j++)
                {
                    _coords[j, FirstObjectArrayIndex] = _arraySource[_firstBasisObject - Compensator, j];
                    _coords[j, SecondObjectArrayIndex] = _arraySource[_secondBasisObject - Compensator, j];
                }
            }
            else
            {
                for (int j = 0; j < _countOfObjects; j++)
                {
                    _coords[j, FirstObjectArrayIndex] = _arraySource[_firstBasisObject - Compensator, j];
                    _coords[j, SecondObjectArrayIndex] = _arraySource[_secondBasisObject - Compensator, j];
                    _coords[j, ThirdObjectArrayIndex] = _arraySource[_thirdBasisObject - Compensator, j];
                }
            }

            _timer.Stop();
            return _coords;
        }
    }
}