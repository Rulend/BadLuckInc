using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] private float			m_ReturnDelay			= 0.5f;	// How long to wait before starting to go back up after reaching the bottom
	[SerializeField] private float			m_ScanDuration			= 0.5f;
	private float							m_ShrinkGrowProgress	= 0.0f;
	private float							m_ScanProgress			= 0.0f;
	private Vector3							m_MinScale;
	private Vector3							m_MaxScale;
	private Vector3							m_MinAngle;
	private Vector3							m_MaxAngle;
	private EScannerState					m_CurrentState;

	[SerializeField] private GameObject		m_ScanResults;		// The parent game object which holds the "Stress" and "Threat" texts, as well as their related bars.
	[SerializeField] private GameObject		m_MonitorScanGif;	// The object which plays the Scanning...-gif.
	[SerializeField] private Image			m_StressLevel;	
	[SerializeField] private Image			m_ThreatLevel;	

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
		m_MonitorScanGif.SetActive( true );

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
						m_ScanProgress = -m_ReturnDelay;
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

					UpdateScannerDisplay( MaskedManager.Instance.Masked );
				}

				break;
		}
	}


	public void UpdateScannerDisplay( MaskedSubject _ScannedSubject )
	{
		// TODO:: Add a small delay between finishing the scan and displaying the results.
		m_MonitorScanGif.SetActive( false );
		m_ScanResults.SetActive( true );

		switch ( _ScannedSubject.StressLevel )
		{
			case 0:
				m_StressLevel.fillAmount = 0.05f;
				m_StressLevel.color = Color.green;
				break;
			case 1:
				m_StressLevel.fillAmount = 0.125f;
				m_StressLevel.color = Color.green;
				break;
			case 2:
				m_StressLevel.fillAmount = 0.25f;
				m_StressLevel.color = Color.green;
				break;
			case 3:
				m_StressLevel.fillAmount = 0.375f;
				m_StressLevel.color = Color.yellow;
				break;
			case 4:
				m_StressLevel.fillAmount = 0.5f;
				m_StressLevel.color = Color.yellow;
				break;
			case 5:
				m_StressLevel.fillAmount = 0.625f;
				m_StressLevel.color = Color.yellow;
				break;
			case 6:
				m_StressLevel.fillAmount = 0.75f;
				m_StressLevel.color = Color.red;
				break;
			case 7:
				m_StressLevel.fillAmount = 0.875f;
				m_StressLevel.color = Color.red;
				break;
			case 8:
				m_StressLevel.fillAmount = 1.0f;
				m_StressLevel.color = Color.red;
				break;
		}


		switch ( _ScannedSubject.ThreatLevel )
		{
			case 0:
				m_ThreatLevel.fillAmount = 0.05f;
				m_ThreatLevel.color = Color.green;
				break;
			case 1:
				m_ThreatLevel.fillAmount = 0.5f;
				m_ThreatLevel.color = Color.yellow;
				break;
			case 2:
				m_ThreatLevel.fillAmount = 1.0f;
				m_ThreatLevel.color = Color.red;
				break;
		}
	}


	public void FadeResults()
	{

	}
}
