using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterMovements;

public class OilSurface : MonoBehaviour, ISurface
{
    public SurfaceType GetSurfaceType()
    {
        return SurfaceType.Oil;
    }
}
