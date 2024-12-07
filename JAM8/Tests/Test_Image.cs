using JAM8.Algorithms.Images;

namespace JAM8.Tests
{
    public class Test_Image
    {
        public static void fft()
        {
            ImageProcess.fft(ImageProcess.from_file_faster() as Bitmap);
        }
    }
}
