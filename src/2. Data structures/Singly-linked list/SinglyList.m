classdef SinglyList < handle
    %SINGLY_LIST provides functionality to use BST data strucutre.
    %
    % Constructor:  list = SinglyList(item);
    %
    % Properties:   None.
    %
    % Methods:
    %               list.PushBack(item);
    %               list.PushStart(item);
    %               value = list.PopBack();
    %               value = list.PopStart();
    %               listSize = list.Size();
    % Author:       Leonid Zaytsev
    %               leonid.zaytsev.lz5@gmail.com
    %
    % Date:         09.11.2019
    
    properties (Access = private)
        m_head % The first list node
        m_tail % The last list node
        m_size % Kist size
    end
    
    methods (Access = public)
        function obj = SinglyList(item)
            %SINGLY_LIST Construct an instance of this class.
            
            if (nargin ~= 0)
                obj.m_head = SinglyListNode(item, []);
                obj.m_tail = obj.m_head;
                obj.m_size = 1;
            else
                obj.m_head = [];
                obj.m_tail = [];
                obj.m_size = 0;
            end
        end % End of 'SinglyList' constructor
        
        function listSize = Size(obj)
            %SIZE Gets current count of elements in the list.
            % ARGUMENTS: None.
            % RETURNS: list size.
            
            listSize = obj.m_size;
        end % End of 'Size' constructor
        
        function PushBack(obj, item)
            %PUSHBACK Adds new value to the end of the list.
            % ARGUMENTS:
            %   item: value to add.
            % RETURNS: None.
            
            newElement = SinglyListNode(item, []);
            
            if obj.m_size <= 0
                obj.m_head = newElement;
                obj.m_tail = obj.m_head;
            else
                obj.m_tail.Next = newElement;
                obj.m_tail = obj.m_tail.Next;
            end
            
            obj.m_size = obj.m_size + 1;
        end % End of 'PushBack' method
        
        function PushStart(obj, item)
            %PUSHSTART Adds new value to the start of the list.
            % ARGUMENTS:
            %   item: value to add.
            % RETURNS: None.           
            
            obj.m_head = SinglyListNode(item, obj.m_head);
            
            obj.m_size = obj.m_size + 1;
        end % End of 'PushStart' method
        
        function value = PopBack(obj)
            %POPBACK Deletes value from the back of the list.
            % ARGUMENTS: None.
            % RETURNS: deleted value.
            
            if obj.m_size <= 0
                error("Trying to delete value from the empty list.");
            end
            
            currentNode = obj.m_head;
            
            % Search prelast node to make it last node
            while ~isempty(currentNode.Next)
               previousNode = currentNode;
               currentNode = currentNode.Next;
            end
            
            value = obj.m_tail.Data;
            
            previousNode.Next = [];
            obj.m_tail = previousNode;            
            
            obj.m_size = obj.m_size - 1;
        end % End of 'PopBack' method
        
        function value = PopStart(obj)
            %POPSTART Deletes value from the start of the list.
            % ARGUMENTS: None.
            % RETURNS: deleted value.
            
            if obj.m_size <= 0
                error("Trying to delete value from the empty list.");
            end
            
            value = obj.m_head.Data;
            obj.m_head = obj.m_head.Next;
            
            obj.m_size = obj.m_size - 1;
        end % End of 'PopStart' method
        
        function Display(obj)
            %DISPLAY Shows current list state.
            % ARGUMENTS: None.
            % RETURNS: None.
            
            fprintf("List:\n");
            currentNode = obj.m_head;
            
            for i = 1 : obj.m_size
                fprintf("%d, ", currentNode.Data);
                currentNode = currentNode.Next;
            end
            
            fprintf("\n");
        end % End of 'Display' method
    end % End of public methods module
end % End of 'SinglyList' class

