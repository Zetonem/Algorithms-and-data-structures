function [list] = CircularListCreate(n)
if n <= 0
    error("Argument exception");
end

% list = zeros(1, n);
for i = 1 : n
    list(i).Prev = i - 1;
    list(i).Next = i + 1;
end
list(1).Prev = n;
list(n).Next = 1;
end

