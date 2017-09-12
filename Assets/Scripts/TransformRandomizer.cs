using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRandomizer : MonoBehaviour {

    List<Transform> childs = new List<Transform>();
    [SerializeField]
    private float rotationMax;
    [SerializeField]
    private float scaleMax;
    [SerializeField]
    private float scaleMin;

    [ContextMenu("Random")]
    public void Randomize()
    {
        childs.Clear();
        for (int i = 0; i < transform.childCount; i++)
            childs.Add(transform.GetChild(i));

        foreach(Transform child in childs)
        {
            child.Rotate(Vector3.up, Random.Range(0, rotationMax));
            child.localScale = child.localScale * (Random.Range(scaleMin, scaleMax));
        }
    }
}
