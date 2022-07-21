using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static	GameManager m_Instance;
	public static	GameManager Instance => m_Instance;

	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "Too many instances of GameManager. Deleting the extra..." );
			Destroy( gameObject );
		}
	}

	// This function can be used in order to have specific functionality when loading a level.
	public void LoadLevel( int _SceneIndex, float _Delay = 0.0f )
	{
		StartCoroutine( LoadLevelAfterDelay( _SceneIndex, _Delay ) );
	}

	private IEnumerator LoadLevelAfterDelay( int _SceneIndex, float _Delay )
	{
		yield return new WaitForSeconds( _Delay );

		SceneManager.LoadScene( _SceneIndex );
	}
}
