using System.Collections;
using System.Collections.Generic;

public enum NodeType { stateNode, productionNode, terminalNode };
public enum Direction { vertical, horizontal };

public interface INode
{
    NodeType type { get; set; }
    INode parent { get; set; }
    Direction direction { get; set; }
    INode Next();
}