using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSceneSetupper : GameManager
{
    private QuickSceneInstantiator inst;
    private QuickSceneReset reset;
    private PoolManager poolMng;
    private LevelManager lvlMgn;
    private UI_Manager uiMng;
    private GroupController groupCtrl;

    protected override void Start()
    {
        if (FindObjectsOfType<GameManager>().Length == 1)
        {
            //Controllo e istanzio se mancano cose
            inst = FindObjectOfType<QuickSceneInstantiator>();
            inst.Setup();

            //Flow normale di setup
            reset = FindObjectOfType<QuickSceneReset>();
            uiMng = FindObjectOfType<UI_Manager>();
            poolMng = FindObjectOfType<PoolManager>();
            groupCtrl = FindObjectOfType<GroupController>();
            lvlMgn = FindObjectOfType<LevelManager>();

            SetLevelManager(lvlMgn);
            SetUIManager(uiMng);

            reset.Setup(groupCtrl, uiMng);
            poolMng.Setup();
            groupCtrl.Setup();
            lvlMgn.Setup();
            uiMng.Setup(this);

            uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Gameplay>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
