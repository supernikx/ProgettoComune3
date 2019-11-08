using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe ch gestisce la macchina a stati del Boss Prototipo
/// </summary>
public class BossPrototipoSMController : StateMachineBase
{
    /// <summary>
    /// Classe che definisce il contesto della BossPrototipoSM
    /// </summary>
    public class Context : IContext
    {
        /// <summary>
        /// Riferimento al BossController
        /// </summary>
        BossPrototipoController bossCtrl;
        /// <summary>
        /// Riferimento al controller della SM
        /// </summary>
        BossPrototipoSMController smController;
        /// <summary>
        /// Riferimento al GameManager
        /// </summary>
        LevelManager lvlMng;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="_bossCtrl"></param>
        /// <param name="_smController"></param>
        /// <param name="_gameMng"></param>
        public Context(BossPrototipoController _bossCtrl, BossPrototipoSMController _smController, LevelManager _lvlMng)
        {
            bossCtrl = _bossCtrl;
            smController = _smController;
            lvlMng = _lvlMng;
        }

        /// <summary>
        /// Funzione che ritorna il riferimento al BossController
        /// </summary>
        /// <returns></returns>
        public BossPrototipoController GetBossController()
        {
            return bossCtrl;
        }

        /// <summary>
        /// Funzione che ritorna il riferimento al GameSMController
        /// </summary>
        /// <returns></returns>
        public BossPrototipoSMController GetSMController()
        {
            return smController;
        }

        /// <summary>
        /// Funzione che ritorna il riferimento al LevelManager
        /// </summary>
        /// <returns></returns>
        public LevelManager GetLevelManager()
        {
            return lvlMng;
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
        if (_exitCondition == 1)
            GoToState("Death");
        else
            GoToNext();
    }
}
