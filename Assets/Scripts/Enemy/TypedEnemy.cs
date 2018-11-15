using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypedEnemy : MonoBehaviour {

	public EnemyTypes type;
}

public enum EnemyTypes {
	FLEE,
	NEUTRAL,
	AGGRO,
	BOSS
}
