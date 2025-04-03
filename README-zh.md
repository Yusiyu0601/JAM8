# 项目说明
- [中文版本](README-zh.md)
- [英文版本](README.md)

JAM8 & snesim_with_reverse_query_search_tree 使用说明文档
# 1. JAM8介绍
- **基本信息**：JAM8是一个基于.net8环境（因此也称为JAM8）、C#语言、采用VS2022开发的地质统计学算法开发包。
- **运行环境**：运行环境是win10及以后的windows版本。
- **使用方法**：具体的使用很简单，从github上下载zip包，直接解压，然后使用VS2022打开sln解决方案文件，即可加载所有项目，使用的第三方包都可以通过nuget自动获取（要求连接互联网）。
- **解决方案项目构成**：
  - **JAM8主项目**：它包括常用工具类（在Utilities文件夹里）、算法库（在Algorithms\Geometry文件夹里），包括IDW插值与kriging插值算法等插值方法，ds与snesim等多点地质统计学方法，以及变差函数与网格处理模块。
  - **JAM8.Work**：这是一个基于JAM8开发的交互式操作的软件项目。
  - **JAM8.Console**：是一个基于JAM8开发的交互式操作的软件项目，与JAM8.Work相同，不同之处在于这个项目以console控制台作为菜单操作界面。优点是让算法开发者不必考虑界面设计浪费时间，可以快速实现大量方法编写和测试。
  - **snesim_with_reverse_query_search_tree**：这是一个基于JAM8开发的snesim测试项目，主要用于测试基于反向查询搜索树进行snesim程序运行。 

# 2. 项目snesim_with_reverse_query_search_tree介绍
## 2.1 snesim_with_reverse_query_search_tree的使用说明
这个项目没有具体的snesim算法实现，而是通过调用JAM8里面的snesim类及其函数。该项目存在的意义是给使用该新算法用户提供一个更加清晰的调用函数说明。
- **二维snesim的实例代码**
```
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            AllocConsole();//开启控制台

            //start FastSnesim simulation by using inverse retrieve search tree
            Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");

            //1. Choose Example Dimension
            string b = EasyConsole.Input.ReadString("Choose Example Dimension (input 2d or 3d) => ");

            //2. Set ratio of inverse retrieve search tree
            int ratio_inverseRetrieve = Input.ReadInt("set ratio of inverse retrieve search tree (input 0 ~ 100) => ", 0, 100);

            #region 2d Example

            if (b == "2d")
            {
                //3. Load Training Image
                Output.WriteLine(ConsoleColor.Yellow, "\nLoad Training Image(2d)");
                var (input_grid, input_fileName) = Grid.create_from_gslibwin("Load Training Image");
                GridProperty TI = input_grid.select_gridProperty_win("Select Property as Training Image").grid_property;
                Output.WriteLine($"\n\tfileName  {input_fileName}");
                Output.WriteLine(TI.gridStructure.view_text());

                //4. Set Simulation Grid Size
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure re_gs = GridStructure.create_simple(100, 100, 1);
                re_gs = GridStructure.create_win(re_gs, "Set Simulation Grid Size");
                Output.WriteLine(re_gs.view_text());

                //5. Load Conditional Data
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData2 cd = null;
                CData2 coarsened_cd = null;
                Grid coarsened_grid = null;
                if (is_use_cd == "Y")
                {
                    string cd_fileName = "";
                    (cd, cd_fileName) = CData2.read_from_gslib_win();
                    Output.WriteLine($"\n\tconditional data fileName  {cd_fileName}\n");
                    //coarsened conditional data and show coarsened grid
                    (coarsened_cd, coarsened_grid) = cd.coarsened(re_gs);
                    coarsened_grid.showGrid_win();
                }

                //6. Run FastSnesim Simulation
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 60, (7, 7, 0), TI, cd, re_gs, ratio_inverseRetrieve);

                //7. Show Simulation Result and Show Simulation Time
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"\nTime {time} milliseconds");
            }

            #endregion
```

