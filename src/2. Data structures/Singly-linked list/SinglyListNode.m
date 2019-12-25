classdef SinglyListNode < handle
    %SINGLYLISTNODE Handles singly-linked list node.
    %
    % Constructor:  node = SinglyListNode(data, next);
    %
    % Properties:
    %               Data - Data in the node;
    %               Next - Next node pointer;
    %
    % Methods:      None.
    % Author:       Leonid Zaytsev
    %               leonid.zaytsev.lz5@gmail.com
    %
    % Date:         09.11.2019

    properties (Access = public)
        Data % Data in the node
        Next % Next node pointer
    end
    
    methods (Access = public)
        function obj = SinglyListNode(data, next)
            %SINGLYLISTNODE Construct an instance of this class.

            obj.Data = data;
            obj.Next = next;
        end % End of 'SinglyListNode' constructor
    end % End of public methods module
end % End of 'SinglyListNode' class

