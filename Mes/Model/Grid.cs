using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes.Model
{
   public class Grid
   {
      /// <summary>
      /// Liczba elementów
      /// </summary>
      public Element[] Elements { get; set; }

      /// <summary>
      /// Liczba węzłów
      /// </summary>
      public Node[] Nodes { get; set; }

      private readonly InputData m_data;

      private readonly int ne;

      private Grid(InputData data)
      {
         m_data = data;
         ne = m_data.Nh - 1; // liczba elementów
      }

      public static Grid Generate(InputData data)
      {
         var grid = new Grid(data);
         grid.GenerateNodes();
         grid.GenerateElements();
         return grid;
      }

      private void GenerateNodes()
      {
         var nh = m_data.Nh; // liczba węzłów
         var deltaX = 0d;
         Nodes = new Node[nh];
         for (int i = 0; i < Nodes.Length; i++)
         {
            var bc = i == 0 ? BoundaryCondition.Stream : i+1 == nh ? BoundaryCondition.Convection : BoundaryCondition.None;
            Nodes[i] = new Node(deltaX, bc);
            deltaX += m_data.L / ne;
         }
      }

      private void GenerateElements()
      {
         Elements = new Element[ne];
         for (int i = 0; i < Elements.Length; i++)
         {
            var element = new Element
            {
               ID = new int[] { i, i + 1 },
               S = m_data.S,
               K = m_data.K,
               L = m_data.L / ne
            };

            var C = (element.S * element.K) / element.L;

            element.HL[0][0] = C;
            element.HL[0][1] = -C;
            element.HL[1][0] = -C;
            element.HL[1][1] = C;

            // dla ostatniego elementu uwzględniamy warunek brzegowy konwekcji
            if (Nodes[element.ID[1]].Bc == BoundaryCondition.Convection)
            {
               var bc = m_data.Alpha * element.S;
               element.HL[1][1] += bc;
            }

            // obliczamy macierz L jeżeli występuje warunek brzegowy
            if (Nodes[element.ID[0]].Bc == BoundaryCondition.Stream)
            {
               var qS = m_data.Q * element.S;
               element.PL[0] = qS;
               element.PL[1] = 0;
            }
            else if (Nodes[element.ID[1]].Bc == BoundaryCondition.Convection)
            {
               var alfaTempOtoczS = -m_data.Alpha * m_data.TempOtoczenia * element.S;
               element.PL[0] = 0;
               element.PL[1] = alfaTempOtoczS;
            }
            else
            {
               element.PL[0] = 0;
               element.PL[1] = 0;
            }
            Elements[i] = element;
         }
      }

   }
}
