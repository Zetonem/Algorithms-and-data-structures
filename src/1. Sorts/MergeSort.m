function array = MergeSort(array)
if length(array) > 1
    m = fix(length(array) / 2);
    array = Merge(MergeSort(array(1 : m)), MergeSort(array(m+1:end)));
end
end

function [mergedArray] = Merge(leftArray, rightArray)
n = length(leftArray);
m = length(rightArray);
mergedArray = zeros(1,n+m);
left = 1;
right = 1;
  for i = 1 : (n + m)
    if left > n
      mergedArray(i) = rightArray(right);
      right = right + 1;
    elseif right > m
        mergedArray(i) = leftArray(left);
        left = left + 1;
    elseif leftArray(left) <= rightArray(right)
        mergedArray(i) = leftArray(left);
        left = left + 1;
    else
      mergedArray(i) = rightArray(right);
      right = right + 1;
    end
  end
end
