function MST = Kruskal(graph)
n = length(graph.Edges);

% Sort edges in the increase order by weights
for i = 1 : n
    for j = 1 : n
        if graph.Weight(j) > graph.Weight(i)
            graph.Weight([i, j]) = graph.Weight([j, i]);
            graph.Edges([i, j]) = graph.Edges([j, i]);
        end
    end
end

% Initialize partition structure
InitiallizePartition(n)

edgeCount = 1;
includedCount = 0;
k = 1;

MST = cell(1, graph.VertexCount - 1);

% Try to connect all vertices
while edgeCount <= n && includedCount <= graph.VertexCount - 1
    % Find roots in the partition strcture of current edge vertices
    parent1 = FindRoot(graph.Edges(edgeCount).u);
    parent2 = FindRoot(graph.Edges(edgeCount).v);
    
    % If vertices in the different partitions, than add this edge to the
    % MST and merge two partitions
    if parent1 ~= parent2
        MST{k} = struct("Edge", graph.Edges(edgeCount), "Weight", graph.Weight(edgeCount));
        k = k + 1;
        includedCount = includedCount + 1;
        Union(parent1, parent2);
    end
    edgeCount = edgeCount + 1;
end
end % End of 'Kruskal' function

function InitiallizePartition(n)
global Parent

Parent = zeros(1, n);
for i = 1 : n
    Parent(i) = -1;    
end
end % End of 'InitiallizePartition' function

function root = FindRoot(x)
global Parent

root = x;
while Parent(root) > 0
    root = Parent(root);
end
end % End of 'FindRoot' function

function Union(x, y)
global Parent

totalElements = Parent(x) + Parent(y);
if Parent(x) >= Parent(x) + Parent(y)
    Parent(x) = y;
    Parent(y) = totalElements;
else
    Parent(y) = x;
    Parent(x) = totalElements;
end
end % End of 'Union' function
