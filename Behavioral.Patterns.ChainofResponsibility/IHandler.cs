using System;
using System.Collections.Generic;
using System.Text;

namespace Behavioral.Patterns.ChainofResponsibility;

public interface IHandler
{
    void SetNext(IHandler handler);

    void Handle(WithdrawRequest request);
}


public abstract class Handler : IHandler
{
    private IHandler _next;

    public void SetNext(IHandler handler)
    {
        _next = handler;
    }

    public virtual void Handle(WithdrawRequest request)
    {
        _next?.Handle(request);
    }
}

