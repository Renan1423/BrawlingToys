using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseBox : MonoBehaviour
{
    private BuffDebuffTestScriptable _buffDebuffInsideBox;

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private Transform _drawnPartContainerTrans;
    [SerializeField]
    private GameObject _surpriseBoxModel;

    public void SetupSurpriseBox(BuffDebuffTestScriptable buffDebuffInsideBox) 
    {
        _buffDebuffInsideBox = buffDebuffInsideBox;

        Renderer modelMesh = _surpriseBoxModel.GetComponent<Renderer>();
        Color newMatColor = (buffDebuffInsideBox.EffectType == EffectType.Buff) ? Color.green : Color.red;
        modelMesh.material.color = newMatColor;
    }

    public void OpenSurpriseBox() 
    {
        _anim.SetTrigger("Open");
        ShowDrawnPart();
    }

    public void ShowDrawnPart() 
    {
        GameObject drawnPart = Instantiate(_buffDebuffInsideBox.SkinPartPrefab, _drawnPartContainerTrans);
        drawnPart.transform.localScale = Vector3.one;
        drawnPart.transform.localPosition = Vector3.zero;
        drawnPart.layer = gameObject.layer;
        for (int i = 0; i < drawnPart.transform.childCount; i++)
        {
            drawnPart.transform.GetChild(i).gameObject.layer = gameObject.layer;
        }

        _surpriseBoxModel.SetActive(false);
    }

    public BuffDebuffTestScriptable GetBuffDebuffInsideBox() { return _buffDebuffInsideBox; }
}
