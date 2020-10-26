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


        // Tema 2

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

        // Tema 3

        public static Image<Gray, byte> Binarizare(Image<Gray, byte> InputImage, int T1, int T2)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] >= (byte)T1 && InputImage.Data[y, x, 0] <= (byte)T2)
                    {
                        Result.Data[y, x, 0] = (byte)(255);
                    }
                    else
                    {
                        Result.Data[y, x, 0] = (byte)(0);
                    }

                }
            }
            return Result;
        }

        public static Image<Gray, byte> Binarizare(Image<Gray, byte> InputImage, double T)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] >= T)
                    {
                        Result.Data[y, x, 0] = (byte)(255);
                    }
                    else
                    {
                        Result.Data[y, x, 0] = (byte)(0);
                    }

                }
            }
            return Result;
        }

        public static double Thresholding(Image<Gray, byte> InputImage)
        {
            double T = 0; // prag
            double lastT = T + 1;
            int counter = 0;
            T = Mid_range(InputImage);
            List<int> C1 = new List<int>();
            List<int> C2 = new List<int>();
            double m1 = 0;
            double m2 = 0;

            //List<int> test = new List<int> { 1, 1, 1, 1, 10 };
            //Console.WriteLine(test.Average());

            while (T != lastT && counter < 5000) 
            {
                counter++;
                Console.WriteLine(counter);
                for (int y = 0; y < InputImage.Height; y++)
                {
                    for (int x = 0; x < InputImage.Width; x++)
                    {
                        if (InputImage.Data[y, x, 0] > T)
                        {
                            C1.Add(InputImage.Data[y, x, 0]);
                        }
                        else if (InputImage.Data[y, x, 0] <= T)
                        {
                            C2.Add(InputImage.Data[y, x, 0]);
                        }
                    }
                }

                m1 = C1.Average();
                m2 = C2.Average();
                C1.Clear();
                C2.Clear();

                lastT = T;
                T = (m1 + m2) / 2;
            }

            return T;
        }

        private static byte Mid_range(Image<Gray, byte> InputImage)
        {
            byte max = 0;
            byte min = 255;

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] > max)
                    {
                        max = InputImage.Data[y, x, 0];
                    }
                    else if (InputImage.Data[y, x, 0] < min)
                    {
                        min = InputImage.Data[y, x, 0];
                    }
                }
            }
            // Console.WriteLine((byte)((min + max) / 2));
            return (byte)((min + max) / 2);
        }
    }
}
