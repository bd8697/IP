using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISIP_FrameworkHelpers;
using System.Windows;

using Emgu.CV;
using Emgu.CV.Structure;
namespace ISIP_Algorithms.Tools
{
    public class Tools
    {
        private static List<Tuple<System.Windows.Point, System.Windows.Point>> lines = new List<Tuple<System.Windows.Point, System.Windows.Point>>();

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
                // Console.WriteLine(255 * Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c * i);
                table[i] = 255 * (Math.Pow(i, E) / (Math.Pow(i, E) + Math.Pow(m, E)) + c * i);
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
            double[] hist = new double[256];
            double[] histRel = new double[256];
            int nrPixeli = InputImage.Width * InputImage.Height;

            double T = 0; // prag
            double lastT = T + 1;
            int counter = 0;
            T = Mid_range(InputImage);
            List<int> C1 = new List<int>();
            List<int> C2 = new List<int>();
            double m1 = 0;
            double m2 = 0;
            double suma1_1 = 0;
            double suma1_2 = 0;
            double suma2_1 = 0;
            double suma2_2 = 0;

            for (int i = 0; i < hist.Length; i++)
            {
                hist[i] = 0;
                histRel[i] = 0;
            }

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    hist[InputImage.Data[y, x, 0]]++;
                }
            }
            for (int i = 0; i < hist.Length; i++)
            {
                histRel[i] = hist[i] / nrPixeli;
            }
            //List<int> test = new List<int> { 1, 1, 1, 1, 10 };
            //Console.WriteLine(test.Average());

            T = Mid_range(InputImage);

            while ((int)T != (int)lastT && counter < 5000)
            {
                // Console.WriteLine("t" + T);
                counter++;
                //  Console.WriteLine(counter);
                //for (int y = 0; y < InputImage.Height; y++)
                //{
                //    for (int x = 0; x < InputImage.Width; x++)
                //    {
                //        if (InputImage.Data[y, x, 0] > T)
                //        {
                //            C1.Add(InputImage.Data[y, x, 0]);
                //        }
                //        else if (InputImage.Data[y, x, 0] <= T)
                //        {
                //            C2.Add(InputImage.Data[y, x, 0]);
                //        }
                //    }
                //}

                m1 = 0;
                m2 = 0;
                suma1_1 = 0;
                suma1_2 = 0;
                suma2_1 = 0;
                suma2_2 = 0;

                for (int y = 0; y < T; y++)
                {
                    suma2_2 += histRel[y];
                    suma2_1 += y * histRel[y];
                }

                for (int y = (int)T + 1; y <= 255; y++)
                {
                    suma1_2 += histRel[y];
                    suma1_1 += y * histRel[y];
                }

                m1 = suma1_1 / suma1_2;
                m2 = suma2_1 / suma2_2;

                //m1 = C1.Average();
                //m2 = C2.Average();
                //C1.Clear();
                //C2.Clear();

                // Console.WriteLine(m1 + "m1");
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


        // Tema 4

        private static int[] PascalTriangle(int n)
        {
            int[] arr = new int[n];
            int currElem = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (j == 0 || i == 0)
                        currElem = 1;
                    else
                        currElem = currElem * (i - j + 1) / j;
                    //  Console.Write(currElem + " ");
                    if (i == n - 1)
                    {
                        arr[j] = currElem;
                    }
                }
                //  Console.WriteLine();
            }
            return arr;
        }

        private static double[,] GetMask(int n)
        {
            double c = 0;
            int[] arr = new int[n];
            arr = PascalTriangle(n);
            int[,] arrT = new int[1, n];
            double[,] mask = new double[n, n];
            for (int j = 0; j < n; j++)
            {
                arrT[0, j] = arr[j];
            }

            for (int i = 0; i < n; i++)
            {
                // Console.WriteLine(arrT[0, i] + " arr");
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    mask[i, j] = arr[i] * arrT[0, j];
                    c += mask[i, j];
                }
            }

            // Console.WriteLine(c);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    // Console.Write(mask[i, j] + " ");
                    mask[i, j] /= c;
                }
                // Console.WriteLine();
            }

            return mask;
        }

        public static Image<Gray, byte> BinomialFilterG(Image<Gray, byte> InputImage, int n)
        {
            if (n % 2 == 0)
                n -= 1;

            double[,] mask = new double[n, n];
            mask = GetMask(n);
            double val = 0;
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (y < n / 2 || y >= InputImage.Height - n / 2 || x < n / 2 || x >= InputImage.Height - n / 2)
                    {
                        Result.Data[y, x, 0] = InputImage.Data[y, x, 0];
                    }
                    else
                    {
                        val = 0;
                        for (int k = -n / 2; k <= n / 2; k++)
                        {
                            for (int l = -n / 2; l <= n / 2; l++)
                            {
                                val += (double)(mask[k + n / 2, l + n / 2] * InputImage.Data[y + l, x + k, 0]);
                            }
                        }
                        Result.Data[y, x, 0] = (byte)(val + 0.5f);
                    }
                }
            }
            return Result;
        }

        public static Image<Bgr, byte> BinomialFilterRGB(Image<Bgr, byte> InputImage, int n)
        {
            if (n % 2 != 0)
                n -= 1;


            double[,] mask = new double[n, n];
            mask = GetMask(n);

            Image<Bgr, byte> Result = new Image<Bgr, byte>(InputImage.Size);

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (y < n / 2 || y >= InputImage.Height - n / 2 || x < n / 2 || x >= InputImage.Height - n / 2)
                    {
                        for (int ch = 0; ch < 3; ch++)
                        {
                            Result.Data[y, x, ch] = InputImage.Data[y, x, ch];
                        }
                    }
                    else
                    {
                        for (int k = -n / 2; k < n / 2; k++)
                        {
                            for (int l = -n / 2; l < n / 2; l++)
                            {
                                for (int ch = 0; ch < 3; ch++)
                                {
                                    Result.Data[y, x, ch] += (byte)(mask[k + n / 2, l + n / 2] * InputImage.Data[y + l, x + k, ch]);
                                }
                            }
                        }
                    }
                }
            }
            return Result;
        }

        // Tema 5

        public static Image<Gray, byte> HorizontalSobel(Image<Gray, byte> InputImage, int t)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            float fx = 0;
            float fy = 0;
            double norma = 0;
            double dir = 0;
            double deg = 0;
            int degThreshold = 5;

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (y < 1 || y >= InputImage.Height - 1 || x < 1 || x >= InputImage.Width - 1)
                    {
                        Result.Data[y, x, 0] = InputImage.Data[y, x, 0];
                    }
                    else
                    {
                        fx = InputImage.Data[y - 1, x + 1, 0] - InputImage.Data[y - 1, x - 1, 0] + 2 * InputImage.Data[y, x + 1, 0] - 2 * InputImage.Data[y, x - 1, 0] + InputImage.Data[y + 1, x + 1, 0] - InputImage.Data[y + 1, x - 1, 0];
                        fy = InputImage.Data[y + 1, x - 1, 0] - InputImage.Data[y - 1, x - 1, 0] + 2 * InputImage.Data[y + 1, x, 0] - 2 * InputImage.Data[y - 1, x, 0] + InputImage.Data[y + 1, x + 1, 0] - InputImage.Data[y - 1, x + 1, 0];

                        norma = Math.Sqrt((fx * fx) + (fy * fy));

                        //dir = Math.Atan2(fx, fy);

                        //if (norma >= t && Math.Abs(dir) > 2f)
                        //{
                        //    Result.Data[y, x, 0] = (byte)(255);
                        //}
                        //else
                        //{
                        //    Result.Data[y, x, 0] = (byte)(0);
                        //}
                        dir = Math.Atan2(fy, fx);
                        deg = (180 / Math.PI) * dir;

                        if (norma >= t && (Math.Abs(deg) >= 90 - degThreshold && Math.Abs(deg) <= 90 + degThreshold))
                        {
                            Result.Data[y, x, 0] = (byte)(255);
                        }
                        else
                        {
                            Result.Data[y, x, 0] = (byte)(0);
                        }
                    }
                }
            }
            return Result;
        }

        // Tema 6

        public static Image<Gray, byte> Closing(Image<Gray, byte> InputImage, int n)
        {
            return Erodare(Dilatare(InputImage, n), n);
        }

        private static Image<Gray, byte> Dilatare(Image<Gray, byte> InputImage, int n)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            int width = InputImage.Width;
            int height = InputImage.Height;

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    //if (y < n / 2 || y >= InputImage.Height - n / 2 || x < n / 2 || x >= InputImage.Width - n / 2)
                    //{
                    //    Result.Data[y, x, 0] = InputImage.Data[y, x, 0];
                    //}
                    //else
                    //{
                    if (InputImage.Data[y, x, 0] == 0)
                    {
                        bool hasWhiteNeighbour = false;
                        for (int k = -n / 2; k <= n / 2; k++)
                        {
                            for (int l = -n / 2; l <= n / 2; l++)
                            {
                                if (y + l >= 0 && y + l < height && x + k >= 0 && x + k < width)
                                    if (InputImage.Data[y + l, x + k, 0] == 255)
                                    {
                                        Result.Data[y, x, 0] = 255;
                                        hasWhiteNeighbour = true;
                                        break;
                                    }
                            }
                            if (hasWhiteNeighbour)
                                break;
                        }
                        //if (hasWhiteNeighbour)
                        //{
                        //    Result.Data[y, x, 0] = 255;
                        //}
                        //else
                        //{
                        //    Result.Data[y, x, 0] = 0;
                        //}
                    }
                    else
                    {
                        Result.Data[y, x, 0] = 255;
                    }
                    // }
                }
            }
            return Result;
        }

        private static Image<Gray, byte> Erodare(Image<Gray, byte> InputImage, int n)
        {
            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);
            int width = InputImage.Width;
            int height = InputImage.Height;

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] == 255)
                    {
                        bool hasBlackNeighbour = false;
                        for (int k = -n / 2; k <= n / 2; k++)
                        {
                            for (int l = -n / 2; l <= n / 2; l++)
                            {
                                if (y + l >= 0 && y + l < height && x + k >= 0 && x + k < width)
                                    if (InputImage.Data[y + l, x + k, 0] == 0)
                                    {
                                        //  Result.Data[y, x, 0] = 0;
                                        hasBlackNeighbour = true;
                                        break;
                                    }
                            }
                            if (hasBlackNeighbour)
                                break;
                        }
                        if (hasBlackNeighbour)
                        {
                            Result.Data[y, x, 0] = 0;
                        }
                        else
                        {
                            Result.Data[y, x, 0] = 255;
                        }
                    }
                    else
                    {
                        Result.Data[y, x, 0] = 0;
                    }
                }
            }
            return Result;
        }

        // Tema 7

        public static Image<Gray, byte> BilinearInterpolationScale(Image<Gray, byte> InputImage, float coef, System.Windows.Point lastClick)
        {
            double xc, yc = 0f;
            int x0, y0, x1, y1 = 0;
            double fy0, fy1, fc = 0f;
            double offsetX = lastClick.X * coef;
            double offsetY = lastClick.Y * coef;
            int offsetScaleDownX = 0;
            int offsetScaleDownY = 0;

            if (coef < 1f)
            {
                offsetScaleDownX = (int)(InputImage.Width * coef);
                offsetScaleDownX = (InputImage.Width - offsetScaleDownX) / 2;
                offsetScaleDownY = (int)(InputImage.Height * coef);
                offsetScaleDownY = (InputImage.Height - offsetScaleDownY) / 2;

                offsetX = 0;
                offsetY = 0;
            }

            Image<Gray, byte> Result = new Image<Gray, byte>(InputImage.Size);

            for (int y = 0; y < Result.Height; y++)
            {
                for (int x = 0; x < Result.Width; x++)
                {
                    xc = (double)(x + offsetX - offsetScaleDownX) / coef;
                    yc = (double)(y + offsetY - offsetScaleDownY) / coef;

                    if (xc < Result.Width - 1 && xc >= 0 && yc >= 0 && yc < Result.Height - 1)
                    {
                        x0 = (int)xc;
                        y0 = (int)yc;
                        x1 = x0 + 1;
                        y1 = y0 + 1;
                        // x0 <= xc <= x1

                        fy0 = (InputImage.Data[y0, x0 + 1, 0] - InputImage.Data[y0, x0, 0]) * (xc - x0) + InputImage.Data[y0, x0, 0];
                        fy1 = (InputImage.Data[y0 + 1, x0 + 1, 0] - InputImage.Data[y0 + 1, x0, 0]) * (xc - x0) + InputImage.Data[y0 + 1, x0, 0];
                        fc = (fy1 - fy0) * (yc - y0) + fy0;

                        Result.Data[y, x, 0] = (byte)fc;
                    }
                    else
                    {
                        Result.Data[y, x, 0] = 0;
                    }
                }
            }
            return Result;
        }

        // Tema 8

        public static Image<Gray, byte> HoughTransform(Image<Gray, byte> InputImage)
        {
            int houghWidth = (int)Math.Ceiling(Math.Sqrt(InputImage.Width * InputImage.Width + InputImage.Height * InputImage.Height));
            int houghHeight = 181;

            var size = InputImage.Size;
            size.Width = houghWidth * 2;
            size.Height = houghHeight;
            Image<Gray, byte> Result = new Image<Gray, byte>(size);

            double r = 0;
            int maxim = 0;

            //hough: axa Y: -90 -> 90, axa X: -diag -> diag

            List<List<int>> hough = new List<List<int>>();

            for (int i = 0; i < houghHeight; i++)
            {
                hough.Add(new List<int>());
                for (int j = -houghWidth; j <= houghWidth; j++)
                {
                    hough[i].Add(0);
                }
            }

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] > 0)
                    {
                        Console.WriteLine();
                        for (int theta = -90; theta <= 90; theta++)
                        {
                            r = x * Math.Cos(theta) + y * Math.Sin(theta);
                            //Console.WriteLine(r);
                            int auxAcc = hough[theta + 90][(int)r + houghWidth]++;
                            Console.Write(auxAcc + " ");
                            if (auxAcc > maxim)
                            {
                                maxim = auxAcc;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < houghHeight; i++)
            {
                for (int j = 0; j < houghWidth * 2; j++)
                {
                    Result.Data[i, j, 0] = (byte)hough[i][j];
                }
            }

            //print haugh
            for (int i = 0; i < hough.Count; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < hough[0].Count; j++)
                {
                    Console.Write(hough[i][j] + " ");
                }
            }
            return Result;
        }

        public static Image<Gray, byte> HoughTransform2(Image<Gray, byte> InputImage)
        {
            lines.Add(new Tuple<System.Windows.Point, System.Windows.Point>(new System.Windows.Point(10, 20), new System.Windows.Point(40, 50)));
            int houghHeight = (int)Math.Ceiling(Math.Sqrt(InputImage.Width * InputImage.Width + InputImage.Height * InputImage.Height));
            int houghWidth = 181;

            var size = InputImage.Size;
            size.Height = houghHeight * 2;
            size.Width = houghWidth;
            Image<Gray, byte> Result = new Image<Gray, byte>(size);

            double r = 0;
            int maxim = 0;
            int edgeSelectionThreshold = 5;

            // result -> input
            int x0 = 0;
            int x1 = InputImage.Width;

            //hough: axa Y: -90 -> 90, axa X: -diag -> diag

            List<List<int>> hough = new List<List<int>>();

            for (int i = 0; i < houghHeight * 2; i++)
            {
                hough.Add(new List<int>());
                for (int j = 0; j <= 180; j++)
                {
                    hough[i].Add(0);
                }
            }

            for (int y = 0; y < InputImage.Height; y++)
            {
                for (int x = 0; x < InputImage.Width; x++)
                {
                    if (InputImage.Data[y, x, 0] > 0)
                    {
                        // Console.WriteLine();
                        for (int theta = -90; theta <= 90; theta++)
                        {
                            double thetaRad = (Math.PI / 180) * theta;
                            r = x * Math.Cos(thetaRad) + y * Math.Sin(thetaRad);
                            //Console.WriteLine(r);
                            int auxAcc = hough[(int)r + houghHeight][theta + 90]++;
                            // Console.Write(auxAcc + " ");
                            if (auxAcc > maxim)
                            {
                                maxim = auxAcc;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < houghHeight * 2; i++) // i = rho, j = theta
            {
                for (int j = 0; j < houghWidth; j++)
                {
                    Result.Data[i, j, 0] = (byte)hough[i][j];
                    if (hough[i][j] >= maxim - edgeSelectionThreshold)
                    {
                        double thetaRad = (Math.PI / 180) * (j + 1);
                        int y0 = (int)(- Math.Cos(thetaRad) / Math.Sin(thetaRad) * x0 + ((i + 1) / Math.Sin(thetaRad)));
                        int y1 = (int)(- Math.Cos(thetaRad) / Math.Sin(thetaRad) * x1 + ((i + 1) / Math.Sin(thetaRad)));

                        lines.Add(new Tuple<System.Windows.Point, System.Windows.Point>(new System.Windows.Point(x0, y0), new System.Windows.Point(x1, y1)));
                    }
                }
            }
            Console.WriteLine(maxim);

            //print haugh
            for (int i = 0; i < hough.Count; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < hough[0].Count; j++)
                {
                    Console.Write(hough[i][j] + " ");
                }
            }
            return Result;
        }

        public static List<Tuple<System.Windows.Point, System.Windows.Point>> GetLines() {
                return lines;
            }

    }
}
