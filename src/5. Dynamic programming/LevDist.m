function [Res] = LevDist(a, b)
%LEVDIST Searchs distance between two words is the minimum number of
%single-character edits.
%   ARGUMENTS:
%       a, b - strings to compare.
%   RETURNS:
%       Res - distance between two words.

n = length(a) + 1;
m = length(b) + 1;
Res = 0;
d = zeros(n, m);

for i = 2 : n
    d(i, 1) = i - 1;
end

for j = 2 : m
    d(1, j) = j - 1;
end

for i = 2 : n
    for j = 2 : m
        if a(i - 1) ~= b(j - 1)
            d(i, j) = min(d(i, j - 1) + 1, min(d(i - 1, j) + 1, d(i - 1, j - 1) + 1));
        else
            d(i, j) = d(i - 1, j - 1);
        end
    end
end

d
Res = d(n, m);
end % End of 'LevDist' function

