using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Images;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_Pic2GSLIB : Form
    {
        Bitmap m_image = null;
        string m_GridName = string.Empty;


        Grid g = null;


        public Form_Pic2GSLIB()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileName = FileDialogHelper.OpenImage();
            if (fileName == string.Empty) return;
            pictureBox1.Image = ImageProcess.from_file_faster(fileName);
            m_GridName = FileHelper.GetFileName(fileName, false);
            m_image = pictureBox1.Image as Bitmap;
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        //离散型图像
        private void button1_Click(object sender, EventArgs e)
        {
            Form_Color2Code frm = new(m_GridName, pictureBox1.Image as Bitmap);
            frm.event_OK += Frm_event_OK;
            frm.ShowDialog();
        }
        //连续型图像
        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("打开彩色图像");
            //首先把图像转换为灰度图像
            var grayImage = ImageProcess.color_to_gray(m_image);
            Console.WriteLine("原图  => 灰度图像(关闭图像进入下一步操作)");
            ImageProcess.Show(grayImage);
            //上下翻转
            grayImage = ImageProcess.RevPicUD(grayImage, grayImage.Width, grayImage.Height);
            Console.WriteLine("原图  => 上下翻转图像");
            ImageProcess.Show(grayImage);
            Console.Write("是否进行反色计算:\n\t[ 是 => 1 ; 否 => 0 ]: ");
            int b = int.Parse(Console.ReadLine());
            if (b == 1)//采用反色计算
                grayImage = ImageProcess.RePic(grayImage, grayImage.Width, grayImage.Height);
            var grayHist = ImageProcess.get_gray_histogram(grayImage);
            Console.WriteLine($"灰度图像的取值区间为:\n\t[{grayHist.Item1[0]},{grayHist.Item1[grayHist.Item1.Length - 1]}]");
            Console.Write("是否修改数值的区间:\n\t[ 是 => 1 ; 否 => 0 ]: ");
            double min = 0;
            double max = 0;
            b = int.Parse(Console.ReadLine());
            DataMapper mapper = new();
            if (b == 0)//采用灰度图像的原始灰度区间
            {
                min = grayHist.Item1[0];
                max = grayHist.Item1[grayHist.Item1.Length - 1];
            }
            else if (b == 1)//修改灰度区间
            {
                Console.Write("输入新区间的Min:\n\t[ Min < Max ]: ");
                min = double.Parse(Console.ReadLine());
                Console.Write("输入新区间的Max:\n\t[ Min < Max ]: ");
                max = double.Parse(Console.ReadLine());
                mapper.Reset(grayHist.Item1[0], grayHist.Item1[grayHist.Item1.Length - 1], min, max);
            }
            GridStructure gs = GridStructure.create_simple(grayImage.Width, grayImage.Height, 1);
            GridProperty gp = GridProperty.create(gs);
            for (int iy = 0; iy < gs.ny; iy++)
            {
                for (int ix = 0; ix < gs.nx; ix++)
                {
                    int i = ix;
                    int j = iy;
                    int grayValue = grayImage.GetPixel(i, j).R;
                    if (b == 0)
                        gp.set_value(ix, iy, grayValue);
                    else
                        gp.set_value(ix, iy, (float?)mapper.MapAToB(grayValue));
                }
            }

            gp.show_win();
            Close();
        }

        private void Frm_event_OK(string GridName, Dictionary<string, double> Color2Code)
        {
            GridStructure gs = GridStructure.create_simple(m_image.Width, m_image.Height, 1);
            //由于图像是二维的
            GridProperty gp = GridProperty.create(gs);
            //坐标转换
            for (int iy = 0; iy < gs.ny; iy++)
            {
                for (int ix = 0; ix < gs.nx; ix++)
                {
                    int i = ix;
                    int j = iy;
                    double colorCode = Color2Code[ColorTranslator.ToHtml(m_image.GetPixel(i, j))];
                    gp.set_value(ix, gs.ny - iy - 1, (float?)colorCode);
                }
            }
            gp.show_win();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("打开彩色图像");
            //上下翻转
            var image = ImageProcess.RevPicUD(m_image, m_image.Width, m_image.Height);
            Console.WriteLine("原图  => 上下翻转图像");
            ImageProcess.Show(image);
            GridStructure gs = GridStructure.create_simple(m_image.Width, m_image.Height, 1);
            Grid rgbGrid = Grid.create(gs);
            rgbGrid.add_gridProperty("R");
            rgbGrid.add_gridProperty("G");
            rgbGrid.add_gridProperty("B");
            for (int iy = 0; iy < gs.ny; iy++)
            {
                for (int ix = 0; ix < gs.nx; ix++)
                {
                    int i = ix;
                    int j = iy;
                    rgbGrid["R"].set_value(ix, iy, image.GetPixel(i, j).R);
                    rgbGrid["G"].set_value(ix, iy, image.GetPixel(i, j).G);
                    rgbGrid["B"].set_value(ix, iy, image.GetPixel(i, j).B);
                }
            }
            rgbGrid.showGrid_win();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (g, var _) = Grid.create_from_gslibwin();
            scottplot4Grid1.update_grid(g);
        }
        //保存为图片
        private void button6_Click(object sender, EventArgs e)
        {
            g.first_gridProperty().draw_image_2d(Color.White, ColorMapEnum.Jet);
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
