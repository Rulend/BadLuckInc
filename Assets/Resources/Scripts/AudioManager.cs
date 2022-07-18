using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager m_Instance;
	public static AudioManager Instance => m_Instance;
	private AudioSource[] m_AudioSources;

	public enum ESound
	{
		ShutterOpen		,
		ShutterClose	,
		ShutterRoll		,
		GunShot			,
		ButtonPress		,
		HandScan		,
		VoiceSpeaking1	,
		VoiceSpeaking2	,
		DoorOpen		,
		DoorClose		,



		NumSoundEffects
	}

	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of AudioManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}


		m_AudioSources = GetComponents<AudioSource>();
	}


	public void PlaySoundEffect( ESound _SoundEffect )
	{
		m_AudioSources[ (int)_SoundEffect ].enabled = false;
		m_AudioSources[ (int)_SoundEffect ].enabled = true;
	}

	public void StopSoundEffect( ESound _SoundEffect )
	{
		m_AudioSources[ (int)_SoundEffect ].enabled = false;
	}
}
