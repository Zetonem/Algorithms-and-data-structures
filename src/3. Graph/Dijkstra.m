function [PathValue, Path] = Dijkstra(graph, startVertex, lastVertex)
%DIJKSTRA  Dijkstra algorithm for finding the shortest paths between nodes
%in a graph.
%   ARGUMENTS:
%      graph - graph to crawl.
%      startVertex - start vertex number to crawl.
%      lastVertex - last vertex to visit.
%   RETURNS:
%      PathValue - value of the path found.
%      Path - array of vertices of the found path.

n = length(graph);
distances = zeros(1, n);
for i = 1 : n
    distances(i) = inf;
end
distances(startVertex) = 0;
visitedVertices = false(1, n);

% Go around all the vertices
for i = 1 : n
    vertex = -1;
    % Find vertex with min distance
    for j = 1 : n
        if ~visitedVertices(j) && (distances(j) < distances(i) || vertex == -1)
            vertex = j;
        end        
    end
    if distances(vertex) == inf
        break;
    end
    visitedVertices(vertex) = true;
    for j = 1 : n
        if graph(vertex, j) > 0 && distances(j) > distances(vertex) + graph(vertex, j)
            distances(j) = distances(vertex) + graph(vertex, j);
        end
    end
end

PathValue = distances(lastVertex);

% Recovery path
Path(1) = lastVertex;
k = 2;
i = lastVertex;
w = distances(i); % Last vertex weight
while i ~= startVertex
    for j = 1 : n
        if graph(j, i) > 0 && distances(j) == w - graph(j, i)
            w = w - graph(j, i);
            i = j;
            Path(k) = i;
            k = k + 1;
            break;
        end
    end
end

end % End of 'Dijkstra' function

