using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorReceiverCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HandTracking.LHTrigger += AdjustPosition;
        HandTracking.RHTrigger += AdjustPosition;
    }

    private void OnDestroy()
    {
        HandTracking.LHTrigger -= AdjustPosition;
        HandTracking.RHTrigger -= AdjustPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AdjustPosition(Vector3 adjustmentVector)
    {
        gameObject.transform.position = gameObject.transform.position + adjustmentVector / 15f;
    }
}
