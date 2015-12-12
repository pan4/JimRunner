using UnityEngine;
using System.Collections;

public class SweeperFollow : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    float _offsetX;

    Transform _transform;

	// Use this for initialization
	void Start () {
        _transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
        _transform.position = new Vector3(target.position.x + _offsetX, _transform.position.y, _transform.position.z);
	}
}
