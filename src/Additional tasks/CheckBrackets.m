function CheckBrackets(parseString)
parseString = char(parseString);
n = strlength(parseString);
stack = StackCreate();
for i = 1 : n
    currentChar = parseString(i);
    if currentChar == '('
        stack = StackPush(stack, currentChar);
    elseif currentChar == ')'
        if StackIsEmpty(stack) || i == 1
            exception = MException('CheckBrackets:parseError', 'Wrong brackets sequence. Symbol position: %d', i);
            throw(exception);
        end
        stack = StackPop(stack);
    end
end

if StackIsEmpty(stack)
    fprintf("Brackets sequence is correct.\n");
else
    exception = MException('CheckBrackets:parseError', 'Wait for symbol ). Symbol position: %d', n + 1);
    throw(exception);
end
end
