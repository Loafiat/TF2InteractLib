using System.Numerics;

namespace TF2InteractLib.Players;

public class TF2ClientPlayer
{
    public int m_nStepside { get; set; }
    public bool m_bPrevForceLocalPlayerDraw { get; set; }
    public Vector3 m_vecPunchAngleVel { get; set; }
    public int m_iHideHUD { get; set; }
    public bool m_bDrawViewmodel { get; set; }
    public Vector3 m_vecPunchAngle { get; set; }
    public bool m_bWearingSuit { get; set; }
    public bool m_bPoisoned { get; set; }
    public bool m_bForceLocalPlayerDraw { get; set; }
    public float m_flFallVelocity { get; set; }
    public bool m_bAllowAutoMovement { get; set; }
    public int m_nOldButtons { get; set; }
    public bool m_bDucked { get; set; }
    public float m_flOldForwardMove { get; set; }
    public bool m_bDucking { get; set; }
    public float m_flStepSize { get; set; }
    public bool m_bInDuckJump { get; set; }
    public float m_flFOVRate { get; set; }
    public float m_flDucktime { get; set; }
    public float m_flDuckJumpTime { get; set; }
    public float m_flJumpTime { get; set; }
}