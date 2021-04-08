using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour {
    public MeshRenderer mr;
    public Material defaultMat;
    public Material invisibleMat;

    public void MakeInvisible() {
        mr.material = invisibleMat;
    }

    public void MakeVisible() {
        mr.material = defaultMat;
    }
}
