using UnityEngine;
using System.Collections;

public class PulseRuner : MonoBehaviour {
	public GameObject pulseSphereTemplate;
	public float timeScale = 2.0f;
	public float maxSolid = 0.5f;

	// Use this for initialization
	void Start () {
		float timeBase = Time.time;
		for (int i = 0; i < 5; i++) {
			GameObject sphere = (GameObject)Instantiate(pulseSphereTemplate, transform.position, Quaternion.identity);
			sphere.GetComponent<Pulse>().init(timeScale, maxSolid, timeBase);
			timeBase += timeScale / 10;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
