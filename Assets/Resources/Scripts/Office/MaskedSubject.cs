using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskedSubject : MonoBehaviour
{
	public enum EFacility
	{
		Undecided		,
		Office			,	// The office where the player works
		Plantation		,	// The plantation facility
		Chamber			,	// The chamber facility
		Rehab			,	// The rehabilitation facility
		Graveyard		,	// The graveyard
		Research		,	// The research facility
	}

	[System.Serializable]
	public class SpecialMasked // xd wanted this to be a struct but structs dont have field initializers in this version of C#
	{
		public MaskedSubjectInfo	m_MaskedInfo;
		public int					m_IntendedLocation = -1; // Set this to -1 if not wanted
	}



	[SerializeField] private SpriteRenderer m_Base;
	[SerializeField] private SpriteRenderer m_Facility;
	[SerializeField] private SpriteRenderer m_Accessory;


	[SerializeField] private Transform m_StartPoint;
	[SerializeField] private Transform m_Office;
	[SerializeField] private Transform m_Plantation;
	[SerializeField] private Transform m_Chamber;
	[SerializeField] private Transform m_Rehab;
	[SerializeField] private Transform m_Graveyard;
	[SerializeField] private Transform m_ResearchLab;


	private MovingCollisionCheckObject	m_rMovingComponent;
	private MaskedSubjectInfo			m_MaskedInfo;

	public EFacility	IntentedFacility => m_MaskedInfo.m_IntentedFacility; // The intended facility of the masked.
	public int			StressLevel => m_MaskedInfo.m_StressLevel; // The stress level.
	public int			ThreatLevel => m_MaskedInfo.m_ThreatLevel; // The threat level.


	private void Awake()
	{
		m_rMovingComponent = GetComponent<MovingCollisionCheckObject>();
	}


	public void SetMaskedInfo( MaskedSubjectInfo _NewMaskedInfo )
	{
		m_MaskedInfo		= _NewMaskedInfo;

		m_Base.sprite		= m_MaskedInfo.m_BaseSprite;
		m_Facility.sprite	= m_MaskedInfo.m_FacilitySprite;
		m_Accessory.sprite	= m_MaskedInfo.m_AccessorySprite;
	}


	public void SetTravelDestination( EFacility _DestinationFacility )
	{
		Transform NewTargetTransform = transform;


		switch ( _DestinationFacility )
		{
			case EFacility.Office:
				transform.position = m_StartPoint.position;
				NewTargetTransform = m_Office;
				break;

			case EFacility.Chamber:
				NewTargetTransform = m_Chamber;
				break;
			case EFacility.Plantation:
				NewTargetTransform = m_Plantation;
				break;
			case EFacility.Research:
				NewTargetTransform = m_ResearchLab;
				break;
			case EFacility.Graveyard:
				NewTargetTransform = m_Graveyard;
				break;
			case EFacility.Rehab:
				NewTargetTransform = m_Rehab;
				break;
		}

		m_rMovingComponent.SetTargetTransform( NewTargetTransform );
	}



}
