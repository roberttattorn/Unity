using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
	public float rate;

void Update () 
{

		if (Input.touchCount == 2)
		{
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			Vector2 curDist = touch1.position - touch2.position;
			Vector2 prevDist = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);
			float delta = curDist.magnitude - prevDist.magnitude;
			if (delta > 10)
			{
				StartCoroutine(zoomOut());
			}
			if (delta < 10)
			{
				StartCoroutine(zoomIn());
			}
		}      
}

	void zoomOut(){
		//camera.fieldofview+rate;   or camera.transform.Move(-camera.transform.forward*rate);
	}

	void zoomIn(){
		//camera.fieldofview-rate;   or camera.transform.Move(camera.transform.forward*rate);
	}

}