function a = QuickSort(a, s, t)
if (s < t)
    [a, q] = partition(a, s, t);
    a = QuickSort(a, s, q - 1);
    a = QuickSort(a, q + 1, t);
end
end

function [a, q] = partition(a, s, t)
pivot = a(t);
r = s - 1;
for p = s : t - 1
    if a(p) <= pivot
        r = r + 1;
        a([p,r]) = a([r,p]);
    end
end
q = r + 1;
a([q, t]) = a([t,q]);
end
