using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : MovingCollisionCheckObject
{
	[SerializeField] private Transform m_ShutterClosedPoint;
	[SerializeField] private Transform m_ShutterOpenedPoint;

	private bool m_IsOpened = false;

	public void Activate()
	{
		if ( !m_IsOpened )
			SetTargetTransform( m_ShutterOpenedPoint );
		else
			SetTargetTransform( m_ShutterClosedPoint );

		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.HandScan );
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ShutterOpen );
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ShutterRoll );
	}



	// It is configured via layers so that this object can only collide with collisionpoints: "open" and "closed".
	protected override void OnTriggerEnter2D( Collider2D _Collider )
	{
		// This is a horrible function that checks the virtual and overriden versions. However, no time to spend on fixing that huhuh.

		if ( _Collider.gameObject.transform != m_TargetTransform )
			return;

		base.OnTriggerEnter2D( _Collider );
		AudioManager.Instance.StopSoundEffect( AudioManager.ESoundEnvironment.ShutterRoll );
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ShutterClose );

		if ( !m_IsOpened )
		{
			m_IsOpened = true;
			DayManager.Instance.StartDay();
		}
		else
		{
			m_IsOpened = false;
			DayManager.Instance.StartDisplayDayPerformance();
		}
	}

}
