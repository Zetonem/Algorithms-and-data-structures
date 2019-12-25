function [resultStack, popValue] = StackPop(stack)
if (~isstruct(stack))
    error("ERROR! Passed stack isn't structure!");
elseif (StackIsEmpty(stack))
    error("Stack is already empty!");
else
popValue = stack.Top.Value;
stack.Top = stack.Top.Next;
stack.Size = stack.Size - 1;
resultStack = stack;
end
