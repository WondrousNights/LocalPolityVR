using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sector Info", menuName = "ScriptableObjects/SectorList")]
public class SectorList : ScriptableObject
{
    public List<SectorInformation> Sectors;
}
