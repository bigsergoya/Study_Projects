using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualChart3D.Common.Visualization
{
    public interface IVisualizer
    {
        int Dimensions { get; }
        double[,] DistMatrix { get; set; }
        double[,] Projection { get; }

        void ToProject();
        int MaximumDimensionsNumber { get; }
    }
}
