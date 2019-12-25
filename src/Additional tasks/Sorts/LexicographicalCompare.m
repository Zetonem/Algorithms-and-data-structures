function Result = LexicographicalCompare(lhs, rhs)
% Ñompares two strings in lexicographical order
%
% Result = LexicographicalCompare(str1, str2)
%
% Arguments
% lhs [string] the first string to compare
% rhs [string] the second string to compare
%
% Example
% result = LexicographicalCompare('abc', 'bcd')
lhsLen = length(lhs);
rhsLen = length(rhs);

for i = 1 : min(lhsLen, rhsLen)
    if lhs(i) > rhs(i)
        Result = 1;
        return;
    elseif lhs(i) < rhs(i)
        Result = -1;
        return;
    end
end

if lhsLen < rhsLen
    Result = -1;
elseif lhsLen > rhsLen
    Result = 1;
else
    Result = 0;
end
end
