function [Res, Collection] = Knapsack01(w, p, Weight)
Collection = [];
Res = 0;
n = length(w);
A = zeros(n + 1, Weight + 1);

for i = 2 : n + 1
    for j = 1 : Weight + 1
        if j > w(i - 1)
            A(i, j) = max(A(i - 1, j), A(i - 1, j - w(i - 1)) + p(i - 1));
        else
            A(i, j) = A(i - 1, j);
        end
    end
end

A
Res = A(n + 1, Weight + 1);

i = n + 1;
j = Weight + 1;
index = 1;

while A(i, j) ~= 0
    if A(i - 1, j) == A(i, j)
        i = i - 1;
    else
        i = i - 1;
        j = j - w(i);
        Collection(index) = i;
        index = index + 1;
    end
end
end % End of 'Knapsack01' function
