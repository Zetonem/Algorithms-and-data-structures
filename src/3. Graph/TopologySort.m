function Order = TopologySort(graph)
global glFlags
global glOrderIndex

n = length(graph);
glFlags = false(1, n);
glOrderIndex = 1;
Order = 0;

for i = 1 : n
    if ~glFlags(i)
        Order = FillOrder(graph, i, Order);
    end
end
end % End of 'TopologySort' function

function order = FillOrder(graph, vertex, order)
global glFlags
global glOrderIndex

n = length(graph);
glFlags(vertex) = true;

for i = 1 : n
    if ~glFlags(i) && graph(vertex, i) > 0
        order = FillOrder(graph, i, order);
    end
end
order(glOrderIndex) = vertex;
glOrderIndex = glOrderIndex + 1;
end % End of 'FillOrder' function