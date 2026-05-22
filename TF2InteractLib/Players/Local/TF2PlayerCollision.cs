using System.Numerics;

namespace TF2InteractLib.Players;

public class TF2PlayerCollision
{
    public Vector3 m_vecMins { get; set; }
    public Vector3 m_vecMaxs { get; set; }
    public Vector3 m_vecMinsPreScaled { get; set; }
    public Vector3 m_vecMaxsPreScaled { get; set; }
    public short m_usSolidFlags { get; set; }
    public bool m_bUniformTriggerBloat { get; set; }
}