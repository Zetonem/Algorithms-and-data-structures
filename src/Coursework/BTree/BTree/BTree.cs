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
using System.Linq;
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

        private Int32 _height;

        /// <summary>
        /// Max elements in the <see cref="BTree{T}"/> node count.
        /// </summary>
        public Int32 MaxElements => 2 * _t - 1;

        /// <summary>
        /// Count of elements contained in the <see cref="BTree{T}"/>.
        /// </summary>
        public Int32 Count => _count;

        public Boolean IsReadOnly => false;

        public Node<T> Root => _root;

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

            Split(oldRoot, 0, _root);
            InsertInNode(_root, item);

            _count++;
            _height++;
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
                Split(child, index, node);

                if (key.CompareTo(node.Keys[index]) > 0)
                    index++;
            }

            InsertInNode(node.Children[index], key);
        } // End of 'InsertInNode' function

        private void Split(Node<T> node, Int32 nodeIndex, Node<T> parent)
        {
            Int32 n = node.Keys.Count;

            if (n < MaxElements)
                return;

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
            if (_root.Keys.Count == 0)
                throw new NullReferenceException("Tree is already empty.");


            // Find node M where z "belongs";
            Int32 index = _root.Keys.TakeWhile(key => item.CompareTo(key) > 0).Count();
            Int32 prevIndex = -1;

            Node<T> curNode = _root;
            Node<T> prevNode = null;

            if (_root.Keys.Contains(item))
            {
                ;
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
                    Adjust(curNode, prevNode, prevIndex);
                }
                // ELSE
                else
                {
                    // find N, the leftmost leaf in the right subtree of z;
                    Node<T> nextNode = curNode.Children[index + 1];

                    while (!nextNode.IsLeaf)
                    {
                        nextNode = nextNode.Children[0];
                    }

                    // let z’ be the smallest key in N;
                    T newKey = nextNode.Keys[0];

                    // remove z’ from N;
                    nextNode.Keys.RemoveAt(0);

                    // replace z in M by z’;
                    curNode.Keys[index] = newKey;

                    // Adjust(N);
                    Adjust(nextNode, curNode, index + 1);
                }
                // ENDIF
            }
            // ENDIF

            _count--;

            return true;
        } // End of 'Remove' method

        private void Adjust(Node<T> node, Node<T> parent, Int32 index)
        {
            //IF M is overflowing THEN
            if (node.Keys.Count >= 2 * _t - 1)
            {
                // IF Right_Sibling_of(M) exists and is not full THEN
                if (index + 1 < parent.Children.Count && parent.Children[index + 1].Keys.Count < 2 * _t - 1)
                {
                    // LR-Redistribute(M);
                    RedistributeLR(node, parent, index);
                }
                // ELSIF Left_Sibling_of(M) exists and is not full THEN
                else if (index - 1 >= 0 && parent.Children[index - 1].Keys.Count < 2 * _t - 1)
                {
                    // RL - Redistribute(M);
                    RedistributeRL(node, parent, index);
                }
                // ELSE--all of M’s immediate siblings are full
                else
                {
                    // Split(M);
                    Split(node, index, parent);

                    // Adjust(Parent_of(M));
                    Pair newParent = ParentOf(parent);
                    Adjust(parent, newParent.Node, newParent.Index);
                }
                //ENDIF;
            }
            // ELSIF M is underflowing THEN
            else if (node.Keys.Count <= _t - 1)
            {
                // IF Left_Sibling_of(M) exists and is not on verge of underflowing THEN
                if (index - 1 >= 0 && parent.Children[index - 1].Keys.Count > _t - 1)
                {
                    // LR - Redistribute(Left_Sibling_of(M));
                    RedistributeLR(parent.Children[index - 1], parent, index - 1);
                }
                // ELSIF Right_Sibling_of(M) exists and is not on verge of underflowing THEN
                else if (index + 1 < parent.Children.Count && parent.Children[index + 1].Keys.Count > _t - 1)
                {
                    // RL - Redistribute(Right_Sibling_of(M));
                    RedistributeRL(parent.Children[index + 1], parent, index + 1);
                }
                // ELSIF M is the root THEN
                else if (parent == null)
                {
                    // IF M has only one child THEN
                    if (node.Children.Count == 1)
                    {
                        // child of M becomes root;
                        _root = node.Children[0];
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
                        Concatenate(node, parent, index);
                    }
                    // ELSE--left sibling must exist
                    else
                    {
                        // Concatenate(Left_Sibling_of(M));
                        Concatenate(parent.Children[index - 1], parent, index - 1);
                    }
                }
                // ENDIF;
            }
            // ENDIF;
            // ELSE--M is neither overflowing nor underflowing
            else
            {
                // do nothing;
            }
            // ENDIF;

        } // End of 'Adjust' method

        private void RedistributeLR(Node<T> node, Node<T> parent, Int32 index)
        {
            Node<T> rightSibling = parent.Children[index + 1];

            if (rightSibling.Keys.Count == 0 && parent.Children.Count == 2)
            {
                if (node.Children.Count == 0)
                     _height--;

                parent.Children.RemoveRange(0, parent.Children.Count);

                parent.Keys.InsertRange(0, node.Keys);
                parent.Children.InsertRange(0, node.Children);
                return;
            }

            rightSibling.Keys.Insert(0, parent.Keys[index]);

            if (node.Children.Count != 0)
            {
                rightSibling.Children.Insert(0, node.Children.Last());
                node.Children.RemoveAt(node.Children.Count - 1);
            }

            parent.Keys[index] = node.Keys.Last();

            node.Keys.RemoveAt(node.Keys.Count - 1);
        } // End of 'RedistributeLR' method

        private void RedistributeRL(Node<T> node, Node<T> parent, Int32 index)
        {
            Node<T> leftSibling = parent.Children[index - 1];

            if (leftSibling.Keys.Count == 0 && parent.Children.Count == 2)
            {
                if (node.Children.Count == 0)
                    _height--;

                parent.Children.RemoveRange(0, parent.Children.Count);

                parent.Keys.AddRange(node.Keys);
                parent.Children.InsertRange(0, node.Children);
                return;
            }

            leftSibling.Keys.Add(parent.Keys[index - 1]);

            if (node.Children.Count != 0)
            {
                leftSibling.Children.Add(node.Children[0]);
                node.Children.RemoveAt(0);
            }

            parent.Keys[index - 1] = node.Keys[0];

            node.Keys.RemoveAt(0);
        } // End of 'RedistributeRL' method

        private void Concatenate(Node<T> node, Node<T> parent, Int32 index)
        {
            node.Keys.Add(parent.Keys[index]);
            node.Keys.AddRange(parent.Children[index + 1].Keys);

            node.Children.AddRange(parent.Children[index + 1].Children);

            parent.Children.RemoveAt(index + 1);
            parent.Keys.RemoveAt(index);

            if (parent.Keys.Count == 0)
            {
                parent.Keys.InsertRange(0, node.Keys);
                parent.Children.RemoveAt(0);
                parent.Children.InsertRange(0, node.Children);

                _height--;

                node = null;
            }
        } // End of 'Concatenate' method

        private Pair ParentOf(Node<T> node)
        {
            T item = node.Keys[0];
            Node<T> curNode = _root;
            Node<T> prevNode = null;

            Int32 prevIndex = -1;
            Int32 index = curNode.Keys.TakeWhile(key => item.CompareTo(key) > 0).Count();

            while ((index < curNode.Keys.Count && curNode.Keys[index].CompareTo(item) != 0) || (index < curNode.Children.Count && !curNode.Keys.Contains(item)))
            {
                prevNode = curNode;
                curNode = curNode.Children[index];
                prevIndex = index;
                index = curNode.Keys.TakeWhile(key => item.CompareTo(key) > 0).Count();
            }

            return new Pair(prevNode, prevIndex);
        } // End of 'ParentOf' method

        private class Pair
        {
            public Node<T> Node;
            public Int32 Index;

            public Pair(Node<T> node, int index)
            {
                this.Node = node;
                this.Index = index;
            }
        }

        /*
                /// <summary>
                /// Deletes key from the B-Tree.
                /// </summary>
                /// <param name="item">Key value to delete.</param>
                public Boolean Remove(T item)
                {
                    if (_root.Keys.Count == 0)
                        throw new NullReferenceException("Tree is already empty.");

                    RemoveFromNode(_root, item);

                    if (_root.Keys.Count == 0 && !_root.IsLeaf)
                    {
                        _root = _root.Children.Single();
                        _height--;
                    }

                    _count--;
                    return true;
                } // End of 'Remove' method

                private void RemoveFromNode(Node<T> node, T key)
                {
                    Int32 index = 0;

                    foreach (var k in node.Keys)
                    {
                        if (k.CompareTo(key) >= 0)
                            break;
                        index++;
                    }

                    // If key to delete in the current node
                    if (index < node.Keys.Count && node.Keys[index].CompareTo(key) == 0)
                    {
                        if (node.IsLeaf)
                        {
                            RemoveFromLeaf(node, index);
                            return;
                        }

                        RemoveFromNonLeaf(node, index, key);
                        return;
                    }

                    if (!node.IsLeaf)
                    {
                        DeleteKeyFromSubtree(node, key, index);
                    }
                } // End of 'RemoveFromNode' method

                private void DeleteKeyFromSubtree(Node<T> parentNode, T key, Int32 index)
                {
                    Node<T> childNode = parentNode.Children[index];

                    if (childNode.Keys.Count == _t - 1)
                    {
                        Int32 leftIndex = index - 1;
                        Node<T> leftSibling = index > 0 ? parentNode.Children[leftIndex] : null;

                        Int32 rightIndex = index + 1;
                        Node<T> rightSibling = index < parentNode.Children.Count - 1 ? parentNode.Children[rightIndex] : null;

                        if (leftSibling != null && leftSibling.Keys.Count > _t - 1)
                        {
                            childNode.Keys.Insert(0, parentNode.Keys[index]);
                            parentNode.Keys[index] = leftSibling.Keys.Last();
                            leftSibling.Keys.RemoveAt(leftSibling.Keys.Count - 1);

                            if (!leftSibling.IsLeaf)
                            {
                                childNode.Children.Insert(0, leftSibling.Children.Last());
                                leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
                            }
                        }
                        else if (rightSibling != null && rightSibling.Keys.Count > _t - 1)
                        {

                            childNode.Keys.Add(parentNode.Keys[index]);
                            parentNode.Keys[index] = rightSibling.Keys.First();
                            rightSibling.Keys.RemoveAt(0);

                            if (!rightSibling.IsLeaf)
                            {
                                childNode.Children.Add(rightSibling.Children.First());
                                rightSibling.Children.RemoveAt(0);
                            }
                        }
                        else
                        {

                            if (leftSibling != null)
                            {
                                childNode.Keys.Insert(0, parentNode.Keys[index]);
                                var oldKeys = childNode.Keys;

                                childNode.Keys.Clear();
                                childNode.Keys.AddRange(leftSibling.Keys);

                                childNode.Keys.AddRange(oldKeys);
                                if (!leftSibling.IsLeaf)
                                {
                                    var oldChildren = childNode.Children;

                                    childNode.Children.Clear();
                                    childNode.Children.AddRange(leftSibling.Children);
                                    childNode.Children.AddRange(oldChildren);
                                }

                                parentNode.Children.RemoveAt(leftIndex);
                                parentNode.Keys.RemoveAt(index);
                            }
                            else
                            {
                                childNode.Keys.Add(parentNode.Keys[index]);
                                childNode.Keys.AddRange(rightSibling.Keys);
                                if (!rightSibling.IsLeaf)
                                {
                                    childNode.Children.AddRange(rightSibling.Children);
                                }

                                parentNode.Children.RemoveAt(rightIndex);
                                parentNode.Keys.RemoveAt(index);
                            }
                        }
                    }

                    RemoveFromNode(childNode, key);
                }

                private void RemoveFromLeaf(Node<T> node, Int32 index)
                {
                    node.Keys.RemoveAt(index);
                } // End of 'RemoveFromLeaf' method

                private void RemoveFromNonLeaf(Node<T> node, Int32 index, T key)
                {
                    Node<T> curChild = node.Children[index];

                    if (curChild.Keys.Count >= _t)
                    {
                        while (!curChild.IsLeaf)
                            curChild = curChild.Children.Last();

                        T result = curChild.Keys[curChild.Keys.Count - 1];
                        curChild.Keys.RemoveAt(curChild.Keys.Count - 1);

                        node.Keys[index] = result;
                    }
                    else
                    {
                        Node<T> nextChild = node.Children[index + 1];

                        if (nextChild.Keys.Count >= _t)
                        {
                            while (!curChild.IsLeaf)
                                curChild = curChild.Children.First();

                            T result = curChild.Keys[0];
                            curChild.Keys.RemoveAt(0);

                            node.Keys[index] = result;
                        }
                        else
                        {
                            curChild.Keys.Add(node.Keys[index]);
                            curChild.Keys.AddRange(nextChild.Keys);
                            curChild.Children.AddRange(nextChild.Children);

                            node.Keys.RemoveAt(index);
                            node.Children.RemoveAt(index + 1);

                            RemoveFromNode(curChild, key);
                        }
                    }
                } // End of 'RemoveFromNonLeaf' method
        */
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
            _height = 0;
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

            if (curH > _height)
                _height = curH;

            return s.ToString();
        } // End of 'ToString' method

        #endregion
    } // End of 'BTree' class
} // end of 'BTree' namespace

// END OF 'BTree.cs' FILE
