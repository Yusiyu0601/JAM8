# Project Description
- [Chinese Version](README-zh.md)
- [English](README.md)

Documentation for the Use of JAM8 & snesim_with_reverse_query_search_tree
# 1. Introduction to JAM8
- **Basic Information**: JAM8 is a geostatistical algorithm development package based on the.net8 environment (hence also known as JAM8), using the C# language and developed with VS2022.
- **Operating Environment**: The operating environment is Windows 10 and later versions of Windows.
- **Usage Method**: The specific usage is very simple. Download the zip package from GitHub, unzip it directly, and then open the sln solution file with VS2022. All projects can be loaded, and the third-party packages used can be automatically obtained through nuget (an internet connection is required).
- **Composition of the Solution Projects**:
  - **JAM8 Main Project**: It includes common utility classes (in the Utilities folder) and an algorithm library (in the Algorithms\Geometry folder), including interpolation methods such as IDW interpolation and Kriging interpolation algorithms, multiple-point geostatistical methods such as ds and snesim, as well as the variogram and grid processing modules.
  - **JAM8.Work**: This is an interactive software project developed based on JAM8.
  - **JAM8.Console**: This is an interactive software project developed based on JAM8. It is the same as JAM8.Work, except that this project uses the console as the menu operation interface. The advantage is that it allows algorithm developers to avoid wasting time on interface design and quickly implement the writing and testing of a large number of methods.
  - **snesim_with_reverse_query_search_tree**: This is a snesim test project developed based on JAM8, mainly used to test the operation of the snesim program based on the reverse query search tree. 

