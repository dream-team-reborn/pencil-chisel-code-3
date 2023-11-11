using System.Collections;
using System.Collections.Generic;
using CharacterMovements;
using UnityEngine;

public class GroundSurface : MonoBehaviour, ISurface
{
    public SurfaceType GetSurfaceType()
    {
        return SurfaceType.Ground;
    }
}
