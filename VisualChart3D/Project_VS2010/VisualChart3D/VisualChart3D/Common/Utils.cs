using System;

namespace VisualChart3D.Common
{
    internal static class Utils
    {
        private const string WarningMessageStandartTitle = "Внимание!";
        private const string ErrorMessageStandartTitle = "Ошибка!";

        //---------------------------------------------------------------------
        internal static double ManhattenDistance(double[] vec1, double[] vec2)
        {
            double distance = 0;

            for (int i = 0; i < vec1.Length; i++)
            {
                distance += Math.Abs(vec1[i] - vec2[i]);
            }

            return distance;
        }

        //---------------------------------------------------------------------
        internal static void FisherYatesShuffle<T>(this T[] array)
        {
            Random rnd = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                // Pick random positoin:
                int pos = rnd.Next(i + 1);

                // Swap:
                T tmp = array[i];
                array[i] = array[pos];
                array[pos] = tmp;
            }
        }

        internal static double[][] GetAnotherStyleOfData(double[,] array)
        {
            int firstDim = array.GetLength(0);
            int secondDim = array.GetLength(1);
            double[][] returnedArray = new double[firstDim][];

            for (int j = 0; j < firstDim; j++)
            {
                returnedArray[j] = new double[secondDim];
            }

            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    returnedArray[i][j] = array[i, j];
                }
            }

            return returnedArray;
        }

        internal static double[,] ExchangeData(double[][] oldArray, int firstDim, int secondDim)
        {
            double[,] outputArray = new double[firstDim, secondDim];
            //double maxValue = 0;

            /*for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    maxValue = oldArray[i][j]>maxValue? oldArray[i][j] :maxValue;
                }
            }*/

            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    outputArray[i, j] = oldArray[i][j]; /// maxValue;
                }
            }

            return outputArray;
        }

        internal static double[,] GetNormalizedData(double[,] array)
        {
            int firstDim = array.GetLength(0);
            int secondDim = array.GetLength(1);
            double[,] outputArray = new double[firstDim, secondDim];

            GetMinAndMaxByDimensions(array, firstDim, secondDim,
                       out double maxX, out double maxY, out double maxZ,
                       out double minX, out double minY, out double minZ);

            for (int i = 0; i < firstDim; i++)
            {
                //for (int j = 0; j < secondDim; j++)
                {
                    outputArray[i, 0] = (array[i, 0] - minX) / (maxX - minX);
                    outputArray[i, 1] = (array[i, 1] - minY) / (maxY - minY);
                    outputArray[i, 2] = (array[i, 2] - minZ) / (maxZ - minZ);
                    //outputArray[i, j] = oldArray[i][j] / maxValue;
                    //outputArray[i, j] = (oldArray[i][j] - minValue) / (maxValue - minValue);
                }
            }

            return outputArray;
        }

        /*internal static double[,] GetNormalizedData(double[,] array)
        {
            int firstDim = array.GetLength(0);
            int secondDim = array.GetLength(1);
            double[,] outputArray = new double[firstDim, secondDim];

            GetMinAndMax(array, firstDim, secondDim, out double minValue, out double maxValue);

            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    outputArray[i, j] = (array[i, j] - minValue) / (maxValue - minValue);
                }
            }

            return outputArray;
        }*/

        /*internal static double[,] ExchangeDataMax(double[][] oldArray, int firstDim, int secondDim)
        {
            double[,] outputArray = new double[firstDim, secondDim];

            GetMinAndMax(oldArray, firstDim, secondDim, out double minValue, out double maxValue);

            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    //outputArray[i, j] = oldArray[i][j] / maxValue;
                    outputArray[i, j] = (oldArray[i][j] - minValue) / (maxValue - minValue);
                }
            }

            return outputArray;
        }*/

        internal static double[,] ExchangeDataByDim(double[][] oldArray, int firstDim, int secondDim)
        {
            double[,] outputArray = new double[firstDim, secondDim];

            //GetMinAndMax(oldArray, firstDim, secondDim, out double minValue, out double maxValue);

            GetMinAndMaxByDimensions(oldArray, firstDim, secondDim,
            out double maxX, out double maxY, out double maxZ,
            out double minX, out double minY, out double minZ);

            for (int i = 0; i < firstDim; i++)
            {
                //for (int j = 0; j < secondDim; j++)
                {
                    outputArray[i, 0] = (oldArray[i][0] - minX) / (maxX - minX);
                    outputArray[i, 1] = (oldArray[i][1] - minY) / (maxY - minY);
                    outputArray[i, 2] = (oldArray[i][2] - minZ) / (maxZ - minZ);
                    //outputArray[i, j] = oldArray[i][j] / maxValue;
                    //outputArray[i, j] = (oldArray[i][j] - minValue) / (maxValue - minValue);
                }
            }

            return outputArray;
        }

        internal static double[] GetLineOfMatrix(double[,] array, int lineIndex)
        {
            int lineLength = array.GetLength(1);
            double[] line = new double[lineLength];

            for (int j = 0; j < lineLength; j++)
            {
                line[j] = array[lineIndex, j];
            }

            return line;
        }

        internal static void ShowWarningMessage(string message, string title = WarningMessageStandartTitle)
        {
            System.Windows.Forms.MessageBox.Show(message,
                title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Warning);
        }


        internal static void ShowErrorMessage(string message, string title = ErrorMessageStandartTitle)
        {
            System.Windows.Forms.MessageBox.Show(message,
                title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }

        private static void GetMinAndMax(double[][] array, int firstDim, int secondDim, out double min, out double max)
        {
            max = array[0][0];
            min = array[0][0];
            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    max = array[i][j] > max ? array[i][j] : max;
                    min = array[i][j] < min ? array[i][j] : min;
                }
            }
        }

        public static void GetMinAndMax(double[,] array, int firstDim, int secondDim, out double min, out double max)
        {
            max = array[0, 0];
            min = array[0, 0];
            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0; j < secondDim; j++)
                {
                    max = array[i, j] > max ? array[i, j] : max;
                    min = array[i, j] < min ? array[i, j] : min;
                }
            }
        }

        public static void GetMinAndMaxByDimensions(double[,] array, int firstDim, int secondDim,
            out double maxX, out double maxY, out double maxZ,
            out double minX, out double minY, out double minZ)
        {
            //max = array[0, 0];
            //min = array[0, 0];
            maxX = array[0, 0];
            minX = array[0, 0];

            maxY = array[0, 1];
            minY = array[0, 1];

            maxZ = array[0, 2];
            minZ = array[0, 2];

            for (int i = 0; i < firstDim; i++)
            {
                //for (int j = 0; j < secondDim; j++)
                {
                    maxX = array[i, 0] > maxX ? array[i, 0] : maxX;
                    minX = array[i, 0] < minX ? array[i, 0] : minX;

                    maxY = array[i, 1] > maxY ? array[i, 1] : maxY;
                    minY = array[i, 1] < minY ? array[i, 1] : minY;

                    maxZ = array[i, 2] > maxZ ? array[i, 2] : maxZ;
                    minZ = array[i, 2] < minZ ? array[i, 2] : minZ;

                    //max = array[i, j] > max ? array[i, j] : max;
                    //min = array[i, j] < min ? array[i, j] : min;
                }
            }
        }

        public static void GetMinAndMaxByDimensions(double[][] array, int firstDim, int secondDim,
            out double maxX, out double maxY, out double maxZ,
            out double minX, out double minY, out double minZ)
        {
            //max = array[0, 0];
            //min = array[0, 0];
            maxX = array[0][0];
            minX = array[0][0];

            maxY = array[0][1];
            minY = array[0][1];

            maxZ = array[0][2];
            minZ = array[0][2];

            for (int i = 0; i < firstDim; i++)
            {
                //for (int j = 0; j < secondDim; j++)
                {
                    maxX = array[i][0] > maxX ? array[i][0] : maxX;
                    minX = array[i][0] < minX ? array[i][0] : minX;

                    maxY = array[i][1] > maxY ? array[i][1] : maxY;
                    minY = array[i][1] < minY ? array[i][1] : minY;

                    maxZ = array[i][2] > maxZ ? array[i][2] : maxZ;
                    minZ = array[i][2] < minZ ? array[i][2] : minZ;

                    //max = array[i, j] > max ? array[i, j] : max;
                    //min = array[i, j] < min ? array[i, j] : min;
                }
            }
        }
    }
}