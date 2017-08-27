using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObj : Photon.MonoBehaviour {

    private float yVelocity = 0.0F;
    bool _packetGet = false;
    float time = 0;

    public List<GameObject> SyncObj = new List<GameObject>();

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
        transform.position = Vector3.Lerp(transform.position, new_state._pos, Time.deltaTime);
        angleY = Mathf.LerpAngle(transform.rotation.eulerAngles.y, new_state._rot.eulerAngles.y, Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, angleY, 0);
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
            if (!_packetGet)
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
                _packetGet = true;
            }
            else
            {
                old_state = new_state;
                new_state = new State((Vector3)stream.ReceiveNext(), (Quaternion)stream.ReceiveNext(), info.timestamp);
            }
  

        }
    }
}
