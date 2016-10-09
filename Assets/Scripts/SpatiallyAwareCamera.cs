using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatiallyAwareCamera : MonoBehaviour {

	// Use this for initialization
    public SpatialMappingObserver spatialObserver;
    

	void Start () {
        spatialObserver.SetObserverOrigin(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position,spatialObserver.GetObserverOrigin()) >= 3f)
        {
            spatialObserver.SetObserverOrigin(transform.position);
            spatialObserver.StartObserving();
        }
	}
}
