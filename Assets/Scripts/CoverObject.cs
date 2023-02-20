using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum CoverType
{
    None,
    Half,
    Full
}

public class CoverObject : MonoBehaviour
{

    [SerializeField] private CoverType coverType;

    public CoverType GetCoverType()
    {
        return coverType;
    }





}