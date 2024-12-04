using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TowerPreviewRange : MonoBehaviour
{
    [SerializeField] public float range = 0f;
    [SerializeField] public Color rangeColor = Color.blue;

    private void OnDrawGizmos()
    {
        if (range > 0f)
        {
            Gizmos.color = rangeColor;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
