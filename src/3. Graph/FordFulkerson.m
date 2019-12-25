function [MaxFlow, Paths] = FordFulkerson(graph, begin, last)
%BFS Ford-Fulkerson algorithm implementation.
%   ARGUMENTS:
%      graph - graph to find max flow.;
%      begin - start vertex number;
%      last - last vertex number;
% RETURNS:
%      MaxFlow - max flow in the fraph from 'begin' to 'last'.

%% Initialization
MaxFlow = 0;
pathNum = 1;

[isPath, parent] = BFS(graph, begin, last);

%% Find paths while paths exist
while isPath
    currentVertex = last;
    pathFlow = inf;
    
    %% Restore path and count flow for it
    i = 1;
    while currentVertex ~= begin
        Paths(pathNum, i) = currentVertex;
        i = i + 1;
        nextVertex = parent(currentVertex);
        pathFlow = min(pathFlow, graph(nextVertex, currentVertex));
        currentVertex = parent(currentVertex);
    end
    
    Paths(pathNum, i) = currentVertex;
    
    %% Reverse path
    [~, m] = size(Paths);
    
    for i = 1 : m / 2
        tmp = Paths(pathNum, i);
        Paths(pathNum, i) = Paths(pathNum, m - i + 1);
        Paths(pathNum, m - i + 1) = tmp;
    end
    
    currentVertex = last;
    
    %% Count new flows for all visited paths
    while currentVertex ~= begin
        nextVertex = parent(currentVertex);
        graph(nextVertex, currentVertex) = graph(nextVertex, currentVertex) - pathFlow;
        graph(currentVertex, nextVertex) = graph(currentVertex, nextVertex) + pathFlow;
        currentVertex = parent(currentVertex);
    end
    
    %% Add current path flow value to the result and find new path
    MaxFlow = MaxFlow + pathFlow;
    pathNum = pathNum + 1;
    [isPath, parent] = BFS(graph, begin, last);
end
end % End of 'FordFulkerson' function

function [IsPath, Parent] = BFS(graph, begin, last)
%BFS Breadth-first search.
%   ARGUMENTS:
%      graph - graph to crawl;
%      begin - start vertex number to start crawl;
%      last - last vertex number to start crawl;
%      parent - store the path.
% RETURNS:
%      IsPath - is exists path from 'begin' to 'last' - true, overwise -
%      false;
%      Parent - result path.

n = length(graph);
q = Queue();
q.Put(begin);
visited = false(1, n);
visited(begin) = true;
Parent = zeros(1, n);
Parent(begin) = -1;

while q.Size ~= 0
    v = q.Get();
    
    for i = 1 : n
        if ~visited(i) && graph(v, i) > 0
            visited(i) = true;
            q.Put(i);
            Parent(i) = v;
        end
    end
end

IsPath = visited(last);
end % End of 'BFS' function
