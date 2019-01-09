using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes.Model
{
   public class Soe
   {
      /// <summary>
      /// Globalne H
      /// </summary>
      public double[][] HG { get; set; }

      /// <summary>
      /// Globalne P
      /// </summary>
      public double[] PG { get; set; }

      /// <summary>
      /// Globalne t
      /// </summary>
      public double[] TG { get; set; }

      private readonly Grid m_grid;

      private Soe(Grid grid)
      {
         m_grid = grid;
         var nh = m_grid.Nodes.Length;
         HG = new double[nh][];
         for (int i = 0; i < HG.Length; i++)
            HG[i] = new double[nh];
         PG = new double[nh];
         TG = new double[nh];
      }

      public static Soe Generate(Grid grid)
      {
         var g = new Soe(grid);
         g.GenerateGlobalMatrixH();
         g.GenerateGlobalMatrixL();
         return g;
      }

      private void GenerateGlobalMatrixH()
      {
         for (int i = 0; i < m_grid.Elements.Length; i++)
         {
            var element = m_grid.Elements[i];

            HG[element.ID[0]][element.ID[0]] += element.HL[0][0];
            HG[element.ID[0]][element.ID[1]] += element.HL[0][1];
            HG[element.ID[1]][element.ID[0]] += element.HL[1][0];
            HG[element.ID[1]][element.ID[1]] += element.HL[1][1];
         }
      }

      private void GenerateGlobalMatrixL()
      {
         for (int i = 0; i < m_grid.Elements.Length; i++)
         {
            var element = m_grid.Elements[i];
            if (m_grid.Nodes[element.ID[0]].Bc == BoundaryCondition.Stream)
            {
               PG[0] += element.PL[0];
               PG[1] += element.PL[1];
            }
            if (m_grid.Nodes[element.ID[1]].Bc == BoundaryCondition.Convection)
            {
               var nh = m_grid.Nodes.Length;
               PG[nh-2] += element.PL[0];
               PG[nh-1] += element.PL[1];
            }

         }
      }

      /// <summary>
      /// Metoda pochodzi ze strony: https://introcs.cs.princeton.edu/java/95linear/GaussianElimination.java.html?fbclid=IwAR19Izvqr1KWLzxtUgqIX1xcsD56APAQm9CEJcPVjgpzgasahsJURMAMdWc
      /// </summary>
      public static double[] GaussianElimination(double[][] HG, double[] PG)
      {
         var b = new double[PG.Length];
         Array.Copy(PG, b, PG.Length);

         // potrzebne, żeby nie modyfikować orginalnych tablic
         var A = new double[HG.Length][];
         for (int i = 0; i < HG.Length; i++)
            A[i] = new double[HG[i].Length];

         for (int i = 0; i < HG.Length; i++)
            for (int j = 0; j < HG[i].Length; j++)
               A[i][j] = HG[i][j];

         const double EPSILON = 1e-10;
         int n = b.Length;

         for (int p = 0; p < n; p++)
         {

            // find pivot row and swap
            int max = p;
            for (int i = p + 1; i < n; i++)
            {
               if (Math.Abs(A[i][p]) > Math.Abs(A[max][p]))
               {
                  max = i;
               }
            }
            double[] temp = A[p]; A[p] = A[max]; A[max] = temp;
            double t = b[p]; b[p] = b[max]; b[max] = t;

            // singular or nearly singular
            if (Math.Abs(A[p][p]) <= EPSILON)
            {
               throw new ArithmeticException("Matrix is singular or nearly singular");
            }

            // pivot within A and b
            for (int i = p + 1; i < n; i++)
            {
               double alpha = A[i][p] / A[p][p];
               b[i] -= alpha * b[p];
               for (int j = p; j < n; j++)
               {
                  A[i][j] -= alpha * A[p][j];
               }
            }
         }
         // back substitution
         double[] x = new double[n];
         for (int i = n - 1; i >= 0; i--)
         {
            double sum = 0.0;
            for (int j = i + 1; j < n; j++)
            {
               sum += A[i][j] * x[j];
            }
            x[i] = (b[i] - sum) / A[i][i];
         }
         return x;
      }

      public override string ToString()
      {
         var result = new StringBuilder();

         result.AppendLine("Macierz H:");

         for (int i = 0; i < HG.Length; i++)
         {
            for (int j = 0; j < HG[i].Length; j++)
            {
               result.Append(HG[i][j] + " " );
            }
            result.AppendLine();
         }

         result.AppendLine("\nMacierz L:");

         for (int i = 0; i < PG.Length; i++)
         {
            result.AppendLine(PG[i] + " ");
         }

         result.AppendLine("\nMacierz wynikowa TG:");

         for (int i = 0; i < TG.Length; i++)
         {
            result.AppendLine(TG[i] + " ");
         }
         return result.ToString();
      }

   }
}
