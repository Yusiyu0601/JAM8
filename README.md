# Project Description

- [Chinese Version](README-zh.md)
- [English Version](README.md)

Documentation for the Usage of JAM8 & snesim_with_reverse_query_search_tree

# 1. Introduction to JAM8

- **Basic Information**: JAM8 is a geostatistical algorithm development package based on the.net8 environment (hence the name JAM8), developed in C# language using VS2022.
- **Operating Environment**: The operating environment is Windows 10 and later Windows versions.
- **Usage Method**: The specific usage is simple. Download the zip package from Github, extract it directly, and then open the sln solution file using VS2022. All projects can be loaded, and the third-party packages used can be automatically obtained through nuget (an internet connection is required).
- **Composition of the Solution Projects**:
  - **JAM8 Main Project**: It includes common utility classes (in the Utilities folder) and an algorithm library (in the Algorithms\Geometry folder), including interpolation methods such as IDW interpolation and kriging interpolation algorithms, multiple-point geostatistical methods such as ds and snesim, as well as variogram and grid processing modules.
  - **JAM8.Work**: This is an interactive software project developed based on JAM8.
  - **JAM8.Console**: It is an interactive software project developed based on JAM8. Similar to JAM8.Work, the difference is that this project uses the console as the menu operation interface. The advantage is that algorithm developers do not need to waste time on interface design and can quickly implement the writing and testing of a large number of methods.
  - **snesim_with_reverse_query_search_tree**: This is a snesim test project developed based on JAM8, mainly used to test the operation of the snesim program based on the reverse query search tree.

# 2. Introduction to the Project snesim_with_reverse_query_search_tree

## 2.1 Usage Instructions for snesim_with_reverse_query_search_tree

This project does not have a specific implementation of the snesim algorithm but calls the snesim class and its functions in JAM8. The significance of this project is to provide users of this new algorithm with a clearer description of the call functions.

- **Example Code for 2D snesim**

```
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            AllocConsole();// Open the console

            // Start FastSnesim simulation by using inverse retrieve search tree
            Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");

            // 1. Choose Example Dimension
            string b = EasyConsole.Input.ReadString("Choose Example Dimension (input 2d or 3d) => ");

            // 2. Set ratio of inverse retrieve search tree
            int ratio_inverseRetrieve = Input.ReadInt("set ratio of inverse retrieve search tree (input 0 ~ 100) => ", 0, 100);

            #region 2d Example

            if (b == "2d")
            {
                // 3. Load Training Image
                Output.WriteLine(ConsoleColor.Yellow, "\nLoad Training Image(2d)");
                var (input_grid, input_fileName) = Grid.create_from_gslibwin("Load Training Image");
                GridProperty TI = input_grid.select_gridProperty_win("Select Property as Training Image").grid_property;
                Output.WriteLine($"\n\tfileName  {input_fileName}");
                Output.WriteLine(TI.gridStructure.view_text());

                // 4. Set Simulation Grid Size
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure re_gs = GridStructure.create_simple(100, 100, 1);
                re_gs = GridStructure.create_win(re_gs, "Set Simulation Grid Size");
                Output.WriteLine(re_gs.view_text());

                // 5. Load Conditional Data
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData2 cd = null;
                CData2 coarsened_cd = null;
                Grid coarsened_grid = null;
                if (is_use_cd == "Y")
                {
                    string cd_fileName = "";
                    (cd, cd_fileName) = CData2.read_from_gslib_win();
                    Output.WriteLine($"\n\tconditional data fileName  {cd_fileName}\n");
                    // coarsened conditional data and show coarsened grid
                    (coarsened_cd, coarsened_grid) = cd.coarsened(re_gs);
                    coarsened_grid.showGrid_win();
                }

                // 6. Run FastSnesim Simulation
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 60, (7, 7, 0), TI, cd, re_gs, ratio_inverseRetrieve);

                // 7. Show Simulation Result and Show Simulation Time
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"\nTime {time} milliseconds");
            }

            #endregion
```

### 2.1.1 Initialization Operations

