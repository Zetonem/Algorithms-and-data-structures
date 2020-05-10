/* FILE NAME   : BTree.cs
 * PROGRAMMER  : Leonid Zaytsev.
 * LAST UPDATE : 04.05.2020.
 * NOTE        : None.
 *
 * Copyright © 2020 Leonid Zaytsev. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Project namespace
namespace BTree
{
    /// <summary>
    /// B-Tree data structure implementation class
    /// </summary>
    /// <typeparam name="T">Keys</typeparam>
    [Serializable]
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
        /// <see cref="BTree{T}"/> height.
        /// </summary>
        private Int32 _height;

        /// <summary>
        /// Max elements count in the <see cref="BTree{T}"/> node.
        /// </summary>
        public Int32 MaxElements => 2 * _t - 1;

        /// <summary>
        /// Min elements count in the <see cref="BTree{T}"/> node.
        /// </summary>
        public Int32 MinElements => _t - 1;

        /// <summary>
        /// Count of elements contained in the <see cref="BTree{T}"/>.
        /// </summary>
        public Int32 Count => _count;

        public Boolean IsReadOnly => false;

        /// <summary>
        /// <see cref="BTree{T}"/> root node.
        /// </summary>
        public Node<T> Root => _root;

        /// <summary>
        /// <see cref="BTree{T}"/> height.
        /// </summary>
        public Int32 Height => _height;

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
            _height = 1;
            _root = new Node<T>(null);
        } // End of 'BTree' constructor

        #region Insert
        /// <summary>
        /// Inserts new key in the B-Tree.
        /// </summary>
        /// <param name="item">Key value to insert.</param>
        public void Add(T item)
        {
            if (_root.IsLeaf)
            {
                Int32 index = SearchIndex(_root, item);

                _root.Keys.Insert(index, item);

                Int32 n = _root.Keys.Count;

                if (n > MaxElements)
                {
                    T middleKey = _root.Keys[n / 2];
                    Node<T> newParent = new Node<T>(null);
                    Node<T> child1 = new Node<T>(newParent);
                    Node<T> child2 = new Node<T>(newParent);

                    newParent.Keys.Add(middleKey);
                    newParent.Children.Add(child1);
                    newParent.Children.Add(child2);

                    child1.Keys.AddRange(_root.Keys.GetRange(0, _t));
                    child2.Keys.AddRange(_root.Keys.GetRange(_t + 1, _t - 1));

                    _root = newParent;

                    _height++;
                }
            }
            else
            {

                Node<T> curNode = _root;

                Int32 index = SearchIndex(curNode, item);
                Int32 prevIndex = -1;

                // Find leaf node M where z "belongs"
                do
                {
                    curNode = curNode.Children[index];
                    prevIndex = index;
                    index = SearchIndex(curNode, item);
                } while (!curNode.IsLeaf);

                // Place z into proper place within M;
                curNode.Keys.Insert(index, item);

                Adjust(curNode, prevIndex);
            }

            _count++;
        } // End of 'Add' method
        #endregion

        /// <summary>
        /// Delete all items from <see cref="BTree{T}"/>.
        /// </summary>
        public void Clear()
        {
            while (_root.Keys.Count > 0)
            {
                this.Remove(_root.Keys[0]);
            }
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
            if (_root.Keys.Count == 0)
                throw new NullReferenceException("Tree is already empty.");

            // Find node M where z "belongs";
            Int32 index = _root.Keys.TakeWhile(key => item.CompareTo(key) > 0).Count();
            Int32 prevIndex = -1;

            Node<T> curNode = _root;
            Node<T> prevNode = null;

            if (_root.Keys.Contains(item))
            {
                if (_root.IsLeaf)
                {
                    _root.Keys.RemoveAt(index);
                    _count--;
                    return true;
                }
            }
            else
            {
                do
                {
                    prevNode = curNode;
                    curNode = curNode.Children[index];
                    prevIndex = index;
                    index = curNode.Keys.TakeWhile(key => item.CompareTo(key) > 0).Count();
                } while ((index < curNode.Keys.Count && curNode.Keys[index].CompareTo(item) != 0) || (index < curNode.Children.Count && !curNode.Keys.Contains(item)));
            }

            // IF z is not in M THEN
            if (!curNode.Keys.Contains(item))
                // do nothing(or report error, if appropriate) ;
                throw new Exception();

            // ELSE
            else
            {
                // IF M is a leaf THEN
                if (curNode.IsLeaf)
                {
                    // remove z from M;
                    curNode.Keys.RemoveAt(index);

                    // Adjust(M);
                    Adjust(curNode, prevIndex);
                }
                // ELSE
                else
                {
                    // find N, the rightmost leaf in the left subtree of z;
                    Node<T> previousNode = curNode.Children[index];
                    Node<T> parent = curNode;

                    while (!previousNode.IsLeaf)
                    {
                        parent = previousNode;
                        previousNode = previousNode.Children[previousNode.Children.Count - 1];
                    }

                    // let z’ be the bigest key in N;
                    T newKey = previousNode.Keys[previousNode.Keys.Count - 1];

                    Int32 newIndex = parent.Keys.TakeWhile(key => newKey.CompareTo(key) > 0).Count();

                    // remove z’ from N;
                    previousNode.Keys.RemoveAt(previousNode.Keys.Count - 1);

                    // replace z in M by z’;
                    curNode.Keys[index] = newKey;

                    // Adjust(N);
                    Adjust(previousNode, newIndex);
                }
                // ENDIF
            }
            // ENDIF

            _count--;

            return true;
        } // End of 'Remove' method
        #endregion

        #region AuxiliaryMethods

        /// <summary>
        /// Adjusts <see cref="BTree{T}"> state.
        /// </summary>
        /// <param name="item">Key value to delete.</param>
        private void Adjust(Node<T> node, Int32 index)
        {
            Node<T> parent = node.Parent;

            //IF M is overflowing THEN
            if (node.Keys.Count >= 2 * _t - 1)
            {
                // IF Right_Sibling_of(M) exists and is not full THEN
                if (index + 1 < parent.Children.Count && parent.Children[index + 1].Keys.Count < 2 * _t - 1)
                {
                    // LR-Redistribute(M);
                    RedistributeLR(node, index);
                }
                // ELSIF Left_Sibling_of(M) exists and is not full THEN
                else if (index >= 0 && parent.Children[index].Keys.Count < 2 * _t - 1)
                {
                    // RL - Redistribute(M);
                    RedistributeRL(node, index - 1);
                }
                // ELSE--all of M’s immediate siblings are full
                else
                {
                    // Split(M);
                    Split(node, index);

                    // Adjust(Parent_of(M));
                    if (parent.Parent != null)
                    {
                        Adjust(parent, SearchIndex(parent.Parent, parent.Keys[0]));
                    }
                    else if (parent.Keys.Count == MaxElements)
                    {
                        T middleKey = _root.Keys[MinElements];
                        Node<T> newParent = new Node<T>(null);
                        Node<T> child1 = new Node<T>(newParent);
                        Node<T> child2 = new Node<T>(newParent);

                        newParent.Keys.Add(middleKey);
                        newParent.Children.Add(child1);
                        newParent.Children.Add(child2);

                        child1.Keys.AddRange(_root.Keys.GetRange(0, MinElements));
                        child2.Keys.AddRange(_root.Keys.GetRange(_t, MinElements));

                        for (Int32 i = 0; i < MinElements + 1; i++)
                        {
                            child1.Children.Add(_root.Children[i]);
                            child1.Children[i].Parent = child1;
                        }

                        for (Int32 i = 0; i < MinElements + 1; i++)
                        {
                            child2.Children.Add(_root.Children[i + MinElements + 1]);
                            child2.Children[i].Parent = child2;
                        }

                        _root = newParent;
                        _height++;
                    }
                }
                //ENDIF;*/
            }
            // ELSIF M is underflowing THEN
            else if (node.Keys.Count <= _t - 1)
            {
                // IF Left_Sibling_of(M) exists and is not on verge of underflowing THEN
                if (index - 1 >= 0 && parent.Children[index - 1].Keys.Count > _t - 1)
                {
                    // LR - Redistribute(Left_Sibling_of(M));
                    RedistributeLR(parent.Children[index - 1], index - 1);
                }
                // ELSIF Right_Sibling_of(M) exists and is not on verge of underflowing THEN
                else if (index + 1 < parent.Children.Count && parent.Children[index + 1].Keys.Count > _t - 1)
                {
                    // RL - Redistribute(Right_Sibling_of(M));
                    RedistributeRL(parent.Children[index + 1], index);
                }
                // ELSIF M is the root THEN
                else if (parent == null)
                {
                    // IF M has only one child THEN
                    if (node.Children.Count == 1)
                    {
                        // child of M becomes root;
                        _root = node.Children[0];

                        _height--;
                        // dispose of M;
                    }
                    // ENDIF;
                }
                // ELSE
                else
                {
                    // --To have arrived here, M must be underflowing, M must have
                    // --at least one sibling, and all of M’s immediate siblings are on
                    // --the verge of underflowing. It follows that concatenation
                    // --(or "merging", if you prefer) is called for
                    // IF Right_Sibling_of(M) exists THEN
                    if (index + 1 < parent.Children.Count)
                    {
                        // Concatenate(M);
                        Concatenate(node, index);                
                    }
                    // ELSE--left sibling must exist
                    else
                    {
                        // Concatenate(Left_Sibling_of(M));
                        Concatenate(parent.Children[index - 1], index - 1);                
                    }

                    if (parent.Keys.Count < MinElements)
                    {
                        if (parent.Parent == null && parent.Children.Count == 1)
                        {
                            // child of M becomes root;
                            _root = parent.Children[0];
                            _root.Parent = null;

                            _height--;
                        }
                        else if (parent.Parent != null)
                        {
                            Adjust(parent, SearchIndex(parent.Parent, parent.Children[0].Keys[0]));
                        }
                    }
                }
                // ENDIF;
            }
            // ENDIF;
            // ELSE--M is neither overflowing nor underflowing
            // do nothing;
            // ENDIF;
        } // End of 'Adjust' method

        private void RedistributeLR(Node<T> node, Int32 index)
        {
            Node<T> rightSibling = node.Parent.Children[index + 1];

            // If current node has got at least 1 child then move it to the right sibling
            if (node.Children.Count > 0)
            {
                Node<T> nodeLastChild = node.Children.Last();

                rightSibling.Children.Insert(0, nodeLastChild);
                rightSibling.Children[0].Parent = rightSibling;
                node.Children.Remove(nodeLastChild);
            }

            T parentKey = node.Parent.Keys[index];
            rightSibling.Keys.Insert(0, parentKey);

            T nodeLastKey = node.Keys.Last();
            node.Keys.Remove(nodeLastKey);
            node.Parent.Keys[index] = nodeLastKey;
        } // End of 'RedistributeLR' method

        private void RedistributeRL(Node<T> node, Int32 index)
        {
            Node<T> leftSibling = node.Parent.Children[index];

            if (node.Children.Count > 0)
            {
                Node<T> nodeFirstChild = node.Children[0];

                leftSibling.Children.Add(nodeFirstChild);
                leftSibling.Children[leftSibling.Children.Count - 1].Parent = leftSibling;
                node.Children.Remove(nodeFirstChild);
            }

            T parentKey = node.Parent.Keys[index];
            leftSibling.Keys.Add(parentKey);

            T nodeFirstKey = node.Keys[0];
            node.Keys.Remove(nodeFirstKey);
            node.Parent.Keys[index] = nodeFirstKey;
        } // End of 'RedistributeRL' method

        private void Split(Node<T> node, Int32 index)
        {
            T middleKey = node.Keys[_t - 1];

            Node<T> child1 = new Node<T>(node.Parent);
            Node<T> child2 = new Node<T>(node.Parent);

            child1.Keys.AddRange(node.Keys.GetRange(0, MinElements));
            child2.Keys.AddRange(node.Keys.GetRange(_t, MinElements));

            if (node.Children.Count > 0)
            {
                for (Int32 i = 0; i <= MinElements; i++)
                {
                    child1.Children.Add(node.Children[i]);
                    child1.Children[i].Parent = child1;
                }

                for (Int32 i = 0; i <= MinElements; i++)
                {
                    child2.Children.Add(node.Children[i + MinElements + 1]);
                    child2.Children[i].Parent = child2;
                }
            }

            node.Parent.Keys.Insert(index, middleKey);
            node.Parent.Children[index] = child1;
            node.Parent.Children.Insert(index + 1, child2);
        } // End of 'Split' method

        private void Concatenate(Node<T> node, Int32 index)
        {
            T parentKey = node.Parent.Keys[index];
            node.Parent.Keys.Remove(parentKey);
            node.Keys.Add(parentKey);

            Node<T> rightSibling = node.Parent.Children[index + 1];
            node.Keys.AddRange(rightSibling.Keys);

            Int32 n = rightSibling.Children.Count;

            for (Int32 i = 0; i < n; i++)
            {
                node.Children.Add(rightSibling.Children[0]);
                rightSibling.Children[0].Parent = node;
                rightSibling.Children.RemoveAt(0);
            }

            node.Parent.Children.RemoveAt(index + 1);
        } // End of 'Concatenate' method
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
        public override String ToString()
        {
            // _height = 0;
            return ToString(_root, new StringBuilder(), 1);
        } // End of 'ToString' method

        /// <summary>
        /// Converts <see cref="BTree{T}"/> to string (internal method).
        /// </summary>
        /// <param name="node">Start node to convert.</param>
        /// <param name="s">Current string.</param>
        /// <returns>Tree in the string format.</returns>
        private String ToString(Node<T> node, StringBuilder s, Int32 curH)
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
                    ToString(child, s, curH + 1);
                    s.Append(", ");
                }

            s.Append(")");

            // if (curH > _height)
            //     _height = curH;

            return s.ToString();
        } // End of 'ToString' method
        #endregion

        #region Load
        /// <summary>
        /// Save <see cref="Node{T}"/> in file.
        /// </summary>
        /// <param name="fileName">File to save.</param>
        /// <returns>True if success.</returns>
        public Boolean Upload(String fileName)
        {
            using (FileStream fs = new FileStream("fileName", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }

            return true;
        } // End of 'Upload' method

        public static BTree<T> Download(String fileName)
        {
            BTree<T> tree = null;

            using (FileStream fs = new FileStream("fileName", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                tree = (BTree<T>)formatter.Deserialize(fs);
            }

            return tree;
        } // End of 'Download' method
        #endregion

        /// <summary>
        /// Searchs item position in <see cref="Node{T}"/>.
        /// </summary>
        /// <param name="node">Node to search position.</param>
        /// <param name="item">Item to search position.</param>
        /// <returns>Item position.</returns>
        private Int32 SearchIndex(Node<T> node, T item)
        {
            Int32 index = 0;

            foreach (var key in node.Keys)
            {
                if (key.CompareTo(item) > 0)
                    break;
                index++;
            }

            return index;
        } // End of 'SearchIndex' method
    } // End of 'BTree' class
} // end of 'BTree' namespace

// END OF 'BTree.cs' FILE
