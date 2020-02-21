function [index] = Pos(str, pattern)
%Pos Searchs substring in the string function.
%   ARGUMENTS:
%       str - string to find a substring;
%       pattern - substring to search.
%   RETURNS:
%       index - if substring has found, than substring start index, else -1.

n = strlength(str);
m = strlength(pattern);
for i = 1 : n - m + 1
    if str(i : i + m - 1) == pattern
        index = i;
        return;
    end
end

index = -1;
end % End of 'Pos.m' function