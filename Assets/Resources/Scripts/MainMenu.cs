using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void ButtonStartGame()
	{
		SceneManager.LoadScene( 1 );
	}

	public void ButtonExit()
	{
		Application.Quit();
	}

	public void Options()
	{

	}
}
