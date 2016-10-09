using UnityEngine;
using System.Collections;

public class ResizeOnGaze : MonoBehaviour {
	public float scaleFactor = 0.8f;
	public float minDistance = 0.3f;
	public float minSolid = 0.0f;
	public float maxSolid = 1.0f;
	public float worldScale = 3.0f;
    public float objScaleMin = 0.25f;
    public float objScaleMax = 5f;
    public float maxDotSize = 40f;
	public GameObject dotTemplate;
	public Camera gazecamera;
	public GameObject twoDimTemplate;

	private Material material;
	private int alpha;
	private GameObject dot;
	private Material dotMaterial;
	private Vector3 youPosition;
	private GameObject twoDimObj;
	private Transform clientRect;
	private float width;
	private float height;
	private float fov;

		void Start() {
		material = GetComponent<Renderer>().material;
		alpha = Shader.PropertyToID("_Color");

		dot = (GameObject)Instantiate(dotTemplate, dotTemplate.GetComponentInParent<Transform>());
		youPosition = dot.transform.localPosition;
		dot.SetActive(true);
		//dotMaterial = dot.GetComponent<Renderer>().material;

		clientRect = twoDimTemplate.GetComponentInParent<Transform>();
		twoDimObj = (GameObject)Instantiate(twoDimTemplate, clientRect);

		width = Screen.currentResolution.width / 4;
		height = Screen.currentResolution.height / 4;
		fov = gazecamera.fieldOfView / 2;
	}

	void Update() {
		Vector3 objray = gazecamera.transform.position - transform.position;
        float objangle = Mathf.Atan2(objray.x, -objray.z) * Mathf.Rad2Deg;
        float gazeangle = gazecamera.transform.localEulerAngles.y;
		//Vector3 ray = gazecamera.transform.InverseTransformDirection(objray);
		//Vector3.ProjectOnPlane(transform, clientRect.
		//Vector3 ray = Vector3.ProjectOnPlane(transform, twoDimObj.transform.pos
		//Vector3 ray = gazecamera.WorldToScreenPoint();
		//Vector3 ray;

		{ // do update dot
			Vector3 newPosition = new Vector3(objray.x, -objray.z, 0) * worldScale;
			float dotSize = newPosition.magnitude;
			if (dotSize > maxDotSize) {
				newPosition.Normalize();
				newPosition *= maxDotSize;
				//dotMaterial.color = new Color(0, 0, 0, Mathf.Clamp01(1f - (dotSize - maxDotSize) * 0.1f));
			}
			else {
				//dotMaterial.color = Color.white;
			}
			newPosition += youPosition;
			dot.transform.localPosition = newPosition;
		}

		{ // do color opacity
			float scale = (objray.magnitude - minDistance) * scaleFactor;
			Color color = material.GetColor(alpha);
			float opacityScale = Mathf.Clamp01(scale);
			color.a = opacityScale * (maxSolid - minSolid) + minSolid;
			material.SetColor(alpha, color);
		}

        //Debug.Log(gazeangle + "+" + objangle);
		{ // do update angle
            float angle = ((gazeangle + objangle + 180) % 360) - 180;
            //Debug.Log(angle);
            //float angle = Mathf.Abs(Mathf.Atan2(ray.y, ray.x) * Mathf.Rad2Deg - 90);
            //Debug.DrawRay( new Vector3( 0, 0, 0 ), new Vector3(ray.x, ray.y, 0), Mathf.Abs(angle) > 30 ? Color.red : Color.green);
            float scale = 0.95f;
            if (Mathf.Abs(angle) < scale * fov) {
				scale = angle / fov;
			}
			else {
				twoDimObj.SetActive(true);
                float edge = Mathf.Clamp(objray.magnitude, -height, height);
				if (angle > 0) {
					twoDimObj.transform.localPosition = new Vector3(width, edge, 0);
				}
				else {
					twoDimObj.transform.localPosition = new Vector3(-width, edge, 0);
				}
			}
            scale = (1f - scale * scale) * (objScaleMax - objScaleMin) + objScaleMin;
            transform.localScale = new Vector3(scale, scale, scale);
            twoDimObj.SetActive(false);
            //Debug.Log(ray);
        }
	}
}
