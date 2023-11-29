using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Points
{
    public class Point : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.black;
        [SerializeField] private float _radius = 1f;
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }   
}