using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	private static	ButtonManager m_Instance;
	public static	ButtonManager Instance => m_Instance;

	[SerializeField] private Shutter	m_Shutter;
	[SerializeField] private Button		m_ShutterButton;

	private bool m_AllowActions = false;

	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of ButtonManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}
	}

	private void Start()
	{
		DayManager.Instance.NextDayEvent += EnableShutterButton;
	}


	public void ButtonUnlockShutter()
	{
		m_Shutter.Activate();

		DisableNormalButtons();
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
		SceneManager.LoadScene( 0 );
		// Todo:: add fade to black transition
	}


	public void ButtonPlantation()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;

		MaskedManager.Instance.Masked.SetTravelDestination( Masked.EFacility.Plantation );
		DisableNormalButtons();
	}

	public void ButtonChamber()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		MaskedManager.Instance.Masked.SetTravelDestination( Masked.EFacility.Chamber );
		DisableNormalButtons();
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

	}


	public void ButtonExtra()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		//DisableNormalButtons(); // Enable it again once the scan completes
	}

	public void ButtonRehab()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		//DisableNormalButtons();
	}


	public void ButtonGraveYard()
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


		//DisableNormalButtons();
	}

	public void ButtonResearchInstitute() // Should have called it Lab 
	{
		AudioManager.Instance.PlaySoundEffect( AudioManager.ESoundEnvironment.ButtonPress );

		if ( !m_AllowActions )
			return;


	//	DisableNormalButtons();
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
