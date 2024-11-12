using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Unit") return;
        var unitBase = other.gameObject.GetComponent<UnitBase>();
        if(unitBase == null || !unitBase.IsMove) return;
        unitBase.OnShipping(this);
    }
}
