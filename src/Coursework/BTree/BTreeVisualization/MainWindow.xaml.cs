using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Media;
using BTree;

namespace BTreeVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const Int32 _t = 2;

        private BTree<Int32> tree = new BTree<Int32>(_t);
        public MainWindow()
        {
            InitializeComponent();
        }

        private struct NodeLev
        {
            public Int32 Level;
            public List<Int32> Key;
        }

        private void Output(Node<Int32> node, Int32 level, List<NodeLev> list)
        {
            if (node.Keys.Count == 0)
                return;

            if (node.Children.Count != 0)
                Output(node.Children[0], level + 1, list);

            NodeLev newNode = new NodeLev{Level = level};

            newNode.Key = new List<Int32>();

            foreach (var key in node.Keys)
            {
                newNode.Key.Add(key);
            }

            list.Add(newNode);

            for (Int32 i = 1; i < node.Children.Count; i++)
                Output(node.Children[i], level + 1, list);
        }

        private async void AddNode_OnClick(object sender, RoutedEventArgs e)
        {
            Int32 num;

            if (!Int32.TryParse(addTextBox.Text, out num))
            {
                MessageBox.Show("Error data");
                return;
            }

            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            tree.Add(num);

            for (Int32 i = 0; i < tree.Height; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition());

            //for (Int32 i = 0; i < Math.Pow(2 * _t, tree.Height - 1); i++)
            //    MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid[] grids = new Grid[MainGrid.RowDefinitions.Count];

            List<NodeLev> nodes = new List<NodeLev>();

            Output(tree.Root, 1, nodes);

            List<Int32> levels = new List<Int32>();

            for (Int32 i = 0; i < nodes[0].Level; i++)
                levels.Add(0);

            for (Int32 i = 0; i < nodes.Count; i++)
                levels[nodes[i].Level - 1]++;

            for (Int32 i = 0; i < grids.Length; i++)
            {
                grids[i] = new Grid();

                for (Int32 j = 0; j < levels[i]; j++)
                    grids[i].ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetRow(grids[i], i);

                MainGrid.Children.Add(grids[i]);
            }


            levels.Clear();

            for (Int32 i = 0; i < nodes[0].Level; i++)
                levels.Add(0);

            for (Int32 i = 0; i < nodes.Count; i++)
            {
                ComboBox cmbBox = new ComboBox();

                foreach (var key in nodes[i].Key)
                    cmbBox.Items.Add(key);

                cmbBox.SelectedIndex = 0;
                cmbBox.FontSize = 25;
                Grid.SetColumn(cmbBox, levels[nodes[i].Level - 1]);
                Grid.SetRow(cmbBox, nodes[i].Level - 1);
                grids[nodes[i].Level - 1].Children.Add(cmbBox);

                levels[nodes[i].Level - 1]++;
            }
        }

        //private async void AddNode_OnClick(object sender, RoutedEventArgs e)
        //{
        //    Int32 num;

        //    if (!Int32.TryParse(addTextBox.Text, out num))
        //    {
        //        MessageBox.Show("Error data");
        //        return;
        //    }

        //    MainGrid.Children.Clear();
        //    MainGrid.ColumnDefinitions.Clear();
        //    MainGrid.RowDefinitions.Clear();

        //    tree.Add(num);

        //    for (Int32 i = 0; i < tree.Height; i++)
        //        MainGrid.RowDefinitions.Add(new RowDefinition());

        //    for (Int32 i = 0; i < Math.Pow(2 * _t, tree.Height - 1); i++)
        //        MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

        //    List<NodeLev> nodes = new List<NodeLev>();

        //    Output(tree.Root, 1, nodes);

        //    List<Int32> levels = new List<Int32>();

        //    for (Int32 i = 0; i < nodes[0].Level; i++)
        //        levels.Add(0);

        //    for (Int32 i = 0; i < nodes.Count; i++)
        //    {
        //        Int32 curCnt = (Int32)Math.Pow(2 * _t, nodes[i].Level - 1);

        //        Int32 partW = MainGrid.ColumnDefinitions.Count / curCnt;

        //        ComboBox cmbBox = new ComboBox();

        //        foreach (var key in nodes[i].Key)
        //            cmbBox.Items.Add(key);

        //        cmbBox.SelectedIndex = 0;
        //        Grid.SetColumn(cmbBox, levels[nodes[i].Level - 1] * partW);
        //        Grid.SetRow(cmbBox, nodes[i].Level - 1);
        //        MainGrid.Children.Add(cmbBox);

        //        levels[nodes[i].Level - 1]++;
        //    }
        //}

