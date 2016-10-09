using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSelect()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}
