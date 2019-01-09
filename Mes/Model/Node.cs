using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes.Model
{
   public enum BoundaryCondition
   {
      None,
      Stream,
      Convection
   }

   public class Node
   {
      public BoundaryCondition Bc { get; set; }

      public double X { get; set; }

      public Node(double x, BoundaryCondition bc)
      {
         x = X;
         Bc = bc;
      }

   }
}
