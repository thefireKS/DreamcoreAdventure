using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxMovingObject : MonoBehaviour
{
    [SerializeField] private float lerpDuration;
    [SerializeField] private float path;

    private float valueToLerp;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
        StartCoroutine(LerpMoving());
    }

    IEnumerator LerpMoving()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(originalPos.x, originalPos.x + path, timeElapsed / lerpDuration);
            transform.position = new Vector3(valueToLerp, transform.position.y, transform.position.z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        valueToLerp = originalPos.x + path;
        transform.position = originalPos;
        StartCoroutine(LerpMoving());
    }
}