function array = BubbleSort(array)
n = length(array);
for p = 1 : n - 1
    for q = 2 : (n - p + 1)
        if (array(q) < array(q - 1))
            array([q, q - 1]) = array([q - 1, q]);
        end
    end
end
