using System.Numerics;
using JAM8.Algorithms.Geometry;
using MathNet.Numerics.IntegralTransforms;

namespace JAM8.Algorithms.Numerics
{
    public class MyFFT_Interface
    {
        /// <summary>
        /// 傅里叶变换1D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        static Complex[] fft(Complex[] samples)
        {
            Complex[] clone = new Complex[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                clone[i] = samples[i];
            }
            Fourier.Forward(clone, FourierOptions.Matlab);
            return clone;
        }
        /// <summary>
        /// 傅里叶逆变换1D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        static Complex[] ifft(Complex[] spectrum)
        {
            Complex[] clone = new Complex[spectrum.Length];
            for (int i = 0; i < spectrum.Length; i++)
            {
                clone[i] = spectrum[i];
            }
            Fourier.Inverse(clone, FourierOptions.Matlab);
            return clone;
        }
        /// <summary>
        /// 傅里叶平移1D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static double[] fftshift(double[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            double[] values2 = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }
        /// <summary>
        /// 傅里叶平移1D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static Complex[] fftshift(Complex[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            Complex[] values2 = new Complex[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }
        /// <summary>
        /// 傅里叶逆平移1D
        /// 说明:该段代码与Python的傅里叶代码对比，是正确的！
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static double[] ifftshift(double[] values)
        {
            double[] result = new double[values.Length];
            int shiftBy = values.Length / 2;

            for (int i = 0; i < values.Length; i++)
                result[i] = values[(i + shiftBy) % values.Length];

            return result;
        }
        /// <summary>
        /// 傅里叶逆平移1D
        /// 说明:该段代码与Python的傅里叶代码对比，是正确的！
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static Complex[] ifftshift(Complex[] values)
        {
            Complex[] result = new Complex[values.Length];
            int shiftBy = values.Length / 2;

            for (int i = 0; i < values.Length; i++)
                result[i] = values[(i + shiftBy) % values.Length];

            return result;
        }

