using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public float triggerSpeed;
    public float resetTime;

    bool lhResetting;
    bool rhResetting;

    RaycastHit hit;

    public static event System.Action<Vector3> LHTrigger;
    public static event System.Action<Vector3> RHTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpeed(OVRInput.Controller.LTouch);
        CheckSpeed(OVRInput.Controller.RTouch);
    }

    void CheckSpeed(OVRInput.Controller controller)
    {
        float velocitySpeed;
        Vector3 velocityVector;
        velocityVector = OVRInput.GetLocalControllerVelocity(controller);
        velocitySpeed = velocityVector.magnitude;

        if(velocitySpeed > triggerSpeed)
        {
            SpeedCheckPassed(controller, velocityVector);
        }
    }

    void SpeedCheckPassed(OVRInput.Controller controller, Vector3 velocityVector)
    {
        if(controller == OVRInput.Controller.LTouch && !lhResetting)
        {
            LHTrigger?.Invoke(velocityVector);
            StartCoroutine(LHResetting());
            RevealWalls(controller, velocityVector);
        }
        else if (controller == OVRInput.Controller.RTouch && !rhResetting)
        {
            RHTrigger?.Invoke(velocityVector);
            StartCoroutine(RHResetting());
            RevealWalls(controller, velocityVector);
        }
    }

    IEnumerator LHResetting()
    {
        lhResetting = true;
        yield return new WaitForSeconds(resetTime);
        lhResetting = false;
    }

    IEnumerator RHResetting()
    {
        rhResetting = true;
        yield return new WaitForSeconds(resetTime);
        rhResetting = false;
    }


    void RevealWalls(OVRInput.Controller controller, Vector3 velocityVector)
    {
        Ray checkForPrison = new Ray(OVRInput.GetLocalControllerPosition(controller), velocityVector);
        if (Physics.Raycast(checkForPrison, out hit, 10f) && hit.transform.gameObject.CompareTag("Prison"))
        {
            StartCoroutine(TemporarilyPullBackTheBlinds(hit.transform.gameObject));
        }
    }

    IEnumerator TemporarilyPullBackTheBlinds(GameObject prisonWall)
    {
        MeshRenderer mesh = prisonWall.GetComponent<MeshRenderer>();
        mesh.enabled = true;
        yield return new WaitForSeconds(resetTime);
        mesh.enabled = false;
    }

}
