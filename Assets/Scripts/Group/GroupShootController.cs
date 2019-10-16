using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo sparo del gruppo
/// </summary>
public class GroupShootController : MonoBehaviour
{
    [Header("Shoot Settings")]
    //Referenza al prefab del proiettile
    [SerializeField]
    private BulletController bulletPrefab;
    [SerializeField]
    private float shootHeight;

    [Header("Reloading Settings")]
    //Tempo di ricarica
    [SerializeField]
    private float reloadingTime;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// bool che indentifica se la classe è setuppata
    /// </summary>
    private bool isSetupped = false;
    /// <summary>
    /// bool che identifica se è stato premuto il tasto di sparo
    /// </summary>
    private bool shoot = false;
    /// <summary>
    /// bool che identifica se è stato premuto il tasto di reload
    /// </summary>
    private bool reload = false;
    /// <summary>
    /// Riferimento al vettore di sparo
    /// </summary>
    private Vector3 shootVector;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        isSetupped = true;
    }

    private void Update()
    {
        if (!isSetupped)
            return;

        ReadInput();
    }

    private void FixedUpdate()
    {
        if (!isSetupped)
            return;

        if (shoot)
        {
            shoot = false;
            ShootAgent();
        }

        if (reload)
        {
            reload = false;
            ReloadAgent();
        }
    }

    /// <summary>
    /// Funzione che si occupa di leggere gl'input
    /// </summary>
    private void ReadInput()
    {
        if (Input.GetJoystickNames().Length > 0 && !string.IsNullOrEmpty(Input.GetJoystickNames()[0]))
        {
            shootVector.x = Input.GetAxis("RHorizontal");
            shootVector.z = Input.GetAxis("RVertical");
        }
        else
        {
            Vector3 screenCenterPosition = Camera.main.WorldToScreenPoint(groupCtrl.GetGroupCenterPoint());
            screenCenterPosition.z = 0;
            Vector3 mouseDirection = (Input.mousePosition - screenCenterPosition).normalized;
            shootVector = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }

        if (Input.GetButtonDown("Shoot"))
            shoot = true;

        if (Input.GetButtonDown("Reload"))
            reload = true;
    }

    /// <summary>
    /// Funzione che si occupa di sparare 
    /// </summary>
    private void ShootAgent()
    {
        if (groupCtrl.RemoveRandomAgent())
        {
            Vector3 shootPoint = groupCtrl.GetGroupCenterPoint();
            shootPoint.y = shootHeight;
            BulletController instantiatedBullet = Instantiate(bulletPrefab, shootPoint, Quaternion.LookRotation(shootVector.normalized));
            instantiatedBullet.Setup();
        }
    }

    /// <summary>
    /// Funzione che si occupa di ricaricare
    /// </summary>
    private void ReloadAgent()
    {
        if (!groupCtrl.IsGroupFull())
            StartCoroutine(ReloadingCoroutine());
    }

    private IEnumerator ReloadingCoroutine()
    {
        yield return new WaitForSeconds(reloadingTime);
        groupCtrl.InstantiateNewAgent();
    }
}
