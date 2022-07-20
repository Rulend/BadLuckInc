using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private FadeElement	m_ScreenFade;
	[SerializeField] private AudioSource	m_StartGameSound;

	public void ButtonStartGame()
	{
		StartCoroutine( LoadGame() );
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	public void Options()
	{

	}

	private IEnumerator LoadGame()
	{
		m_ScreenFade.FadeIn();
		m_StartGameSound.enabled = true;

		yield return new WaitForSeconds( 6 );

		SceneManager.LoadScene( 1 );
	}
}
