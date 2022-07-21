using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// Put this component on the element that has the image.
public class FadeElement : MonoBehaviour
{
	[SerializeField]	private bool			m_StartOnPlay		= false;
	[SerializeField]	private float			m_FadeDuration		= 1.0f;
						private float			m_CurrentFadeTime	= 0.0f;
	[SerializeField]	private float			m_Delay				= 0.0f;
						protected Image			m_ImageToFade;
						protected Color			m_ColorFade;
	[SerializeField]	private bool			m_FadeOut			= true;
						protected UnityEvent	m_FadedToFullEvent;	// These two are not Serialized, since that would mean you could use them in the inspector. That makes it hard to keep track of where functions are called from.
						protected UnityEvent	m_FadedToZeroEvent;	// These two are not Serialized, since that would mean you could use them in the inspector. That makes it hard to keep track of where functions are called from.

	protected virtual void Start()
	{
		if ( m_FadeOut )
			m_CurrentFadeTime	= m_FadeDuration;

		m_ImageToFade		= GetComponent<Image>();
		m_ColorFade			= m_ImageToFade.color;

		enabled = false;

		if ( m_StartOnPlay )
			StartCoroutine( StartFade() );
	}

	// Update is called once per frame
	void Update()
    {
		m_ColorFade.a		= ( m_CurrentFadeTime / m_FadeDuration );
		m_ImageToFade.color = m_ColorFade;

		if ( m_FadeOut )
		{
			m_CurrentFadeTime -= Time.deltaTime;

			if ( m_CurrentFadeTime < 0.0f )
			{
				enabled					= false;
				m_ImageToFade.enabled	= false;

				if ( m_FadedToZeroEvent != null )
					m_FadedToZeroEvent.Invoke();
			}
		}
		else
		{
			m_CurrentFadeTime += Time.deltaTime;

			if ( m_CurrentFadeTime > m_FadeDuration )
			{
				enabled = false;

				if ( m_FadedToFullEvent != null )
					m_FadedToFullEvent.Invoke();
			}
		}
    }


	public void FadeIn()
	{
		m_FadeOut = false;

		StartCoroutine( StartFade() );
	}

	// Called from UI button, 0 references
	public void FadeOut()
	{
		m_FadeOut = true;

		StartCoroutine( StartFade() );
	}

	private IEnumerator StartFade()
	{
		yield return new WaitForSeconds( m_Delay );

		enabled					= true;
		m_ImageToFade.enabled	= true;
	}
}
