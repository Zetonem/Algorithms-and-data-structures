N = 1:200:20000;
TMergeSort = zeros(1, length(N));
TQuickSort = zeros(1, length(N));
THeapSort = zeros(1, length(N));
times=zeros(1, length(N));

for i = 1: length(N)
    if (rem(i, 10) == 0)
      disp(i);
    end
    a = randperm(N(i));
    tic;
    sortedArray = MergeSort(a);
    TMergeSort(i)=toc;
    if (~IsSorted(sortedArray))
        fprintf("Merge sort has got wrong");
    end
    tic;
    sortedArray = QuickSort(a, 1, length(a));
    TQuickSort(i)=toc;
    if (~IsSorted(sortedArray))
        fprintf("Quick sort has got wrong");
    end
    tic;
    sortedArray = HeapSort(a);
    THeapSort(i)=toc;
    if (~IsSorted(sortedArray))
        fprintf("Heap sort has got wrong");
    end
end
figure;
hold on;
grid on;
title('The O(N^2) sorts results graphic'); 
xlabel('Count of elements in array to sort');
ylabel('Sorting time (seconds)');

plot(N, TMergeSort, 'r')
pl = polyfit(N.*log(N), TMergeSort, 1);
resultMergeSort = polyval(pl, 10^9 * log(10^9))

plot(N, TQuickSort, 'g')
p2 = polyfit(N.*log(N), TQuickSort, 1);
resultQuickSort = polyval(p2, 10^9 * log(10^9))

plot(N, THeapSort, 'b')
p3 = polyfit(N.*log(N), THeapSort, 1);
resultHeapSOrt = polyval(p3, 10^9 * log(10^9))

legend('Merge sort', 'Quick sort', 'Heap sort');