#if false
        private async void AddNode_OnClick(object sender, RoutedEventArgs e)
        {
            Int32 num;

            if (!Int32.TryParse(addTextBox.Text, out num))
            {
                MessageBox.Show("Error data");
                return;
            }

            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            tree.Add(num);

            for (Int32 i = 0; i < tree.Height; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition());

            for (Int32 i = 0; i < Math.Pow(2 * _t, tree.Height - 1); i++)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var list = TraversTree();

            ComboBox comboBox = new ComboBox();

            for (Int32 i = 0; i < 2 * _t - 1 && i < list[0].Count; i++)
                comboBox.Items.Add(list[0][i]);

            comboBox.SelectedIndex = 0;
            Grid.SetColumn(comboBox, MainGrid.ColumnDefinitions.Count / 2);
            Grid.SetRow(comboBox, 0);
            MainGrid.Children.Add(comboBox);

            Int32 curCnt;

            for (Int32 i = 1; i < list.Count; i += curCnt)
            {
                Int32 h = (Int32)Math.Log((Int32)((i + 1) / 2) + 1, 2);
                curCnt = (Int32)Math.Pow(2 * _t, h);

                Int32 partW = MainGrid.ColumnDefinitions.Count / curCnt;

                for (Int32 j = 0; j < curCnt && j + i < list.Count; j++)
                {
                    ComboBox cmbBox = new ComboBox();

                    foreach (var el in list[i + j])
                        cmbBox.Items.Add(el);
                    cmbBox.SelectedIndex = 0;
                    Grid.SetColumn(cmbBox, j * partW);
                    Grid.SetRow(cmbBox, h);
                    MainGrid.Children.Add(cmbBox);
                }
            }

            //MainCanvas.Children.Clear();

            //tree.Add(num);

            //Double width = -Left + MainCanvas.ActualWidth;
            //Double height = -Top + MainCanvas.ActualHeight;

            //Double partH = height / tree.Height;

            //List<List<Int32>> list = TraversTree();
            //// List<ComboBox> comboBoxes = new List<ComboBox>(list.Count);

            //Int32 curCnt = 0;
            //Int32 k = 0;

            //for (Int32 i = 0; i < list.Count; i += curCnt)
            //{
            //    Int32 h = (Int32)Math.Log((Int32)((i + 1) / 2) + 1, 2);
            //    curCnt = (Int32)Math.Pow(2 * _t, h);
            //    Double partW = width / curCnt;

            //    for (Int32 j = 0; j < curCnt && j + i < list.Count; j++)
            //    {
            //        ComboBox comboBox = new ComboBox();

            //        foreach (var el in list[i + j])
            //        {
            //            comboBox.Items.Add(el);
            //        }

            //        comboBox.SelectedIndex = 0;
            //        MainCanvas.Children.Add(comboBox);
            //        Canvas.SetLeft(comboBox, (j + 1) * partW);
            //        Canvas.SetTop(comboBox, (h + 1) * partH / 2);
            //    }

            //    k++;
            //}

            //ComboBox comboBox = new ComboBox();
            //Node<Int32> curNode = tree.Root;

            //foreach (var key in curNode.Keys)
            //{
            //    comboBox.Items.Add(key);
            //}

            ////textBlock.Text = tree.ToString();
            ////textBlock.Background = Brushes.DarkSlateGray;
            ////textBlock.FontSize = 20;
            ////textBlock.FontFamily = new FontFamily("Consolas");

            //MainCanvas.Children.Add(comboBox);

            //Canvas.SetLeft(comboBox, width / 2);
            //Canvas.SetTop(comboBox, height / 2);
        }
#endif

        private List<List<Int32>> TraversTree()
        {
            List<List<Int32>> list = new List<List<Int32>>();
            Queue<Node<Int32>> q = new Queue<Node<Int32>>();

            q.Enqueue(tree.Root);

            while (q.Count != 0)
            {
                Node<Int32> curNode = q.Dequeue();

                List<Int32> curList = new List<Int32>();

                foreach (var key in curNode.Keys)
                    curList.Add(key);

                list.Add(curList);

                foreach (var child in curNode.Children)
                    q.Enqueue(child);
            }

            return list;
        }
    }
}
