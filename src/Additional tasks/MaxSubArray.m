function [ResultSum, ResultLeftIndex, ResultRightIndex] = MaxSubArray(array, first, last)
n = length(array);
ResultLeftIndex = first;
ResultRightIndex = last;
if n == 1
    ResultSum = array(1);
else
    mid = fix(n / 2);
    arrayLeft = array(1 : mid);
    arrayRight = array(mid + 1 : n);
    
    %% Left subarray processing
    leftResult = -inf;    
    for leftIndex = 1 : length(arrayLeft)
        leftArraySum = sum(arrayLeft(leftIndex : end));
        if leftArraySum > leftResult
            leftResult = leftArraySum;
            ResultLeftIndex = leftIndex + first - 1;
        end
    end
    
    %% Right subarray processing
    rightResult = -inf;    
    for rightIndex = 1 : length(arrayRight)
        rightArraySum = sum(arrayRight(1 : rightIndex));
        if rightArraySum > rightResult
            rightResult = rightArraySum;
            ResultRightIndex = first + round((last - first) / 2) + rightIndex - 1;
        end
    end
    
    %% Result getting
    midResult = leftResult + rightResult;
    [arrayLeftResult, arrayLeftResultLeftIndex, arrayLeftResultRightIndex] = MaxSubArray(arrayLeft, first, first + mid - 1);
    [arrayRightResult, arrayRightResultLeftIndex, arrayRightResultRightIndex] = MaxSubArray(arrayRight, first + mid, last);
    if midResult > arrayLeftResult && midResult > arrayRightResult
        ResultSum = midResult;
    elseif arrayLeftResult > arrayRightResult
        ResultSum = arrayLeftResult;
        ResultLeftIndex = arrayLeftResultLeftIndex;
        ResultRightIndex = arrayLeftResultRightIndex;
    else
        ResultSum = arrayRightResult;
        ResultLeftIndex = arrayRightResultLeftIndex;
        ResultRightIndex = arrayRightResultRightIndex;
    end
end
end

