using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu( fileName = "Conversation", menuName = "Conversation", order = 1 )]
public class Conversation : ScriptableObject
{
	[System.Serializable]
	public class SDialogue
	{
		public string							m_Message;                  // The text that the dialogue contains.
		public DialogueManager.EDialogueWindow	m_Window;
		public bool								m_ClearPrevious = true;		// Used for dividing a dialogue into multiple parts. Set to false if you want to have the effect of "YOU. GOD. DAMN. IDIOT." where one word appears every time you press next.
	}

	public SDialogue[] m_Dialogues;
}