        /// <summary>
        /// 傅里叶变换2D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        static Complex[,] fft2(Complex[,] samples)
        {
            int n_dim0 = samples.GetLength(0);
            int n_dim1 = samples.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = samples[idx_dim0, idx_dim1];
                }
                Fourier.Forward(array_of_dim0, FourierOptions.Matlab);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                Fourier.Forward(array_of_dim1, FourierOptions.Matlab);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶逆变换2D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        static Complex[,] ifft2(Complex[,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = spectrum[idx_dim0, idx_dim1];
                }
                Fourier.Inverse(array_of_dim0, FourierOptions.Matlab);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                Fourier.Inverse(array_of_dim1, FourierOptions.Matlab);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static Complex[,] fftshift2(Complex[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = fftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = fftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static double[,] fftshift2(double[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                double[] array_of_dim0 = new double[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = fftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                double[] array_of_dim1 = new double[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = fftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶逆平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static Complex[,] ifftshift2(Complex[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍ifft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = ifftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍ifft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = ifftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶逆平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static double[,] ifftshift2(double[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            //第1遍ifft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                double[] array_of_dim0 = new double[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = ifftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍ifft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                double[] array_of_dim1 = new double[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = ifftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 计算频域信号的幅值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static double[,] abs(Complex[,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            for (int idx_dim1 = 0; idx_dim1 < spectrum.GetLength(0); idx_dim1++)
            {
                for (int idx_dim2 = 0; idx_dim2 < spectrum.GetLength(1); idx_dim2++)
                {
                    result[idx_dim1, idx_dim2] = Math.Log(spectrum[idx_dim1, idx_dim2].Magnitude);
                }
            }
            return result;
        }

        /// <summary>
        /// 傅里叶变换3D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        static Complex[,,] fft3(Complex[,,] samples)
        {
            int n_dim0 = samples.GetLength(0);
            int n_dim1 = samples.GetLength(1);
            int n_dim2 = samples.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = samples[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim0, FourierOptions.Matlab);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim1, FourierOptions.Matlab);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim2, FourierOptions.Matlab);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶逆变换3D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        static Complex[,,] ifft3(Complex[,,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            int n_dim2 = spectrum.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = spectrum[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim0, FourierOptions.Matlab);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim1, FourierOptions.Matlab);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim2, FourierOptions.Matlab);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶平移3D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static Complex[,,] fftshift3(Complex[,,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            int n_dim2 = values.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fftshift
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim0);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim1);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim2);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }

        static void show_win(Complex[,] values)
        {
            var gp = GridProperty.create(GridStructure.create_simple(values.GetLength(0), values.GetLength(1), 1));
            for (int idx_dim1 = 0; idx_dim1 < values.GetLength(0); idx_dim1++)
            {
                for (int idx_dim2 = 0; idx_dim2 < values.GetLength(1); idx_dim2++)
                {
                    gp.set_value(idx_dim1, idx_dim2, (float?)Math.Log(values[idx_dim1, idx_dim2].Magnitude));
                }
            }
            gp.show_win();
        }

    }
    /// <summary>
    /// 傅里叶变换
    /// </summary>
    public class MyFFT
    {
        /// <summary>
        /// 傅里叶变换1D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static Complex[] fft(Complex[] samples)
        {
            Complex[] clone = new Complex[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                clone[i] = samples[i];
            }
            Fourier.Forward(clone, FourierOptions.Matlab);
            return clone;
        }
        /// <summary>
        /// 傅里叶逆变换1D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public static Complex[] ifft(Complex[] spectrum)
        {
            Complex[] clone = new Complex[spectrum.Length];
            for (int i = 0; i < spectrum.Length; i++)
            {
                clone[i] = spectrum[i];
            }
            Fourier.Inverse(clone, FourierOptions.Matlab);
            return clone;
        }
        /// <summary>
        /// 傅里叶平移1D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[] fftshift(double[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            double[] values2 = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }
        /// <summary>
        /// 傅里叶平移1D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Complex[] fftshift(Complex[] values)
        {
            int shiftBy = (values.Length + 1) / 2;

            Complex[] values2 = new Complex[values.Length];
            for (int i = 0; i < values.Length; i++)
                values2[i] = values[(i + shiftBy) % values.Length];

            return values2;
        }
        /// <summary>
        /// 傅里叶逆平移1D
        /// 说明:该段代码与Python的傅里叶代码对比，是正确的！
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[] ifftshift(double[] values)
        {
            double[] result = new double[values.Length];
            int shiftBy = values.Length / 2;

            for (int i = 0; i < values.Length; i++)
                result[i] = values[(i + shiftBy) % values.Length];

            return result;
        }
        /// <summary>
        /// 傅里叶逆平移1D
        /// 说明:该段代码与Python的傅里叶代码对比，是正确的！
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Complex[] ifftshift(Complex[] values)
        {
            Complex[] result = new Complex[values.Length];
            int shiftBy = values.Length / 2;

            for (int i = 0; i < values.Length; i++)
                result[i] = values[(i + shiftBy) % values.Length];

            return result;
        }

        /// <summary>
        /// 傅里叶变换2D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static Complex[,] fft2(Complex[,] samples)
        {
            int n_dim0 = samples.GetLength(0);
            int n_dim1 = samples.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = samples[idx_dim0, idx_dim1];
                }
                Fourier.Forward(array_of_dim0, FourierOptions.Matlab);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                Fourier.Forward(array_of_dim1, FourierOptions.Matlab);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶逆变换2D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public static Complex[,] ifft2(Complex[,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = spectrum[idx_dim0, idx_dim1];
                }
                Fourier.Inverse(array_of_dim0, FourierOptions.Matlab);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                Fourier.Inverse(array_of_dim1, FourierOptions.Matlab);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Complex[,] fftshift2(Complex[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = fftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = fftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[,] fftshift2(double[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            //第1遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                double[] array_of_dim0 = new double[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = fftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍fft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                double[] array_of_dim1 = new double[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = fftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶逆平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Complex[,] ifftshift2(Complex[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            Complex[,] result = new Complex[n_dim0, n_dim1];
            //第1遍ifft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                Complex[] array_of_dim0 = new Complex[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = ifftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍ifft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                Complex[] array_of_dim1 = new Complex[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = ifftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 傅里叶逆平移2D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[,] ifftshift2(double[,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            //第1遍ifft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                double[] array_of_dim0 = new double[n_dim0];
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1];
                }
                array_of_dim0 = ifftshift(array_of_dim0);
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim0[idx_dim0];
                }
            }
            //第2遍ifft
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                double[] array_of_dim1 = new double[n_dim1];
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1];
                }
                array_of_dim1 = ifftshift(array_of_dim1);
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    result[idx_dim0, idx_dim1] = array_of_dim1[idx_dim1];
                }
            }
            return result;

        }
        /// <summary>
        /// 计算频域信号的幅值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double[,] abs(Complex[,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            double[,] result = new double[n_dim0, n_dim1];
            for (int idx_dim1 = 0; idx_dim1 < spectrum.GetLength(0); idx_dim1++)
            {
                for (int idx_dim2 = 0; idx_dim2 < spectrum.GetLength(1); idx_dim2++)
                {
                    result[idx_dim1, idx_dim2] = Math.Log(spectrum[idx_dim1, idx_dim2].Magnitude);
                }
            }
            return result;
        }

        /// <summary>
        /// 傅里叶变换3D
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static Complex[,,] fft3(Complex[,,] samples)
        {
            int n_dim0 = samples.GetLength(0);
            int n_dim1 = samples.GetLength(1);
            int n_dim2 = samples.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = samples[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim0, FourierOptions.Matlab);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim1, FourierOptions.Matlab);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Forward(array_of_dim2, FourierOptions.Matlab);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶逆变换3D
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public static Complex[,,] ifft3(Complex[,,] spectrum)
        {
            int n_dim0 = spectrum.GetLength(0);
            int n_dim1 = spectrum.GetLength(1);
            int n_dim2 = spectrum.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = spectrum[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim0, FourierOptions.Matlab);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim1, FourierOptions.Matlab);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    Fourier.Inverse(array_of_dim2, FourierOptions.Matlab);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 傅里叶平移3D
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Complex[,,] fftshift3(Complex[,,] values)
        {
            int n_dim0 = values.GetLength(0);
            int n_dim1 = values.GetLength(1);
            int n_dim2 = values.GetLength(2);
            Complex[,,] result = new Complex[n_dim0, n_dim1, n_dim2];
            //第1遍fftshift
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    Complex[] array_of_dim0 = new Complex[n_dim0];
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        array_of_dim0[idx_dim0] = values[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim0);
                    for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim0[idx_dim0];
                    }
                }
            }
            //第2遍fft
            for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim1 = new Complex[n_dim1];
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        array_of_dim1[idx_dim1] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim1);
                    for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim1[idx_dim1];
                    }
                }
            }
            //第3遍fft
            for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
            {
                for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
                {
                    Complex[] array_of_dim2 = new Complex[n_dim2];
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        array_of_dim2[idx_dim2] = result[idx_dim0, idx_dim1, idx_dim2];
                    }
                    fftshift(array_of_dim2);
                    for (int idx_dim2 = 0; idx_dim2 < n_dim2; idx_dim2++)
                    {
                        result[idx_dim0, idx_dim1, idx_dim2] = array_of_dim2[idx_dim2];
                    }
                }
            }
            return result;
        }

        static void show_win(Complex[,] values)
        {
            var gp = GridProperty.create(GridStructure.create_simple(values.GetLength(0), values.GetLength(1), 1));
            for (int idx_dim1 = 0; idx_dim1 < values.GetLength(0); idx_dim1++)
            {
                for (int idx_dim2 = 0; idx_dim2 < values.GetLength(1); idx_dim2++)
                {
                    gp.set_value(idx_dim1, idx_dim2, (float?)Math.Log(values[idx_dim1, idx_dim2].Magnitude));
                }
            }
            gp.show_win();
        }
    }
}
