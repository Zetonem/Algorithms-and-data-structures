function [Res, Collection] = Knapsack(w, p, Weight)
Res = 0;
Collection = [];

n = length(w);
d = zeros(n + 1, Weight + 1);

for i = 2 : n + 1
    for j = 1 : Weight + 1
        if j > w(i - 1)
            d(i, j) = max(d(i - 1, j), d(i, j - w(i - 1)) + p(i - 1));
        else
            d(i, j) = d(i - 1, j);
        end
    end
end
d
Res = d(i, j);

i = n + 1;
j = Weight + 1;
index = 1;

while d(i, j) ~= 0
    if d(i - 1, j) == d(i, j)
        i = i - 1;
    else
        j = j - w(i - 1);
        Collection(index) = i - 1;
        index = index + 1;
    end
end
end % End of 'Knapsack' function

