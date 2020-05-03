/* FILE NAME   : Node.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 03.05.2020.
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
        /// <summary>
        /// List of keys in the node.
        /// </summary>
        public List<Type> Keys { get; } = new List<Type>();

        /// <summary>
        /// List of children nodes.
        /// </summary>
        public List<Node<Type>> Children { get; } = new List<Node<Type>>();

        /// <summary>
        /// Check whether it is a leaf or not.
        /// </summary>
        public Boolean IsLeaf => Children.Count == 0;
    } // End of 'Node' class
} // end of 'BTree' namespace

// END OF 'Node.cs' FILE
