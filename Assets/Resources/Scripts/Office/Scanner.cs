using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
	private enum EScannerState
	{
		Opening			,
		ScanningDown	,
		ScanningUp		,
		Closing			,
	}


	[SerializeField] private GameObject		m_RayParent;
	[SerializeField] private float			m_MinXScale				= 0.1f;
	[SerializeField] private float			m_MaxXScale				= 4.0f;
	[SerializeField] private float			m_MinXAngle				= -95.0f;
	[SerializeField] private float			m_MaxXAngle				= 0.0f;
	[SerializeField] private float			m_ShrinkGrowDuration	= 0.5f;
	[SerializeField] private float			m_ScanDuration			= 0.5f;
	private float							m_ShrinkGrowProgress	= 0.0f;
	private float							m_ScanProgress			= 0.0f;
	private Vector3							m_MinScale;
	private Vector3							m_MaxScale;
	private Vector3							m_MinAngle;
	private Vector3							m_MaxAngle;
	private EScannerState					m_CurrentState;

	private void Awake()
	{
		Vector3 RayScale = m_RayParent.transform.localScale;

		RayScale.x = m_MinXScale;
		m_MinScale = RayScale;

		RayScale.x = m_MaxXScale;
		m_MaxScale = RayScale;

		Vector3 RayAngles = m_RayParent.transform.rotation.eulerAngles;

		RayAngles.x = m_MaxXAngle;
		m_MaxAngle = RayAngles;

		RayAngles.x = m_MinXAngle;
		m_MinAngle = RayAngles;

		gameObject.SetActive( false );
	}


	private void OnEnable()
	{
		m_RayParent.SetActive( true );

		Vector3 NewScale					= m_RayParent.transform.localScale;
		NewScale.x							= m_MinXScale;
		m_RayParent.transform.localScale	= NewScale;
		m_CurrentState						= EScannerState.Opening;
	}

	// Update is called once per frame
	void Update()
    {
		switch ( m_CurrentState )
		{
			case EScannerState.Opening:
				{
					m_ShrinkGrowProgress += Time.deltaTime;
					m_RayParent.transform.localScale = Vector3.Lerp( m_MinScale, m_MaxScale, ( m_ShrinkGrowProgress / m_ShrinkGrowDuration ) );

					if ( m_ShrinkGrowProgress > m_ShrinkGrowDuration )
					{
						m_CurrentState			= EScannerState.ScanningDown;
						m_ShrinkGrowProgress	= 0.0f;
					}
				}

				break;

			case EScannerState.ScanningDown:
				{
					m_ScanProgress += Time.deltaTime;

					Vector3 NewAngles = Vector3.Lerp( m_MinAngle, m_MaxAngle, 1 - ( m_ScanProgress / m_ScanDuration ) );

					m_RayParent.transform.eulerAngles = NewAngles;


					if ( m_ScanProgress > m_ScanDuration )
					{
						m_CurrentState = EScannerState.ScanningUp;
						m_ScanProgress = 0.0f;
					}
				}
				break;

			case EScannerState.ScanningUp:
				{
					m_ScanProgress += Time.deltaTime;

					Vector3 NewAngles = Vector3.Lerp( m_MinAngle, m_MaxAngle, ( m_ScanProgress / m_ScanDuration ) );

					m_RayParent.transform.eulerAngles = NewAngles;


					if ( m_ScanProgress > m_ScanDuration )
					{
						m_CurrentState = EScannerState.Closing;
						m_ScanProgress = 0.0f;
					}
				}
				break;

			case EScannerState.Closing:
				m_ShrinkGrowProgress += Time.deltaTime;

				m_RayParent.transform.localScale = Vector3.Lerp( m_MinScale, m_MaxScale, 1 - ( m_ShrinkGrowProgress / m_ShrinkGrowDuration ) );

				if ( m_ShrinkGrowProgress > m_ShrinkGrowDuration )
				{
					gameObject.SetActive( false ); // TODO: Make other buttons pressable again, bring up dialogue options, etc.
					ButtonManager.Instance.EnableNormalButtons();
					m_ShrinkGrowProgress = 0.0f;
				}

				break;
		}
	}
}
