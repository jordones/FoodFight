using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance = null;
	public GameObject spawnPoint = null;
	private int killed {get; set;} = 0;
	public string next = "MainMenu";
	private int bossKilled {get; set;} = 0;
	public int enemyGoal = 10;
	public int bossGoal = 1;
	public bool finished = false;

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

			GameObject character = GameObject.FindWithTag("Character");

			Debug.Log("Moving " + character + " to " + spawnPoint);
			character.transform.position = spawnPoint.transform.position;
			Character stats = character.GetComponent<Character>() as Character;
			stats.health = stats.maxHealth;
        } else {
            Destroy(gameObject);
        }
	}

	void Update() {
		if (killed >= enemyGoal && bossKilled >= bossGoal && !finished) {
			finished = true;
			Debug.Log("Level finished");
			
			StartCoroutine(EndLevel());
		}
	}

	void OnGUI () {
		GUI.Label(new Rect (50,50,100,50), "Kills: " + killed + "/" + enemyGoal);
		GUI.Label(new Rect (50,70,100,50), "Bosses Killed: " + bossKilled + "/" + bossGoal);
	}

	public IEnumerator EndLevel() {
		yield return StartCoroutine(LoadScene.AsyncLoadScene(next));
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
				Debug.Log("Killed Type: Boss");
			    bossKilled++;
				break;
		}
	}
}
public interface OnLevelGoal
{
    void OnLevelGoal();
}
