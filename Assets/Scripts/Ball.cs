using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	Rigidbody rb_;

	void OnScore(ScoreEvent e)
	{
		Destroy(this.gameObject);
	}

	void OnEnable()
	{
		EventManager.Instance.Connect<ScoreEvent>(OnScore, null);
	}

	void OnDisable()
	{
		EventManager.Instance.Disconnect<ScoreEvent>(OnScore, null);
	}

	// Use this for initialization
	void Start () 
	{
		rb_ = this.GetComponent<Rigidbody>();
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Player" && !collider.GetComponent<CController>().OnGround())
		{
			rb_.isKinematic = true;
			this.transform.SetParent(collider.transform);
			this.transform.localPosition = Vector3.zero;
		}
		else
		{
			rb_.isKinematic = false;
			this.transform.SetParent(null);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
