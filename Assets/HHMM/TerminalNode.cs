using System.Collections;
using System.Collections.Generic;

public class TerminalNode : INode
{
    public NodeType type { get; set; } = NodeType.terminalNode;
    public INode parent { get; set; }
    public Direction direction { get; set; } = Direction.vertical;

    public TerminalNode(INode parent)
    {
        this.parent = parent;
        this.type = NodeType.terminalNode;
    }

    public INode Next()
    {
        this.parent.direction = Direction.horizontal;
        return this.parent;
    }
}
