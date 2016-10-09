using UnityEngine;
using System.Collections;

public class MaterialCycler : MonoBehaviour {

    public string floatCrystalRange = "crystal_offset";
    public string floatFurRange = "fur_offsett";
    public float cycleTime = 100;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float idleSpeed = 2f;
        float idleCosRate = Mathf.Cos(Time.time * idleSpeed);

        //Material mat = gameObject.GetComponent<Renderer>().material;
       // mat["crystal_offset"]["X"] = Random.Range(0, 1);

        ProceduralMaterial substance = gameObject.GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
        if (substance)
        {
            float lerp = Mathf.Sin(Time.time / cycleTime);
            float lerp2 = Mathf.Cos(Time.time / cycleTime);
            //substance.SetProceduralFloat(floatRangeProperty, lerp);
            substance.SetProceduralVector(floatCrystalRange, new Vector2(lerp, lerp2));
            substance.RebuildTextures();

            substance.SetProceduralVector(floatFurRange, new Vector2(lerp2, lerp));
            substance.RebuildTextures();
        }
    }
}
