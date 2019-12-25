function result = josephus(n, k)
%JOSEPHUS Josephus problem solution.

list = CircularListCreate(n);
current = mod(k, n);
if current == 0
    current = 1;
end

for i = 1 : n - 1
    list(list(current).Prev).Next = list(current).Next;
    list(list(current).Next).Prev = list(current).Prev;
    
    for j = 1 : k
        current = list(current).Next;
    end
end
result = list(current).Next;
end

