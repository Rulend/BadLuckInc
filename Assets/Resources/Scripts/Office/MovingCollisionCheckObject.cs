using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollisionCheckObject : MonoBehaviour
{
						private Vector3		m_TravelDirection;
	[SerializeField]	private float		m_TravelSpeed = 4.0f;
						public Transform	m_TargetTransform;


	// Update is called once per frame
	void Update()
    {
		transform.position = Vector2.MoveTowards( transform.position, transform.position + m_TravelDirection, Time.deltaTime * m_TravelSpeed );
	}

	protected virtual void OnTriggerEnter2D( Collider2D _Collider )
	{
		if ( _Collider.transform == m_TargetTransform )
			enabled = false;
	}

	public void SetTargetTransform( Transform _NewTargetTransform )
	{
		enabled = true;

		m_TargetTransform = _NewTargetTransform;

		m_TravelDirection = ( _NewTargetTransform.position - transform.position );
	}
}
