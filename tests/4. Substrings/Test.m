n = 100 : 100 : 10000;
m = 5 : 5 : 20;
t1 = zeros(length(m), length(n));
t2 = zeros(length(m), length(n));
t3 = zeros(length(m), length(n));
array = 'abcdefghijklmnopqrstuvwxyz';

for i = 1 : length(m)
    for j = 1 : length(n)
        for k = 1 : n(j)
            str(k) = array(floor(length(array) * rand(1, 1)) + 1);
        end
        
        index = floor((n(j) - m(i)) * rand(1, 1)) + 1;
        % index = length(str) - m(i) - 1;
        substr = str(index : index + m(i) - 1);
        
        tic
        resIndex = Pos(str, substr);
        t1(i, j) = t1(i, j) + toc;
        
        if (resIndex ~= index)
            display("Wrong result.");
        end
        
        tic
        resIndex = RabinKarp(str, substr);
        t2(i, j) = t2(i, j) + toc;
        
        if (resIndex ~= index)
            display("Wrong result.");
        end
        
        tic
        resIndex = KnuthMorrisPratt(str, substr);
        t3(i, j) = t3(i, j) + toc;
        
        if (resIndex ~= index)
            display("Wrong result.");
        end
    end
end
figure;
hold on;

grid on;
title('Substring search graphic'); 
xlabel('String length');
ylabel('Search time (seconds)');


for i = 1 : length(n)
    tmp(i) = t1(2, i);
end
plot(n, tmp, 'r')

for i = 1 : length(n)
    tmp(i) = t2(2, i);
end
plot(n, tmp, 'g')

for i = 1 : length(n)
    tmp(i) = t3(2, i);
end
plot(n, tmp, 'b')