using System.Numerics;

namespace TF2InteractLib.Players;

public class TF2LocalPlayer
{
    public TF2SharedPlayer m_Shared { get; } = new();
    public TF2ClientPlayer m_Local { get; } = new();
    public TF2PlayerCollision m_Collision { get; } = new();
    
    public int m_nSkin { get; set; }
    public float m_flCycle { get; set; }
    public int m_nBody { get; set; }
    public float[] m_flEncodedController { get; set; } = new float[4];
    public int m_nSequence { get; set; }
    public float m_flPlaybackRate { get; set; }
    public int m_nResetEventsParity { get; set; }
    public int m_nNewSequenceParity { get; set; }
    public float m_flTauntYaw { get; set; }
    public float m_flInspectTime { get; set; }
    public float m_flCurrentTauntMoveSpeed { get; set; }
    public float m_flHelpmeButtonPressTime { get; set; }
    public float m_flVehicleReverseTime { get; set; }
    public int m_iFOV { get; set; }
    public float m_flFOVTime { get; set; }
    public int m_iFOVStart { get; set; }
    public float m_flMaxspeed { get; set; }
    public int m_iHealth { get; set; }
    public int m_nNextThinkTick { get; set; }
    public int m_iBonusProgress { get; set; }
    public int m_iBonusChallenge { get; set; }
    public bool m_fOnTarget { get; set; }
    public Vector3 m_vecBaseVelocity { get; set; }
    public int m_nButtons { get; set; }
    public float m_flWaterJumpTime { get; set; }
    public int m_nImpulse { get; set; }
    public int m_afButtonLast { get; set; }
    public float m_flStepSoundTime { get; set; }
    public int m_afButtonPressed { get; set; }
    public float m_flSwimSoundTime { get; set; }
    public int m_afButtonReleased { get; set; }
    public Vector3 m_vecLadderNormal { get; set; }
    public int m_nTickBase { get; set; }
    public int m_flPhysics { get; set; }
    public float m_surfaceFriction { get; set; }
    public int[] m_iAmmo { get; set; } = new int[32];
    public float m_flNextAttack { get; set; }
    public int m_nPrevSequence { get; set; }
    public TF2Team m_iTeamNum { get; set; }
    public Vector3 m_vecNetworkOrigin { get; set; }
    public Vector3 m_angNetworkAngles { get; set; }
    public Vector3 m_vecAbsOrigin { get; set; }
    public Vector3 m_angAbsRotation { get; set; }
    public Vector3 m_vecAbsVelocity { get; set; }
    public Vector3 m_vecOrigin { get; set; }
    public Vector3 m_vecVelocity { get; set; }
    public Vector3 m_angRotation { get; set; }
    public int m_fFlags { get; set; }
    public Vector3 m_vecAngVelocity { get; set; }
    public Vector3 m_vecViewOffset { get; set; }
    public bool m_bDormant { get; set; }
    public short m_nModelIndex { get; set; }
    public float m_flFriction { get; set; }
    public int m_iEFlags { get; set; }
    public float m_flGravity { get; set; }
    public float m_flProxyRandomValue { get; set; }
}