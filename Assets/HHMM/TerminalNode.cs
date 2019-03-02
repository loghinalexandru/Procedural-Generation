using System.Collections;
using System.Collections.Generic;

public class TerminalNode : INode
{
    public INode parent { get; set; }
    public Direction direction { get; set; } = Direction.vertical;

    public TerminalNode(INode parent)
    {
        this.parent = parent;
    }

    public INode Next()
    {
        this.parent.direction = Direction.horizontal;
        return this.parent;
    }
}
