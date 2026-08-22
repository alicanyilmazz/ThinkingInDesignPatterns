using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern;

// ======================================================
// COMMAND
// ======================================================

public interface IOrderCommand
{
    void Execute();
}