# 2. Introduction to the Project snesim_with_reverse_query_search_tree
## 2.1 Instructions for Using snesim_with_reverse_query_search_tree
This project does not have a specific implementation of the snesim algorithm but calls the snesim class and its functions in JAM8. The significance of this project is to provide a clearer description of the calling functions for users of this new algorithm.
- **Sample Code for 2D snesim**
```
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            AllocConsole();//Open the console

            //Start FastSnesim simulation by using inverse retrieve search tree
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

### 2.1.1 Initialization Operations
1. **COM Wrapper Registration**: Call the `ComWrappers.RegisterForMarshalling` method to register `WinFormsComInterop.WinFormsComWrappers.Instance` for subsequent COM interoperation-related operations.
2. **Open the Console**: Call the `AllocConsole` function to open a console window for subsequent input and output operations.

### 2.1.2 Simulation Start Prompt
Output information to the console to prompt the start of the FastSnesim simulation using the inverse retrieve search tree.
    ```
    Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");
    ```

### 2.1.3 User Interaction Selection and Settings
1. **Choose Example Dimension**: Prompt the user to enter the example dimension, which can be "2d" or "3d". The program will read the user's input and store it in the variable `b`.
    ```
    Choose Example Dimension (input 2d or 3d) =>
    ```
2. **Set the Ratio of the Inverse Retrieve Search Tree**: Prompt the user to enter the ratio of the inverse retrieve search tree, which means what percentage of the simulation progress will use the reverse search tree query method, and the rest will use the traditional forward query. The range is between 0 and 100. The program will read the user's input integer and store it in the variable `ratio_inverseRetrieve`. It is recommended to set it to 35.
    ```
    set ratio of inverse retrieve search tree (input 0 ~ 100) =>
    ```

### 2.1.4 Processing Flow for the 2D Example (The operation flow for the 3D example is the same)
If the user selects the example dimension as "2d", the following steps will be executed:
1. **Load the Training Image**
    - Output prompt information in yellow font on the console to prompt loading the 2D training image.
    - Call the `Grid.create_from_gslibwin` method to pop up a window for the user to select the training image file (for example: test data\Training Image B.out), and obtain the input grid and file name.
		![select training image](/images/select_training_image_2d.jpg)<br>
    - Call the `input_grid.select_gridProperty_win` method to pop up a window for the user to select a property as the training image, and obtain the corresponding grid property `TI`.
		![select training image](/images/select_property_as_training_image.jpg)<br>
    - Output the file name and the text information of the training image grid structure on the console.
2. **Set the Simulation Grid Size**
    - Output prompt information in yellow font on the console to prompt setting the simulation grid size.
    - First, create a simple grid structure `re_gs` of 100x100x1.
    - Call the `GridStructure.create_win` method to pop up a window for the user to set `re_gs` and update the simulation grid structure.
    - Output the text information of the updated simulation grid structure on the console.
3. **Load the Conditional Data**
    - Prompt the user whether to use the 2D conditional data, and the user can enter "Y" or "N". The program will read the user's input.
    ```
    use conditional data(2d) or not? (input Y or N) =>
    ```
    - If the user enters "Y", call the `CData2.read_from_gslib_win` method to pop up a window for the user to select the conditional data file (for example: test data\cd_for_trainingImage_B.out), and obtain the conditional data `cd` and the file name.
		![select training image](/images/read_conditional_data.jpg)<br>
    - Output the file name of the conditional data on the console.
    - Call the `cd.coarsened` method to coarsen the conditional data to obtain the coarsened conditional data `coarsened_cd` and the coarsened grid `coarsened_grid`, and pop up a window to display the coarsened grid.
4. **Run the FastSnesim Simulation**
    - Create a `Snesim` object `snesim`.
    - Call the `snesim.run` method to perform the simulation, and pass in the parameters required for the simulation (such as the seed number, the number of realizations, the maximum number of searches, the search template size, the training image, the conditional data, the simulation grid structure, the ratio of the inverse retrieve search tree), and obtain the simulation result `re` and the simulation time `time`.
5. **Display the Simulation Result and Time**
    - Pop up a window to display the simulation result `re`. It should be noted that JAM8 cannot display true 3D and can only display it in the form of 2D slices. If you need to view the result in 3D, you need to click the export module in the display interface, export the result as gslib, and then use other model visualization software to display the result.
    - Output the time spent on the simulation (in milliseconds) in red font on the console. 

## 2.2 Description of snesim
The path of the snesim algorithm class is JAM8\Algorithms\Geometry\Simulate\Snesim. This class contains two `run` functions.
- **The simulation function for the specified grid level is named**
   ```
   public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
   ```
1. **Parameter Meanings**:
    - `TI`: Training image, of type `GridProperty`.
    - `cd`: Conditional data, of type `CData2`, which can be `null`.
    - `gs_re`: Simulation grid structure, of type `GridStructure`.
    - `random_seed`: Random seed, used to initialize the random number generator.
    - `mould`: Simulation template, of type `Mould`.
    - `multigrid_level`: Multigrid level, with a default value of 1.
    - `progress_for_retrieve_inverse`: In the simulation progress, the proportion of reverse query in the previous simulation progress, with a default value of 0.
2. **Implementation Process**:
    1. Initialize a `Random` object `rnd` using `random_seed`, and create a `Grid` object `result` according to `gs_re`.
    2. If there is conditional data `cd`, coarsen it and add the coarsened conditional data to `result`; otherwise, directly add an attribute to `result`.
    3. Create a search tree `tree` according to `mould` and `TI`.
    4. Initialize some dictionaries and lists to store information such as nodes and probability distributions.
    5. Create a simulation path `path`.
    6. Start a `Stopwatch` to record the simulation time (excluding the time for building the search tree).
    7. When the simulation path has not been fully visited, loop through the following operations:
        - If the simulation progress is an integer and different from the previous progress, stop the `Stopwatch`, record the time and accumulate the total time, and then restart the `Stopwatch`.
        - Print the simulation progress.
        - Obtain the next node `si` to be visited. If the value of this node is empty, calculate the conditional probability `cpdf` according to the search tree and the current progress, and sample a node value according to `cpdf` and set it in `result`.
    8. Stop the `Stopwatch` and return the simulation result `result` and the total simulation time. 
- **The other `run` function is the multigrid simulation function**
   ```
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
    - `progress_for_retrieve_inverse`: In the simulation progress, the proportion of reverse query in the previous simulation progress, with a default value of 0.
2. **Implementation Process**:
    1. Start a `Stopwatch` to detect the simulation time.
    2. Create a `Grid` object `g` according to `gs_re`.
    3. If there is conditional data `cd`, coarsen it and add the coarsened conditional data to `g`.
    4. Loop from `multigrid_count` to 1, create a mould `mould` according to the grid level, and call another `run` function for simulation.
    5. Add the result of each simulation to `g` and update the conditional data `current_cd`.
    6. Record the time of each simulation and output it.
    7. When the grid level is 1, stop the `Stopwatch`, display the simulation result, and return the simulation result and the total simulation time. 