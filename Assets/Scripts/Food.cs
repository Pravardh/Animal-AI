using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.TryGetComponent<ChickenManager>(out ChickenManager man))
        {
            other.gameObject.GetComponent<StatsManager>().IncreaseHunger(20);
            Destroy(gameObject);
        }
    }
}