1. **COM Wrapper Registration**: Call the `ComWrappers.RegisterForMarshalling` method to register `WinFormsComInterop.WinFormsComWrappers.Instance` for subsequent COM interoperation-related operations.
2. **Open the Console**: Call the `AllocConsole` function to open a console window for subsequent input and output operations.

### 2.1.2 Simulation Start Prompt

Output information to the console to prompt the start of the FastSnesim simulation using the inverse retrieve search tree.
```
Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");
```

### 2.1.3 User Interaction Selection and Settings

1. **Select Example Dimension**: Prompt the user to enter the example dimension, which can be "2d" or "3d". The program will read the user's input and store it in the variable `b`.
```
Choose Example Dimension (input 2d or 3d) =>
```
2. **Set the Ratio of the Inverse Retrieve Search Tree**: Prompt the user to enter the ratio of the inverse retrieve search tree, which indicates what percentage of the simulation progress will use the reverse search tree query method, and the rest will use the traditional forward query. The range is between 0 and 100. The program will read the user's input integer and store it in the variable `ratio_inverseRetrieve`. It is recommended to set it to 35.
```
set ratio of inverse retrieve search tree (input 0 ~ 100) =>
```

### 2.1.4 Processing Flow of the 2D Example (The Operation Process of the 3D Example is the Same)

If the user selects the example dimension as "2d", the following steps will be executed:

1. **Load the Training Image**
   - Output prompt information in yellow font in the console to prompt the loading of the 2D training image.
   - Call the `Grid.create_from_gslibwin` method to pop up a window for the user to select the training image file (e.g., test data\Training Image B.out) to obtain the input grid and the file name.<br>
     ![select training image](/images/(1)select_training_image_2d.jpg)<br>
   - Call the `input_grid.select_gridProperty_win` method to pop up a window for the user to select a property as the training image to obtain the corresponding grid property `TI`.<br>
     ![select training image](/images/(2)select_property_as_training_image.jpg)<br>
   - Output the file name and the text information of the training image grid structure in the console.

2. **Set the Simulation Grid Size**
   - Output prompt information in yellow font in the console to prompt the setting of the simulation grid size.
   - First, create a simple grid structure `re_gs` of 100x100x1.
   - Call the `GridStructure.create_win` method to pop up a window for the user to set `re_gs` and update the simulation grid structure.
   - Output the text information of the updated simulation grid structure in the console.

3. **Load Conditional Data**
   - Prompt the user whether to use 2D conditional data, and the user can enter "Y" or "N". The program will read the user's input.
```
use conditional data(2d) or not? (input Y or N) =>
```
   - If the user enters "Y", call the `CData2.read_from_gslib_win` method to pop up a window for the user to select the conditional data file (e.g., test data\cd_for_trainingImage_B.out) to obtain the conditional data `cd` and the file name.<br>
     ![select training image](/images/(3)read_conditional_data.jpg)<br>
   - Output the file name of the conditional data in the console.
   - Call the `cd.coarsened` method to coarsen the conditional data to obtain the coarsened conditional data `coarsened_cd` and the coarsened grid `coarsened_grid`, and pop up a window to display the coarsened grid.

4. **Run the FastSnesim Simulation**
   - Create a `Snesim` object `snesim`.
   - Call the `snesim.run` method to perform the simulation, and pass in the parameters required for the simulation (such as the seed number, the number of realizations, the maximum number of searches, the size of the search template, the training image, the conditional data, the simulation grid structure, and the ratio of the inverse retrieve search tree) to obtain the simulation result `re` and the simulation time `time`.

5. **Display the Simulation Result and Time**
   - Pop up a window to display the simulation result `re`. It should be noted that JAM8 cannot display true 3D and can only display the result in the form of 2D slices. If you need to view the result in 3D, you need to click the export module in the display interface to export the result as gslib, and then use other model visualization software to display the result.
   - Output the simulation time spent (in milliseconds) in red font in the console.

## 2.2 snesim Instructions

