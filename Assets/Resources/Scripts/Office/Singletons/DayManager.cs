using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
	private static DayManager m_Instance;
	public static DayManager Instance => m_Instance;


	[System.Serializable]
	public struct Day // A day starts when the shutter is raised.
	{
		public List<MaskedSubject.SpecialMasked> m_SpecialMasks;

		public float	m_DayDuration;
		public int		m_AmountInfectedMasks;
		public int		m_AmountMixtureMasks;
		public int		m_AmountTotalMasks;
	}


	[SerializeField]		private List<Day>		m_Days;
	[SerializeField]		private Text			m_TimeDisplay;
	[SerializeField]		private EndingScreen	m_EndingScreen;
	[SerializeField]		private GameObject		m_VictorySlidesParent;
	[SerializeField]		private GameObject		m_DefeatSlidesParent;
							private float			m_DayTimeLeft;
							private int				m_CurrentDay = 0;

	public delegate void	DayStartHandler();
	public event			DayStartHandler DayStartEvent;

	public delegate void	DayEndHandler();
	public event			DayEndHandler DayEndDisplayperformanceEvent;

	public delegate void	NextDayHandler();
	public event			NextDayHandler NextDayEvent;


	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of DayManager detected. Deleting the extra...Hopefully nothing bad happens..hehe" );
			Destroy( gameObject );
		}

		m_DayTimeLeft = m_Days[0].m_DayDuration;

		enabled = false;
	}

	// While running update, the Daymanager will set the time of the clock in the bottom left corner.
	// This should definitively be added to its own class; TODO:: Fix that.
	private void Update()
	{
		m_DayTimeLeft -= Time.deltaTime;

		if ( m_DayTimeLeft > 0 )
		{
			int Seconds = (int)( m_DayTimeLeft % 60 );
			int Minutes = (int)( m_DayTimeLeft - Seconds ) / 60;

			string DisplayedTime = "";

			if ( Minutes < 10 )
				DisplayedTime += "0";

			DisplayedTime += $"{Minutes}:";

			if ( Seconds < 10 )
				DisplayedTime += "0";

			DisplayedTime += $"{Seconds}";

			m_TimeDisplay.text = DisplayedTime;
		}
		// TODO:: Add the logic that controls what happens to the points when you go overtime. 
	}


	// Should only ever have 1 reference: called from the shutter once it reaches its assigned point above the screen.
	public void StartDay()
	{
		DayStartEvent.Invoke();
		enabled = true;
	}


	// Should only ever have 1 reference: called from the shutter once it reaches its assigned point in the middle of the srceen
	public void EndDay()
	{
		enabled = false;
	}



	public Day GetCurrentDay()
	{
		return m_Days[ m_CurrentDay ];
	}


	// Displays the performance of the completed day.
	public void StartDisplayDayPerformance()
	{
		if ( DayEndDisplayperformanceEvent != null )
			DayEndDisplayperformanceEvent.Invoke();
	}


	// Progresses to the next day.

	public void ProgressToNextDay()
	{
		m_CurrentDay++;

		if ( ScoreManager.Instance.PlayerCredits < 0 ) // If the player has negative credits, meaning they made a lot of mistakes or were very unlucky
		{
			m_EndingScreen.Activate( m_DefeatSlidesParent );
			return;
		}

		if ( m_CurrentDay >= m_Days.Count ) // If the player survived for the specified amount of days, let them win. (TODO:: This should give the default ending, which happens when you don't complete enough events for the true ending or if you don't mess up bad enough for the bad ending)
		{
			m_EndingScreen.Activate( m_VictorySlidesParent );
			return;
		}


		m_DayTimeLeft = m_Days[ m_CurrentDay ].m_DayDuration;
		m_TimeDisplay.text = "00:00";

		if ( NextDayEvent != null ) // If there is sometihng subscribed to the next day event, invoke it
			NextDayEvent.Invoke();
	}
}
