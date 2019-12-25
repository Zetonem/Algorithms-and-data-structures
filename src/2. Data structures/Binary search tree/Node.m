classdef Node < handle
    properties (Access = public)
        Data,
        LeftChild,
        RightChild,
        Parent,
        Index
    end % End of public properties module
    
    methods (Access = public)
        function obj = Node(data, leftChild, rightChild, parent, index)
            obj.Data = data;
            obj.LeftChild = leftChild;
            obj.RightChild = rightChild;
            obj.Parent = parent;
            obj.Index = index;
        end % End of 'Node' constructor
    end % End of public methods mudule
end % End of 'Node' class

