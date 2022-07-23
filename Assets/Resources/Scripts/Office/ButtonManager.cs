using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	private static	ButtonManager m_Instance;
	public static	ButtonManager Instance => m_Instance;

	[SerializeField] private Shutter					m_Shutter;
	[SerializeField] private Button						m_ShutterButton;
	[SerializeField] private GameObject					m_ExtraButtonsParent;
	[SerializeField] private GameObject					m_Scanner;
	[SerializeField] private FadeGraphicalElements		m_FadeGraphicElements;

	public delegate void LocationButtonPressedDelegate(); // Changing my naming convention for delegates after understanding more about them. Might conflict with other delegate names. TODO: Fix that.
	public event LocationButtonPressedDelegate LocationButtonPressedEvent;

	private bool m_AllowActions = false;
	private List<LocationButtonPressedDelegate> m_OneTimeFunctionCalls;

	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of ButtonManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}

		m_OneTimeFunctionCalls = new List<LocationButtonPressedDelegate>();
	}

	private void Start()
	{
		DayManager.Instance.NextDayEvent += EnableShutterButton;
	}


	public void ButtonUnlockShutter()
	{
		m_Shutter.Activate();

		ButtonPressedFollowup();
		m_ShutterButton.interactable = false;
	}

	public void ButtonLockShutter()
	{
		m_Shutter.Activate();
	}

	// This is for the gun, which was supposed to do something else, but for now it will be the exit button.
	public void ButtonExit()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.GunShot );
		GameManager.Instance.LoadLevel( 0, 1.0f );
		// Todo:: add fade to black transition
	}


	public void ButtonPlantation()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Plantation );
		ButtonPressedFollowup();
	}

	public void ButtonChamber()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Chamber );
		ButtonPressedFollowup();
	}

	public void ButtonGun()
	{
		if ( !m_AllowActions )
			return;

	}

	public void ButtonScan()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		m_Scanner.SetActive( true );

		ButtonPressedFollowup(); // Enable it again once the scan completes

		m_FadeGraphicElements.InstantOut();
		LocationButtonPressedEvent += m_FadeGraphicElements.FadeOut;
		m_OneTimeFunctionCalls.Add( m_FadeGraphicElements.FadeOut );
	}


	public void ButtonExtra()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( m_ExtraButtonsParent.activeSelf )
			m_ExtraButtonsParent.SetActive( false );
		else
			m_ExtraButtonsParent.SetActive( true );
	}

	public void ButtonRehab()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Rehab );
		ButtonPressedFollowup();
	}


	public void ButtonGraveYard()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Graveyard );
		ButtonPressedFollowup();
	}

	public void ButtonResearchInstitute() // Should have called it Lab 
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( MaskedSubject.EFacility.Research );
		ButtonPressedFollowup();
	}

	private void ButtonPressedFollowup()
	{
		if ( LocationButtonPressedEvent != null )
		{
			LocationButtonPressedEvent.Invoke();

			int ArraySize = LocationButtonPressedEvent.GetInvocationList().Length;

			for ( int DelegateIndex = 0; DelegateIndex < ArraySize; ++DelegateIndex )// Remove all one-time events
			{
				LocationButtonPressedDelegate CurrentDelegate = (LocationButtonPressedDelegate)LocationButtonPressedEvent.GetInvocationList()[ DelegateIndex ];

				if ( CurrentDelegate == m_OneTimeFunctionCalls[ DelegateIndex ] )
					LocationButtonPressedEvent -= CurrentDelegate;
			}
		}

		DisableNormalButtons();
	}


	public void DisableNormalButtons()
	{
		m_AllowActions = false;
	}

	public void EnableNormalButtons()
	{
		m_AllowActions = true;
	}

	public void EnableShutterButton()
	{
		m_ShutterButton.interactable = true;
	}
}
