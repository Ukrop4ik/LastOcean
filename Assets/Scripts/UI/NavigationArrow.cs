using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NavigationArrow : MonoBehaviour {

    public Transform player;
    public Transform target;


	// Use this for initialization
	void Start () {

       
	}

    // Update is called once per frame
    void Update() {

        if (target == null || player == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(target.position, player.position) > 50f)
        {
            this.gameObject.GetComponent<Image>().enabled = false;
        }
        else 
        {
            this.gameObject.GetComponent<Image>().enabled = true;
        }



        float angle = Vector3.Angle(player.transform.forward, target.transform.position - player.transform.position);
        Vector3 croos = Vector3.Cross(player.transform.forward, target.transform.position - player.transform.position);


        if (croos.y > 0)
            angle = -angle;

        var rotator = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotator;
    }
}
