using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BrawlingToys.Actors;
using System;
using BrawlingToys.UI;

public class SurpriseBoxUI : BaseScreen
{
    private SurpriseBox _currentSurpriseBox;

    [Header("UI components")]
    [SerializeField]
    private TextMeshProUGUI _titleText;
    [SerializeField]
    private TextMeshProUGUI _partNameText;
    [SerializeField]
    private TextMeshProUGUI _partDescriptionText;
    [SerializeField]
    private Image _partIconImage;
    [SerializeField]
    private GameObject _openBoxButton;
    [SerializeField]
    private GameObject _equipBoxButton;
    
    [Space(10)]

    [Header("Animations")]
    [SerializeField]
    private Animator _anim;

    [Space(10)]

    [Header("Screens References")]
    [SerializeField]
    private EffectsSelectionScreen _equipEffectsSelectionScreen;

    protected override void OnScreenEnable()
    {
        ResetSurpriseBoxUi();
    }

    public void ResetSurpriseBoxUi() 
    {
        _anim.SetTrigger("Neutral");
        _titleText.gameObject.SetActive(true);
        _partNameText.text = "";
        _partDescriptionText.text = "";
        _partIconImage.gameObject.SetActive(false);
        _openBoxButton.SetActive(false);
        _equipBoxButton.SetActive(false);
    }

    public void SetCurrentSurpriseBox(SurpriseBox surpriseBox) { _currentSurpriseBox = surpriseBox; }

    [Tooltip("Deve ser chamado quando o player apertar o bot�o de confirmar")]
    public void OpenCurrentSurpriseBox() 
    {
        _openBoxButton.SetActive(false);
        _currentSurpriseBox.OpenSurpriseBox();

        ShowDrawnPartInfo();
    }

    public void ShowDrawnPartInfo() 
    {
        ModifierScriptable drawnPart = _currentSurpriseBox.GetBuffDebuffInsideBox();

        _partNameText.text = drawnPart.EffectName;
        _partDescriptionText.text = drawnPart.EffectDescription;
        _partIconImage.sprite = drawnPart.EffectIcon;

        StartCoroutine(StartShowPartInfoAnimation());
    }

    private IEnumerator StartShowPartInfoAnimation()
    {
        yield return new WaitForSeconds(2.25f);

        _anim.SetTrigger("ShowPartInfo");
        _equipBoxButton.SetActive(true);
    }

    public void OpenEquipPartScreen() 
    {
        _equipEffectsSelectionScreen.SetDrawnEffect(_currentSurpriseBox.GetBuffDebuffInsideBox());

        _equipEffectsSelectionScreen.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void EnableOpenButton()
    {
        StartCoroutine(ToggleObjectWithDelay(_openBoxButton, true, 1.5f));
    }

    private IEnumerator ToggleObjectWithDelay(GameObject go, bool result, float delayTime) 
    {
        yield return new WaitForSeconds(delayTime);

        go.SetActive(result);
    }
}
