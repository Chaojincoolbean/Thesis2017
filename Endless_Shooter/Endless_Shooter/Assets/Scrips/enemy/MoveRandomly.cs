using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MoveRandomly : MonoBehaviour {

	NavMeshAgent navMeshAgent;
	NavMeshPath path;
	public float timeForNewPath;
    public float range = 50f;
	bool inCoRoutine;
	Vector3 target;
	bool validPath;

	void Start ()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		path = new NavMeshPath();
	}

	void Update ()
	{
		if (!inCoRoutine)
			StartCoroutine(DoSomething());
	}

	Vector3 getNewRandomPosition ()
	{
		float x = Random.Range(-range, range);
		float z = Random.Range(-range, range);
		float y = Random.Range(-range, range);

		Vector3 pos = new Vector3(x, y, z);
		return pos;
	}

	IEnumerator DoSomething ()
	{
		inCoRoutine = true;
		yield return new WaitForSeconds(timeForNewPath);
		GetNewPath();
		validPath = navMeshAgent.CalculatePath(target, path);
		if (!validPath) Debug.Log("Found an invalid Path");

		while (!validPath)
		{
			yield return new WaitForSeconds(0.01f);
			GetNewPath();
			validPath = navMeshAgent.CalculatePath(target, path);
		}
		inCoRoutine = false;
	}

	void GetNewPath ()
	{
		target = getNewRandomPosition();
		navMeshAgent.SetDestination(target);
	}

}