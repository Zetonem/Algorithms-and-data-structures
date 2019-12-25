function IsVisited = BFS(graph, startVertex, lastVertex)
%BFS Breadth-first search.
%   ARGUMENTS:
%      graph - graph to crawl.
%      startVertex - start vertex number to crawl.
%      lastVertex - last vertex to visit.
% RETURNS:
%      IsVisited - is last vertex was visited.
n = length(graph);
q = Queue();
q.Put(startVertex);
visited = false(1, n);
visited(startVertex) = true;
while q.Size ~= 0
    v = q.Get();
    for i = 1 : n
        if ~visited(i) && graph(v, i) > 0
            if i == lastVertex
                IsVisited = true;
                return;
            end
            visited(i) = true;
            q.Put(i);
        end
    end
end
IsVisited = visited(lastVertex);
end % End of 'BFS' function
