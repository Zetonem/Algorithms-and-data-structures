stack = StackCreate();
N = 5;
for i = 1 : N
    stack = StackPush(stack, randi([-50, 50], 1));
end
fprintf("Create stack with random values:\n");
StackDisplay(stack);
fprintf("\n");

for i = 1 : N
    [stack, value] = StackPop(stack);
    disp(value)
end

StackPop(stack);
