/* FILE NAME   : Program.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 13.02.2020.
 * NOTE        : None.
 *
 * Copyright © 2020 Leonid Zaytsev. All rights reserved.
 */
using System;

// Project namespace
namespace BTree
{
    /// <summary>
    /// Сlass containing entry point to the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main program method.
        /// </summary>
        /// <param name="args">Input parameters to the program</param>
        static void Main(String[] args)
        {
            BTree<Int32> tree = new BTree<Int32>(2);

            Int32[] a = new[] {7, 5, 9, 8, 2, 3, 27, 26, 19, 13, 99, 96, 11};

            tree.Add(7);
            tree.Add(5);
            tree.Add(9);
            tree.Add(8);
            tree.Add(2);
            tree.Add(3);
            tree.Add(27);
            tree.Add(26);
            tree.Add(19);
            tree.Add(13);
            tree.Add(99);
            tree.Add(96);
            tree.Add(11);

            Console.WriteLine(tree.ToString());

            foreach (var el in a)
            {
                Console.Write(tree.Contains(el) + ", ");
            }
        } // End of 'Main' method
    } // End of 'Program' class
} // end of 'BTree' namespace

// END OF 'Program.cs' FILE
