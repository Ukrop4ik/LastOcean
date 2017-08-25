using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObj : Photon.MonoBehaviour {


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
        if (!photonView.isMine)
        {
            new_state = old_state = new State(transform.position, transform.rotation, 0f);
        }
    
    }
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, new_state._pos, (float)new_state._stamp - (float) old_state._stamp);
            //  transform.rotation = Quaternion.RotateTowards(transform.rotation, this.correctPlayerRot, 180f * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(pos);
            stream.SendNext(rot);
        }
        else
        {
            old_state = new_state;
            new_state = new State((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext(), info.timestamp);
        }
    }
}
