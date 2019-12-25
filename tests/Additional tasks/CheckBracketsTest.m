testSequences = ["", ")", "(", "()", "()()()", "(()())", "((())()","((()()(()())))"];

for i = 1 : length(testSequences)
    fprintf("%d) Input string: %s\n", i, testSequences(i));
    try
        CheckBrackets(testSequences(i));
    catch ex
        warning(ex.message);
        fprintf("\n");
    end
end