function DFS(graph)
%DFS Depth-first search.
%   Starts a crawl for every unvisited vertex.
%   ARGUMENTS:
%      graph - graph to crawl.
%   RETURNS: None.

global glFlags % Vertices state
n = length(graph);
glFlags = false(1, n);

for i = 1 : n
    if ~glFlags(i)
        BackgroungDFS(graph, i)
    end
end
end % End of 'DFS' function

function BackgroungDFS(graph, vertex)
%BACKGROUNDDFS Recursive traversal function for each vertex neighbor.
%   ARGUMENTS:
%      graph - graph to crawl.
%      vertex - vertex number to check neighbors.
%   RETURNS: None.

global glFlags
glFlags(vertex) = true;
neighbors = graph(vertex).Neighbors;
n = length(neighbors);

for i = 1 : n
    next = neighbors(i);
    if ~glFlags(next)
        BackgroungDFS(graph, next);
    end
end
end % End of 'BackgroungDFS' function
