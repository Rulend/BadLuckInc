using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	private static DialogueManager m_Instance;

	public static DialogueManager Instance => m_Instance;


	public enum EDialogueWindow
	{
		Controller	,
		Subject		,
		Alexis		,
		Radio		,

		NumDialogueWindows
	}

	[System.Serializable]
	private struct SDialogueWindow
	{
		public TextMeshPro		m_TextField;
		public EDialogueWindow	m_Window;
	}

	[SerializeField] private SDialogueWindow[]	m_DialogueWindows;
	private Conversation						m_CurrentConversation;
	private Conversation.SDialogue				m_CurrentDialogue;
	private Conversation.SDialogue				m_PreviousDialogue;
	private int									m_DialogueIndex;


	private void Awake()
	{
		if ( m_Instance == null )
		{
			m_Instance = this;
		}
		else
		{
			Debug.LogError( "More than one instance of DialogueManager found, deleting the extra..." ); 
			Destroy( gameObject );
		}
	}


	public void AddConversation( Conversation _NewConversation )
	{
		// TODO:: (maybe)
		// Empty all the windows texts
		// Disable all the windows

		m_CurrentConversation = _NewConversation;

		m_DialogueIndex = -1; // Needs to be done in order for ProgressConversation to work. It might be a hack, but it's Easy and won't fail unless you do something crazy.


		foreach ( SDialogueWindow CurrentWindow in m_DialogueWindows )
			CurrentWindow.m_TextField.gameObject.transform.parent.gameObject.SetActive( false );
	}


	public bool ProgressConversation()
	{
		m_DialogueIndex++;

		if ( !m_CurrentConversation )
			return false;

		if ( m_DialogueIndex >= m_CurrentConversation.m_Dialogues.Length )
		{
			Debug.Log( "Ending conversation" );

			// TODO::
			// Set all windows text to be: ""
			// Disable all windows
			// Set current conversation and current dialogue to null
			// Create an event that will be triggered here, name it something like EndOfConversationEvent and invoke it.
			// After invoking the event, unsubscribe all of the delegates. 
			// Add the original infected's outburst to that list.
			InterruptConversation();

			return false;
		}

		m_PreviousDialogue						= m_CurrentDialogue;
		m_CurrentDialogue						= m_CurrentConversation.m_Dialogues[ m_DialogueIndex ];

		if ( m_DialogueIndex == 0 ) // If this is the first dialogue in the conversation
		{
			m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.gameObject.transform.parent.gameObject.SetActive( true );// TODO:: Find a better way to do this
			m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.text = m_CurrentDialogue.m_Message;
			return true;
		}

		if ( m_CurrentDialogue.m_ClearPrevious ) // If the current dialogue should clear the previous dialogue
		{
			if ( m_PreviousDialogue.m_Window == m_CurrentDialogue.m_Window )
				m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.text = m_CurrentDialogue.m_Message;
			else
			{
				m_DialogueWindows[ (int)m_PreviousDialogue.m_Window ].m_TextField.gameObject.transform.parent.gameObject.SetActive( false ); // TODO:: Find a better way to do this
				m_DialogueWindows[ (int)m_PreviousDialogue.m_Window ].m_TextField.text	= "Empty!";

				m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.gameObject.transform.parent.gameObject.SetActive( true ); // TODO:: Find a better way to do this
				m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.text	= m_CurrentDialogue.m_Message;
			}
		}
		else
		{
			if ( m_PreviousDialogue.m_Window == m_CurrentDialogue.m_Window )
				m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.text += m_CurrentDialogue.m_Message;
			else
				m_DialogueWindows[ (int)m_CurrentDialogue.m_Window ].m_TextField.text = m_CurrentDialogue.m_Message;
		}

		return true;
	}


	public void InterruptConversation()
	{
		m_CurrentConversation	= null;
		m_CurrentDialogue		= null;
		m_PreviousDialogue		= null;

		foreach ( SDialogueWindow CurrentWindow in m_DialogueWindows )
			CurrentWindow.m_TextField.gameObject.transform.parent.gameObject.SetActive( false );
	}
}
