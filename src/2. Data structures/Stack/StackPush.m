function [resultStack] = StackPush(stack, item)
if (~isstruct(stack))
    resultStack = stack;
elseif (StackIsEmpty(stack))
    resultStack.Size = 1;
    resultStack.Top = struct("Value", item, "Next", []);
else
    stack.Top = struct("Value", item, "Next", stack.Top);
    stack.Size = stack.Size + 1;
    resultStack = stack;
end
end
