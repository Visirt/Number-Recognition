using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Number_Recognition.Models
{
    public class NeuralNetwork
    {
        private Matrix<double>[] values { get; set; }
        
        public NeuralNetwork(int[] numNodes)
        {
            Matrix<double>[] matrices = new Matrix<double>[numNodes.Length -1];
            for(int i = 0; i < matrices.Length; i++)
            {
                int prevNodes = numNodes[i] + 1;
                int nextNodes = numNodes[i + 1];

                matrices[i] = Matrix<double>.Build.Random(nextNodes, prevNodes);
            }

            this.values = matrices;
        }

        public NeuralNetwork(Matrix<double>[] other)
        {
            this.values = new Matrix<double>[other.Length];
            
            for(int i = 0; i < this.values.Length; i++)
            {
                this.values[i] = other[i].Clone();
            }
        }

        public NeuralNetwork(NeuralNetwork other)
            : this(other.values)
        {}

        public Matrix<double> forwardPropagate(Matrix<double> inputs)
        {
            Matrix<double> prevMatrix = null;
            Matrix<double> nextMatrix = inputs;
            foreach(var values in this.values)
            {
                prevMatrix = nextMatrix.InsertRow(prevMatrix.RowCount,Vector<double>.Build.Dense(prevMatrix.ColumnCount, 1.0));
                var aux = values.Multiply(prevMatrix);
                nextMatrix = sigmoid(aux);
            }
            return nextMatrix;
        }

        private Matrix<double> sigmoid(Matrix<double> input)
        {
            return input.Multiply(-1.0).PointwiseExp().Add(1).DivideByThis(1.0);
        }
    }
}
