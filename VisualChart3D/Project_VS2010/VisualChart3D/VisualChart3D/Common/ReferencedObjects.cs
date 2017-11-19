// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualChart3D
{
    class ReferencedObjects
    {
        private int[] referencedObjects; 
        public ReferencedObjects (double[,] SourceArray, int[] countOfClassObjects)
        { //+= countOfClassObjects[f] после первого конца класса. Первый конец - нулевой элемент.
            referencedObjects = new int[countOfClassObjects.Length];
            int currentClassLastElement = countOfClassObjects[0]-1;
            int currentClassFirstElement = 0;
            int numberOfCurrentClass = 0;
            int referencedObjectForCurrentClass = 0;
            double referencedDistance = FindSumOfDistances(SourceArray, currentClassFirstElement,
                   currentClassLastElement, 0);
            double currentReferencedDistance = 0;
            for (int i = 1; i < SourceArray.GetLength(0); i++)
            {  // посчитать для первого элемента каждого класса отдельно посчитать дистанцию!
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
                    referencedObjects[numberOfCurrentClass] = referencedObjectForCurrentClass + 1;
                    if (i < SourceArray.GetLength(0)-1)
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
                sumfOfDistances += SourceArray[currentObject,i];
            }
            return sumfOfDistances;
        }
        public int[] getReferencedObjects() { return referencedObjects; }
        public List<string> getReferencedObjectsWithClassNames(List<string> ClassesNames) { 
            List<string> ReferencedObjectsWithClassNames = new List<string>();
            for (int i = 0; i < referencedObjects.Length; i++)
            {
                ReferencedObjectsWithClassNames.Add("Класс - " + ClassesNames[i] + ", № Эталона - " + referencedObjects[i]+".");
            }
            return ReferencedObjectsWithClassNames;
        }
    }
}
