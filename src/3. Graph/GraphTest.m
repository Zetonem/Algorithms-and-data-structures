% g(1) = GraphCreate(1, 2);
% g(2) = GraphCreate(2, [3, 4]);
% g(3) = GraphCreate(3, []);
% g(4) = GraphCreate(4, [1, 3]);
% 
% DFS(g);
% BFS(g, 1);

% graph = zeros(4);
% 
% graph(1, 2) = 1000;
% graph(1, 3) = 1000;
% 
% graph(2, 3) = 1;
% graph(2, 4) = 1000;
% 
% graph(3, 4) = 1000;

% s = [1 1 2 2 3 4 4 5 5];
% t = [2 3 3 4 5 3 6 4 6];
% weights = [25 24 7 18 32 6 15 3 40];
% G = digraph(s,t,weights);
% p = plot(G,'EdgeLabel',G.Edges.Weight);
% [T,pred] = minspantree(G);
% highlight(p,T)
graph = zeros(6);

graph(1, 2) = 25;
graph(1, 3) = 24;

graph(2, 4) = 18;
graph(2, 3) = 7;
graph(3, 5) = 32;
graph(5, 4) = 3;
graph(4, 6) = 15;
graph(4, 3) = 6;
graph(5, 6) = 40;

o = TopologySort(graph);

for i = 1 : length(o)
    fprintf("%d ", o(i));
end

Kruskal(graph);
[val, path] = Dijkstra(graph, 1, 6);
res = Kosaraju(graph);

[n, m] = size(res);

for i = 1 : n
    j = 1;
    while j <= m && res(i, j) ~= 0
        fprintf("%d ", res(i, j));
        j = j + 1;
    end
    fprintf("\n");
end

fprintf("The shortest path value from the 1st to the 6th vertex: %d\n", val);
fprintf("Path:\n");
for i = 1 : length(path)
    fprintf("%d ", path(length(path) - i + 1));
end

fprintf("\n");


% graph = zeros(6);
% 
% graph(1, 2) = 16;
% graph(1, 3) = 13;
% 
% graph(2, 3) = 10;
% graph(2, 4) = 12;
% 
% graph(3, 2) = 4;
% graph(3, 5) = 14;
% 
% graph(4, 3) = 9;
% graph(4, 6) = 20;
% 
% graph(5, 4) = 7;
% graph(5, 6) = 4;

[maxFlow, Paths] = FordFulkerson(graph, 1, 6);
fprintf("Max flow is %d\nAll paths:\n", maxFlow);

[n, m] = size(Paths);
for i = 1 : n
    for j = 1 : m
        if Paths(i, j) ~= 0
            fprintf("%d ", Paths(i, j));
        end
    end
    fprintf("\n");
end
