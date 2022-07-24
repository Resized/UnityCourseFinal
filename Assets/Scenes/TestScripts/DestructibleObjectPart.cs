using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class DestructibleObjectPart : MonoBehaviour
{
    [SerializeField] public float breakingPointInPercents;
    MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }
    public void ToggleBreakPoint()
    {
        meshRenderer.enabled = !meshRenderer.enabled;
    }
}
