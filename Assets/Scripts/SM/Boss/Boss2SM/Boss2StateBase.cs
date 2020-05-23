using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe dello stato base della state machine del Boss 2
/// </summary>
public class Boss2StateBase : StateBase
{
    [Header("General Settings")]
    //Se il boss può prendere danno diretto
    [SerializeField]
    protected bool canTakeDirectDamage;

    /// <summary>
    /// Riferimento sovrascritto del context
    /// </summary>
    protected new Boss2SMController.Context context;

    /// <summary>
    /// Setup dello stato
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_onStateStartCallback"></param>
    /// <param name="_onStateEndCallback"></param>
    public override void Setup(IContext _context, Action<IState> _onStateStartCallback, Action<IState> _onStateEndCallback)
    {
        base.Setup(_context, _onStateStartCallback, _onStateEndCallback);
        context = _context as Boss2SMController.Context;
    }
}
