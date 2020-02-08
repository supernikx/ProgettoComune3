using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSceneSetupper : GameManager
{
    private QuickSceneInstantiator inst;
    private QuickSceneReset reset;
    private PoolManager poolMng;
    private GroupController groupCtrl;

    protected override void Start()
    {
        if (FindObjectsOfType<GameManager>().Length == 1)
        {
            //Controllo e istanzio se mancano cose
            inst = FindObjectOfType<QuickSceneInstantiator>();
            inst.Setup();

            //Flow normale di setup
            SetUIManager(FindObjectOfType<UI_Manager>());
            SetLevelManager(FindObjectOfType<LevelManager>());
            reset = FindObjectOfType<QuickSceneReset>();
            poolMng = FindObjectOfType<PoolManager>();
            groupCtrl = FindObjectOfType<GroupController>();

            reset.Setup(this, groupCtrl, uiMng);
            poolMng.Setup();
            groupCtrl.Setup();
            lvlMng.Setup();
            uiMng.Setup(this);

            uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Gameplay>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
