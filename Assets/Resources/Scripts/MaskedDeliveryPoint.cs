using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedDeliveryPoint : MonoBehaviour
{
	// A class used to check if the masked sent to the correct facility.

	[SerializeField] private Masked.EFacility m_Facility;

	private void OnTriggerEnter2D( Collider2D _MaskedCollider )
	{
		// TODO:: Add more logic here

		ScoreManager.Instance.UpdateDailyCounter();

		if ( _MaskedCollider.GetComponent<Masked>().IntentedFacility != m_Facility ) // If they were sent to the wrong facility
			// TODO:: Make a check here to see if the amount of daily mistakes has exceeded amount X, if it has, insta lose or get fired from your job.
			ScoreManager.Instance.UpdateDailyMistakes();

		MaskedManager.Instance.SendNewMask();
	}
}
