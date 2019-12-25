list = SinglyList();

N = 5;
for i = 1 : N
    list.PushBack(randi([-50, 50], 1));
end
fprintf("Create list with random values:\n");
list.Display();
 
for i = 1 : N
    value = list.PopBack();
    fprintf("Recieved value: %d\n", value);
end

list.PopBack();
