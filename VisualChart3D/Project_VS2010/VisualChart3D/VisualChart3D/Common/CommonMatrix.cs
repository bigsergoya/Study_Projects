// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace VisualChart3D.Common
{
    static class CommonMatrix
    {
        private const string ChangingMinkovskiDegreeErrorMessage = "Ошибка задания порядка расстояния Минковского";
        private const string BadDataFileNameErrorMessage = "Ошибка чтения файла по имени {0}";
        private const string EmptyFileErrorMessage = "Файл не должен быть пустым";
        private const string BadFileStructureErrorMessage = "Некорректная структура файла";

        /// <summary>
        /// Считывание матрицы расстояний
        /// </summary>
        /// <param name="fileName">файл с матрицей расстояний</param>
        /// <returns>массив с матрицей расстояний</returns>
        public static double[,] ReadMatrixDistance(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(String.Format(BadDataFileNameErrorMessage, fileName));
            }

            string[] sourceData = File.ReadAllLines(fileName);
            double[,] result = new double[sourceData.Length, sourceData.Length];
            CultureInfo cult = new CultureInfo("en-US");

            for (int i = 0; i < sourceData.Length; i++)
            {
                string[] values = sourceData[i].Replace(',', '.').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < values.Length; j++)
                {
                    result[i, j] = double.Parse(values[j], cult);
                }
            }

            return result;
        }

        /// <summary>
        /// Считывание матрицы признаков
        /// </summary>
        /// <param name="fileName">файл с матрицей признаков</param>
        /// <returns>массив с матрицей признаков</returns>
        public static double[,] ReadMatrixAttribute(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(String.Format(BadDataFileNameErrorMessage, fileName));
            }

            string[] sourceData = File.ReadAllLines(fileName);
            string[] temp = sourceData[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int colCount = temp.Length;
            double[,] result = new double[sourceData.Length, colCount];
            CultureInfo cult = new CultureInfo("en-US");

            for (int i = 0; i < sourceData.Length; i++)
            {
                string[] values = sourceData[i].Replace(',', '.').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < colCount; j++)
                {
                    result[i, j] = double.Parse(values[j], cult);
                }
            }
            return result;
        }

        /// <summary>
        /// Считывание матрицы классов "Один-к-кодному"
        /// </summary>
        /// <param name="fileName">файл с матрицей классов "Один-к-кодному"</param>
        /// <returns>массив классов</returns>
        public static string[] ReadMatrixOneToOne(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(String.Format(BadDataFileNameErrorMessage, fileName));
            }

            string[] sourceDataRead = File.ReadAllLines(fileName);
            string[] sourceData = sourceDataRead.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (sourceData.Length == 0 || String.IsNullOrEmpty(sourceData[0]))
            {
                throw new FormatException(EmptyFileErrorMessage);
            }

            return sourceData;
        }

        /// <summary>
        /// Считывание матрицы классов "Число объектов класса"
        /// </summary>
        /// <param name="fileName">файл с матрицей классов "Число объектов класса"</param>
        /// <returns>массив классов</returns>
        public static string[,] ReadMatrixCountObj(string fileName)
        {
            int MaxColCount = 2;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(String.Format(BadDataFileNameErrorMessage, fileName));
            }

            string[] sourceData = File.ReadAllLines(fileName);
            string[] temp = sourceData[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int colCount = temp.Length;

            if (colCount > MaxColCount)
            {
                throw new FormatException(BadFileStructureErrorMessage);
            }

            string[,] result = new string[sourceData.Length, 2];

            for (int i = 0; i < sourceData.Length; i++)
            {
                string[] values = sourceData[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                result[i, 0] = values[0];
                if (values.Length == 2)
                {
                    result[i, 1] = values[1];
                }
            }

            return result;
        }

        /*/// <summary>
        /// Преобразование матрицы признаков в матрицу расстояний
        /// </summary>
        /// <param name="matr">матрица признаков</param>
        /// <param name="countRow">количество строк в матрице признаков</param>
        /// <returns>матрица расстояний</returns>
        public static double[,] ObjectAttributeToDistance(double[,] matr, int countRow)
        {
            int countColumn = matr.Length / countRow;
            double[,] result = new double[countRow, countRow];

            for (int i = 0; i < countRow; i++)
            {
                for (int j = 0; j < countRow; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = 0;
                    }
                    else
                    {
                        double temp = 0;

                        for (int k = 0; k < countColumn; k++)
                        {
                            temp += Math.Pow(matr[i, k] - matr[j, k], 2);
                        }

                        temp = Math.Sqrt(temp);
                        result[i, j] = result[j, i] = temp;
                    }
                }
            }
            return result;
        }
        */

        /// <summary>
        /// Преобразование матрицы признаков в матрицу расстояний
        /// </summary>
        /// <param name="matr">матрица признаков</param>
        /// <param name="countRow">количество строк в матрице признаков</param>
        /// <returns>матрица расстояний</returns>
        public static double[,] ObjectAttributeToDistance(double[,] matr, int countRow, int minkovskiDegree)
        {
            if (minkovskiDegree == 0)
            {
                throw new ArgumentException(ChangingMinkovskiDegreeErrorMessage);
            }

            int countColumn = matr.Length / countRow;
            double[,] result = new double[countRow, countRow];

            for (int i = 0; i < countRow; i++)
            {
                for (int j = 0; j < countRow; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = 0;
                    }
                    else
                    {
                        double temp = 0;

                        for (int k = 0; k < countColumn; k++)
                        {
                            temp += Math.Pow(Math.Abs(matr[i, k] - matr[j, k]), minkovskiDegree);
                        }

                        temp = Math.Pow(temp, 1d / minkovskiDegree);
                        result[i, j] = result[j, i] = temp;
                    }
                }
            }

            return result;
        }

    }
}
