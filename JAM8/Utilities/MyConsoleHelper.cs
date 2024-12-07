namespace JAM8.Utilities
{
    public class MyConsoleHelper
    {
        public static int read_int_from_console(string content, string prompt = null)
        {
            prompt = prompt ?? "无说明";
            Console.WriteLine();
            Console.Write($"{content}<整数> [{prompt}]\n\t输入值=>");
            string input = Console.ReadLine();
            return int.Parse(input);
        }
        public static float read_float_from_console(string content, string prompt = null)
        {
            prompt = prompt ?? "无说明";
            Console.WriteLine();
            Console.Write($"{content}<浮点数> [{prompt}]\n\t输入值=>");
            string input = Console.ReadLine();
            return float.Parse(input);
        }
        public static double read_double_from_console(string content, string prompt = null)
        {
            prompt = prompt ?? "无说明";
            Console.WriteLine();
            Console.Write($"{content}<双精度浮点数> [{prompt}]\n\t输入值=>");
            string input = Console.ReadLine();
            return double.Parse(input);
        }
        public static string read_string_from_console(string content, string prompt = null)
        {
            prompt = prompt ?? "无说明";
            Console.WriteLine();
            Console.Write($"{content}<字符> [{prompt}]\n\t输入值=>");
            string input = Console.ReadLine();
            return input;
        }

        public static void write_string_to_console(string content, string prompt = null)
        {
            Console.WriteLine($"{content} [{prompt}]");
        }
    }
}
