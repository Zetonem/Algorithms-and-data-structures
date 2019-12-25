function StackDisplay(stack)
if (~isstruct(stack) || StackIsEmpty(stack))
    fprintf("Stack is already empty");
else
    N = stack.Size;
    for i = 1 : N
        [stack, value] = StackPop(stack);
        fprintf("%d) Data: %d\n\n", N - i + 1, value);
    end
end