The path of the snesim algorithm class is JAM8\Algorithms\Geometry\Simulate\Snesim, and this class contains two `run` functions.

- **The simulation function name at the specified grid level is**

```
public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
```

1. **Parameter Meanings**:
   - `TI`: Training image, of type `GridProperty`.
   - `cd`: Conditional data, of type `CData2`, which can be `null`.
   - `gs_re`: Simulation grid structure, of type `GridStructure`.
   - `random_seed`: Random seed, used to initialize the random number generator.
   - `mould`: Simulation template, of type `Mould`.
   - `multigrid_level`: Multigrid level, with a default value of `1`.
   - `progress_for_retrieve_inverse`: In the simulation progress, the proportion of the reverse query in the previous simulation progress, with a default value of `0`.
2. **Implementation Process**:
   1. Initialize a `Random` object `rnd` using `random_seed`, and create a `Grid` object `result` according to `gs_re`.
   2. If there is conditional data `cd`, coarsen it and add the coarsened conditional data to `result`; otherwise, directly add a property to `result`.
   3. Create a search tree `tree` according to `mould` and `TI`.
   4. Initialize some dictionaries and lists to store nodes, probability distributions, etc.
   5. Create a simulation path `path`.
   6. Start a `Stopwatch` to record the simulation time (excluding the time to build the search tree).
   7. When the simulation path has not been fully accessed, loop through the following operations:
      - If the simulation progress is an integer and different from the previous progress, stop the `Stopwatch`, record the time and accumulate the total time, and then restart the `Stopwatch`.
      - Print the simulation progress.
      - Get the next node `si` to be accessed. If the value of this node is empty, calculate the conditional probability `cpdf` according to the search tree and the current progress, and sample to get the node value according to `cpdf` and set it in `result`.
   8. Stop the `Stopwatch` and return the simulation result `result` and the total simulation time.

- **The other `run` function is the multigrid simulation function**

```c#
public (Grid, double time) run(int random_seed, int multigrid_count, int max_number,(int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_re,int progress_for_retrieve_inverse = 0)  
```

1. **Parameter Meanings**:
   - `random_seed`: Random seed, used to initialize the random number generator.
   - `multigrid_count`: The total number of multigrids.
   - `max_number`: The total number of nodes in the actual template.
   - `template`: A tuple containing three integers, representing the dimensions of the template in the `x`, `y`, and `z` directions respectively.
   - `TI`: Training image, of type `GridProperty`.
   - `cd`: Conditional data, of type `CData2`, which can be `null`.
   - `gs_re`: The size of the simulation grid structure, of type `GridStructure`.
   - `progress_for_retrieve_inverse`: In the simulation progress, the proportion of the reverse query in the previous simulation progress, with a default value of `0`.
2. **Implementation Process**:
   1. Start a `Stopwatch` to detect the simulation time.
   2. Create a `Grid` object `g` according to `gs_re`.
   3. If there is conditional data `cd`, coarsen it and add the coarsened conditional data to `g`.
   4. Loop from `multigrid_count` to `1`, create a mould `mould` according to the grid level, and call another `run` function for simulation.
   5. Add the result of each simulation to `g` and update the conditional data `current_cd`.
   6. Record the simulation time of each time and output it.
   7. When the grid level is `1`, stop the `Stopwatch`, display the simulation result, and return the simulation result and the total simulation time.

## 2.3 Example Demonstration

Data Usage Instructions: The example training images are located in the test data directory of the project snesim_with_reverse_query_search_tree. The ones without the 3d suffix indicate 2D training images. There is a conditional data file cd_for_trainingImage_B.out in this directory, which can be used for 2D conditional constraint simulation testing.

Computer Computing Hardware and System: The CPU is 13th Gen Intel(R) Core(TM) i7-13790F (16 cores (8 performance cores + 8 efficiency cores), a total of **24 threads**), and the system is Windows 11.

### 2.3.1 2D Example

- The test data used includes **training image** (test data\Training Image B.out) and **conditional data** (test data\cd_for_trainingImage_B.out).
- The test uses the **Release** running mode of VS2022.

