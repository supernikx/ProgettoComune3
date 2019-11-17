using System;
using UnityEngine;

public class Boss1StateBase : StateBase
{
    /// <summary>
    /// Riferimento sovrascritto del context
    /// </summary>
    protected new Boss1SMController.Context context;

    /// <summary>
    /// Setup dello stato
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_onStateStartCallback"></param>
    /// <param name="_onStateEndCallback"></param>
    public override void Setup(IContext _context, Action<IState> _onStateStartCallback, Action<IState> _onStateEndCallback)
    {
        base.Setup(_context, _onStateStartCallback, _onStateEndCallback);
        context = _context as Boss1SMController.Context;
    }
}
