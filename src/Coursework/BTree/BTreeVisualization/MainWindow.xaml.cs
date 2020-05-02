using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

            tree.Add(100);
            tree.Add(200);
            tree.Add(300);
            tree.Add(400);
            tree.Add(75);
            tree.Add(500);
            tree.Add(600);
            tree.Add(700);
            tree.Add(800);
            tree.Add(900);
            tree.Add(1000);
            tree.Add(1100);
            tree.Add(1200);
            tree.Add(1300);
            tree.Add(1400);
            tree.Add(1500);
            tree.Add(1600);
            tree.Add(1700);
            tree.Add(25);
            tree.Add(50);
        }

        private struct NodeLev
        {
            public Int32 Level;
            public List<Int32> Key;
        }

        private void TraversTree(Node<Int32> node, Int32 level, List<NodeLev> list)
        {
            //if (node.Keys.Count == 0)
            //   return;

            if (node.Children.Count != 0)
                TraversTree(node.Children[0], level + 1, list);

            NodeLev newNode = new NodeLev{Level = level};

            newNode.Key = new List<Int32>();

            foreach (var key in node.Keys)
            {
                newNode.Key.Add(key);
            }

            list.Add(newNode);

            for (Int32 i = 1; i < node.Children.Count; i++)
                TraversTree(node.Children[i], level + 1, list);
        }

        private void Output(List<NodeLev> nodes)
        {
            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.RowDefinitions.Clear();

            tree.ToString();

            for (Int32 i = 0; i < tree.Height; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition());

            Grid[] grids = new Grid[MainGrid.RowDefinitions.Count];

            List<Int32> levels = new List<Int32>();

            for (Int32 i = 0; i < tree.Height; i++)
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

            for (Int32 i = 0; i < tree.Height; i++)
                levels.Add(0);

            for (Int32 i = 0; i < nodes.Count; i++)
            {
                Border border = new Border
                {
                    CornerRadius = new CornerRadius(5),
                    Background = Brushes.GhostWhite,
                    Width = 300,
                    Height = 70,
                    Margin = new Thickness(10)
                };

                TextBlock textBlock = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 35
                };

                foreach (var key in nodes[i].Key)
                    textBlock.Text += key.ToString() + ", ";

                border.Child = textBlock;
                Grid.SetColumn(border, levels[nodes[i].Level - 1]);
                Grid.SetRow(border, nodes[i].Level - 1);
                grids[nodes[i].Level - 1].Children.Add(border);

                levels[nodes[i].Level - 1]++;
            }
        }

        private void AddNode_OnClick(object sender, RoutedEventArgs e)
        {
            Int32 num;

            if (!Int32.TryParse(addTextBox.Text, out num))
            {
                MessageBox.Show("Error data");
                return;
            }

            tree.Add(num);

            List<NodeLev> nodes = new List<NodeLev>();

            TraversTree(tree.Root, 1, nodes);

            Output(nodes);
        }

        private void removeNode_Click(object sender, RoutedEventArgs e)
        {
            Int32 num;

            if (!Int32.TryParse(removeTextBox.Text, out num))
            {
                MessageBox.Show("Error data");
                return;
            }

            if (!tree.Contains(num))
            {
                MessageBox.Show($"{num} isn't in tree.");
                return;
            }

            tree.Remove(num);

            List<NodeLev> nodes = new List<NodeLev>();

            TraversTree(tree.Root, 1, nodes);

            Output(nodes);
        }
    }
}
