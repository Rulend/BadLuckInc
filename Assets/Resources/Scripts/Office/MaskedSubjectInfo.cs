using UnityEngine;

[ CreateAssetMenu( fileName = "SpecialSubject", menuName = "MaskedSubject", order = 0 ) ]
public class MaskedSubjectInfo : ScriptableObject
{
	// In C++ this would be more fitting as a struct, but structs work differently in C#
	public MaskedSubject.ESubjectWorth	m_Value;
	public int							m_StressLevel;
	public int							m_ThreatLevel;
	public MaskedSubject.EFacility		m_IntentedFacility;
	public Sprite						m_BaseSprite;
	public Sprite						m_FacilitySprite;
	public Sprite						m_AccessorySprite;
}
