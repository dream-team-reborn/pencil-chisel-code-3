using System.Collections;
using System.Collections.Generic;
using CharacterMovements;
using UnityEngine;

public class SlidingSurface : MonoBehaviour, ISurface
{
    public SurfaceType GetSurfaceType()
    {
        return SurfaceType.Sliding;
    }
}
