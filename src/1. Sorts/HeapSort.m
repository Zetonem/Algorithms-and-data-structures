function array = HeapSort(array)
array = BuildHeap(array);
for p = length(array) : -1 : 2
    array([p, 1]) = array([1, p]);
    array = HeapPick(array, 1, p - 1);
end
end

function array = BuildHeap(array)
n = length(array);
for p = fix(n / 2) : -1 : 1
    array = HeapPick(array, p, n);
end
end

function array = HeapPick(array, index, heapSize)
left = 2 * index;
right = 2 * index + 1;
if left <= heapSize && array(left) > array(index)
    largest = left;
else
    largest = index;
end
if (right <= heapSize) && (array(right) > array(largest))
    largest = right;
end
if largest ~=index
    array([index, largest]) = array([largest, index]);
    array = HeapPick(array, largest, heapSize);
end
end
