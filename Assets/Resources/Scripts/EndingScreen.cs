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



	// Activate the ending screen.
	// This method creates an array of SSlides with the size of _SlidesParent.transform.childCount.
	// SSlides are then created for every child of the _SlidesParent. Ex: If the parent has 3 direct children, then 3 slides will be created.
	// Every one of these children are expected to have their own children with text components attached to them. These texts are put into the 
	// SSlide as a message, and an empty string is prepared which can be filled with one char at a time. Structure looks like this:
	//	_SlidesParent
	//		SSlide1
	//			Message1 // text component: ("Thank you for your work today.")
	//			Message2 // text component: ("You worked very hard!")
	//		SSlide2
	//			Message1 // text component: ("Although we appreciate you...")
	//			Message2 // text component: ("You have come to know more than necessary.")
	//			Message3 // text component: ("The time has come to correct that.")
	//		SSlide3
	//			Message1 // text component: ("Goodbye.")


	// TODO:: Make better use of classes in order to make this easier to UNDERSTAND (code wise); it is already very easy to use.

	public void Activate( GameObject _SlidesParent )
	{
		m_SlidesParent = _SlidesParent;

		m_Slides = new SSlide[ m_SlidesParent.transform.childCount ]; // Setup the amount of slides

		for ( int ChildIndex = 0; ChildIndex < m_SlidesParent.transform.childCount; ++ChildIndex ) // Loop through all the slide-children
		{
			SSlide NewSlide = new SSlide();

			NewSlide.m_Texts	= _SlidesParent.transform.GetChild( ChildIndex ).GetComponentsInChildren<Text>( true );	// Set the 
			NewSlide.m_Messages = new string[ NewSlide.m_Texts.Length ];	// 

			for ( int MessageIndex = 0; MessageIndex < NewSlide.m_Texts.Length; ++MessageIndex )	// Loop through the children which have text components
			{
				NewSlide.m_Messages[ MessageIndex ]		= NewSlide.m_Texts[ MessageIndex ].text;	// Save down the text written inside the editor as a string
				NewSlide.m_Texts[ MessageIndex ].text	= "";										// Empty the actual text component, so that it can be filled one char at a time.
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
	// Whenever update is running in the ending screen, it will check if there is time to display a letter in its slides.
	void Update()
	{
		m_LetterCooldownTimeLeft -= Time.deltaTime;

		if ( m_LetterCooldownTimeLeft < 0.0f ) // If enough time has passed to show the next letter
		{
			if ( m_CurrentText.text.Length < m_CurrentMessage.Length ) // If a text in a slide is not completed
			{
				char AddedChar = m_CurrentMessage[ m_CurrentText.text.Length ]; // Get the char to add to the message

				if ( AddedChar != ' ' ) // If the letter is not a space, add a delay until the next char is displayed and play a sound effect
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

					m_LetterCooldownTimeLeft = m_LetterCooldownDuration * 5.0f; // Since the current text was completed, set a longer delay than usual. Make the 5 adjustable from the editor.
				}
				else // All the texts of the slide have been filled
					m_NextSlideButton.gameObject.SetActive( true );
			}
		}
	}


	// Called from m_NextSlideButton
	// Shows the next slide,
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
		else // If all slides have been completed, and the player presses the next button, disable everything and exit. TODO:: Add so it will be adjustable what happens here.
		{
			m_SlidesParent.transform.GetChild( m_SlideIndex ).gameObject.SetActive( false );
			enabled = false;

			OfficeButtonManager.Instance.ButtonExit();
			// Goo back to menu.
		}

		m_NextSlideButton.gameObject.SetActive( false ); // Prevent the button from being clicked multiple times
	}
}
