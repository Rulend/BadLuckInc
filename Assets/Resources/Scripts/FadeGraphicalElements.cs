using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fades all the Images/text components in the children of the specified gameobject. If it should be used on itself, you can leave it empty.
public class FadeGraphicalElements : MonoBehaviour
{
	[SerializeField]	private GameObject	m_ParentToGraphicObjects;
	[SerializeField]	private bool		m_StartOnPlay;		// Whether or not to start fading upon loading the scene ( pressing the play button or loading it via scenemanager. )
	[SerializeField]	private float		m_FadeDuration;		// How long the fade is.
						private float		m_FadeProgress;		// How much the fade has progressed.
	[SerializeField]	private float		m_Delay;			// How long to wait before starting the fade. If blinking is set to true, will take this delay into consideration before fading in/out.
	[SerializeField]	private bool		m_Blinking;			// If the elements previously faded out, they will start fading in if this is set to true. The same applies if they previously faded in.
	[SerializeField]	private bool		m_FadeOut = true;	// Whether or not to start with fading out.
	[SerializeField]	private bool		m_ResetUponEnabling = false;	// Whether or not to start with fading out.


	private MaskableGraphic[]				m_MaskableGraphicComponents;
	private Color[]							m_Colors;


	private void Awake()
	{
		m_MaskableGraphicComponents = m_ParentToGraphicObjects.GetComponentsInChildren<MaskableGraphic>();
		m_Colors					= new Color[ m_MaskableGraphicComponents.Length ];

		if ( m_FadeOut )
			m_FadeProgress = m_FadeDuration;

		if ( !m_StartOnPlay )
			enabled = false;
	}

	private void OnEnable()
	{
		if ( !m_ResetUponEnabling )
			return;

		for ( int ColorIndex = 0; ColorIndex < m_MaskableGraphicComponents.Length; ++ColorIndex ) // Enable all the graphics and get the correct colors
		{
			m_MaskableGraphicComponents[ ColorIndex ].enabled = true;
			m_Colors[ ColorIndex ] = m_MaskableGraphicComponents[ ColorIndex ].color;
		}

		if ( m_FadeOut )
			m_FadeProgress = m_FadeDuration;
	}


	void Update()
    {
		for ( int ColorIndex = 0; ColorIndex < m_Colors.Length; ++ColorIndex )
		{
			m_Colors[ ColorIndex ].a = ( m_FadeProgress / m_FadeDuration );
			m_MaskableGraphicComponents[ ColorIndex ].color = m_Colors[ ColorIndex ];
		}

		if ( m_FadeOut )
		{
			m_FadeProgress -= Time.deltaTime;

			if ( m_FadeProgress < 0.0f )
			{
				enabled = false;

				if ( m_Blinking )
					FadeIn();
				else
				{
					for ( int ComponentIndex = 0; ComponentIndex < m_Colors.Length; ++ComponentIndex ) // Reset colors and turn the components off
					{
						m_Colors[ ComponentIndex ].a = 1;
						m_MaskableGraphicComponents[ ComponentIndex ].color = m_Colors[ ComponentIndex ];
					}

					gameObject.SetActive( false );
				}
			}
		}
		else
		{
			m_FadeProgress += Time.deltaTime;

			if ( m_FadeProgress > m_FadeDuration )
			{
				enabled = false;

				if ( m_Blinking )
					FadeOut();
			}
		}
    }


	public void FadeIn()
	{
		m_FadeOut = false;

		StartCoroutine( StartFade() );
	}


	public void FadeOut()
	{
		m_FadeOut = true;

		StartCoroutine( StartFade() );
	}


	public void InstantOut()
	{
		gameObject.SetActive( false );
	}



	private IEnumerator StartFade()
	{
		yield return new WaitForSeconds( m_Delay );

		enabled	= true;

		for ( int ColorIndex = 0; ColorIndex < m_MaskableGraphicComponents.Length; ++ColorIndex ) // Enable all the graphics and get the correct colors
		{
			m_MaskableGraphicComponents[ ColorIndex ].enabled = true;
			m_Colors[ ColorIndex ] = m_MaskableGraphicComponents[ ColorIndex ].color;
		}
	}
}
