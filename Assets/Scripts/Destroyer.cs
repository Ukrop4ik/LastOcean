using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    [SerializeField]
    private float _destroytime;

	void Start () {

        StartCoroutine(DestroyerObj());
		
	}
	
    private IEnumerator DestroyerObj()
    {
        yield return new WaitForSeconds(_destroytime);

        Destroy(gameObject);
    }
}
