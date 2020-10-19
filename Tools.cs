using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV;
using Emgu.CV.Structure;
namespace ISIP_Algorithms.Tools
{
    public class Tools
    {
        public static Image<Gray, byte> Invert(Image<Gray, byte> InputImage)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    Result.Data[y, x, 0] = (byte)(255 - InputImage.Data[y, x, 0]);
                }
            }
            return Result;
        }

        public static Image<Gray, byte> ChangeContrast(Image<Gray, byte> InputImage, int E, int m)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            double[] table = new double[256];
            double c = CalculateC(E, m);
            table = CalcLookUpTable(table, c, E, m);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    //if(InputImage.Data[y, x, 0] < 0)
                    //{
                    //    Result.Data[y, x, 0] = 0;
                    //} else if(InputImage.Data[y, x, 0] > 255)
                    //{
                    //    Result.Data[y, x, 0] = 255;
                    //} else
                    {
                        Console.WriteLine(FinalValue(table, InputImage.Data[y, x, 0]));
                        Result.Data[y, x, 0] = Convert.ToByte(FinalValue(table, InputImage.Data[y, x, 0]));
                    }

                }
            }
            return Result;
        }

        public static double[] CalcLookUpTable(double[] table, double c, float E, float m)
        {
            for (int i = 0; i < table.Length; i++)
            {
                Console.WriteLine(255 * Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c * i);
                table[i] = (255 * Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c * i);
            }
            return table;
        }

        public static double CalculateC(double E, double m)
        {
            return ((1 - (Math.Pow(255, E) / (Math.Pow(255, E) + Math.Pow(m, E)))) / 255);
        }

        public static double FinalValue(double[] table, byte pixel)
        {
            //  Console.WriteLine(table[pixel]);
            return table[pixel];
        }
    }
}
