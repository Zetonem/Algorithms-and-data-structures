function result = CheckSortString(array)
result = true;
for i = 1 : length(array) - 1
    if LexicographicalCompare(array(i), array(i + 1)) > 0
        result = false;
        return;
    end
end
end

