using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OfficeButtonManager : MonoBehaviour
{
	private static	OfficeButtonManager m_Instance;
	public static	OfficeButtonManager Instance => m_Instance;

	[SerializeField] private Shutter					m_Shutter;
	[SerializeField] private Button						m_ShutterButton;
	[SerializeField] private GameObject					m_ExtraButtonsParent;
	[SerializeField] private GameObject					m_Scanner;
	[SerializeField] private FadeGraphicalElements		m_FadeGraphicElements;

	public delegate void LocationButtonPressedDelegate(); // Changing my naming convention for delegates after understanding more about them. Might conflict with other delegate names. TODO: Fix that.
	public event LocationButtonPressedDelegate LocationButtonPressedEvent;

	private HashSet<LocationButtonPressedDelegate> m_OneTimeFunctionCalls;
	private bool m_AllowActions = false;
	private bool m_AllowTalking = true;



	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of ButtonManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}

		m_OneTimeFunctionCalls = new HashSet<LocationButtonPressedDelegate>();
	}

	private void Start()
	{
		DayManager.Instance.NextDayEvent += EnableShutterButton;
	}


	// Note: methods with zero references are usually called from a UI button.

	// From the office, called when the button in the bottom left with a hand on it is pressed.
	public void ButtonToggleShutter()
	{
		m_Shutter.Activate();

		ButtonPressedFollowup();
		m_ShutterButton.interactable = false;
	}


	// This is for the gun, which was supposed to do something else, but for now it will be the exit button.
	public void ButtonExit()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.GunShot );
		GameManager.Instance.LoadLevel( 0, 1.0f );
		// Todo:: add fade to black transition
	}

	// Called from the green button in the office.
	public void ButtonPlantation()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Plantation );
		ButtonPressedFollowup();
	}


	// Called from the red button in the office.
	public void ButtonChamber()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Chamber );
		ButtonPressedFollowup();
	}


	// Is supposed to be called from the gun in the office.
	public void ButtonGun()
	{
		if ( !m_AllowActions )
			return;

	}


	// Is called from the small blue button with a magnifying glass on it in the office.
	public void ButtonScan()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		m_Scanner.SetActive( true );

		DisableNormalButtons(); // Enabled again from Scanner.cs, when the scan has completed.

		m_FadeGraphicElements.InstantOut();

		if ( m_OneTimeFunctionCalls.Add( m_FadeGraphicElements.FadeOut ) ) // Add returns true if FadeOut is not already in the hashset
			LocationButtonPressedEvent += m_FadeGraphicElements.FadeOut;
	}


	// Called from the cyan button in the office; has an arrow on it.
	public void ButtonExtra()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( m_ExtraButtonsParent.activeSelf )
			m_ExtraButtonsParent.SetActive( false );
		else
			m_ExtraButtonsParent.SetActive( true );
	}


	// Called from the orange button in the office; has a hammer and nails on it
	public void ButtonRehab()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Rehab );
		ButtonPressedFollowup();
	}


	// Called from the purple button in the office; has a skull on it.
	public void ButtonGraveYard()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Graveyard );
		ButtonPressedFollowup();
	}


	// Called from the black button in the office; has an icon depicting a lab-glass
	public void ButtonResearchInstitute() // Should have called it Lab 
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Research );
		ButtonPressedFollowup();
	}


	// Called from the white button in the office, should have a speech bubble on it.
	public void ButtonTalk()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( m_AllowTalking )
		{
			m_AllowTalking = DialogueManager.Instance.ProgressConversation();
			//MaskedManager.Instance.Masked.AdjustStressLevel( 1 );
			//m_Scanner.GetComponent<Scanner>().UpdateScannerDisplay( MaskedManager.Instance.Masked );
		}
	}




	private void ButtonPressedFollowup()
	{
		if ( LocationButtonPressedEvent != null )
		{
			LocationButtonPressedEvent.Invoke();

			foreach ( LocationButtonPressedDelegate CurrentDelegate in m_OneTimeFunctionCalls )
				LocationButtonPressedEvent -= CurrentDelegate;

			m_OneTimeFunctionCalls.Clear();
		}

		DisableNormalButtons();
	}


	public void DisableNormalButtons()
	{
		m_AllowActions = false;
		m_AllowTalking = false;
	}


	public void EnableNormalButtons()
	{
		m_AllowActions = true;
	}


	public void EnableAllButtons()
	{
		m_AllowActions = true;
		m_AllowTalking = true;
	}


	public void EnableShutterButton()
	{
		m_ShutterButton.interactable = true;
	}
}
