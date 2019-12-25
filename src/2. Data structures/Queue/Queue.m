classdef Queue < handle
    %Queue provides functionality to use FIFO data strucutre.
    %
    % Constructor: q = Queue(capacity)
    %
    % Properties: None.
    %
    % Methods:
    %           queueSize = q.Size();
    %                       q.Put(item);
    %           value     = q.Get();
    %                       q.Display();
    % Author:   Leonid Zaytsev
    %           leonid.zaytsev.lz5@gmail.com
    %
    % Date:     09.11.2019
    
    properties (Access = private)
        m_headIndex % The first queue element index
        m_tailIndex % The first last element index
        m_buffer    % The queue buffer to store data
        m_capacity  % Queue capacity
    end
    
    methods
        function obj = Queue(capacity)
            % Class constructor by capacity.
            
            % If capacity haven't be passed set capacity to the default
            % value
            if nargin == 0
                capacity = 4;
            end
            
            if capacity < 1
                error("Argument invalid exception. Capacity must be more than zero.");
            end
            
            obj.m_headIndex = 1;
            obj.m_tailIndex = 1;
            obj.m_capacity = capacity;
            obj.m_buffer = cell(obj.m_capacity, 1);            
        end % End of 'Queue' constructor
     
        function queueSize = Size(obj)
            % Gets current queue size.
            % ARGUMENTS: None.
            % RETURNS: current queue size.
            
            if obj.m_tailIndex >= obj.m_headIndex
                queueSize = obj.m_tailIndex - obj.m_headIndex;
            else
                queueSize = obj.m_tailIndex - obj.m_headIndex + obj.m_capacity;
            end
        end % End of 'Size' method
        
        function Put(obj, item)
            % Puts new element to the queue.
            % ARGUMENTS:
            %   item: value to put;
            % RETURNS: None.
            
            currentSize = obj.Size();
            
            % If current queue size is more than storage can store allocate
            % more memory for new elements.
            if currentSize >= obj.m_capacity - 1
                if obj.m_tailIndex >= obj.m_headIndex
                    obj.m_buffer(1 : currentSize) = obj.m_buffer(obj.m_headIndex : obj.m_tailIndex - 1);
                else
                    obj.m_buffer(1 : currentSize) = obj.m_buffer(obj.m_headIndex : obj.m_capacity - 1, 1 : obj.m_tailIndex);
                end
                obj.m_buffer(currentSize + 1 : obj.m_capacity * 2) = cell(obj.m_capacity * 2 - currentSize, 1);
                obj.m_capacity = numel(obj.m_buffer);
                obj.m_headIndex = 1;
                obj.m_tailIndex = currentSize + 1;
            end
            
            obj.m_buffer{obj.m_tailIndex} = item;
            obj.m_tailIndex = mod(obj.m_tailIndex, obj.m_capacity) + 1;
        end % End of 'Put' method

        function value = Get(obj)
            % Gets element from the queue start.
            % ARGUMENTS: None.
            % RETURNS: got value.
        
            if obj.Size() <= 0
                error("Trying to get element from the empty queue.")
            else
                value = obj.m_buffer{obj.m_headIndex};
                obj.m_headIndex = mod(obj.m_headIndex, obj.m_capacity) + 1;
                if obj.m_headIndex > obj.m_capacity
                    obj.m_headIndex = 1;
                end
            end
        end % End of 'Get' method
        
        function Display(obj)
            % Shows current queue state.
            % ARGUMENTS: None.
            % RETURNS: None.
        
            fprintf("Queue the first element index: %d\n", obj.m_headIndex);
            fprintf("Queue the last element index: %d\n", obj.m_tailIndex);
            fprintf("Queue current size: %d\n", obj.Size());
            fprintf("Queue capacity: %d\n", obj.m_capacity);
            
            if obj.Size > 0
                if obj.m_tailIndex >= obj.m_headIndex
                    for i = obj.m_headIndex : obj.m_headIndex + obj.Size()
                        disp(obj.m_buffer{i});
                    end
                else
                    for i = obj.m_headIndex : obj.m_capacity
                        disp(obj.m_buffer{i});
                    end
                end
            end            
        end % End of 'Display' method
    end % End of public methods module
end % End of 'Queue' class

