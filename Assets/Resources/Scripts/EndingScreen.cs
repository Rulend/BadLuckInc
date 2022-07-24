using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{
	private struct SSlide 
	{
		public Text[]	m_Texts;
		public string[] m_Messages;
	}

	[SerializeField] private Button m_NextSlideButton;

	[SerializeField] private float	m_LetterCooldownDuration = 0.048f;
	private float					m_LetterCooldownTimeLeft;
	
	private GameObject				m_SlidesParent;

	private Text					m_CurrentText;
	private string					m_CurrentMessage;


	private SSlide[]				m_Slides;
	private int						m_SlideIndex = 0;
	private int						m_MessageIndex;


	private void Awake()
	{
		enabled = false; // Turn off the component since we don't want to run the update function.
	}


	public void Activate( GameObject _SlidesParent )
	{
		m_SlidesParent = _SlidesParent;

		m_Slides = new SSlide[ m_SlidesParent.transform.childCount ];

		for ( int ChildIndex = 0; ChildIndex < m_SlidesParent.transform.childCount; ++ChildIndex )
		{
			SSlide NewSlide = new SSlide();

			NewSlide.m_Texts	= _SlidesParent.transform.GetChild( ChildIndex ).GetComponentsInChildren<Text>( true );
			NewSlide.m_Messages = new string[ NewSlide.m_Texts.Length ];

			for ( int MessageIndex = 0; MessageIndex < NewSlide.m_Texts.Length; ++MessageIndex )
			{
				NewSlide.m_Messages[ MessageIndex ]		= NewSlide.m_Texts[ MessageIndex ].text;
				NewSlide.m_Texts[ MessageIndex ].text	= "";
			}

			m_Slides[ ChildIndex ] = NewSlide;
		}


		m_SlideIndex = 0;

		m_SlidesParent.transform.GetChild( m_SlideIndex ).gameObject.SetActive( true );
		m_CurrentText		= m_Slides[ m_SlideIndex ].m_Texts[ m_MessageIndex ];
		m_CurrentMessage	= m_Slides[ m_SlideIndex ].m_Messages[ m_MessageIndex ];


		m_NextSlideButton.onClick.AddListener( ShowNextSlide );

		enabled = true;
	}



	// Update is called once per frame
	void Update()
	{
		m_LetterCooldownTimeLeft -= Time.deltaTime;

		if ( m_LetterCooldownTimeLeft < 0.0f )
		{
			if ( m_CurrentText.text.Length < m_CurrentMessage.Length ) // If a text in a slide is not completed
			{
				char AddedChar = m_CurrentMessage[ m_CurrentText.text.Length ];

				if ( AddedChar != ' ' )
				{
					m_LetterCooldownTimeLeft = AddedChar == '.' ? m_LetterCooldownDuration * 3.0f : m_LetterCooldownDuration;

					AudioManager.Instance.PlayVoice( AudioManager.ESoundVoice.BadLuckInc );
				}

				m_CurrentText.text += AddedChar;
			}
			else // Meaning that a text for the slide has been completed
			{
				if ( m_MessageIndex < m_Slides[m_SlideIndex].m_Texts.Length - 1 ) // If not all texts in the slide have been filled
				{
					m_MessageIndex++;
					m_CurrentText		= m_Slides[ m_SlideIndex ].m_Texts[ m_MessageIndex ];
					m_CurrentMessage	= m_Slides[ m_SlideIndex ].m_Messages[ m_MessageIndex ];

					m_LetterCooldownTimeLeft = m_LetterCooldownDuration * 5.0f;
				}
				else // All the texts of the slide have been filled
					m_NextSlideButton.gameObject.SetActive( true );
			}
		}
	}


	// Called from m_NextSlideButton
	public void ShowNextSlide()
	{
		if ( m_SlideIndex < m_Slides.Length - 1 ) // If not all slides have been completed
		{
			m_SlideIndex++;

			m_MessageIndex = 0;

			m_CurrentText		= m_Slides[ m_SlideIndex ].m_Texts[ m_MessageIndex ];
			m_CurrentMessage	= m_Slides[ m_SlideIndex ].m_Messages[ m_MessageIndex ];

			m_SlidesParent.transform.GetChild( m_SlideIndex - 1 ).gameObject.SetActive( false );
			m_SlidesParent.transform.GetChild( m_SlideIndex ).gameObject.SetActive( true );

		}
		else // If all slides have been completed
		{
			m_SlidesParent.transform.GetChild( m_SlideIndex ).gameObject.SetActive( false );
			enabled = false;

			ButtonManager.Instance.ButtonExit();
			// Goo back to menu.
		}

		m_NextSlideButton.gameObject.SetActive( false ); // Prevent the button from being clicked multiple times
	}
}
