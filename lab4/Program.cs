using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class Pol
    {
        public Pol()
        {
            operators = new List<string>(standart_operators);
        }

        private List<string> operators;
        private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^" });

        private IEnumerable<string> Separate(string input)
        {
            int pos = 0;
            while (pos < input.Length)
            {
                string s = string.Empty + input[pos];
                if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                            s += input[i];
                    else if (Char.IsLetter(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
                            s += input[i];
                }
                yield return s;
                pos += s.Length;
            }
        }
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 4;
            }
        }

        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }

        public decimal result(string input)
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(ConvertToPostfixNotation(input));
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    decimal summ = 0;
                    try
                    {

                        switch (str)
                        {

                            case "+":
                                {
                                    decimal a = Convert.ToDecimal(stack.Pop());
                                    decimal b = Convert.ToDecimal(stack.Pop());
                                    summ = a + b;
                                    break;
                                }
                            case "-":
                                {
                                    decimal a = Convert.ToDecimal(stack.Pop());
                                    decimal b = Convert.ToDecimal(stack.Pop());
                                    summ = b - a;
                                    break;
                                }
                            case "*":
                                {
                                    decimal a = Convert.ToDecimal(stack.Pop());
                                    decimal b = Convert.ToDecimal(stack.Pop());
                                    summ = b * a;
                                    break;
                                }
                            case "/":
                                {
                                    decimal a = Convert.ToDecimal(stack.Pop());
                                    decimal b = Convert.ToDecimal(stack.Pop());
                                    summ = b / a;
                                    break;
                                }
                            case "^":
                                {
                                    decimal a = Convert.ToDecimal(stack.Pop());
                                    decimal b = Convert.ToDecimal(stack.Pop());
                                    summ = Convert.ToDecimal(Math.Pow(Convert.ToDouble(b), Convert.ToDouble(a)));
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    stack.Push(summ.ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }
            return Convert.ToDecimal(stack.Pop());
        }
    }

    class Program
    {
        static Random r = new Random();

        static void QuickSort(int[] array, int first, int last, ref int count)
        {
            int p = array[(last - first) / 2 + first];
            int temp;
            int i = first;
            int j = last;
            while(i <= j)
            {
                while (array[i] < p && i <= last) ++i;
                while (array[j] > p && j >= first) --j;
                if (i <= j)
                {
                    temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    ++i; --j;
                    count++;
                }
            }
            if (j > first) QuickSort(array, first, j, ref count);
            if (i < last) QuickSort(array, i, last, ref count);
        }

        static void BubbleSort(int[] array, ref int count)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                        count++;
                        swapped = true;
                    }
                }
                if(!swapped)
                   break;
            }
        }

        static void Input(int size, int[] array)
        {
            for (int i = 0; i < size; i++)
            {
                array[i] = r.Next(0, 99);
            }
        }

        static void Show(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine("\n");
        }

        static void DoSort()
        {
            Console.Write("Введите размерность массива: ");
            int size = int.Parse(Console.ReadLine());
            int[] array = new int[size];
            int[] array2 = new int[size];
            Console.WriteLine("Первоночальный массив:");
            Input(size, array);
            Show(array);
            Array.Copy(array, array2, size);
            int countSwap1 = 0;
            Console.WriteLine("После сортировки методом Пузырька:");
            BubbleSort(array,ref countSwap1);
            int n = (size - 1) * size / 2;
            Console.WriteLine("Количество перестановок = " + countSwap1);
            Console.WriteLine("Количество перестановок = " + n);
            Show(array);
            int countSwap2 = 0;
            Console.WriteLine("После сортировки методом Хоара:");
            QuickSort(array2, 0, size - 1, ref countSwap2);
            Console.WriteLine("Количество перестановок = " + countSwap2);
            Show(array2);
            Console.ReadLine();
        }

        public static void Polish()
        {
            Pol pol = new Pol();
            string str = "1,6*(4,9-5,7)/(0,8+2,3)";
            decimal res = pol.result(str);
            Console.WriteLine(res);
            Console.ReadLine();
        }

        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите действие");
                Console.WriteLine("1. Сортировка пузырьком / Метод Хоара");
                Console.WriteLine("2. Польская запись");
                Console.WriteLine("3. Выход");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        {
                            DoSort();
                            break;
                        }
                    case 2:
                        {
                            Polish();
                            break;
                        }
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default :
                        {
                            Console.WriteLine("Неверный выбор!");
                            break;
                        }
                }
            }
        }
    }
}
