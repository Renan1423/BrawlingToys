using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private Transform _modelParentTrans;
    public GameObject ChildModel { get; private set; }

    public RenderTexture SetupRenderTextureCamera()
    {
        RenderTexture rendTex = new RenderTexture(512, 512, 16);
        _cam.targetTexture = rendTex;

        return rendTex;
    }

    public Transform GetModelParentTransform() => _modelParentTrans;

    public void SetChildModel(GameObject model) 
    {
        ChildModel = model;
    }
}
