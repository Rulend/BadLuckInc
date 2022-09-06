using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeTravelPoint : MonoBehaviour
{
	// TODO:: Maybe try to merge this behaviour with MaskedDeliveryPoint? They do kinda different stuff, but still
	private void OnTriggerEnter2D( Collider2D other )
	{
		OfficeButtonManager.Instance.EnableNormalButtons();
	}
}
