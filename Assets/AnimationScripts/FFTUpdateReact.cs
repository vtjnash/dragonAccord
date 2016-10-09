using UnityEngine;
using System.Collections;

public class FFTUpdateReact : MonoBehaviour {
	public string element = "low";
	public float scaling = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FFTUpdate(AudioInfo.Sample s) {
		float height;
		if (element == "low")
			height = s.low;
		else if (element == "mid")
			height = s.mid;
		else if (element == "high")
			height = s.high;
		else {
			Debug.LogAssertion("bad FFT element type");
			return;
		}
		transform.localScale = new Vector3(0.1f, height / scaling, 0.1f);
	}
}
