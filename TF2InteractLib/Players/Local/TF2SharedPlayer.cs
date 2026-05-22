namespace TF2InteractLib.Players;

public class TF2SharedPlayer
{
    public bool m_bRageDraining { get; set; }
    public int m_nPlayerState { get; set; }
    public float m_flNextRageEarnTime { get; set; }
    public int m_nPlayerCond { get; set; }
    public float m_flEnergyDrinkMeter { get; set; }
    public float m_flCloakMeter { get; set; }
    public float m_flHypeMeter { get; set; }
    public float m_flRageMeter { get; set; }
    public float m_flChargeMeter { get; set; }
    public float m_flDuckTimer  { get; set; }
    public bool m_bJumping { get; set; }
    public float m_flInvisChangeCompleteTime { get; set; }
    public int m_iAirDash { get; set; }
    public float m_flLastStealthExposeTime { get; set; }
    public int m_nAirDucked { get; set; }
    public float m_flStealthNextChangeTime { get; set; }
    public TF2Team m_nDisguiseTeam { get; set; }
    public int m_nDisguiseClass { get; set; }
    public int m_nDisguiseSkinOverride { get; set; }
    public int m_nMaskClass { get; set; }
    public TF2Team m_nDesiredDisguiseTeam { get; set; }
    public float m_flDisguiseCompleteTime { get; set; }
    public bool m_bLastDisguisedAsOwnTeam { get; set; }
    public bool m_bHasPasstimeBall { get; set; }
    public bool m_bFeignDeathReady { get; set; }
    public bool m_bIsTargetedForPasstimePass { get; set; } 
    public int m_nPlayerCondEx { get; set; }
    public float m_askForBallTime { get; set; }
    public int m_nPlayerCondEx2 { get; set; }
    public float m_flHolsterAnimTime { get; set; }
    public int m_nPlayerCondEx3 { get; set; }
    public float[] m_flItemChargeMeter { get; set; } = new float[10];
    public int m_iStunIndex { get; set; }
    public bool m_bScattergunJump { get; set; }
}