using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public float m_fDistance = 5f;
    public float m_fSpeed = 5f;
    public float m_fRotationSpeed = 10f;
    public CharacterController m_tTarget;

    private float _height;
    private Vector3 _heightVector;
    private float _sqr2 = 1f / Mathf.Sqrt(2);
    private Camera _camera;

    private void Awake()
    {
        transform.position = m_tTarget.transform.position;
        transform.rotation = Quaternion.identity;
        transform.position -= transform.forward * m_fDistance;

        
        _camera = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {        
        UpdatePosition();
    }

    private void UpdatePosition()
    {        
        Vector3 newPos = m_tTarget.transform.position + (-m_tTarget.transform.forward * m_fDistance);
        RaycastHit hit1, hit2;
        

        
        if (Physics.SphereCast(m_tTarget.transform.position, m_tTarget.height / 2, -m_tTarget.transform.forward, out hit1, m_fDistance))
        {
            if (Physics.SphereCast(m_tTarget.transform.position, m_tTarget.height / 2, -m_tTarget.transform.forward + m_tTarget.transform.up, out hit2, m_fDistance))
            {
                newPos = hit1.point;

            }
            else
            {
                newPos = m_tTarget.transform.position + ((-m_tTarget.transform.forward+m_tTarget.transform.up) * _sqr2 * m_fDistance);
            }
                      
        }

        
        transform.position = Vector3.Slerp(transform.position, newPos, m_fSpeed * Time.deltaTime);
        transform.LookAt(m_tTarget.transform.position); // position already does slerp and speed calculation
         
    }
}
