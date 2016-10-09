using UnityEngine;
using System.Collections;

public class AngleOnGaze : MonoBehaviour {
	public Camera gazecamera;

	private float initAngle;

	// Use this for initialization
	void Start () {
		initAngle = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 angle = transform.eulerAngles;
		angle.z = gazecamera.transform.localRotation.eulerAngles.y + initAngle;
		transform.eulerAngles = angle;
	}
}
