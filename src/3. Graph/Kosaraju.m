function Components = Kosaraju(graph)
global glFlags
global glOrderIndex
global glComponentIndex

% Initialization
n = length(graph);
glFlags = false(1, n);
glOrderIndex = 1;
glComponentIndex = 1;
order = 0;

% Topology sort
for i = 1 : n
    if ~glFlags(i)
        order = FillOrder(graph, i, order);
    end
end

% Graph transpose
graphTranspose = zeros(n);

for i = 1 : n
    for j = 1 : n
        graphTranspose(i, j) = graph(j, i);
    end
end

% Traversal transposed graph by received order
glFlags = false(1, n);
component = 0;
k = 1;
oldComponent = -1;
for i = 1 : n
    vertex = order(n - i + 1);
    if ~glFlags(vertex)
        component = FillComponent(graphTranspose, vertex, component);
        diff = setdiff(component, oldComponent);
        for j = 1 : length(diff)
            Components(k, j) = diff(j);
        end
        oldComponent = component;
        k = k + 1;
    end
end
end % End of 'Kosaraju' function

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

function component = FillComponent(graph, vertex, component)
global glFlags
global glComponentIndex

n = length(graph);
glFlags(vertex) = true;
component(glComponentIndex) = vertex;
glComponentIndex = glComponentIndex + 1;

for i = 1 : n
    if ~glFlags(i) && graph(vertex, i) > 0
        component = FillComponent(graph, i, component);
    end
end
end % End of 'FillComponent' function
