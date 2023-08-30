using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Transform myTransform;
    Vector3 randomRotation;

    float minScale = 0.8f;
    float maxScale = 1.2f;
    float rotationOffset = 50f;

    private void Awake()
    {
        myTransform = transform;
    }
    void Start()
    {
        var scale = new Vector3();
        scale.x = Random.Range(minScale, maxScale);
        scale.y = Random.Range(minScale, maxScale);
        scale.z = Random.Range(minScale, maxScale);
        myTransform.localScale = scale;

        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);
       
    }

    void Update()
    {
        myTransform.Rotate(randomRotation * Time.deltaTime);

    }
}
