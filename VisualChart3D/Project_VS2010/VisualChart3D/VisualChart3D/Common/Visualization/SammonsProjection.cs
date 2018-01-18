using System;
using System.Collections.Generic;

namespace VisualChart3D.Common.Visualization
{
    public interface ISammon : IVisualizer
    {
        double Criteria { get; set; }
        double MinStep { get; set; }
        double IterationStep { get; set; }
        double MaxStep { get; set; }
        List<double> CalculatedCriteria { get; }
    }
    public class SammonsProjection : ISammon
    {
        private const int MaxAvaibleDimension = 3;
        private const string VisualizationErrorFormat = "Ошибка при работе алгоритма визуализации методом Сэммона: {0}";

        private double _e = 5.0;
        private double _minStep = 0.001;
        private double _iterationStep = 2.0;
        private double _maxStep = 10.0;

        private int _dimensions;
        private int _countOfObjects;

        private double[,] _projection;
        private double[,] _distMatrix;

        private List<Double> _calculatedCriteria;

        public SammonsProjection(int dimensions, double[,] distMatrix)
        {
            _calculatedCriteria = new List<Double>();
            _distMatrix = distMatrix;
            _dimensions = dimensions;
            if (_dimensions < 1)
            {
                throw new ArgumentException("Некорректное значение числа размерностей");
            }
        }

        private double evaluateCriteria(double[,] dm, double[,] distmatrix, double sum2dist)
        {
            double result = 0.0D;
            int dimensions = _dimensions;

            for (int i = 0; i < _countOfObjects; i++)
            {
                for (int j = i + 1; j < _countOfObjects; j++)
                {
                    double sum = 0.0D;
                    double dist = distmatrix[i, j];

                    for (int k = 0; k < dimensions; k++)
                    {
                        sum += (dm[i, k] - dm[j, k]) * (dm[i, k] - dm[j, k]);
                    }

                    sum = Math.Sqrt(sum);

                    result += (sum - dist) * (sum - dist);
                }
            }

            result /= sum2dist;
            _calculatedCriteria.Add(result);
            return result;
        }

        private double[] evaluateGradient(int index, double[,] dm, double[,] distmatrix, double sum2dist)
        {
            int dimensions = _dimensions;
            double[] result = new double[dimensions];

            for (int i = 0; i < _countOfObjects; i++)
            {
                if (i != index)
                {
                    double sum = 0.0D;
                    double dist = distmatrix[index, i];

                    for (int k = 0; k < dimensions; k++)
                    {
                        sum += (dm[i, k] - dm[index, k]) * (dm[i, k] - dm[index, k]);
                    }

                    sum = Math.Sqrt(sum);

                    for (int k = 0; k < dimensions; k++)
                    {
                        result[k] = ((dm[index, k] - dm[i, k]) * (sum - dist) / sum);
                    }
                }
            }

            for (int k = 0; k < dimensions; k++)
            {
                result[k] = (result[k] * _iterationStep / sum2dist);
                //result[k] = (result[k] * 2.0D / sum2dist);
            }

            return result;
        }

        public int Dimensions { get => _dimensions; }

        public int MaximumDimensionsNumber { get => MaxAvaibleDimension; }

        public double[,] Projection { get => _projection; }

        public double Criteria { get => _e; set => _e = value; }
        public double MinStep { get => _minStep; set => _minStep = value; }
        public double IterationStep { get => _iterationStep; set => _iterationStep = value; }
        public double MaxStep { get => _maxStep; set => _maxStep = value; }
        public double[,] DistMatrix { get => _distMatrix; set => _distMatrix = value; }

        public List<double> CalculatedCriteria => _calculatedCriteria;

        public void ToProject()
        {
            _countOfObjects = _distMatrix.GetLength(0);
            _calculatedCriteria.Clear();

            try
            {
                double[,] dm = new double[_countOfObjects, _dimensions];
                for (int i = 0; i < _dimensions; i++)
                {
                    for (int j = 0; j < _countOfObjects; j++)
                    {
                        dm[j, i] = _distMatrix[j, i];
                    }
                }

                double sum2dist = 0.0;
                for (int i = 0; i < _countOfObjects; i++)
                {
                    for (int j = i + 1; j < _countOfObjects; j++)
                    {
                        //double dist = distMatrix[i, j];
                        sum2dist += Math.Pow(_distMatrix[i, j], 2);
                    }
                }

                double criteria = evaluateCriteria(dm, _distMatrix, sum2dist);

                int counter = 0;
                double step = _maxStep;

                while ((counter < _countOfObjects) && (criteria > _e))
                //while (criteria > _e)
                //for (; criteria > _e; (i < N) && (criteria > _e))
                {
                    //i = 0; continue;

                    double[] gradient = evaluateGradient(counter, dm, _distMatrix, sum2dist);

                    while (step > _minStep)
                    {

                        for (int k = 0; k < _dimensions; k++)
                        {
                            dm[counter, k] = dm[counter, k] - step * gradient[k];
                        }

                        double newcriteria = evaluateCriteria(dm, _distMatrix, sum2dist);
                        if (newcriteria < criteria)
                        {
                            criteria = newcriteria;
                            break;
                        }

                        for (int k = 0; k < _dimensions; k++)
                        {
                            dm[counter, k] = dm[counter, k] + step * gradient[k];
                        }

                        step /= _iterationStep;
                    }

                    step = _maxStep;
                    counter++;
                }

                _projection = dm;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format(VisualizationErrorFormat, ex.StackTrace));
            }
        }
    }
}
