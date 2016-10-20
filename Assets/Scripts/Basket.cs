using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScoreEvent : CustomEvent
{
}

public class Basket : MonoBehaviour {

	public List<GameObject> ToCreate  = new List<GameObject>();
	public List<GameObject> ToDestroy = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Ball")
		{
			EventManager.Instance.SendEvent<ScoreEvent>(new ScoreEvent(), null);

			foreach(var obj in ToCreate)
			{
				Instantiate(obj);
			}
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
