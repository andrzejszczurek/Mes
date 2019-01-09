using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes.Model
{
   public class Element
   {
      /// <summary>
      /// Identyfikatory węzłów w elemencie 
      /// </summary>
      public int[] ID { get; set; } = new int[2];

      /// <summary>
      /// 
      /// </summary>
      public double S { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public double K { get; set; }

      /// <summary>
      /// Długość elementu
      /// </summary>
      public double L { get; set; }

      /// <summary>
      /// Lokalna macierz H
      /// </summary>
      public double[][] HL{ get; set; }

      /// <summary>
      /// Lokalna macierz P
      /// </summary>
      public double[] PL { get; set; }

      public Element()
      {
         HL = new double[2][] { new double[2], new double[2] };
         PL = new double[2];
      }


   }
}
