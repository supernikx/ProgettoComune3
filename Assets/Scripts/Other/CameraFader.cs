using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funzione che esegue il fade dei materiali tra la camera e il gruppo
/// </summary>
public class CameraFader : MonoBehaviour
{
	[Header("Fade Settings")]
	[SerializeField]
	private LayerMask fadeLayer;
	[SerializeField]
	private float fadeValue;

	private GroupController groupCtrl;
	private Transform groupCenterTransform;
	private RaycastHit rayInfo;
	private MeshRenderer oldRenderer;

	private void Start()
	{
		groupCtrl = FindObjectOfType<GroupController>();
		groupCenterTransform = groupCtrl.GetGroupCenterTransform();
	}

	private void Update()
	{
		if (groupCtrl == null)
			return;

		if (Physics.Linecast(transform.position, groupCenterTransform.position, out rayInfo, fadeLayer))
		{
			MeshRenderer hitRenderer = rayInfo.transform.parent.gameObject.GetComponent<MeshRenderer>();
			hitRenderer.sharedMaterial.SetColor("_BaseColor", new Color(hitRenderer.sharedMaterial.color.r, hitRenderer.sharedMaterial.color.g, hitRenderer.sharedMaterial.color.b, (fadeValue > 1) ? fadeValue / 255f : fadeValue));

			if (oldRenderer != null)
			{
				if (oldRenderer != hitRenderer)
				{
					oldRenderer.sharedMaterial.SetColor("_BaseColor", new Color(oldRenderer.sharedMaterial.color.r, oldRenderer.sharedMaterial.color.g, oldRenderer.sharedMaterial.color.b, 1f));
					oldRenderer = hitRenderer;
				}
			}
			else
			{
				oldRenderer = hitRenderer;
			}
		}
		else
		{
			if (oldRenderer != null)
			{
				oldRenderer.sharedMaterial.SetColor("_BaseColor", new Color(oldRenderer.sharedMaterial.color.r, oldRenderer.sharedMaterial.color.g, oldRenderer.sharedMaterial.color.b, 1f));
			}
		}

		Debug.DrawLine(transform.position, groupCenterTransform.position, Color.red);
	}

	private void OnApplicationQuit()
	{
		if (oldRenderer != null)
			oldRenderer.sharedMaterial.SetColor("_BaseColor", new Color(oldRenderer.sharedMaterial.color.r, oldRenderer.sharedMaterial.color.g, oldRenderer.sharedMaterial.color.b, 1f));
	}
}
