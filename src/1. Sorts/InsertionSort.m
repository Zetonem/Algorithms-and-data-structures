function array = InsertionSort(array)
for p=2:length(array)
    for q = p - 1 : -1 : 1
        if (array(q) > array(q + 1))
            array([q, q + 1]) = array([q + 1, q]);
        end
    end
end
end