### 2.1.1 初始化操作
1. **COM 包装器注册**：调用 `ComWrappers.RegisterForMarshalling` 方法，将 `WinFormsComInterop.WinFormsComWrappers.Instance` 进行注册，以便后续进行 COM 互操作相关的操作。
2. **开启控制台**：调用 `AllocConsole` 函数开启一个控制台窗口，用于后续的输入输出操作。

### 2.1.2 模拟开始提示
向控制台输出信息，提示开始使用逆检索搜索树进行 FastSnesim 模拟。
    ```
    Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");
    ```

### 2.1.3 用户交互选择与设置
1. **选择示例维度**：提示用户输入示例维度，可输入 "2d" 或 "3d"，程序会读取用户输入的内容并存储在变量 `b` 中。<br>
    ```
    Choose Example Dimension (input 2d or 3d) =>
    ```
2. **设置逆检索搜索树的比例**：提示用户输入逆检索搜索树的比例,将模拟进度的前多少百分比部分采用反向搜索树查询方法，其余部分是采用传统的正向查询，范围在 0 到 100 之间，程序会读取用户输入的整数并存储在变量 `ratio_inverseRetrieve` 中,建议设置为35。<br>
    ```
    set ratio of inverse retrieve search tree (input 0 ~ 100) =>
    ```

### 2.1.4 二维示例处理流程(三维实例是相同的操作流程)
如果用户选择的示例维度为 "2d"，则执行以下步骤：
1. **加载训练图像**
    - 以黄色字体在控制台输出提示信息，提示加载二维训练图像。
    - 调用 `Grid.create_from_gslibwin` 方法，弹出窗口让用户选择训练图像文件(例如: test data\Training Image B.out)，获取输入网格和文件名。<br>
		![select training image](/images/select_training_image_2d.jpg)<br>
    - 调用 `input_grid.select_gridProperty_win` 方法，弹出窗口让用户选择一个属性作为训练图像，获取对应的网格属性 `TI`。<br>
		![select training image](/images/select_property_as_training_image.jpg)<br>
    - 在控制台输出文件名和训练图像网格结构的文本信息。
2. **设置模拟网格大小**
    - 以黄色字体在控制台输出提示信息，提示设置模拟网格大小。
    - 先创建一个简单的 100x100x1 的网格结构 `re_gs`。
    - 调用 `GridStructure.create_win` 方法，弹出窗口让用户对 `re_gs` 进行设置，更新模拟网格结构。
    - 在控制台输出更新后的模拟网格结构的文本信息。
3. **加载条件数据**
    - 提示用户是否使用二维条件数据，可输入 "Y" 或 "N"，程序会读取用户输入的内容。<br>
    ```
    use conditional data(2d) or not? (input Y or N) =>
    ```
    - 如果用户输入 "Y"，则调用 `CData2.read_from_gslib_win` 方法，弹出窗口让用户选择条件数据文件（例如:test data\cd_for_trainingImage_B.out），获取条件数据 `cd` 和文件名。<br>
		![select training image](/images/read_conditional_data.jpg)<br>
    - 在控制台输出条件数据的文件名。
    - 调用 `cd.coarsened` 方法对条件数据进行粗化处理，得到粗化后的条件数据 `coarsened_cd` 和粗化后的网格 `coarsened_grid`，并弹出窗口显示粗化后的网格。
4. **运行 FastSnesim 模拟**
    - 创建一个 `Snesim` 对象 `snesim`。
    - 调用 `snesim.run` 方法进行模拟，传入模拟所需的参数（如种子数、实现数、最大搜索数、搜索模板大小、训练图像、条件数据、模拟网格结构、逆检索搜索树比例），得到模拟结果 `re` 和模拟时间 `time`。
