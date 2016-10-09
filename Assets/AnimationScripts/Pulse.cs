using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {
	private float timeScale = 1;
	private float maxSolid = 0;
	private float timeBase = 0;
	private Material material;
	private int tintColor;

	public void Start () {
		material = GetComponent<Renderer>().material;
		tintColor = Shader.PropertyToID("_TintColor");
	}
		
	// Use this for initialization
	public void init(float timeScale, float maxSolid, float timeBase) {
		this.timeScale = timeScale;
		this.maxSolid = maxSolid;
		this.timeBase = timeBase;
	}
	
	// Update is called once per frame
	void Update () {
		float scale = ((Time.time - timeBase) / timeScale) % 1f;
		transform.localScale = new Vector3(scale, scale, scale);

		Color color = material.GetColor(tintColor);
		float opacityScale = Mathf.Sqrt(scale);
		color.a = (1 - opacityScale) * maxSolid;
		material.SetColor(tintColor, color);
	}
}
