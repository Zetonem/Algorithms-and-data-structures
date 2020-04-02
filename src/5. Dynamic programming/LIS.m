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
        if array(j) < array(i) && 1 + d(j) > d(i)
            d(i) = d(j) + 1;
            prev(i) = j;
        end
    end
end

Res = d(1);
pos = 1;

for i = 1 : n
    if (d(i) > Res)
        Res = d(i);
        pos = i;
    end
end

i = 1;
while pos ~= -1
    Path(i) = pos;
    pos = prev(pos);
    i = i + 1;
end

n = length(Path);
for i = 1 : n / 2
    tmp = Path(i);
    Path(i) = Path(n - i + 1);
    Path(n - i + 1) = tmp;
end
end % End of 'LIS' function

