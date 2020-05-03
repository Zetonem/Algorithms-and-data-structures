/* FILE NAME   : UnitTest1.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 03.05.2020.
 * NOTE        : None.
 *
 * Copyright © 2020 Leonid Zaytsev. All rights reserved.
 */
using System;
using NUnit.Framework;
using BTree;

// Test project namespace
namespace MainTest
{
    public class Tests
    {
        [TestCase()]
        [TestCase(100, 200, 300, 400, 75, 500, 60, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 25, 50)]
        [TestCase(100, 1000, 1100, 300, 400, 75, 500, 60, 1300, 1400, 1500, 1600, 700, 800, 900, 1200, 1700, 200, 25, 50)]
        public void TestClear(params Int32[] array)
        {
            BTree<Int32> t = new BTree<Int32>(4);

            foreach (var el in array)
            {
                t.Add(el);
            }

            t.ToString();

            CheckConsistency(t.Root, 1, t.Height);

            t.Clear();

            Assert.AreEqual(0, t.Count);
        } // End of 'TestClear' method

        [TestCase()]
        [TestCase(100, 200, 300, 400, 75, 500, 60, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 25, 50, 812, 911, 733, 24, 174, 225, 730, 239, 228, 984, 1653, 7)]
        [TestCase(100, 1000, 1100, 300, 400, 75, 500, 60, 1300, 1400, 1500, 1600, 700, 800, 900, 1200, 1700, 200, 25, 50)]
        public void TestDelete1(params Int32[] array)
        {
            BTree<Int32> t = new BTree<Int32>(3);

            foreach (var el in array)
            {
                t.Add(el);
            }

            t.ToString();

            CheckConsistency(t.Root, 1, t.Height);

            while (t.Root.Keys.Count > 0)
            {
                t.Remove(t.Root.Keys[0]);
                t.ToString();
                CheckConsistency(t.Root, 1, t.Height);
            }

            Assert.AreEqual(0, t.Count);
        }

        private void CheckConsistency(Node<Int32> node, Int32 level, Int32 height)
        {
            if (level > height)
                throw new Exception("Inconsistency. Wrong leaf depth");

            if (node.Children.Count != 0)
                CheckConsistency(node.Children[0], level + 1, height);

            for (Int32 i = 0; i < node.Keys.Count - 1; i++)
            {
                Int32 prev = node.Keys[i];
                Int32 cur = node.Keys[i + 1];

                if (cur < prev)
                    throw new Exception("Inconsistency. Wrong leaf depth");
            }

            for (Int32 i = 1; i < node.Children.Count; i++)
                CheckConsistency(node.Children[i], level + 1, height);
        }
    } // End of 'Tests' class
} // end of 'MainTest' namespace

// END OF 'UnitTest1.cs' FILE
