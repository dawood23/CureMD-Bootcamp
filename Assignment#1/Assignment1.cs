using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace assignment1
{
    public class Questions
    {
        //Question1
        public void MultiplicationTableGenerator(int number)
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{number} * {i} = {number * i}");
            }
        }

        //Question2
        public void EvenOrOdd(int number)
        {
            if (number % 2 == 0) { Console.WriteLine($"{number} is even"); }
            else Console.WriteLine($"{number} is odd");
        }

        //Question3
        public void MaxNum(int num1, int num2, int num3)
        {
            int largest;

            if (num1 > num2 && num1 > num3)
            {
                largest = num1;
            }
            else if (num2 > num1 && num2 > num3)
            {
                largest = num2;
            }
            else
            {
                largest = num3;
            }

            Console.WriteLine("The largest number is: " + largest);
        }

        //Question4
        public int Nsum(int n)
        {
            int sum = 0;
            for (int i = 1; i <= n; i++)
            {
                sum += i;
            }

            Console.WriteLine($"The sum of numbers from 1 to {n} is: {sum}");
            return sum;
        }

        //Question5
        public int ReversedNumber(int number)
        {
            int reversed = 0;

            while (number > 0)
            {
                int digit = number % 10;
                reversed = reversed * 10 + digit;
                number /= 10;
            }

            Console.WriteLine($"Reversed number is: {reversed}");
            return reversed;
        }

        //Question6
        public int Factorial(int number)
        {
            int factorial = 1;

            for (int i = 1; i <= number; i++)
            {
                factorial *= i;
            }

            Console.WriteLine($"The factorial of {number} is: {factorial}");

            return factorial;
        }

        //Question7
        public void LeapYear(int year)
        {
            if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
                Console.WriteLine($"{year} is a leap year.");
            else
                Console.WriteLine($"{year} is not a leap year.");
        }

        //Question8

        public void Fibonacci(int n)
        {
            int a = 0, b = 1;

            Console.Write("Fibonacci Series: ");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{a} ");
                int temp = a;
                a = b;
                b = temp + b;
            }
        }

        //Question 9
        public void PrimeNumber(int num)
        {
            bool isPrime = num > 1;

            for (int i = 2; i <= Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            if (isPrime)
                Console.WriteLine($"{num} is a prime number.");
            else
                Console.WriteLine($"{num} is not a prime number.");
        }

        //Question 10
        public int GCD(int a, int b)
        {
            int result = Math.Min(a, b);

            while (result > 0)
            {
                if (a % result == 0 && b % result == 0)
                {
                    break;
                }
                result--;
            }

            return result;
        }

        //Question#11
        public float SimpleCalculator(int num1, int num2, char op)
        {
            float result = 0;

            switch (op)
            {
                case '+':
                    result = num1 + num2;
                    Console.WriteLine($"Result: {num1 + num2}");
                    break;
                case '-':
                    result = num1 - num2;
                    Console.WriteLine($"Result: {num1 - num2}");
                    break;
                case '*':
                    result = num1 * num2;
                    Console.WriteLine($"Result: {num1 * num2}");
                    break;
                case '/':
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        Console.WriteLine($"Result: {num1 / num2}");
                    }
                    else
                        Console.WriteLine("Error: Division by zero.");
                    break;
                case '%':
                    if (num2 != 0)
                    {
                        result = num1 % num2;
                        Console.WriteLine($"Result: {num1 % num2}");
                    }
                    else
                        Console.WriteLine("Error: Modulo by zero.");
                    break;
                default:
                    Console.WriteLine("Invalid operation.");
                    break;

            }

            return result;
        }

        //Question#12
        public int CountDigits(int num)
        {
            int count = 0;
            long temp = Math.Abs(num);

            if (temp == 0)
                count = 1;
            else
            {
                while (temp > 0)
                {
                    temp /= 10;
                    count++;
                }
            }

            Console.WriteLine($"Number of digits: {count}");

            return count;
        }

        //Question#13
        public bool isPalindrome(int number)
        {
            if (number < 0) return false;

            int originalNumber = number;
            int reversedNumber = 0;

            while (number > 0)
            {
                int digit = number % 10;
                reversedNumber = reversedNumber * 10 + digit;
                number /= 10;
            }

            if (number == reversedNumber)
                Console.WriteLine($"Number {number} is Palindrome");
            else
                Console.WriteLine($"Number {number} is not a Palindrome");

            return originalNumber == reversedNumber;

        }

        //Question#14
        public int sumOfDigits(int number)
        {
            int temp = number;
            int sum = 0;

            while (temp > 0)
            {
                sum += temp % 10;
                temp /= 10;
            }
            Console.WriteLine($"Sum of Digits {number} is : {sum}");
            return sum;
        }

        //Question#15
        public bool IsArmstrong(int num)
        {
            string numStr = num.ToString();
            int digitsCount = numStr.Length;
            int sum = 0;

            foreach (char c in numStr)
            {
                int digit = (int)char.GetNumericValue(c);
                sum += (int)Math.Pow(digit, digitsCount);
            }

            if (sum == num) Console.WriteLine($"The number {num} is an armstrong");
            else Console.WriteLine($"The number {num} is not an armstrong");

            return sum == num;
        }

        //Question#16
        public void MinAndMax(int[] nums)
        {
            int min = nums[0];
            int max = nums[0];

            foreach (int num in nums)
            {
                if (num < min) min = num;
                if (num > max) max = num;

            }

            Console.WriteLine($"The min number is: {min}");
            Console.WriteLine($"The max number is: {max}");

        }

        //Question#17
        public int LinearSearch(int[] arr, int key)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == key)
                {
                    Console.WriteLine($"{key} found at index {i}");
                    return i;
                }
            }
            Console.WriteLine($"{key} not found in array.");
            return -1;
        }

        //Question#18
        public void SortArray(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            Console.WriteLine("Sorted Array: " + string.Join(", ", arr));
        }


        //Question#19
        public void EvenOddCounter(int[] arr)
        {
            int even = 0, odd = 0;
            foreach (int num in arr)
            {
                if (num % 2 == 0) even++;
                else odd++;
            }
            Console.WriteLine($"Even: {even}, Odd: {odd}");
        }

        //Question#20
        public void SortNames(List<string> names)
        {
            names.Sort();
            Console.WriteLine("Sorted Names: " + string.Join(", ", names));
        }


        //Question#21
        public void FrequencyCounter(int[] arr)
        {
            Dictionary<int, int> freq = new Dictionary<int, int>();
            foreach (int num in arr)
            {
                if (!freq.ContainsKey(num))
                    freq[num] = 0;
                freq[num]++;
            }
            foreach (var pair in freq)
                Console.WriteLine($"{pair.Key} => {pair.Value} times");
        }


        //Question#22
        public void MatrixAddition(int[,] mat1, int[,] mat2)
        {
            int rows = mat1.GetLength(0);
            int cols = mat1.GetLength(1);
            int[,] sum = new int[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    sum[i, j] = mat1[i, j] + mat2[i, j];

            Console.WriteLine("Matrix Sum:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(sum[i, j] + " ");
                Console.WriteLine();
            }
        }


        //Question#23
        public void VowelCounter(string input)
        {
            int count = 0;
            string vowels = "aeiouAEIOU";
            foreach (char c in input)
            {
                if (vowels.Contains(c)) count++;
            }
            Console.WriteLine($"Number of vowels: {count}");
        }


        //Question#24
        public void StringPalindrome(string input)
        {
            string clean = input.ToLower().Replace(" ", "");
            string reversed = new string(clean.Reverse().ToArray());

            if (clean == reversed)
                Console.WriteLine($"'{input}' is a palindrome.");
            else
                Console.WriteLine($"'{input}' is not a palindrome.");
        }


        //Question#25
        public void ReverseWords(string sentence)
        {
            string[] words = sentence.Split(' ');
            Array.Reverse(words);
            Console.WriteLine("Reversed sentence: " + string.Join(" ", words));
        }


        //Question#26
        public void RemoveDuplicates(int[] arr)
        {
            HashSet<int> unique = new HashSet<int>(arr);
            Console.WriteLine("Unique values: " + string.Join(", ", unique));
        }


        //Question#27
        public void StudentMarksManager()
        {
            Dictionary<string, int> marks = new Dictionary<string, int>();
            marks["Ali"] = 85;
            marks["Sara"] = 90;

            Console.WriteLine("All Students:");
            foreach (var pair in marks)
                Console.WriteLine($"{pair.Key}: {pair.Value}");

            // Search
            if (marks.TryGetValue("Ali", out int mark))
                Console.WriteLine($"Ali's marks: {mark}");

            // Update
            marks["Ali"] = 88;
            Console.WriteLine("Updated Ali's marks to 88");
        }


        //Question#28
        public void PatientVisitApp()
        {
            List<Patient> patients = new List<Patient>
            {
                new Patient { Name = "Ahmed", Reason = "Flu" }
            };

            // Add
            patients.Add(new Patient { Name = "Sana", Reason = "Fever" });

            // Search
            var found = patients.Find(p => p.Name == "Ahmed");
            Console.WriteLine($"Found patient: {found.Name}, Reason: {found.Reason}");

            // Update
            found.Reason = "Cold";
            Console.WriteLine("Updated reason for Ahmed");

            // Delete
            patients.RemoveAll(p => p.Name == "Sana");
            Console.WriteLine("Deleted patient Sana");
        }


        //Question#29
        public void WordFrequency(string paragraph)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            var words = paragraph.ToLower().Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (!wordCount.ContainsKey(word)) wordCount[word] = 0;
                wordCount[word]++;
            }

            foreach (var pair in wordCount)
                Console.WriteLine($"{pair.Key}: {pair.Value}");
        }


        //Question#30
        public string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
                sb.Append(chars[rand.Next(chars.Length)]);

            Console.WriteLine("Generated Password: " + sb.ToString());
            return sb.ToString();
        }

        // Patient Class for patient management
        public class Patient
        {
            public string Name { get; set; }
            public string Reason { get; set; }



            public static void Main(string[] args)
            {
                Questions q = new Questions();

                Console.WriteLine("\n===== Question 1: Multiplication Table =====");
                q.MultiplicationTableGenerator(5);

                Console.WriteLine("\n===== Question 2: Even or Odd =====");
                q.EvenOrOdd(7);

                Console.WriteLine("\n===== Question 3: Maximum of Three Numbers =====");
                q.MaxNum(3, 9, 5);

                Console.WriteLine("\n===== Question 4: Sum from 1 to N =====");
                q.Nsum(10);

                Console.WriteLine("\n===== Question 5: Reverse Number =====");
                q.ReversedNumber(1234);

                Console.WriteLine("\n===== Question 6: Factorial =====");
                q.Factorial(5);

                Console.WriteLine("\n===== Question 7: Leap Year =====");
                q.LeapYear(2024);

                Console.WriteLine("\n===== Question 8: Fibonacci Series =====");
                q.Fibonacci(7);

                Console.WriteLine("\n===== Question 9: Prime Number Check =====");
                q.PrimeNumber(29);

                Console.WriteLine("\n===== Question 10: GCD =====");
                Console.WriteLine("GCD: " + q.GCD(20, 30));

                Console.WriteLine("\n===== Question 11: Simple Calculator =====");
                q.SimpleCalculator(20, 5, '+');

                Console.WriteLine("\n===== Question 12: Count Digits =====");
                q.CountDigits(54321);

                Console.WriteLine("\n===== Question 13: Integer Palindrome =====");
                q.isPalindrome(121);

                Console.WriteLine("\n===== Question 14: Sum of Digits =====");
                q.sumOfDigits(456);

                Console.WriteLine("\n===== Question 15: Armstrong Number =====");
                q.IsArmstrong(153);

                Console.WriteLine("\n===== Question 16: Min and Max in Array =====");
                q.MinAndMax(new int[] { 1, 7, 3, 9, 2 });

                Console.WriteLine("\n===== Question 17: Linear Search =====");
                q.LinearSearch(new int[] { 4, 6, 9, 3 }, 9);

                Console.WriteLine("\n===== Question 18: Sort Array (Ascending) =====");
                q.SortArray(new int[] { 9, 5, 1, 3 });

                Console.WriteLine("\n===== Question 19: Even/Odd Counter in Array =====");
                q.EvenOddCounter(new int[] { 2, 3, 4, 5, 6 });

                Console.WriteLine("\n===== Question 20: Sort List of Names =====");
                q.SortNames(new List<string> { "Zain", "Ali", "Hina" });

                Console.WriteLine("\n===== Question 21: Frequency Counter in Array =====");
                q.FrequencyCounter(new int[] { 1, 2, 2, 3, 1, 1, 4 });

                Console.WriteLine("\n===== Question 22: Matrix Addition =====");
                int[,] mat1 = { { 1, 2 }, { 3, 4 } };
                int[,] mat2 = { { 5, 6 }, { 7, 8 } };
                q.MatrixAddition(mat1, mat2);

                Console.WriteLine("\n===== Question 23: Vowel Counter in String =====");
                q.VowelCounter("Hello World");

                Console.WriteLine("\n===== Question 24: String Palindrome =====");
                q.StringPalindrome("Madam");

                Console.WriteLine("\n===== Question 25: Reverse Words in Sentence =====");
                q.ReverseWords("Hello world this is C#");

                Console.WriteLine("\n===== Question 26: Remove Duplicates from Array =====");
                q.RemoveDuplicates(new int[] { 1, 2, 2, 3, 4, 4 });

                Console.WriteLine("\n===== Question 27: Student Marks Manager =====");
                q.StudentMarksManager();

                Console.WriteLine("\n===== Question 28: Patient Visit Console App =====");
                q.PatientVisitApp();

                Console.WriteLine("\n===== Question 29: Word Frequency Counter =====");
                q.WordFrequency("This is a test. This test is only a test.");

                Console.WriteLine("\n===== Question 30: Random Password Generator =====");
                q.GeneratePassword(12);

                Console.WriteLine("\n===== All functions tested successfully. =====\n");
            }



        }
    }
}