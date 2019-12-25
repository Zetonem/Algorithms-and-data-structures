q = Queue();

N = 5;
for i = 1 : N
    q.Put(randi([-50, 50], 1));
end
fprintf("Create queue with random values:\n");

q.Display();

for i = 1 : N
    value = q.Get();
    fprintf("Recieved value: %d\n", value);
end

% Try to make error
q.Get()

% q.Put(1);
% q.Display();
% q.Put(2);
% q.Display();
% q.Put(3);
% q.Display();
% q.Put(4);
% 
% q.Get()
% q.Get()
% q.Get()
% 
% a = [6, 7, 8, 9, 10];
% 
% q.Put(1);
% q.Display();
% q.Put(2);
% q.Display();
% q.Put(3);
% q.Display();
% q.Put(4);
% q.Display();
% q.Put((5));
% q.Display();
% 
% for i = 1 : length(a)
%     q.Put(a(i));
% end
% 
% q.Display();
% 
% for i = 1 : length(a) + 5
%     q.Get()
% end
% 
% for i = 1 : length(a)
%     q.Put(a(i));
% end
% 
% q.Display();
% 
% q.Put(13);
% 
% q.Display();
% 
% for i = 1 : length(a) + 1
%     q.Get();
%     q.Display();
% end