5. **显示模拟结果和时间**
    - 弹出窗口显示模拟结果 `re`。需要说明的是，JAM8无法显示真三维，只能以二维切片的形式展示，如果需要通过三维形式查看结果，需要在显示界面点击导出模块，将结果导出为gslib，再使用其他模型可视化软件展示结果。
    - 以红色字体在控制台输出模拟所花费的时间（单位：毫秒）。 

## 2.2 snesim说明<br>
snesim算法类位置的路径是JAM8\Algorithms\Geometry\Simulate\Snesim，该类包含了两个run函数<br>
- **其中指定网格级别的模拟函数名是**
   ```
   public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
   ```
1. **参数含义**：
    - `TI`：训练图像，类型为 `GridProperty`。
    - `cd`：条件数据，类型为 `CData2`，可以为 `null`。
    - `gs_re`：模拟网格结构，类型为 `GridStructure`。
    - `random_seed`：随机种子，用于初始化随机数生成器。
    - `mould`：模拟模板，类型为 `Mould`。
    - `multigrid_level`：多重网格级别，默认值为 `1`。
    - `progress_for_retrieve_inverse`：在模拟进度中，反向查询在前一次模拟进度中的比例，默认值为 `0`。
2. **实现过程**：
    1. 使用 `random_seed` 初始化一个 `Random` 对象 `rnd`，并根据 `gs_re` 创建一个 `Grid` 对象 `result`。
    2. 如果存在条件数据 `cd`，对其进行粗化处理，并将粗化后的条件数据添加到 `result` 中；否则，直接在 `result` 中添加一个属性。
    3. 根据 `mould` 和 `TI` 创建一个搜索树 `tree`。
    4. 初始化一些字典和列表，用于存储节点、概率分布等信息。
    5. 创建一个模拟路径 `path`。
    6. 启动一个 `Stopwatch` 来记录模拟时间（不包括构建搜索树的时间）。
    7. 当模拟路径未访问完时，循环执行以下操作：
        - 如果模拟进度是整数且与上一次进度不同，停止 `Stopwatch`，记录时间并累加总时间，然后重新启动 `Stopwatch`。
        - 打印模拟进度。
        - 获取下一个要访问的节点 `si`，如果该节点的值为空，则根据搜索树和当前进度计算条件概率 `cpdf`，并根据 `cpdf` 进行采样得到节点值，将其设置到 `result` 中。
    8. 停止 `Stopwatch`，返回模拟结果 `result` 和总模拟时间。 
- **另外一个run函数是多重网格模拟函数**
   ```
   public (Grid, double time) run(int random_seed, int multigrid_count, int max_number,(int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_re,int progress_for_retrieve_inverse = 0)  
   ```
1. **参数含义**：
    - `random_seed`：随机种子，用于初始化随机数生成器。
    - `multigrid_count`：多重网格的总数。
    - `max_number`：实际模板中的节点总数。
    - `template`：一个包含三个整数的元组，分别表示模板在 `x`、`y`、`z` 方向上的尺寸。
    - `TI`：训练图像，类型为 `GridProperty`。
    - `cd`：条件数据，类型为 `CData2`，可以为 `null`。
    - `gs_re`：模拟网格结构的大小，类型为 `GridStructure`。
    - `progress_for_retrieve_inverse`：在模拟进度中，反向查询在前一次模拟进度中的比例，默认值为 `0`。
2. **实现过程**：
    1. 启动一个 `Stopwatch` 来检测模拟时间。
    2. 根据 `gs_re` 创建一个 `Grid` 对象 `g`。
    3. 如果存在条件数据 `cd`，对其进行粗化处理，并将粗化后的条件数据添加到 `g` 中。
    4. 从 `multigrid_count` 到 `1` 进行循环，根据网格级别创建模具 `mould`，并调用另一个 `run` 函数进行模拟。
    5. 将每次模拟的结果添加到 `g` 中，并更新条件数据 `current_cd`。
    6. 记录每次模拟的时间并输出。
    7. 当网格级别为 `1` 时，停止 `Stopwatch`，显示模拟结果，并返回模拟结果和总模拟时间。
