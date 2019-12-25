stringsArray = ["Leonid", "Julia", "Petr", "Flow", "Abc"];
if ~CheckSortString(stringsArray)
    fprintf("Array unsorted.\n");
end
sortedArray = MergeSortStrings(stringsArray);
if CheckSortString(sortedArray)
    fprintf("Array sorted.\n");
end
