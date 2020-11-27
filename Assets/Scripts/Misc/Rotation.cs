using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform _transformToMove;
    public float _angularSpeed = 90;

    public void Update()
    {
        _transformToMove.Rotate(new Vector3(0, 0, _angularSpeed * Time.deltaTime), Space.Self);
    }
}
