using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe ch gestisce la macchina a stati di Gioco
/// </summary>
public class GameSMController : StateMachineBase
{
    /// <summary>
    /// Classe che definisce il contesto della GameSM
    /// </summary>
    public class Context : IContext
    {
        /// <summary>
        /// Riferimento al GameManager
        /// </summary>
        GameManager gameMng;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="_bossCtrl"></param>
        /// <param name="_smController"></param>
        /// <param name="_gameMng"></param>
        public Context(GameManager _gameMng)
        {
            gameMng = _gameMng;
        }

        /// <summary>
        /// Funzione che ritorna il riferimento al GameManager
        /// </summary>
        /// <returns></returns>
        public GameManager GetGameManager()
        {
            return gameMng;
        }
    }

    /// <summary>
    /// Contesto corrente
    /// </summary>
    private Context currentContext;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_context"></param>
    public override void Setup(IContext _context)
    {
        base.Setup(_context);
        currentContext = _context as Context;
        GoToNext();
    }

    /// <summary>
    /// Funzione che gestisce l'evento OnStateComplete
    /// </summary>
    /// <param name="_state"></param>
    /// <param name="_exitCondition"></param>
    protected override void HandleOnStateComplete(IState _state, int _exitCondition)
    {
        switch (_state.GetID())
        {
            case "Gameplay":
                if (_exitCondition == 0)
                    GoToState("MainMenu");
                else if (_exitCondition == 1)
                    GoToState("GameChangeScene");
                else if (_exitCondition == 2)
                    GoToState("Pause");
                break;
            case "Pause":
                if (_exitCondition == 0)
                    GoToState("Gameplay");
                else if (_exitCondition == 1)
                    GoToState("LevelUnsetup");
                break;
            default:
                GoToNext();
                break;
        }

    }
}