Press `Ctrl+F5 (Run without Debugging)` to start the program and enter the parameter setting console interface.

```
start FastSnesim simulation by using inverse retrieve search tree
```

According to the prompt, enter the dimension of the simulation test example and select **2d**, indicating the simulation of a 2D example.

```
Choose Example Dimension (input 2d or 3d) => 2d
```

Then, it prompts to set the proportion of the reverse query search tree. To compare the changes in simulation speed, set it to 0 for the first simulation, indicating that no reverse query is used; set it to 35 for the second test, setting the first 35% of the simulation progress to use the reverse simulation.

The first simulation:

```
set ratio of inverse retrieve search tree (input 0 ~ 100) => 0
```

The second simulation:

```
set ratio of inverse retrieve search tree (input 0 ~ 100) => 35
```

At this time, it prompts to load the training image.

```
Load Training Image(2d)
```

Select the **Training Image B.out** file in the form.

<img src="/images/(4)Select the Training Image B.out file in the form.png" alt="image-20250404151110498" style="zoom:50%;" />

Click the OK button to read the GSLIB format Grid. If there are multiple properties in the Grid file, you need to select the target property. Since there is only one property in the Grid at this time, just click the Select GridProperty button to complete the import of the training image.

<img src="/images/(5)Select GridProperty.png" alt="image-20250404151342688" style="zoom:50%;" />

At this point, you are prompted to set the simulation grid size.

```
Set Simulation Grid Size
```

If the simulation grid size is too small, the simulation time will be very short, and it may not be possible to effectively compare the computational efficiency. Set the size to 500×500.

<img src="/images/(6)Set Simulation Grid Size.png" alt="image-20250404152138545" style="zoom:50%;" />

Use the conditional simulation mode.

```
use conditional data(2d) or not? (input Y or N) => Y
```

**Other Parameters**

| **Parameter Name** | **Example Value** | **Type/Description** |
| :---: | :---: | --- |
| `random_seed` | `1001` | `int`: Random number seed used to reproduce the results. |
| `multigrid_count` | `1` | `int`: The number of multigrid levels. |
| `max_number` | `60` | `int`: The maximum number of iterations or conditions for the simulation. |
| `template` | `(7, 7, 0)` | `(int rx, int ry, int rz)`: Template size (radius in the X/Y/Z directions, 0 means ignoring that dimension). |

**Computation Time Table**

| **Ratio of Inverse Retrieve Search Tree (%)** | **Time (s)** | **Speedup (x)** |
| :---: | :---: | :---: |
| 0 | 51.47 | 1.00 (Baseline) |
| 35 | 11.23 | 4.58 |

### 2.3.2 3D Example

The **3D training image** used in the test is (test data\Training Image C(3d).out).

**Test Parameter Table**

| **Parameter Name** | **Example Value** | **Type/Description** |
| :---: | :---: | --- |
| `random_seed` | `1001` | `int`: Random number seed used to reproduce the results. |
| `multigrid_count` | `1 or 3` | `int`: The number of multigrid levels. |
| `max_number` | 80 | `int`: The maximum number of iterations or conditions for the simulation. |
| `template` | `(7, 7, 3)` | `(int rx, int ry, int rz)`: Template size (radius in the X/Y/Z directions, 0 means ignoring that dimension). |
| `TI` | `Training Image C(3d).out` | Training image file |
| `cd` | `null` | Conditional data |
| `re_gs` | `100×100×30` | Simulation grid size |

**Computation Time Table**

| Multigrid Count | **Ratio of Inverse Retrieve Search Tree (%)** | Time (s) | **Speedup per Level (x)** |
| :---: | :---: | :---: | :---: |
| **3** | 0 | 592 s | 1.00x (Baseline) |
| **3** | 35 | 145 s | **4.08x** (vs. 3 - Level = 592s) |
| **1** | 0 | 1152 s | 1.00x (Baseline) |
| **1** | 35 | 128 s | **9.00x** (vs. 1 - Level = 1152s) |