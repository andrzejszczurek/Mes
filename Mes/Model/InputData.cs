using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mes.Model
{
   public class InputData
   {
      public double S { get; set; }

      public double K { get; set; }

      public double L { get; set; }

      public int Nh { get; set; }

      public double Alpha { get; set; }

      public double Q { get; set; }

      public double TempOtoczenia { get; set; }

      public static InputData GetFromFile()
      {
         var data = new List<double>();

         var filepath = AppDomain.CurrentDomain.BaseDirectory + "../../";
         var fileName = "dane.txt";

         using (StreamReader sr = new StreamReader(filepath + fileName))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
               data.Add(int.Parse(line));
            }
         }

         var inputData = new InputData()
         {
            S = data[0],
            K = data[1],
            L = data[2],
            Nh = (int)data[3],
            Alpha = data[4],
            Q = data[5],
            TempOtoczenia = data[6],
         };

         return inputData;
      }

   }
}
