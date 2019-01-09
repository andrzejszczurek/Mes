using Mes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes
{
   class Program
   {
      static void Main(string[] args)
      {
         var rawData = InputData.GetFromFile();
         var grid = Grid.Generate(rawData);
         var soe = Soe.Generate(grid);
         soe.TG = Soe.GaussianElimination(soe.HG, soe.PG);

         Console.WriteLine(soe.ToString());

         Console.ReadKey();
      }
   }
}
