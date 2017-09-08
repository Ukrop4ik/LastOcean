using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPoints : MonoBehaviour {

    [SerializeField]
    private List<Point> _points = new List<Point>();
    [SerializeField]
    private int startindx;

    private Vector3 _newPos;
    private Quaternion _newRot;

    private void Start()
    {
        MoveToPoint(0);
    }

    private void Update()
    {
        if (_newPos != transform.position)
            transform.position = Vector3.Lerp(transform.position, _newPos, Time.deltaTime);
        if (_newRot != transform.rotation)
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, _newRot, Time.deltaTime);
    }

    [ContextMenu("AddPoint")]
    public void AddPoint()
    {
        _points.Add(new Point(transform.position, transform.rotation));
    }
    [ContextMenu("MoveToPoint")]
    public void MoveToPoint(int indx)
    {
        _newPos = _points[indx].Position;
        _newRot = _points[indx].Rotation;
    }

    [System.Serializable]
    private class Point
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Point(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }
}
