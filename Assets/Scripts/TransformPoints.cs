using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPoints : MonoBehaviour {

    [SerializeField]
    private List<Point> _points = new List<Point>();
    [SerializeField]
    private int startindx;
    [SerializeField]
    private int changeindx;
    [SerializeField]
    private int moveindx;

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

    [ContextMenu("ChangePoint")]
    public void ChangePoint()
    {
        _points[changeindx].Position = transform.position;
        _points[changeindx].Rotation = transform.rotation;

    }

    [ContextMenu("AddPoint")]
    public void AddPoint()
    {
        _points.Add(new Point(transform.position, transform.rotation));
    }

    public void MoveToPoint(int indx)
    {
        _newPos = _points[indx].Position;
        _newRot = _points[indx].Rotation;
    }
    [ContextMenu("MoveToPoint")]
    public void MoveToPoint()
    {
        _newPos = _points[moveindx].Position;
        _newRot = _points[moveindx].Rotation;
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
