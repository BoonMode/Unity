using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxCollisionDectector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Selection.Instance.CheckAndAdd(other.gameObject);
    }
}
