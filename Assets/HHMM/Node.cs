using System.Collections;
using System.Collections.Generic;

public enum Direction { vertical, horizontal };

public interface INode
{
    INode parent { get; set; }
    Direction direction { get; set; }
    INode Next();
}