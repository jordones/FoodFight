using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance = null;
	private int killed {get; set;} = 0;
	private int bossKilled {get; set;} = 0;
	public int enemyGoal = 10;
	public int bossGoal = 1;

	public List<OnLevelGoal> goalSubscribers = new List<OnLevelGoal>();

	public void subscribeToGoal(OnLevelGoal obj) {
		goalSubscribers.Add(obj);
		
	}

	// Use this for initialization
	void Start () {
		
	}

	void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
	}

	void OnGUI () {
		GUI.Label(new Rect (Screen.width - 150,0,100,50), "Kills: " + killed + "/" + enemyGoal);
		GUI.Label(new Rect (Screen.width - 150,20,100,50), "Bosses Killed: " + bossKilled + "/" + bossGoal);
	}

	public IEnumerator EndLevel() {
		yield return StartCoroutine(LoadScene.AsyncLoadScene("MainMenu"));
	}

	public void Killed(TypedEnemy obj) {
		switch(obj.type) {
			case EnemyTypes.FLEE:
			case EnemyTypes.AGGRO:
			case EnemyTypes.NEUTRAL:
			    killed++;
				Debug.Log("Enemy Killed");
				if (killed == enemyGoal) {
					// Do none or many things when the goal is completed
					foreach(var goalSub in goalSubscribers) {
						goalSub.OnLevelGoal();
					}
				}
				break;
			case EnemyTypes.BOSS:
				Debug.Log("Killed Tpye: Boss");
			    bossKilled++;
				if (bossKilled == bossGoal) {
				    Debug.Log("Boss Killed");
					StartCoroutine(EndLevel());
				}
				break;
		}
	}
}
public interface OnLevelGoal
{
    void OnLevelGoal();
}
