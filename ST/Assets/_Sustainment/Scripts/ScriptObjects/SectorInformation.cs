using UnityEngine;

[CreateAssetMenu(fileName = "Sector Info", menuName = "ScriptableObjects/SectorInformation")]
public class SectorInformation : ScriptableObject
{
    public SectorType SectorType;
    public Vector3 SectorBasePosition;
    public Vector3 SectorHallPosition;
}
