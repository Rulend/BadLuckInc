using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	private static ScoreManager m_Instance;
	public static ScoreManager Instance => m_Instance;


	private int m_PlayerCredits; // The player's credits.
	private int m_DailyCounter;
	private int m_DailyMistakes;

	// TODO:: Add different kinds of credit gains/losses, depending on masked type
	[SerializeField] private int m_CreditGain;
	[SerializeField] private int m_CreditLoss;
	[SerializeField] private int m_DailyLivingCosts; // How many credits are removed at the end of each day

	public int PlayerCredits	=> m_PlayerCredits; //
	public int DailyCounter		=> m_DailyCounter;
	public int DailyMistakes	=> m_DailyMistakes;
	public int CreditGain => m_CreditGain;
	public int CreditLoss => m_CreditLoss;



	// TODO:: Add a UI manager if more like this should be done
	[SerializeField] private Text m_Counter;


	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of ScoreManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}
	}


	private void Start()
	{
		DayManager.Instance.NextDayEvent += ResetCounters;
	}



	public void AdjustCredits( int _Adjustment )
	{
		m_PlayerCredits += _Adjustment;
	}



	// Increases or decreases the player's value.
	public int CalculateCredits()
	{
		m_PlayerCredits += m_CreditGain * m_DailyCounter;
		m_PlayerCredits -= m_CreditLoss * m_DailyMistakes;
		m_PlayerCredits -= m_DailyLivingCosts;

		return m_PlayerCredits;
	}



	public void UpdateDailyCounter()
	{
		// TODO:: Implement different grades of credits gained
		m_DailyCounter++;

		string UpdatedCounter = "";

		if ( m_DailyCounter < 10 )
			UpdatedCounter += "000";
		else if ( m_DailyCounter < 100 )
			UpdatedCounter += "00";
		else if ( m_DailyCounter < 1000 )
			UpdatedCounter += "0";

		UpdatedCounter += m_DailyCounter;

		m_Counter.text = UpdatedCounter;
	}

	public void UpdateDailyMistakes()
	{
		// TODO:: Implement different grades of mistakes.
		m_DailyMistakes++;
	}

	public void ResetCounters()
	{
		m_DailyCounter	= 0;
		m_DailyMistakes = 0;

		m_Counter.text = "0000";
	}
}
