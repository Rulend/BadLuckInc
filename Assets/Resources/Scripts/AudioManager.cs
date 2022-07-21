using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioManager m_Instance;
	public static AudioManager Instance => m_Instance;


	public enum ESoundEnvironment
	{
		ShutterOpen,
		ShutterClose,
		ShutterRoll,
		GunShot,
		ButtonPress,
		HandScan,
		DoorOpen,
		DoorClose,

		NumSoundEnvironment
	}

	public enum ESoundVoice
	{
		BadLuckInc,

		NumSoundVoices
	}


	[SerializeField] private GameObject m_VoiceSourceParent;
	[SerializeField] private GameObject m_EnvironmentSourceParent;

	private AudioSource[] m_SoundsVoices;
	private AudioSource[] m_SoundsEnvironment;

	private float[] m_VoicePitches;


	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of AudioManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}

		m_SoundsVoices		= m_VoiceSourceParent.GetComponentsInChildren<AudioSource>();
		m_SoundsEnvironment = m_EnvironmentSourceParent.GetComponentsInChildren<AudioSource>();

		m_VoicePitches = new float[] { 0.85f, 1.0f, 1.45f };
	}


	public void PlaySoundEffect( ESoundEnvironment _SoundEffect )
	{
		m_SoundsEnvironment[ (int)_SoundEffect ].enabled = false;
		m_SoundsEnvironment[ (int)_SoundEffect ].enabled = true;
	}

	public void StopSoundEffect( ESoundEnvironment _SoundEffect )
	{
		m_SoundsEnvironment[ (int)_SoundEffect ].enabled = false;
	}


	public void PlayVoice( ESoundVoice _VoiceToUse )
	{
		m_SoundsVoices[ (int)_VoiceToUse ].pitch = m_VoicePitches[ Random.Range( 0, m_VoicePitches.Length ) ];

		m_SoundsVoices[ (int)_VoiceToUse ].enabled = false;
		m_SoundsVoices[ (int)_VoiceToUse ].enabled = true;
	}
}
