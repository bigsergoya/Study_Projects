// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;

namespace VisualChart3D.Common
{
    class ReferencedObjects
    {
        private int[] _referencedObjects;

        public ReferencedObjects(double[,] SourceArray, int[] countOfClassObjects)
        {
            //+= countOfClassObjects[f] после первого конца класса. Первый конец - нулевой элемент.
            _referencedObjects = new int[countOfClassObjects.Length];
            int currentClassLastElement = countOfClassObjects[0] - 1;
            int currentClassFirstElement = 0;
            int numberOfCurrentClass = 0;
            int referencedObjectForCurrentClass = 0;
            double referencedDistance = FindSumOfDistances(SourceArray, currentClassFirstElement,
                   currentClassLastElement, 0);
            double currentReferencedDistance = 0;

            for (int i = 1; i < SourceArray.GetLength(0); i++)
            {
                // посчитать для первого элемента каждого класса отдельно посчитать дистанцию!
                //currentReferencedDistance = FindSumOfDistances(currentCoords, currentClassFirstElement, 
                //    currentClassLastElement);

                if ((currentReferencedDistance = FindSumOfDistances(SourceArray, currentClassFirstElement,
                   currentClassLastElement, i)) < referencedDistance)
                {
                    referencedDistance = currentReferencedDistance;
                    referencedObjectForCurrentClass = i;
                }

                referencedDistance = currentReferencedDistance < referencedDistance
                    ? currentReferencedDistance : referencedDistance;

                if (i == currentClassLastElement)
                {
                    _referencedObjects[numberOfCurrentClass] = referencedObjectForCurrentClass + 1;

                    if (i < SourceArray.GetLength(0) - 1)
                    {
                        numberOfCurrentClass++;
                        i++;
                        currentClassFirstElement = i;
                        currentClassLastElement += countOfClassObjects[numberOfCurrentClass];
                        referencedDistance = FindSumOfDistances(SourceArray, currentClassFirstElement,
                       currentClassLastElement, currentClassFirstElement);
                        referencedObjectForCurrentClass = currentClassFirstElement;
                    }
                    //в условии также считать расстояние до каждого первого объекта класса и потом в общий цикл
                }

            }
        }

        private double FindSumOfDistances(double[,] SourceArray, int classStartObject, int classEndObject, int currentObject)
        {
            double sumfOfDistances = 0;

            for (int i = classStartObject; i <= classEndObject; i++)
            {
                /*sumfOfDistances = Math.Sqrt(
                    Math.Pow((SourceArray[0, i] - SourceArray[0, currentObject]), 2)
                    + Math.Pow((SourceArray[1, i] - SourceArray[1, currentObject]), 2)
                    + Math.Pow((SourceArray[2, i] - SourceArray[2, currentObject]), 2)
                    );*/
                sumfOfDistances += SourceArray[currentObject, i];
            }

            return sumfOfDistances;
        }

        /// <summary>
        /// ПЕРЕПИСАТЬ ВЫШЕЛЕЖАЩУЮ ФУНКЦИЮ ТАК, ЧТОБЫ БЫЛ ЗНАК БОЛЬШЕ В ЦИКЛЕ!!!1111
        /// </summary>
        /// <param name="SourceArray"></param>
        /// <param name="classStartObject"></param>
        /// <param name="classEndObject"></param>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        private static double FindSumOfDistancesWORKING(double[,] SourceArray, int classStartObject, int classEndObject, int currentObject)
        {
            double sumfOfDistances = 0;

            for (int i = classStartObject; i < classEndObject; i++)
            {
                /*sumfOfDistances = Math.Sqrt(
                    Math.Pow((SourceArray[0, i] - SourceArray[0, currentObject]), 2)
                    + Math.Pow((SourceArray[1, i] - SourceArray[1, currentObject]), 2)
                    + Math.Pow((SourceArray[2, i] - SourceArray[2, currentObject]), 2)
                    );*/
                sumfOfDistances += SourceArray[currentObject, i];
            }

            return sumfOfDistances;
        }

        public int[] getReferencedObjects() { return _referencedObjects; }

        public List<string> getReferencedObjectsWithClassNames(List<string> ClassesNames)
        {
            List<string> ReferencedObjectsWithClassNames = new List<string>();

            for (int i = 0; i < _referencedObjects.Length; i++)
            {
                ReferencedObjectsWithClassNames.Add("Класс - " + ClassesNames[i] + ", № Эталона - " + _referencedObjects[i] + ".");
            }

            return ReferencedObjectsWithClassNames;
        }

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

            /*int objectsCount = sourceArray.GetLength(0);
            double[] mostRemoteObjects = new double[objectsCount];
            int[] mostRemoteObjectIndexes = new int[objectsCount];
            
            for(int i = 0; i < objectsCount; i++)
            {
                mostRemoteObjectIndexes[i] = i;
                mostRemoteObjects[i] = FindSumOfDistances(sourceArray, 0, objectsCount, i);
            }

            ReverseComparer reverseComparer = new ReverseComparer();

            System.Array.Sort(mostRemoteObjects, mostRemoteObjectIndexes, reverseComparer);

            int[] returnedIndexes = mostRemoteObjectIndexes.SubArray<int>(0, countOfObjects);

            return returnedIndexes;*/
        }

        private static int GetMaxColumnValueIndex(double[,] sourceArray, int column, int columnCount)
        {
            double value = Double.MinValue;
            int index = -1;

            for (int i = 0; i< columnCount; i++)
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
                if(firstIndex == i)
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
    }
}
