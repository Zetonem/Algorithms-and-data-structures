[ans, coll] = Knapsack([3, 4, 5, 8, 9], [1, 6, 4, 7, 6], 13)
%LevDist('exponential', 'polynomial')

% a(1) = 1;
% a(2) = 5;
% a(3) = 3;
% a(4) = 7;
% a(5) = 1;
% a(6) = 4;
% a(7) = 10;
% a(8) = 15;
% 
% [ans, path] = LIS(a)
% 
% n = 100 : 100 : 10000
% t1 = zeros(1, length(n));
% 
% for i = 1 : length(n)
%     a = zeros(1, n(i));
%     for k = 1 : n(i)
%         a(k) = length(a) * rand(1, 1);
%     end
%     
%     tic
%     LIS(a);
%     t1(i) = toc;
% end
% 
% figure;
% hold on;
% 
% plot(n, t1);