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

    // The value is used to get angled unit vector by adding two unit vectors and multiplying with this.
    private float _sqr2 = 1f / Mathf.Sqrt(2);

    private void Awake()
    {
        transform.position = m_tTarget.transform.position;
        transform.rotation = Quaternion.identity;
        transform.position -= transform.forward * m_fDistance;

    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Default position is behind the target. I am doing the camera rotation differently, so I need to use target forward.
        Vector3 newPos = m_tTarget.transform.position + (-m_tTarget.transform.forward * m_fDistance);
        // Only hit1 is actually used.
        RaycastHit hit1, hit2;


        // Check if there is somethng behind the target blocking the camera view.
        if (Physics.SphereCast(m_tTarget.transform.position, m_tTarget.height / 2, -m_tTarget.transform.forward, out hit1, m_fDistance))
        {
            // If there was, check if the view from an angle (45 degrees?) is free. This covers going down a slope.
            if (Physics.SphereCast(m_tTarget.transform.position, m_tTarget.height / 2, -m_tTarget.transform.forward + m_tTarget.transform.up, out hit2, m_fDistance))
            {
                // If you are blocked either way, set camera behind but at a distance before the hit.
                newPos = hit1.point;

            }
            else
            {
                // If the angled view was free use that.
                newPos = m_tTarget.transform.position + ((-m_tTarget.transform.forward + m_tTarget.transform.up) * _sqr2 * m_fDistance);
            }

        }

        // The same slerp I copied from Esa. Presumably from class?
        transform.position = Vector3.Slerp(transform.position, newPos, m_fSpeed * Time.deltaTime);

        // Position already does slerp and speed calculation, so I did not get the point of using slerp and speed for the rotation.
        transform.LookAt(m_tTarget.transform.position);

    }
}
