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
                    Node<T> nextNode = curNode.Children[index];
                    Node<T> parent = curNode;

                    while (!nextNode.IsLeaf)
                    {
                        parent = nextNode;
                        nextNode = nextNode.Children[nextNode.Children.Count - 1];
                    }

                    // let z’ be the smallest key in N;
                    T newKey = nextNode.Keys[nextNode.Keys.Count - 1];

                    Int32 newIndex = parent.Keys.TakeWhile(key => newKey.CompareTo(key) > 0).Count();

                    // remove z’ from N;
                    nextNode.Keys.RemoveAt(nextNode.Keys.Count - 1);

                    // replace z in M by z’;
                    curNode.Keys[index] = newKey;

                    // Adjust(N);
                    Adjust(nextNode, parent, newIndex);
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

                    // If no children left in parent node
                    if (parent.Children.Count == 0)
                    {
                        Pair p =  ParentOf(parent);
                        Node<T> parentOfParent = p.Node;
                        Int32 newIndex = p.Index;
                        Int32 indexNotEmpty = newIndex + 1;
                        Int32 indexEmpty = newIndex;

                        if (parentOfParent == null)
                            return;
                            
                        if (newIndex + 1 >= parentOfParent.Children.Count)
                        {
                            indexNotEmpty = newIndex - 1;
                        }

                        bool isDone = true;
                        do
                        {
                            Node<T> nonEmptyChild = parentOfParent.Children[indexNotEmpty];
                            Node<T> emptyChild = parentOfParent.Children[indexEmpty];
                            
                            // If parent of parent has got only 1 key
                            if (parentOfParent.Keys.Count == 1)
                            {
                                if (nonEmptyChild.Keys.Count != 1)
                                {
                                    if (newIndex + 1 < parentOfParent.Children.Count)
                                    {
                                        T newKey = parentOfParent.Keys[indexEmpty];
                                        parentOfParent.Keys.RemoveAt(indexEmpty);
                                        parentOfParent.Keys.Add(nonEmptyChild.Keys[0]);
                                        nonEmptyChild.Keys.RemoveAt(0);

                                        Node<T> newChildren1 = emptyChild;
                                        Node<T> newChildren2 = nonEmptyChild.Children[0];
                                        parentOfParent.Children.RemoveAt(indexEmpty);
                                        nonEmptyChild.Children.RemoveAt(0); // TODO: ??? nonEmpty or Empty
                                        Node<T> newNode = new Node<T>();

                                        newNode.Keys.Add(newKey);
                                        newNode.Children.Add(newChildren1);
                                        newNode.Children.Add(newChildren2);

                                        parentOfParent.Children.Insert(indexEmpty, newNode);
                                    }
                                    else
                                    {
                                        Node<T> newNode = new Node<T>();

                                        newNode.Keys.Add(parentOfParent.Keys[indexNotEmpty]);
                                        newNode.Children.Add(nonEmptyChild.Children.Last());
                                        newNode.Children.Add(emptyChild);
                                        parentOfParent.Keys.RemoveAt(indexNotEmpty);
                                        parentOfParent.Keys.Insert(indexNotEmpty, nonEmptyChild.Keys.Last());

                                        nonEmptyChild.Keys.RemoveAt(nonEmptyChild.Keys.Count - 1);
                                        nonEmptyChild.Children.RemoveAt(nonEmptyChild.Children.Count - 1);

                                        parentOfParent.Children.RemoveAt(indexEmpty);
                                        parentOfParent.Children.Add(newNode);
                                    }
                                }
                                else
                                {                            
                                    parentOfParent.Children.RemoveAt(indexNotEmpty);

                                    T newKey = nonEmptyChild.Keys[0];

                                    parentOfParent.Keys.Insert(indexNotEmpty, newKey);
                                    parentOfParent.Children.InsertRange(indexNotEmpty, nonEmptyChild.Children);

                                    isDone = false;
                                }
                            }
                            else
                            {
                                if (newIndex + 1 < parentOfParent.Children.Count)
                                {
                                    if (nonEmptyChild.Keys.Count < MaxElements)
                                    {
                                        parentOfParent.Children.RemoveAt(indexEmpty);

                                        nonEmptyChild.Keys.Insert(0, parentOfParent.Keys[indexEmpty]);
                                        parentOfParent.Keys.RemoveAt(indexEmpty);
                                        nonEmptyChild.Children.Insert(0, emptyChild);
                                    }
                                    else
                                    {
                                        Node<T> newNode = new Node<T>();

                                        newNode.Keys.Add(parentOfParent.Keys[indexEmpty]);
                                        newNode.Children.Add(emptyChild);
                                        newNode.Children.Add(nonEmptyChild.Children[0]);

                                        parentOfParent.Keys.RemoveAt(indexEmpty);
                                        parentOfParent.Children.RemoveAt(indexEmpty);
                                        parentOfParent.Children.Insert(indexEmpty, newNode);
                                        parentOfParent.Keys.Insert(indexEmpty, nonEmptyChild.Keys[0]);

                                        nonEmptyChild.Keys.RemoveAt(0);
                                        nonEmptyChild.Children.RemoveAt(0);
                                    }
                                }
                                else
                                {
                                    if (nonEmptyChild.Keys.Count < MaxElements)
                                    {
                                        Node<T> newChildren = emptyChild;
                                        parentOfParent.Children.RemoveAt(indexEmpty);

                                        nonEmptyChild.Keys.Add(parentOfParent.Keys[indexNotEmpty]);
                                        parentOfParent.Keys.RemoveAt(indexNotEmpty);
                                        nonEmptyChild.Children.Add(newChildren);
                                    }
                                    else
                                    {
                                        Node<T> newNode = new Node<T>();

                                        newNode.Keys.Add(parentOfParent.Keys[indexNotEmpty]);
                                        newNode.Children.Add(nonEmptyChild.Children.Last());
                                        newNode.Children.Add(emptyChild);

                                        parentOfParent.Keys.RemoveAt(indexNotEmpty);
                                        parentOfParent.Children.RemoveAt(indexEmpty);
                                        parentOfParent.Children.Insert(indexEmpty, newNode);
                                        parentOfParent.Keys.Insert(indexNotEmpty, nonEmptyChild.Keys.Last());

                                        nonEmptyChild.Keys.RemoveAt(nonEmptyChild.Keys.Count - 1);
                                        nonEmptyChild.Children.RemoveAt(nonEmptyChild.Children.Count - 1);
                                    }
                                }
                            }

                            p =  ParentOf(parentOfParent);
                            parentOfParent = p.Node;
                            newIndex = p.Index;
                            indexNotEmpty = newIndex + 1;
                            indexEmpty = newIndex;

                            if (parentOfParent != null && newIndex + 1 >= parentOfParent.Children.Count)
                            {
                                indexNotEmpty = newIndex - 1;
                            }
                        } while (!isDone && parentOfParent != null);
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
