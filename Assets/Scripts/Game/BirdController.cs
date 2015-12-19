using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    [SerializeField]
    private Vector3 translationSpeed = Vector3.forward;
    [SerializeField]
    private bool local = true;

    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        if (local)
            _transform.Translate(translationSpeed * Time.deltaTime);
        else
            _transform.Translate(translationSpeed * Time.deltaTime, Space.World);
    }
}
