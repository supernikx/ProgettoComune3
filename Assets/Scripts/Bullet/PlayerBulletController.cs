using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il bullet del Player
/// </summary>
public class PlayerBulletController : BulletControllerBase
{
	[Header("Graphic Settings")]


	[Header("Player Bullet Settings")]
	//Danno del proiettile
	[SerializeField]
	protected int bulletDamage;

	private GroupController groupCtrl;
	private GroupOrbController groupOrbCtrl;

	public override void PoolInit()
	{
		if (trailVFX != null)
			trailVFX.SetActive(false);

		base.PoolInit();
	}

	public override void ResetPool()
	{
		if (trailVFX != null)
			trailVFX.SetActive(false);

		base.ResetPool();
	}

	public override void Setup()
	{
		base.Setup();
		groupCtrl = GameManager.instance.GetLevelManager().GetGroupController();
		groupOrbCtrl = groupCtrl.GetGroupOrbController();

		if (trailVFX != null)
			trailVFX.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isSetupped)
			return;

		IBossDamageable bossDamageable = other.GetComponentInParent<IBossDamageable>();
		if (bossDamageable != null)
		{
			bossDamageable.TakeDamage(bulletDamage);
			BulletDestroy();
		}
		else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("ShootInteractable"))
			BulletDestroy();
	}

	/// <summary>
	/// Override funzione di destroy
	/// </summary>
	public override void BulletDestroy()
	{
		if (trailVFX != null)
			trailVFX.SetActive(false);

		groupOrbCtrl.InstantiatedOrb(transform.position);
		base.BulletDestroy();
	}
}
