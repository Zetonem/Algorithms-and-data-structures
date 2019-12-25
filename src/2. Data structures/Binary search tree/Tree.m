classdef Tree < handle
    properties (Access = public)
        Nodes = { [] };
        Root;
        Size;
    end % End of private properties module
    
    methods (Access = public)
        function obj = Tree()
            %TREE Class default constructor
            
            obj.Size = 0;
        end % End of 'Tree' constructor
        
        function resultNodeIndex = Search(obj, item)
            %SEARCH Searchs node index in the tree.
            %   ARGUMENTS:
            %       item - node data to find.
            %   RETURNS:
            %       resultNodeIndex - found index.
            
            currentNodeIndex = obj.Root;
            
            while true
                if obj.Nodes{currentNodeIndex}.Data == item
                    resultNodeIndex = currentNodeIndex;
                    return;
                elseif obj.Nodes{currentNodeIndex}.Data > item
                    currentNodeIndex = obj.Nodes{currentNodeIndex}.LeftChild;
                else
                    currentNodeIndex = obj.Nodes{currentNodeIndex}.RightChild;
                end
            end
        end % End of 'Search' function
        
        function InsertNode(obj, item)
            %INSERTNODE Insert new data in the tree.
            %   ARGUMENTS:
            %       item - data to insert.
            %   RETURNS: None.
            
            % Create new node
            node = Node(item, [], [], [], []);
            
            % If the first node in the tree
            if obj.Size == 0
                node.Index = 1;
                obj.Nodes = { node };
                obj.Root = 1;
                obj.Size = obj.Size + 1;
                return;
            end
            
            % Add new node to the tree
            n = numel(obj.Nodes) + 1;
            obj.Nodes{n} = node;
            obj.Nodes{n}.Index = n;
            
            currentNode = obj.Nodes{obj.Root};
            
            % Find parent node for current node
            while true
                if currentNode.Data > node.Data
                    if isempty(currentNode.LeftChild)
                        parent = currentNode.Index;
                        obj.Nodes{parent}.LeftChild = n;
                        break;
                    else
                        currentNode = obj.Nodes{currentNode.LeftChild};
                    end
                else
                    if isempty(currentNode.RightChild)
                        parent = currentNode.Index;
                        obj.Nodes{parent}.RightChild = n;
                        break;
                    else
                        currentNode = obj.Nodes{currentNode.RightChild};
                    end
                end
            end
            
            % Add parent node for the current node
            obj.Nodes{n}.Parent = parent;
            
            % Update tree size
            obj.Size = obj.Size + 1;
        end % End of 'InsertNode' function
        
        function RemoveNode(obj, item)
            %REMOVENODE Delete data from the tree.
            %   ARGUMENTS:
            %       item - data to delete.
            %   RETURNS: None.
            
            % Search 
            nodeIndex = obj.Search(item);
            node = obj.Nodes{nodeIndex};
            parent = node.Parent;
            
            % If node has got 0 children
            if isempty(node.LeftChild) && isempty(node.RightChild)
                if obj.Nodes{parent}.LeftChild == node.Index
                    obj.Nodes{parent}.LeftChild = [];
                end
                if obj.Nodes{parent}.RightChild == node.Index
                    obj.Nodes{parent}.RightChild = [];
                end
            % If node has got 1 children
            elseif isempty(node.LeftChild) || isempty(node.RightChild)
                % Determine the node in which the child
                if isempty(node.LeftChild)
                    if obj.Nodes{parent}.LeftChild == node.Index
                        obj.Nodes{parent}.LeftChild = node.RightChild;
                    else
                        obj.Nodes{parent}.RightChild = node.RightChild;
                    end                    
                    obj.Nodes{node.RightChild}.Parent = parent;
                else
                    if obj.Nodes{parent}.LeftChild == node.Index
                        obj.Nodes{parent}.LeftChild = node.LeftChild;
                    else
                        obj.Nodes{parent}.RightChild = node.LeftChild;
                    end                    
                    x = obj.Nodes{node.LeftChild};
                    if ~isempty(x.LeftChild)
                        obj.Nodes{{obj.Nodes{node.LeftChild}.LeftChild}}.Parent = parent;
                    end
                end
            % If node has got 2 children
            else
                % Find node to replace current node
                next = obj.Minimum(obj.Nodes{node.RightChild});
                
                % Update data in the node
                obj.Nodes{nodeIndex}.Data = next.Data;
                
                if obj.Nodes{next.Parent}.LeftChild == next.Index
                    obj.Nodes{next.Parent}.LeftChild = next.RightChild;
                    if ~isempty(next.RightChild)
                        obj.Nodes{next.RightChild}.Parent = next.Parent;
                    end
                else
                    obj.Nodes{next.Parent}.RightChild = next.LeftChild;
                    if ~isempty(next.LeftChild)
                        obj.Nodes{next.RightChild}.Parent = next.Parent;
                    end
                end
            end
            
            % Update tree size
            obj.Size = obj.Size - 1;
        end % End of 'RemoveNode' function
        
        function result = Across(obj)
            %ACROSS Wide walk tree function.
            %   ARGUMENTS: None.
            %   RETURNS: None.
            
            q = Queue();
            q.Put(obj.Nodes{1});
            
            result = cell(1, obj.Size);
            k = 1; % Current position in the result array
            
            % Traversal all vertices
            while q.Size ~= 0
                a = q.Peek();
                
                if ~isempty(a.LeftChild)
                    q.Put(obj.Nodes{a(1).LeftChild});
                end
                
                if ~isempty(a(1).RightChild)
                    q.Put(obj.Nodes{a(1).RightChild});
                end
            
                % Save answer to the result array
                result{k} = q.Peek();
                k = k + 1;
                
                fprintf("%d, ", q.Get().Data);
            end
            fprintf("\n");
        end % End of 'Across' function
        
        function Display(obj)
            %DISPLAY Display tree on the screen function.
            %   ARGUMENTS: None.
            %   RETURNS: None.
            
            obj.DisplayBackground(obj.Nodes{1}, 0);
        end % End of 'Display' function
    end % End of public methods mudule
    
    methods (Access = private)
        function DisplayBackground(obj, node, level)
            %DISPLAYBACKGROUND Display tree on the screen function.
            %   ARGUMENTS:
            %       node - current node to output;
            %       level - current node level in the tree.
            %   RETURNS: None.
            
            if ~isempty(node)
                % Process right child                
                if ~isempty(node.RightChild)
                    obj.DisplayBackground(obj.Nodes{node.RightChild}, level + 1);
                else
                    obj.DisplayBackground([], level + 1);
                end
                
                % Output current node
                for i = 1 : level
                    fprintf("    ");
                end
                
                fprintf("%d\n\n", node.Data);
                
                % Process left child
                if ~isempty(node.LeftChild)
                    obj.DisplayBackground(obj.Nodes{node.LeftChild}, level + 1);
                else
                    obj.DisplayBackground([], level + 1);
                end
            end
        end % End of 'DisplayBackground' function
        
        function result = Minimum(obj, node)
            %MINIMUM Find min node in the current subtree.
            %   ARGUMENTS:
            %       node - subtree to find.
            %   RETURNS:
            %       result - min node.
            
            if isempty(node.LeftChild)
                result = obj.Nodes{node.Index};
                return;
            end
            result = obj.Minimum(obj.Nodes{node.LeftChild});
        end % End of 'Minimum' function
    end % End of private methods mudule
end % End of 'Tree' class

