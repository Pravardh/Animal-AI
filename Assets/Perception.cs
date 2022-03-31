using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public bool hasFoundFood;
    public float radius = 1.0f;
    public Vector3 destination;
    public LayerMask foodLayer;
    public bool SearchForFood()
    {
        if (!hasFoundFood)
        {
            radius+= .1f;
            Collider[] col = Physics.OverlapSphere(transform.position, radius, foodLayer);
            foreach (Collider c in col)
            {
                hasFoundFood = true;
                destination = c.transform.position;
                radius = 1.0f;
                Debug.Log("Found food");
                return true;
            }
        }
        return false;
    }

    public Vector3 getDestination()
    {
        return destination;
    }

}
