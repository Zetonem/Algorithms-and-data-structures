function [Res, Path] = LIS(array)
%LIS Searchs the largest increasing subsequence algorithm.
%   ARGUMENTS:
%       array - sequence to search.
%   RETURNS:
%       Res - the largest increasing subsequence length;
%       Path - the largest increasing subsequence.

Res = 0;
Path = [];

n = length(array);
d = zeros(1, n);
prev = zeros(1, n);

for i = 1 : n
    d(i) = 1;
    prev(i) = -1;
    for j = 1 : n
        if array(j) < array(i) && d(j) + 1 > d(i)
        end
    end
end
end % End of 'LIS' function

