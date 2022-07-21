using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FullscreenElement : FadeElement
{
	[SerializeField] private EndOfDayFixer m_EODF;

    // Start is called before the first frame update
    protected override void Start()
    {
		base.Start();

		DayManager.Instance.DayEndDisplayperformanceEvent += FadeIn;
		DayManager.Instance.NextDayEvent += FadeOut;

		m_FadedToFullEvent = new UnityEngine.Events.UnityEvent();
		m_FadedToFullEvent.AddListener( m_EODF.Activate );
	}

	// A function that instantly turns on/off the fullscreened UI.
	public void InstantChange( bool _Active )
	{
		enabled					= false;
		m_ImageToFade.enabled	= _Active;
		m_ImageToFade.color		= m_ColorFade;

	}
}
