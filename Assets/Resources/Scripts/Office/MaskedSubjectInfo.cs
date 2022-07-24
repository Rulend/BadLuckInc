using UnityEngine;

[ CreateAssetMenu( fileName = "RegularSubject", menuName = "MaskedSubject", order = 0 ) ]
public class MaskedSubjectInfo : ScriptableObject
{
	public MaskedSubject.ESubjectWorth	m_Value;
	public int							m_StressLevel;
	public int							m_ThreatLevel;
	public MaskedSubject.EFacility		m_IntentedFacility;
	public Sprite						m_BaseSprite;
	public Sprite						m_FacilitySprite;
	public Sprite						m_AccessorySprite;
}
