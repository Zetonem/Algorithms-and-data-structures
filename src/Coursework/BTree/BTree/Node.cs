/* FILE NAME   : Node.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 11.02.2020.
 * NOTE        : None.
 *
 * Copyright © 2020 Leonid Zaytsev. All rights reserved.
 */
using System;
using System.Collections.Generic;

// Project namespace
namespace BTree
{
    /// <summary>
    /// Defines node of the B-Tree data structure.
    /// </summary>
    /// <typeparam name="Type">The type of object in the node.</typeparam>
    public sealed class Node<Type> where Type : IComparable<Type>
    {
        // B-Tree power
        private readonly Int32 _t;

        // List of keys in the node
        public List<Type> Keys { get; } = new List<Type>();

        // Reference to the parent node
        public Node<Type> Parent { get; } = null;

        // List of children nodes
        public List<Node<Type>> Children { get; } = new List<Node<Type>>();

        // check whether it is a leaf or not
        public Boolean IsLeaf => Children.Count == 0;

        /// <summary>
        /// Class default constructor.
        /// </summary>
        public Node()
        {
            _t = 2;
        } // End of 'Node' constructor

        /// <summary>
        /// Class constructor by B-Tree power value.
        /// </summary>
        /// <param name="t">B-Tree power value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Wrong B-Tree power passed.</exception>
        public Node(Int32 t)
        {
            if (t < 2)
                throw new ArgumentOutOfRangeException("Wrong B-Tree power passed.");

            _t = t;
        } // End of 'Node' constructor

        /// <summary>
        /// Class constructor by reference to the parent node.
        /// </summary>
        /// <param name="parent"></param>
        public Node(Node<Type> parent)
        {
            _t = 2;
            Parent = parent;
        } // End of 'Node' constructor

        /// <summary>
        /// Class constructor by B-Tree power value and reference to the parent node.
        /// </summary>
        /// <param name="t">B-Tree power value.</param>
        /// <param name="parent">Parent node for new node.</param>
        /// <exception cref="ArgumentOutOfRangeException">Wrong B-Tree power passed.</exception>
        public Node(Int32 t, Node<Type> parent)
        {
            if (t < 2)
                throw new ArgumentOutOfRangeException("Wrong B-Tree power passed.");

            _t = t;
            Parent = parent;
        } // End of 'Node' constructor
    } // End of 'Node' class
} // end of 'BTree' namespace

// END OF 'Node.cs' FILE
