using System;
using System.Numerics;
using System.Security.Cryptography;
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
        public int GCD(int a,int b)
        {
           int result=Math.Min(a,b);

            while (result > 0)
            {
                if(a%result==0 && b % result == 0)
                {
                    break;
                }
                result--;
            }

            return result;
        }

        //Question#11
        public float SimpleCalculator(int num1,int num2,char op)
        {
            float result=0;

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

            while (number>0) 
        {
                int digit = number % 10;            
                reversedNumber = reversedNumber * 10 + digit;  
                number /= 10;                      
            }

            if (number==reversedNumber)
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



        public static void Main(string[] args)
                {
                    Questions questions = new Questions();

                    questions.MultiplicationTableGenerator(6);

                    questions.EvenOrOdd(6);

            questions.sumOfDigits(123);
            questions.IsArmstrong(153);
            questions.MinAndMax([1,2,3,4,5]);
                }

    }
}