using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayFixer : MonoBehaviour
{
	
	[SerializeField] private Button m_NextSlideButton;

	[SerializeField] private float m_LetterCooldownDuration;
	[SerializeField] private float m_LetterCooldownTimeLeft;

	[SerializeField] private GameObject[] m_Slides;

	private GameObject m_CurrentSlide;

	private Text[] m_Slide1Texts;
	private Text[] m_Slide2Texts;
	private Text[] m_Slide3Texts;

	private Text[][]	m_TextArrays;
	private Text[]		m_CurrentTextArray;
	private Text		m_CurrentText;

	private string[]	m_Slide1Messages;
	private string[]	m_Slide2Messages;
	private string[]	m_Slide3Messages;

	private string[][]	m_StringArrays;
	private string[]	m_CurrentStringArray;
	private string		m_CurrentMessage;

	private string[]	m_RandomEvents;
	private int			m_EmployeeNumber;

	private int			m_CurrentTextIndex = 0;
	private int			m_CurrentTextArrayIndex = 0;




	private void Awake()
	{
		m_Slide1Texts = m_Slides[ 0 ].GetComponentsInChildren<Text>();
		m_Slide2Texts = m_Slides[ 1 ].GetComponentsInChildren<Text>();
		m_Slide3Texts = m_Slides[ 2 ].GetComponentsInChildren<Text>();

		m_TextArrays = new Text[3][];

		m_TextArrays[ 0 ] = m_Slide1Texts;
		m_TextArrays[ 1 ] = m_Slide2Texts;
		m_TextArrays[ 2 ] = m_Slide3Texts;


		m_EmployeeNumber = Random.Range( 0, 1000 );

		m_RandomEvents = new string[ 3 ];
		m_RandomEvents[ 0 ] = "because you forgot to take out the trash yesterday, ";
		m_RandomEvents[ 1 ] = "because you didn't cry when watching that romantic movie a month ago, ";
		m_RandomEvents[ 2 ] = "because you waited to buy clothes on sale, ";

		m_StringArrays	= new string[ 3 ][];
	}


	public void Activate()
	{
		int DailyMistakes = ScoreManager.Instance.DailyMistakes;

		m_Slide1Messages = new string[ 3 ];
		m_Slide1Messages[ 0 ] = $"Today your daily counter reached { ScoreManager.Instance.DailyCounter }.";
		m_Slide1Messages[ 1 ] = $"You made a total of { DailyMistakes } mistakes today.";

		if ( ScoreManager.Instance.DailyMistakes < 1 )
			m_Slide1Messages[ 2 ] = $"Outstanding work, employee { m_EmployeeNumber }.";
		else
			m_Slide1Messages[ 2 ] = $"Because of your mistakes, you will be fined { DailyMistakes * ScoreManager.Instance.CreditLoss } credits for company damages.";



		m_Slide2Messages		= new string[ 3 ];
		int RandomCreditReward	= Random.Range( -2, 3 );
		ScoreManager.Instance.AdjustCredits( RandomCreditReward );
		int RemainingCredits = ScoreManager.Instance.CalculateCredits();

		if ( DailyMistakes < 1 )
			m_Slide2Messages[ 0 ] = "As a reward for making 0 mistakes, you have been entered into the credits adjustment system.";
		else
			m_Slide2Messages[ 0 ] = $"As a punishment for making { DailyMistakes } mistakes, you have been entered into the credits adjustment system.";

		m_Slide2Messages[ 1 ] = "The system states that " + m_RandomEvents[ Random.Range( 0, m_RandomEvents.Length ) ] + $"you have been awarded { RandomCreditReward } credits. Hooray.";

		m_Slide2Messages[ 2 ] = $"After paying your daily rent, employment fees and food costs, you end up at { RemainingCredits } credits.";


		m_Slide3Messages = new string[ 1 ];
		m_Slide3Messages[ 0 ] = "BadLuck Inc thanks you for your service.";

		// Setup needed stuff that needs to be done every time
		m_StringArrays[ 0 ] = m_Slide1Messages;
		m_StringArrays[ 1 ] = m_Slide2Messages;
		m_StringArrays[ 2 ] = m_Slide3Messages;


		m_CurrentTextArrayIndex = 0;
		m_CurrentTextIndex		= 0;

		m_CurrentTextArray		= m_Slide1Texts;
		m_CurrentText			= m_CurrentTextArray[ m_CurrentTextIndex ];
		m_CurrentStringArray	= m_StringArrays[ m_CurrentTextArrayIndex ];
		m_CurrentMessage		= m_CurrentStringArray[ m_CurrentTextIndex ];
		m_CurrentSlide			= m_Slides[ m_CurrentTextArrayIndex ];
		m_CurrentSlide.SetActive( true );

		// Clean out all the texts so they won't be filled 
		foreach ( Text[] CurrentTextArray in m_TextArrays )
		{
			foreach ( Text CurrentText in CurrentTextArray )
				CurrentText.text = "";
		}


		enabled = true;
	}



	// Update is called once per frame
	void Update()
    {
		// TODO:: Set this up
		m_LetterCooldownTimeLeft -= Time.deltaTime;

		if ( m_LetterCooldownTimeLeft < 0.0f )
		{
			if ( m_CurrentText.text.Length < m_CurrentMessage.Length )
			{
				char AddedChar = m_CurrentMessage[ m_CurrentText.text.Length ];

				if ( AddedChar != ' ' )
				{
					m_LetterCooldownTimeLeft = m_LetterCooldownDuration;

					int RandomSound = Random.Range( 0, 2 );

					if ( RandomSound == 0 )
						AudioManager.Instance.PlaySoundEffect( AudioManager.ESound.VoiceSpeaking1 );
					else
						AudioManager.Instance.PlaySoundEffect( AudioManager.ESound.VoiceSpeaking2 );
				}

				m_CurrentText.text += AddedChar;
			}
			else // Meaning that a text for the slide has been completed
			{
				if ( m_CurrentTextIndex < m_CurrentTextArray.Length -1 ) // If not all texts in the slide have been filled
				{
					m_CurrentTextIndex++;
					m_CurrentText		= m_CurrentTextArray[ m_CurrentTextIndex ];
					m_CurrentMessage	= m_CurrentStringArray[ m_CurrentTextIndex ];

					m_LetterCooldownTimeLeft = m_LetterCooldownDuration * 5.0f;
				}
				else // All the texts of the slide have been filled
				{
					m_NextSlideButton.gameObject.SetActive( true );
				}
			}
		}
    }


	public void ShowNextSlide()
	{
		if ( m_CurrentTextArrayIndex < m_TextArrays.Length - 1 ) // If not all slides have been completed
		{
			m_CurrentTextArrayIndex++;

			m_CurrentTextIndex = 0;

			m_CurrentTextArray	= m_TextArrays[ m_CurrentTextArrayIndex ];
			m_CurrentText		= m_CurrentTextArray[ m_CurrentTextIndex ];

			m_CurrentStringArray	= m_StringArrays[ m_CurrentTextArrayIndex ];
			m_CurrentMessage		= m_CurrentStringArray[ m_CurrentTextIndex ];

			m_CurrentSlide.SetActive( false );
			m_CurrentSlide = m_Slides[ m_CurrentTextArrayIndex ];
			m_CurrentSlide.SetActive( true );
		}
		else // If all slides have been completed
		{
			m_CurrentSlide.SetActive( false );

			enabled = false;
			DayManager.Instance.ProgressToNextDay();
		}


		m_NextSlideButton.gameObject.SetActive( false ); // Prevent the button from being clicked multiple times
	}

}