% Initialize graph
v1 = [1 1 2 2 3 3 4 4 5];
v2 = [2 3 3 4 4 5 5 6 6];
w = [25 24 7 18 6 32 3 15 40];

for i = 1 : length(v1)
    e(i) = struct("u", v1(i), "v", v2(i));
end

g = struct("VertexCount", 6, "Edges", e, "Weight", w);

% Find MST by matlab function
G = graph(v1, v2, w);

figure
p = plot(G,'EdgeLabel',G.Edges.Weight);
[T, pred] = minspantree(G);
highlight(p,T)

% Find MST by selfmade function
result = Kruskal(g);

n = length(result);
v1 = zeros(1, n);
v2 = zeros(1, n);
w = zeros(1, n);
for i = 1 : n
    a = result{i};
    v1(i) = result{i}.Edge.u;
    v2(i) = result{i}.Edge.v;
    w(i) = result{i}.Weight;
end
G = graph(v1, v2, w);
p = plot(G,'EdgeLabel',G.Edges.Weight);

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

%G = struct("V", v, "E", e, "W", w);