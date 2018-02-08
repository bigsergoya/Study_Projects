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
        const string BadInputMessage = "Ошибка исходных данных в методе Sammons Mapping";

        #region Fields
        private int _maxIteration;
        private double _lambda = 1;     // 1 - Start value
        private int[] _indexesI;
        private int[] _indexesJ;

        /// <summary>
        /// The precalculated distance-matrix.
        /// </summary>
        private double[][] _distanceMatrix;
        #endregion

        #region Properties
        /// <summary>
        /// The input-data.
        /// </summary>
        //public double[][] InputData { get; protected set; }

        /// <summary>
        /// The number of input-vectors.
        /// </summary>
        public int Count {
            //get { return this.InputData.Length; }
            get { return this._distanceMatrix.Length; }
        }

        /// <summary>
        /// The dimension in that the projection should be performed.
        /// </summary>
        public int OutputDimension { get; protected set; }

        /// <summary>
        /// Type of input matrix. True - distanceMarix, False - no.
        /// </summary>
        //private SourceFileMatrix _matrixType;

        /// <summary>
        /// The projected vectors.
        /// </summary>
        public double[][] Projection { get; protected set; }

        /// <summary>
        /// The number of iterations.
        /// </summary>
        public int MaxIterations { get => _maxIteration; set => _maxIteration = value; }

        /// <summary>
        /// Current iteration
        /// </summary>
        private int _iteration;
        #endregion

        #region Constructor
        /*/// <summary>
        /// Creates a new instance of Sammon's Mapping.
        /// </summary>
        /// <param name="inputData">The input-vectors.</param>
        /// <param name="outputDimension">The dimension of the projection.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name=">inputVectors"/> is <c>null</c>.
        /// </exception>
        public SammonsProjection(double[][] inputData, int outputDimension)
			: this(inputData, outputDimension, inputData.Length * (int)1e4) { }*/

        /// <summary>
        /// Creates a new instance of Sammon's Mapping.
        /// </summary>
        /// <param name="inputData">The input-vectors.</param>
        /// <param name="outputDimension">The dimension of the projection.</param>
        /// <param name="maxIteration">
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
            int maxIteration)
        {
            if (inputData == null || inputData.Length == 0)
            {
                throw new ArgumentNullException(BadInputMessage);
            }

            //-----------------------------------------------------------------
            _distanceMatrix = inputData;
            //this.InputData = inputData;
            this.OutputDimension = outputDimension;
            _maxIteration = maxIteration;
            //_matrixType = matrixType;
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
            for (int i = _maxIteration; i >= 0; i--)
            {
                this.Iterate();
            }
        }

        //---------------------------------------------------------------------
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
                        //Dij = Double.MinValue;
                    }


                    double delta = _lambda * (dij - Dij) / Dij;

                    for (int k = 0; k < projectionJ.Length; k++)
                    {
                        double correction =
                            delta * (projectionI[k] - projectionJ[k]);

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

            double ratio = (double)_iteration / _maxIteration;

            // Start := 1, End := 0.01
            _lambda = Math.Pow(0.1, ratio);
        }
        #endregion
    }
}
