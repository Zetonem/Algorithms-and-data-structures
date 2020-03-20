function [index] = KnuthMorrisPratt(str, pattern)
%KnuthMorrisPratt Searchs substring in the string function by Knuth-Morris-Pratt algorithm.
%   ARGUMENTS:
%       str - string to find a substring;
%       pattern - substring to search.
%   RETURNS:
%       index - if substring has found, than substring start index, else -1.

n = length(str);
m = length(pattern);

a = [pattern, '#', str]; 
pref = PrefixFuction(a);

for i = 1 : n
    if pref(m + i) == m
        index = i - m;
        return;
    end
end
end % End of 'KnuthMorrisPratt' function

function pref = PrefixFuction(str)
n = length(str);
pref = zeros(1, n);
for i = 2 : n
    k = pref(i - 1);
    while k > 0 && str(i) ~= str(k + 1)
        k = pref(k);
    end
    if str(i) == str(k + 1)
        k = k + 1;
    end
    pref(i) = k;
end
end % End of 'PrefixFuction' function

