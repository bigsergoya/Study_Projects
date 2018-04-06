using System;
using System.Linq;

namespace VisualChart3D.Common.Visualization
{
    /// <summary>
    /// Represents a nonlinear projection implemented as Sammon's Mapping.
    /// </summary>
    /// <remarks>
    ///	<para>
    ///	As distance-measure the so called Manhattan-distance is used.
    /// </para>
    /// </remarks>
    [Serializable]
    public class KohonenProjection
    {
        const string BadInputMessage = "Ошибка исходных данных в методе Kohonen Mapping";
        const int IterationsLimit = 10000;
        const int StartIterations = 100;

        #region Fields
        private int _iterationsCount;
        private double _lambda = 1;     // 1 - Start value
        private int[] _indexesI;
        private int[] _indexesJ;
        private ITimer _timer;

        /// <summary>
        /// Current iteration
        /// </summary>
        private int _iteration;

        /// <summary>
        /// The precalculated distance-matrix.
        /// </summary>
        private double[][] _distanceMatrix;
        #endregion

        #region Properties
        /// <summary>
        /// The number of input-vectors.
        /// </summary>
        public int Count => this._distanceMatrix.Length;

        /// <summary>
        /// The dimension in that the projection should be performed.
        /// </summary>
        public int OutputDimension { get; protected set; }

        /// <summary>
        /// The projected vectors.
        /// </summary>
        public double[][] Projection { get; protected set; }

        /// <summary>
        /// The number of iterations.
        /// </summary>
        public int IterationsCount { get => _iterationsCount; set => _iterationsCount = value; }

        public int MaxIterations { get => IterationsLimit; } 

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of Sammon's Mapping.
        /// </summary>
        /// <param name="inputData">The input-vectors.</param>
        /// <param name="outputDimension">The dimension of the projection.</param>
        /// <param name="iterationsCount">
        /// Maximum number of iterations. For a statistical acceptable accuracy
        /// this should be 10e4...1e5 times the number of points. It has shown
        /// that a few iterations (100) yield a good projection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name=">inputVectors"/> is <c>null</c>.
        /// </exception>
        public KohonenProjection(
            double[][] inputData,
            int outputDimension,
            int iterationsCount = StartIterations)
        {
            if (inputData == null || inputData.Length == 0)
            {
                throw new ArgumentNullException(BadInputMessage);
            }

            _timer = new CustomTimer();

            _distanceMatrix = inputData;
            this.OutputDimension = outputDimension;
            _iterationsCount = iterationsCount;

            // Initialize the projection:
            Initialize();

            // Create the index-arrays:
            _indexesI = Enumerable.Range(0, this.Count).ToArray();
            _indexesJ = new int[this.Count];
            _indexesI.CopyTo(_indexesJ, 0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Runs all the iterations and thus create the mapping.
        /// </summary>
        public void CreateMapping()
        {
            _timer.Start();

            for (int i = _iterationsCount; i >= 0; i--)
            {
                this.Iterate();
            }

            _timer.Stop();
        }

        /// <summary>
        /// Performs one iteration of the (heuristic) algorithm.
        /// </summary>
        public void Iterate()
        {
            int[] indexI = _indexesI;
            int[] indexJ = _indexesJ;
            double[][] distanceMatrix = _distanceMatrix;
            double[][] projection = this.Projection;

            // Shuffle the indices-array for random pick of the points:
            indexI.FisherYatesShuffle();
            indexJ.FisherYatesShuffle();

            for (int i = 0; i < indexI.Length; i++)
            {
                //Столбец матрицы расстояний для индекса[i]
                double[] distancesI = distanceMatrix[indexI[i]];

                //Строка - проекция для индекса[i]
                double[] projectionI = projection[indexI[i]];

                for (int j = 0; j < indexJ.Length; j++)
                {
                    if (indexI[i] == indexJ[j])
                    {
                        continue;
                    }

                    double[] projectionJ = projection[indexJ[j]];

                    double dij = distancesI[indexJ[j]];
                    double Dij = Utils.ManhattenDistance(
                            projectionI,
                            projectionJ);

                    // Avoid division by zero:
                    if (Dij == 0)
                    {
                        Dij = 1e-10;
                    }

                    double delta = _lambda * (dij - Dij) / Dij;

                    for (int k = 0; k < projectionJ.Length; k++)
                    {
                        double correction = delta * (projectionI[k] - projectionJ[k]);

                        projectionI[k] += correction;
                        projectionJ[k] -= correction;
                    }
                }
            }

            // Reduce lambda monotonically:
            ReduceLambda();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the algorithm.
        /// </summary>
        private void Initialize()
        {
            // Initialize random points for the projection:
            Random rnd = new Random();
            double[][] projection = new double[this.Count][];
            this.Projection = projection;

            for (int i = 0; i < projection.Length; i++)
            {
                double[] projectionI = new double[this.OutputDimension];
                projection[i] = projectionI;

                for (int j = 0; j < projectionI.Length; j++)
                {
                    projectionI[j] = rnd.Next(0, this.Count);
                }
            }
        }

        /// <summary>
        /// Reducing lambda depending on iterations.
        /// </summary>
        private void ReduceLambda()
        {
            _iteration++;

            double ratio = (double)_iteration / _iterationsCount;
                        
            _lambda = Math.Pow(0.1, ratio);
        }
        #endregion
    }
}
