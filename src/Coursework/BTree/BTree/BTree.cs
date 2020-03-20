/* FILE NAME   : BTree.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 20.03.2020.
 * NOTE        : None.
 *
 * Copyright © 2020 Leonid Zaytsev. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

// Project namespace
namespace BTree
{
    /// <summary>
    /// B-Tree data structure implementation class
    /// </summary>
    /// <typeparam name="T">Keys</typeparam>
    public class BTree<T> : ICollection<T> where T : IComparable<T>
    {
        /// <summary>
        /// <see cref="BTree{T}"/> power.
        /// </summary>
        private readonly Int32 _t;

        /// <summary>
        /// <see cref="BTree{T}"/> root node.
        /// </summary>
        private Node<T> _root;

        /// <summary>
        /// Count of elements contained in the <see cref="BTree{T}"/>.
        /// </summary>
        private Int32 _count;

        /// <summary>
        /// Max elements in the <see cref="BTree{T}"/> node count.
        /// </summary>
        public Int32 MaxElements => 2 * _t - 1;

        /// <summary>
        /// Count of elements contained in the <see cref="BTree{T}"/>.
        /// </summary>
        public Int32 Count => _count;

        public Boolean IsReadOnly => false;

        /// <summary>
        /// Class default by B-Tree power.
        /// </summary>
        /// <param name="t">B-Tree power value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Wrong B-Tree power passed.</exception>
        public BTree(Int32 t)
        {
            if (t < 2)
                throw new ArgumentOutOfRangeException("Wrong B-Tree power passed.");

            _t = t;
            _count = 0;
            _root = new Node<T>();
        } // End of 'BTree' constructor

        #region Insert

        /// <summary>
        /// Inserts new key in the B-Tree.
        /// </summary>
        /// <param name="item">Key value to insert.</param>
        public void Add(T item)
        {
            if (_root.Keys.Count < MaxElements)
            {
                InsertInNode(_root, item);
                _count++;
                return;
            }

            Node<T> oldRoot = _root;
            _root = new Node<T>();

            _root.Children.Add(oldRoot);
            oldRoot.Parent = _root;

            Split(oldRoot, 0);
            InsertInNode(_root, item);

            _count++;
        } // End of 'Add' method

        private void InsertInNode(Node<T> node, T key)
        {
            Int32 index = 0;

            foreach (var k in node.Keys)
            {
                if (k.CompareTo(key) > 0)
                    break;
                index++;
            }

            if (node.IsLeaf)
            {
                node.Keys.Insert(index, key);
                return;
            }

            Node<T> child = node.Children[index];

            if (child.Keys.Count >= MaxElements)
            {
                Split(child, index);

                if (key.CompareTo(node.Keys[index]) > 0)
                    index++;
            }

            InsertInNode(node.Children[index], key);
        } // End of 'InsertInNode' function

        private void Split(Node<T> node, Int32 nodeIndex)
        {
            Int32 n = node.Keys.Count;

            if (n < MaxElements)
                return;

            Node<T> parent = node.Parent;

            parent.Keys.Insert(nodeIndex, node.Keys[_t - 1]);

            Node<T> newNode = new Node<T>(parent);
            newNode.Keys.AddRange(node.Keys.GetRange(_t, _t - 1));
            node.Keys.RemoveRange(_t - 1, _t);

            if (!node.IsLeaf)
            {
                newNode.Children.AddRange(node.Children.GetRange(_t, _t));
                node.Children.RemoveRange(_t, _t);
            }

            parent.Children.Insert(nodeIndex + 1, newNode);
        } // End of 'Split' method

        #endregion

        public void Clear()
        {
            throw new NotImplementedException();
        } // End of 'Clear' method

        /// <summary>
        /// Searchs key in the B-Tree.
        /// </summary>
        /// <param name="item">Key to find.</param>
        /// <returns>If key is in the tree return true, otherwise false.</returns>
        public Boolean Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Node<T> curNode = _root;

            do
            {
                Int32 index = 0;

                foreach (var key in curNode.Keys)
                {
                    if (key.CompareTo(item) >= 0)
                        break;
                    index++;
                }

                if (index < curNode.Keys.Count && curNode.Keys[index].CompareTo(item) == 0)
                    return true;

                if (curNode.Children.Count != 0)
                    curNode = curNode.Children[index];
                else
                    return false;
            } while (true);
        } // End of 'Contains' method

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        } // End of 'CopyTo' method

        #region Delete

        /// <summary>
        /// Deletes key from the B-Tree.
        /// </summary>
        /// <param name="item">Key value to delete.</param>
        public Boolean Remove(T item)
        {
            throw new NotImplementedException();
        } // End of 'Remove' method

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        } // End of 'GetEnumenator' method

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        } // End of 'IEnumerable.GetEnumerator' method

        #region ToString

        /// <summary>
        /// Converts <see cref="BTree{T}"/> to string.
        /// </summary>
        /// <returns>Tree in the string format.</returns>
        public override String ToString() => ToString(_root, new StringBuilder());

        /// <summary>
        /// Converts <see cref="BTree{T}"/> to string (internal method).
        /// </summary>
        /// <param name="node">Start node to convert.</param>
        /// <param name="s">Current string.</param>
        /// <returns>Tree in the string format.</returns>
        private String ToString(Node<T> node, StringBuilder s)
        {
            foreach (var key in node.Keys)
            {
                s.Append(key.ToString()).Append(", ");
            }

            s.Append("(");

            if (node.Children.Count == 0)
                s.Append("*");
            else
                foreach (var child in node.Children)
                {
                    ToString(child, s);
                    s.Append(", ");
                }

            s.Append(")");

            return s.ToString();
        } // End of 'ToString' method

        #endregion
    } // End of 'BTree' class
} // end of 'BTree' namespace

// END OF 'BTree.cs' FILE
