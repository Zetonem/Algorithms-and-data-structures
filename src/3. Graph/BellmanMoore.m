function [distances, Paths] = BellmanMoore(graph, start)
n = length(graph);
Parents = zeros(1, n);
distances = zeros(1, n);
for i = 1 : n
    distances(i) = inf;
end
distances(start) = 0;

for i = 1 : n - 1    
    for j = 1 : n
        if graph(i, j) > 0 && distances(j) > distances(i) + graph(i, j)
            distances(j) = distances(i) + graph(i, j);
            Parents(j) = i;
        end
    end
end

Paths = cell(1, n);
Paths{1} = start;
for i = 2 : n
    if Parents(i) ~= 0
        j = i;
        Paths{i} = i;
        while Parents(j) ~= 0
            Paths{i} = [Parents(j) Paths{i}];
            j = Parents(j);
        end
    end
end
end

