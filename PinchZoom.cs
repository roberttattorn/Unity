using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
 public Camera selectedCamera;
 float camerascalar;
 public float speed=1.0f;

void Update () 
{

    if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
    {

        curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
        prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
        touchDelta = curDist.magnitude - prevDist.magnitude;
        speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
        speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

        var cameravector = selectedcamera.transform.forward;  

        if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
        {

            //selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView + (1 * speed),15,90);
             camerascalar = Mathf.Clamp(camerascalar + (1 * speed),15,90);
             selectedcamera.transform.position = transform.parent.position+cameravector*camerascalar;
        }

        if ((touchDelta +varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
        {

            //selectedCamera.fieldOfView = Mathf.Clamp(selectedCamera.fieldOfView - (1 * speed),15,90);
             camerascalar = Mathf.Clamp(camerascalar - (1 * speed),15,90);
             selectedcamera.transform.position = transform.parent.position+cameravector*camerascalar;
        }

    }       
}

}