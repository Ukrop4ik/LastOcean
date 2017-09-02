using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObj : Photon.MonoBehaviour {

    private float yVelocity = 0.0F;
    bool _packetGet = false;
    float time = 0;

    private enum SyncType
    {
        All,
        Position,
        Rotation,
        ForceRotation
    }

    [SerializeField]
    private SyncType Type;

    private struct State
    {
        public Vector3 _pos;
        public Quaternion _rot;
        public double _stamp;

        public State(Vector3 pos, Quaternion rot, double stamp)
        {
            _pos = pos;
            _rot = rot;
            _stamp = stamp;
        }
    }

    private State new_state;
    private State old_state;

    private void Start()
    {

            new_state = old_state = new State(transform.position, transform.rotation, 0f);
        
   
    }
    void Update()
    {
        
        if (!_packetGet) return;

        float angleY;

        switch (Type)
        {
            case SyncType.All:
                transform.position = Vector3.Lerp(transform.position, new_state._pos, Time.deltaTime);
                angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, new_state._rot.eulerAngles.y, Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, angleY, 0);
                break;
            case SyncType.Position:
                transform.position = Vector3.Lerp(transform.position, new_state._pos, Time.deltaTime);
                break;
            case SyncType.Rotation:
                angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, new_state._rot.eulerAngles.y, Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, angleY, 0);
                break;
            case SyncType.ForceRotation:
                angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, new_state._rot.eulerAngles.y, Time.deltaTime*0.5f);
                transform.rotation = Quaternion.Euler(0, angleY, 0);
                break;
        }



    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            switch(Type)
            {
                case SyncType.All:
                    stream.SendNext(pos);
                    stream.SendNext(rot);
                    break;
                case SyncType.Position:
                    stream.SendNext(pos);
                    break;
                case SyncType.Rotation:
                    stream.SendNext(rot);
                    break;
            }

        }
        else
        {
            if (!_packetGet)
            {
                switch (Type)
                {
                    case SyncType.All:
                        transform.position = (Vector3)stream.ReceiveNext();
                        transform.rotation = (Quaternion)stream.ReceiveNext();
                        break;
                    case SyncType.Position:
                        transform.position = (Vector3)stream.ReceiveNext();
                        break;
                    case SyncType.Rotation:
                        transform.rotation = (Quaternion)stream.ReceiveNext();
                        break;
                }
                _packetGet = true;
            }
            else
            {
                old_state = new_state;

                switch (Type)
                {
                    case SyncType.All:
                        new_state = new State((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext(), info.timestamp);
                        break;
                    case SyncType.Position:
                        new_state = new State((Vector3)stream.ReceiveNext(), Quaternion.identity, info.timestamp);
                        break;
                    case SyncType.Rotation:
                        new_state = new State(transform.position, (Quaternion)stream.ReceiveNext(), info.timestamp);
                        break;
                }               
            }
  

        }
    }
}
