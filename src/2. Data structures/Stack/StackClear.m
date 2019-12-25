function [emptyStack] = StackClear(stack)
while stack.Size > 0
    [stack, ~] = StackPop(stack);
end
emptyStack = stack;
end

