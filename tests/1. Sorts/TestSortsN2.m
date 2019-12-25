N=1:100:2000;
TBubbleSort = zeros(1, length(N));
TInsertionSort = zeros(1, length(N));

for i= 1 : length(N)
    if (rem(i, 5) == 0)
      disp(i);
    end
    array = randperm(N(i));
    tic;
    sortedArray = BubbleSort(array);
    TBubbleSort(i) = toc;
    
    if (~IsSorted(sortedArray))
        fprintf("Bubble sort has got wrong");
    end
    tic; 
    sortedArray = InsertionSort(array);
    TInsertionSort(i) = toc;
    if (~IsSorted(sortedArray))
        fprintf("Insertion sort has got wrong");
    end
end
figure;
hold on;

grid on;
title('The O(N^2) sorts results graphic'); 
xlabel('Count of elements in array to sort');
ylabel('Sorting time (seconds)');

plot(N ,TBubbleSort, 'g')
pl = polyfit(N, TBubbleSort, 2);
disp(polyval(pl, 10^9));

plot(N, TInsertionSort, 'r')
p2 = polyfit(N, TInsertionSort, 2);
disp(polyval(p2, 10^9));

legend('Bubble sort', 'Insertion sort');