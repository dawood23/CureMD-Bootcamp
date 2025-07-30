using System;
using System.Collections.Generic;

namespace Assignment_02
{
    public class Node
    {
        public int value;
        public Node next;

        public Node()
        {
            value = 0;
            next = null;
        }
        public Node(int value)
        {
            this.value = value;
            next = null;
        }
    }
    public class CustomLinkedList
    {
        public Node head;

        public void Add(int data)
        {
            Node newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.next != null)
                {
                    current = current.next;
                }
                current.next = newNode;
            }
        }
    }

    class Questions
    {
        //Question#01
        public bool balancesParanthesis(string expression)
        {
            char[] arr = expression.ToCharArray();
            Stack<int> s = new Stack<int>();

            for(int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == '(')
                {
                    s.Push(arr[i]);
                }
                else if (arr[i] == ')')
                {
                    if (s.Count == 0) return false;

                    s.Pop();
                }
            }
            return s.Count==0;
        }

        //Question#02
        public void ReverseQueue(Queue<int> Que)
        {
            Stack<int> stack = new Stack<int>();

            while(Que.Count>0)
            {
                stack.Push(Que.Dequeue());
            }
            while (stack.Count > 0)
            {
                Que.Enqueue(stack.Pop());
            }
        }
        //Question#03
        public void TraverseLL(Node head)
        {
            Node curr = head;
            while (curr != null)
            {
                Console.Write(curr.value + " ");
                curr = curr.next;
            }
        }

        //Question#4
        public void ArrayRotation(int[] arr,int k)
        {
            for(int i = 0; i < k; i++)
            {
                int element = arr[0];


                for (int j = 0; j < arr.Length - 1; j++)
                {
                    arr[j] = arr[j + 1];
                }

                arr[arr.Length - 1] = element;
            }
         
        }

         public static void Main(string[] args)
        {
            Questions question = new Questions();

            Console.WriteLine("Question#1");
            Console.WriteLine("Input: (a+b)*c  -- Output: " + question.balancesParanthesis("(a+b)*c"));
            Console.WriteLine("Input: ((a+b)*c)  -- Output: " + question.balancesParanthesis("((a+b)*c)"));
            Console.WriteLine("Input: (a+b)*(c-d  -- Output: " + question.balancesParanthesis("(a+b)*(c-d"));

            Console.WriteLine();
            Console.WriteLine("Question#2");
            Queue<int> que = new Queue<int>();
            que.Enqueue(1);
            que.Enqueue(2);
            que.Enqueue(3);
            que.Enqueue(4);
            Console.WriteLine("Actual Queue");

            foreach(int items in que)
            {
                Console.Write(items+" ");
            }
            question.ReverseQueue(que);
            Console.WriteLine();
            Console.WriteLine("Reversed Queue");

            foreach(int items in que)
            {
                Console.Write(items+" ");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Question#3");
            Console.WriteLine("Traversing The Custom LL");

            CustomLinkedList list = new CustomLinkedList();
            list.Add(10);
            list.Add(20);
            list.Add(30);
            question.TraverseLL(list.head);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Question#4");
            int[] arr = new int[] { 1, 2, 3, 4, 5 };
            Console.Write("Input Array: ");
            foreach(int item in arr)
            {
                Console.Write(item + " ");
            }
            Console.Write(" k=2");

            Console.WriteLine();

            question.ArrayRotation(arr, 2);
            Console.Write("Output Array: ");
            foreach (int item in arr)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
