function result = IsSorted(array)
result = true;
for i = 1 : length(array) - 1
    if (array(i) > array(i + 1))
        result = false;
        return;
    end
end
end

