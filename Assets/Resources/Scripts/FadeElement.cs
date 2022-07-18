using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// Put this component on the element that has the image.
public class FadeElement : MonoBehaviour
{
	[SerializeField]	private float		m_FadeDuration		= 1.0f;
						private float		m_CurrentFadeTime	= 1.0f;
						private Image		m_ImageToFade;
						private Color		m_ColorFade;
						private bool		m_FadingOut			= true;
	[SerializeField]	private UnityEvent	m_StartDayEndScreen;

	private void Start()
	{
		m_CurrentFadeTime	= m_FadeDuration;
		m_ImageToFade		= GetComponent<Image>();
		m_ColorFade			= m_ImageToFade.color;

		DayManager.Instance.DayEndDisplayperformanceEvent += FadeIn;
		DayManager.Instance.NextDayEvent += FadeOut;
	}

	// Update is called once per frame
	void Update()
    {
		m_ColorFade.a = ( m_CurrentFadeTime / m_FadeDuration );

		if ( m_FadingOut )
		{
			m_CurrentFadeTime -= Time.deltaTime;
			m_ImageToFade.color = m_ColorFade;

			if ( m_CurrentFadeTime < 0.0f )
				gameObject.SetActive( false );
		}
		else
		{
			m_CurrentFadeTime += Time.deltaTime;
			m_ImageToFade.color = m_ColorFade;

			if ( m_CurrentFadeTime > m_FadeDuration )
			{
				enabled = false;
				m_StartDayEndScreen.Invoke();
			}
		}
    }


	public void FadeIn()
	{
		gameObject.SetActive( true );

		m_FadingOut = false;
	}

	// Called from UI button, 0 references
	public void FadeOut()
	{
		enabled = true;

		m_FadingOut = true;
	}
}
