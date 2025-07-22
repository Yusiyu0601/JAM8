namespace JAM8.Utilities
{
    /// <summary>
    /// Console 辅助类，提供从控制台读取不同类型输入的方法。
    /// </summary>
    public class MyConsoleHelper
    {
        /// <summary>
        /// 从控制台读取一个整数输入，并根据提供的提示信息进行显示。
        /// </summary>
        /// <param name="content">提示信息的主要内容，用于描述需要输入的内容。</param>
        /// <param name="prompt">可选参数，提供额外的提示信息。如果为 null，则不显示提示。</param>
        /// <returns>用户输入的有效整数。</returns>
        public static int read_int_from_console(string content, string prompt = null)
        {
            while (true)
            {
                // 输出空行以提高可读性
                Console.WriteLine();

                // 如果提供了提示信息，显示内容+提示；否则只显示内容
                Console.WriteLine(!string.IsNullOrEmpty(prompt) ? $@"{content} [{prompt}]" : content);

                // 提示用户输入整数
                Console.Write(@"请输入整数 => ");

                // 读取用户输入
                var input = Console.ReadLine();

                // 校验输入是否为有效的整数
                if (int.TryParse(input, out var result))
                {
                    // 如果解析成功，返回结果
                    return result;
                }
                else
                {
                    // 如果解析失败，提示用户重新输入
                    Console.WriteLine(@"输入无效，请输入一个有效的整数。");
                    // 递归调用该函数，直到输入有效值
                }
            }
        }

        /// <summary>
        /// 从控制台读取一个浮点数输入，并根据提供的提示信息进行显示。
        /// </summary>
        /// <param name="content">提示信息的主要内容，用于描述需要输入的内容。</param>
        /// <param name="prompt">可选参数，提供额外的提示信息。如果为 null，则不显示提示。</param>
        /// <returns>用户输入的有效浮点数。</returns>
        public static float read_float_from_console(string content, string prompt = null)
        {
            // 输出空行以提高可读性
            Console.WriteLine();

            // 如果提供了提示信息，显示内容+提示；否则只显示内容
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine($@"{content} [{prompt}]");
            }
            else
            {
                Console.WriteLine(content);
            }

            // 提示用户输入浮点数
            Console.Write("请输入浮点数 => ");

            // 读取用户输入
            string input = Console.ReadLine();

            // 校验输入是否为有效的浮点数
            if (float.TryParse(input, out float result))
            {
                // 如果解析成功，返回结果
                return result;
            }
            else
            {
                // 如果解析失败，提示用户重新输入
                Console.WriteLine(@"输入无效，请输入一个有效的浮点数。");
                // 递归调用该函数，直到输入有效值
                return read_float_from_console(content, prompt);
            }
        }

        /// <summary>
        /// 从控制台读取一个双精度浮点数输入，并根据提供的提示信息进行显示。
        /// </summary>
        /// <param name="content">提示信息的主要内容，用于描述需要输入的内容。</param>
        /// <param name="prompt">可选参数，提供额外的提示信息。如果为 null，则不显示提示。</param>
        /// <returns>用户输入的有效双精度浮点数。</returns>
        public static double read_double_from_console(string content, string prompt = null)
        {
            // 输出空行以提高可读性
            Console.WriteLine();

            // 如果提供了提示信息，显示内容+提示；否则只显示内容
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine($@"{content} [{prompt}]");
            }
            else
            {
                Console.WriteLine(content);
            }

            // 提示用户输入双精度浮点数
            Console.Write("请输入双精度浮点数 => ");

            // 读取用户输入
            string input = Console.ReadLine();

            // 校验输入是否为有效的双精度浮点数
            if (double.TryParse(input, out double result))
            {
                // 如果解析成功，返回结果
                return result;
            }
            else
            {
                // 如果解析失败，提示用户重新输入
                Console.WriteLine(@"输入无效，请输入一个有效的双精度浮点数。");
                // 递归调用该函数，直到输入有效值
                return read_double_from_console(content, prompt);
            }
        }

        /// <summary>
        /// 从控制台读取一个字符串输入，并根据提供的提示信息进行显示。
        /// </summary>
        /// <param name="content">提示信息的主要内容，用于描述需要输入的内容。</param>
        /// <param name="prompt">可选参数，提供额外的提示信息。如果为 null，则不显示提示。</param>
        /// <returns>用户输入的字符串。如果输入为空，函数会提示重新输入并递归调用。</returns>
        public static string read_string_from_console(string content, string prompt = null)
        {
            // 输出空行，以提高可读性
            Console.WriteLine();

            // 如果提供了提示信息，显示内容+提示；否则只显示内容
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine($@"{content} [{prompt}]");
            }
            else
            {
                Console.WriteLine(content);
            }

            // 提示用户输入字符串
            Console.Write("请输入字符串 => ");

            // 读取用户输入的字符串
            string input = Console.ReadLine();

            // 输入校验：如果输入为空，则提示用户重新输入
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine(@"输入不能为空，请重新输入。");
                // 递归调用该函数，直到输入有效值
                return read_string_from_console(content, prompt);
            }

            // 返回有效的输入值
            return input;
        }

        /// <summary>
        /// 向控制台输出一段字符串内容，并根据提供的提示信息显示额外说明。
        /// 如果 content 为空，则仅输出空行。
        /// </summary>
        /// <param name="content">主要输出内容。</param>
        /// <param name="prompt">可选参数，提供额外的说明信息。如果为 null，则不显示说明。</param>
        public static void write_string_to_console(string content = null, string prompt = null)
        {
            // 输出一个空行，便于视觉分隔
            Console.WriteLine();

            // 如果 content 为 null 或空，则仅输出空行
            if (string.IsNullOrEmpty(content))
            {
                return; // 直接返回，不输出任何内容
            }

            // 如果提供了提示信息，显示内容+提示；否则只显示内容
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine($@"{content} [{prompt}]");
            }
            else
            {
                Console.WriteLine(content);
            }
        }


    }
}
