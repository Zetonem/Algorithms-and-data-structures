function [index] = RabinKarp(str, pattern)
%RabinKarp Searchs substring in the string function by Rabin-Karp algorithm.
%   ARGUMENTS:
%       str - string to find a substring;
%       pattern - substring to search.
%   RETURNS:
%       index - if substring has found, than substring start index, else -1.

n = strlength(str);
m = strlength(pattern);
q = 433494437;
p = 29;
h = 1;

% Evalute p^m
for i = 2 : m + 1
    h = ModuloMult(h, p, q);
end

hStr = 0;
hPattern = 0;
% Evalute hash value for pattern string and the first window
for i = 1 : m
    hPattern = ModuloAdd(pattern(i), ModuloMult(p, hPattern, q), q);
    hStr = ModuloAdd(str(i), ModuloMult(p, hStr, q), q);
end


if hStr == hPattern
    index = 1;
    return;
end

for i = 2 : n - m + 1   
   hStr = ModuloAdd(ModuloMult(p, hStr, q), ModuloAdd(-ModuloMult(h, str(i - 1), q), str(i + m - 1), q), q);
   
   if hStr < 0
       hStr = hStr + q;
   end
   
   if hStr == hPattern
       index = i;
       for j = index : index + m - 1
           if str(j) ~= pattern(j - index + 1)
               break;
           end
       end
       return;
   end
end
index = -1;
end % End of 'RabinKarp' function

function res = ModuloAdd(x, y, q)
res = rem(rem(x, q) + rem(y, q), q);
end % End of 'ModuloAdd' function

function res = ModuloMult(x, y, q)
res = rem(rem(x, q) * rem(y, q), q);
end % End of 'ModuloMult' function

