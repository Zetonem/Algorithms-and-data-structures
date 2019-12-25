t = Tree();

t.InsertNode(5);
t.InsertNode(4);
t.InsertNode(9);
t.InsertNode(8);
t.InsertNode(15);
t.InsertNode(6);
t.InsertNode(10);
t.InsertNode(7);
t.InsertNode(3);
t.InsertNode(1);
fprintf("Full tree:\n");
t.Display();
fprintf("\n");

fprintf("Remove node 9 (with 2 children):\n");
t.RemoveNode(9);
t.Display();
fprintf("\n");

fprintf("Remove node 1 (with 0 children):\n");
t.RemoveNode(1);
t.Display();
fprintf("\n");

fprintf("Remove node 6 (with 1 children):\n");
t.RemoveNode(6);
t.Display();
fprintf("\n");