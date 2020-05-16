using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di super attack del Boss 2
/// </summary>
public class Boss2SuperAttackState : Boss2StateBase
{
	public override void Enter()
	{
		Complete();
	}
}
