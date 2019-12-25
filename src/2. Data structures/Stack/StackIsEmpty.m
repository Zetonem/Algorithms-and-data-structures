function [isEmpty] = StackIsEmpty(stack)
isEmpty = (isempty(stack.Size)) || (stack.Size <= 0);
end
