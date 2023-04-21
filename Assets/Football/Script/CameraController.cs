using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_LookTraget;
    [SerializeField] private Vector3 m_Offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = m_LookTraget.position + m_Offset;
    }
}
