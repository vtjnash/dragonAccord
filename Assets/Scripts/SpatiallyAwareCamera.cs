using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatiallyAwareCamera : MonoBehaviour {

	// Use this for initialization
    public SpatialMappingObserver spatialObserver;
    

	void Start () {
        //spatialObserver.SetObserverOrigin(transform.position);

	}
	
	// Update is called once per frame
	void Update () {
        
        /*var normal = -transform.forward;     // Normally the normal is best set to be the opposite of the main camera's forward vector
                                                         // If the content is actually all on a plane (like text), set the normal to the normal of the plane
                                                         // and ensure the user does not pass through the plane
        var position = transform.position - normal*3;
        UnityEngine.VR.WSA.HolographicSettings.SetFocusPointForFrame(position, normal);*/

        /*if (Vector3.Distance(transform.position,spatialObserver.GetObserverOrigin()) >= 3f)
        {
            spatialObserver.SetObserverOrigin(transform.position);
            spatialObserver.StartObserving();
        }*/
	}
}
