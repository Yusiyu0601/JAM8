# 项目说明
- [中文版本](README-zh.md)
- [English](README.md)

JAM8 & snesim_with_reverse_query_search_tree 使用说明文档
# 1. JAM8
## 1.1 JAM8介绍
- JAM8是一个基于.net8环境（因此也成为JAM8）、C#语言、采用VS2022开发的地质统计学算法开发包。
- 运行环境是win10及以后windows版本。
- 具体的使用很简单，从github上下载zip包，直接解压，然后使用VS2022打开sln解决方案文件，即可加载所有项目，使用的第三方包都可以通过nuget自动获取（要求连接互联网）。
- 解决方案的所有项目包括<br>
(1)JAM8主项目，它包括常用工具类（在Utilities文件夹里）、算法库（在Algorithms\Geometry文件夹里），包括IDW插值与kriging插值算法等插值方法，ds与snesim等多点地质统计学方法，以及变差函数与网格处理模块;<br>
(2)JAM8.Work,这是一个基于JAM8开发的交互式操作的软件项目<br>
(3)JAM8.Console，是一个基于JAM8开发的交互式操作的软件项目，与JAM8.Work相同，不同之处在于这个项目以console控制台作为菜单操作界面。优点是让算法开发者不必考虑界面设计浪费时间，可以快速实现大量方法编写和测试<br>
(4)snesim_with_reverse_query_search_tree，这是一个基于JAM8开发的snesim测试项目，主要用于测试基于反向查询搜索树进行snesim程序运行。

# 2. 项目snesim_with_reverse_query_search_tree的使用说明
## 2.1 snesim_with_reverse_query_search_tree的说明
这个项目没有具体的snesim算法实现，而是通过调用JAM8里面的snesim类及其函数。该项目存在的意义是给使用该新算法用户提供一个更加清晰的调用函数说明。<br>
如下代码块是二维snesim的测试实例
```
            if (b == "2d")
            {
                Output.WriteLine(ConsoleColor.Yellow, "Load Training Image(2d)");
                GridProperty TI = Grid.create_from_gslibwin("Load Training Image").grid
                    .select_gridProperty_win("Select Property as Training Image").grid_property;
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData cd = null;
                if (is_use_cd == "Y")
                    (cd, var _) = CData.read_from_gslibwin();
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure gs = GridStructure.create_simple(500, 500, 1);
                gs = GridStructure.create_win(gs, "Set Simulation Grid Size");
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 60, (7, 7, 0), TI, cd, gs, ratio_inverseRetrieve);
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"使用时间:{time}");
            }
```
代码块中，<br>
- 第1步：GridProperty TI = Grid.create_from_gslibwin()采用窗体界面读取gslib格式的网格，并选取目标属性作为训练图像TI；<br>
- 第2步:如果使用条件数据约束模拟，则使用CData.read_from_gslibwin()采用窗体界面读取gslib格式的条件数据；<br>
- 第3步:使用GridStructure gs = GridStructure.create_simple()创建模拟网格尺寸或者gs = GridStructure.create_win从已有的网格尺寸重新设置模拟网格的尺寸，需要注意的是网格单元的大小与训练图像要保持一致；<br>
- 第4步：使用语句Snesim snesim = Snesim.create()创建多点模拟对象，并使用var (re, time) = snesim.run(1001, 1, 60, (7, 7, 0), TI, cd, gs, ratio_inverseRetrieve)语句开始运行计算；<br>
- 第5步:使用re.showGrid_win()语句调用显示模块，采用窗体显示模拟结果。需要说明的是，JAM8无法显示真三维，只能以二维切片的形式展示，如果需要通过三维形式查看结果，需要在显示界面点击导出模块，将结果导出为gslib，再使用其他模型可视化软件展示结果。<br>

## 2.2 snesim说明<br>
snesim算法类位置的路径是JAM8\Algorithms\Geometry\Simulate\Snesim<br>
- 该类有两个run函数<br>
- 其中单网格模拟函数名是
```
public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,
    Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
```
- 另外一个run函数是多重网格模拟函数
```
public (Grid, double time) run(int random_seed, int multigrid_count, int max_number,(int rx, int ry, int rz) template, 
GridProperty TI, CData cd, GridStructure gs_re,int progress_for_retrieve_inverse = 0)
```
## 2.3 运行项目snesim_with_reverse_query_search_tree，需要按照输入指定参数，具体流程如下：<br>
-- 运行提示:
```
start FastSnesim simulation by using inverse retrieve search tree
```
-- 第1步要求输入: 
```
Choose Example Dimension (input 2d or 3d) =>
```
此时，如果根据模拟对象的维度，输入2d或者3d，现假设输入2d，按下回车键。<br>
此时进入第2步的输入
```
set ratio of inverse retrieve search tree (input 0 ~ 100) =>
```
此时输入的是将模拟进度的前多少百分比部分采用反向搜索树查询方法，其余部分是采用传统的正向查询。建议设置为35。<br>
此时提示输入Load Training Image(2d)，并弹出一个对话框，用于选取待模拟用的训练图像
![测试图](/images/select training image(2d).png)


