using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskedManager : MonoBehaviour
{
	private static MaskedManager m_Instance;
	public static MaskedManager Instance => m_Instance;


	[SerializeField] private Masked m_Masked;
	public Masked Masked => m_Masked;


	// TODO:: once these fields have been confirmed to work, they don't have to be serializefielded
	[SerializeField] private Sprite[] m_MaskedBases;
	[SerializeField] private Sprite[] m_DiceBases;
	//[SerializeField] private List<Sprite> m_BaseMutated;
	//[SerializeField] private List<Sprite> m_BaseDice;
	[SerializeField] private Sprite[] m_FacilityPlantation;
	[SerializeField] private Sprite[] m_FacilityChamber;
	[SerializeField] private Sprite[] m_FacilityMixture;
	[SerializeField] private Sprite[] m_FacilityMutated;
	[SerializeField] private Sprite[] m_FacilityUnique;
	[SerializeField] private Sprite[] m_FacilityDice;
	[SerializeField] private Sprite[] m_AccessoriesMasked;
	[SerializeField] private Sprite[] m_AccessoriesDice;

	[SerializeField] private List<MaskedInfo> m_MaskedInfos;

	private void Awake()
	{
		if ( m_Instance == null )
			m_Instance = this;
		else
		{
			Debug.LogError( "More than once instance of MaskedManager detected. Deleting the extra..." );
			Destroy( gameObject );
		}


		m_MaskedInfos = new List<MaskedInfo>();



		m_MaskedBases			= Resources.LoadAll<Sprite>( "Art/Masked/Bases/Masked" );
		m_DiceBases				= Resources.LoadAll<Sprite>( "Art/Masked/Bases/Dice" );
		m_FacilityPlantation	= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Plantation" );
		m_FacilityChamber		= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Chamber"  );
		m_FacilityMixture		= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Mixture"  );
		m_FacilityMutated		= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Mutated"  );
		m_FacilityUnique		= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Unique"  );
		m_FacilityDice			= Resources.LoadAll<Sprite>( "Art/Masked/FacilityColors/Dice" );
		m_AccessoriesMasked		= Resources.LoadAll<Sprite>( "Art/Masked/Accessories/Masked" );
		m_AccessoriesDice		= Resources.LoadAll<Sprite>( "Art/Masked/Accessories/Dice" );
	}

	private void Start()
	{
		DayManager.Instance.DayStartEvent += GenerateMasks;
	}



	public void GenerateMasks()
	{
		m_MaskedInfos.Clear();

		// Fill entire list with regular masks
		while ( m_MaskedInfos.Count < DayManager.Instance.GetCurrentDay().m_AmountTotalMasks )
		{
			MaskedInfo NewMaskedInfo = ScriptableObject.CreateInstance<MaskedInfo>(); // This method is used rather than typing "new" since this is how Unity suggests it

			int MaskDecider = Random.Range( 0, 2 );

			if ( MaskDecider == 0 )
			{
				NewMaskedInfo.m_Value				= 1;
				NewMaskedInfo.m_IntentedFacility	= Masked.EFacility.Plantation;
				NewMaskedInfo.m_BaseSprite			= m_MaskedBases[ Random.Range(0, m_MaskedBases.Length) ];
				NewMaskedInfo.m_FacilitySprite		= m_FacilityPlantation[ Random.Range(0, m_FacilityPlantation.Length) ];
				NewMaskedInfo.m_AccessorySprite		= m_AccessoriesMasked[ Random.Range(0, m_AccessoriesMasked.Length) ];
			}
			else
			{
				NewMaskedInfo.m_Value				= 1;
				NewMaskedInfo.m_IntentedFacility	= Masked.EFacility.Chamber;
				NewMaskedInfo.m_BaseSprite			= m_MaskedBases[ Random.Range(0, m_MaskedBases.Length) ];
				NewMaskedInfo.m_FacilitySprite		= m_FacilityChamber[ Random.Range(0, m_FacilityChamber.Length) ];
				NewMaskedInfo.m_AccessorySprite		= m_AccessoriesMasked[ Random.Range(0, m_AccessoriesMasked.Length) ];
			}

			m_MaskedInfos.Add( NewMaskedInfo );
		}

		// Add special masks
		//foreach ( SpecialMasked CurrentSpecial in m_Days[ m_CurrentRound ].m_SpecialMasks )
		//{
		//	if ( CurrentSpecial.m_IntendedLocation > -1 )
		//	{
		//		if ( m_MaskedInfos[ CurrentSpecial.m_IntendedLocation ].m_Value > 1 )
		//		{
		//			Debug.LogError( $"On round { m_CurrentRound } there are multiple special masks with the same IntendedLocation. Randomizing position." );
		//		}
		//		else
		//		{
		//			m_MaskedInfos[ CurrentSpecial.m_IntendedLocation ] = CurrentSpecial.m_MaskedInfo;
		//		}
		//	}
		//	else // IntendedLocation was left at -1, so randomize position in list
		//	{

		//	}
		//}

		SendNewMask();
	}



	public void SendNewMask()
	{
		if ( m_MaskedInfos.Count == 0 )
		{
			// TODO:: End the day here.
			DayManager.Instance.EndDay();
			ButtonManager.Instance.EnableShutterButton();
			return;
		}

		m_Masked.SetMaskedInfo( m_MaskedInfos[ m_MaskedInfos.Count - 1 ] );
		m_Masked.SetTravelDestination( Masked.EFacility.Office );

		m_MaskedInfos.RemoveAt( m_MaskedInfos.Count - 1 );

		// Removing from the back is way faster, so in GenerateMasks we reverse the order of everything to make it line up.
	}
}
