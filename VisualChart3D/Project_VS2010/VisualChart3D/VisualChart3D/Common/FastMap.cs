// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace VisualChart3D
{
    internal class FastMap
    {
        /// <summary>
        /// Делегат функции, вычисляющая расстояние между элементами
        /// </summary>
        /// <param name="idx1">индекс первого элемента</param>
        /// <param name="idx2">инжекс второго элемент</param>
        /// <returns></returns>
        private delegate float Dist(int idx1, int idx2);

        /// <summary>
        /// Функция, вычисляющая расстояние между элементами
        /// </summary>
        private readonly Dist _distanceFunc;

        /// <summary>
        /// Матрица расстояний
        /// </summary>
        private readonly double[,] _distanceArr;

        /// <summary>
        /// Количество элементов
        /// </summary>
        private readonly int _countElements;

        /// <summary>
        /// Массив координат
        /// </summary>
        private double[,] _arrCoord;

        /// <summary>
        /// Для генерации случайного значения
        /// </summary>
        private readonly Random _rnd = new Random();

        /// <summary>
        /// Текущий столбец
        /// </summary>
        private int _currentColumn;

        /// <summary>
        /// Структура для хранения двух индексов
        /// </summary>
        private struct TwoElement
        {
            public TwoElement(int idxa, int idxb)
            {
                Idxa = idxa;
                Idxb = idxb;
            }

            public int Idxa;
            public int Idxb;
        }

	    private MetricsEnum _metric;


	    /// <summary>
	    /// Конструктор
	    /// </summary>
	    /// <param name="distanceArr">Матрица расстояний</param>
	    /// <param name="metric">Метрика</param>
	    public FastMap(double[,] distanceArr, MetricsEnum metric)
        {
            _distanceArr = distanceArr;
            _countElements = (int) Math.Sqrt(distanceArr.Length);
            _distanceFunc = (idx1, idx2) => (float) _distanceArr[idx1, idx2];
	        _metric = metric;
        }

        /// <summary>
        /// Вычислить координаты
        /// </summary>
        /// <param name="cntProjection">кол-во осей</param>
        /// <returns>Массив координат</returns>
        public double[,] GetCoordinates(int cntProjection)
        {
            _arrCoord = new double[_countElements, cntProjection];
            _currentColumn = 0;
            FastMapAlghoritm(cntProjection, _distanceFunc);
            return _arrCoord;
        }


        /// <summary>
        /// Реализация алгоритма FastMap
        /// </summary>
        /// <param name="k">кол-во проекций</param>
        /// <param name="dist">Функция для вычисления расстояния между элементами</param>
        private void FastMapAlghoritm(int k, Dist dist)
        {
            if (k <= 0)
                return;

            TwoElement two = ChoseDistanceObject(dist);
            if (Math.Abs(dist(two.Idxa, two.Idxb)) < 0.0001)
            {
                for (int i = 0; i < _countElements; i++)
                    _arrCoord[i, _currentColumn] = 0;
                return;
            }

            int a = two.Idxa;
            int b = two.Idxb;
            double[] xi = new double[_countElements];
            for (int i = 0; i < _countElements; i++)
            {
                float dai = dist(a, i);
                float dab = dist(a, b);
                float dbi = dist(b, i);
	            switch (_metric)
	            {
		            case MetricsEnum.Euclidean:
						xi[i] = ((Math.Pow(dai, 2) + Math.Pow(dab, 2) - Math.Pow(dbi, 2)) / (2 * dab));
			            break;
		            case MetricsEnum.NonEuclidean:
						xi[i] = (Math.Pow(dai, 2) - Math.Pow(dbi, 2)) / Math.Pow(dab,2);
			            break;
		            default:
			            throw new ArgumentOutOfRangeException();
	            }
	          
                _arrCoord[i, _currentColumn] = double.IsNaN(xi[i]) ? 0 : xi[i];
            }
            _currentColumn++;

            FastMapAlghoritm(k - 1,
                (o1, o2) => (float) Math.Sqrt(Math.Pow(dist(o1, o2), 2) - Math.Pow(xi[o1] - xi[o2], 2)));
        }

        /// <summary>
        /// Вычисляет опорные элементы
        /// </summary>
        /// <param name="dist">Функция для вычисления расстояния между элементами</param>
        /// <returns>индексы опорных элементов</returns>
        private TwoElement ChoseDistanceObject(Dist dist)
        {

            float max = 0;
            int a = 0, b = 0;
            for (int i = 0; i < _countElements; i++)
                for (int j = i; j < _countElements; j++)
                {
                    if (dist(i, j) > max)
                    {
                        max = dist(i, j);
                        a = i;
                        b = j;
                    }
                }
            return new TwoElement(a, b);
        }
    }
}
