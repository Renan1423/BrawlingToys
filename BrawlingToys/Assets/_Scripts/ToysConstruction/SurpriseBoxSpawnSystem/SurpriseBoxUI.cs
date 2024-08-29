using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurpriseBoxUI : MonoBehaviour
{
    private SurpriseBox _currentSurpriseBox;

    [Header("UI components")]
    [SerializeField]
    private TextMeshProUGUI _partNameText;
    [SerializeField]
    private TextMeshProUGUI _partDescriptionText;
    [SerializeField]
    private GameObject _openBoxButton;

    [Header("Animations")]
    [SerializeField]
    private Animator _anim;

    private void OnEnable()
    {
        ClearSurpriseBoxUi();
    }

    public void ClearSurpriseBoxUi() 
    {
        _partNameText.text = "";
        _partDescriptionText.text = "";
    }

    public void SetCurrentSurpriseBox(SurpriseBox surpriseBox) { _currentSurpriseBox = surpriseBox; }

    [Tooltip("Deve ser chamado quando o player apertar o botão de confirmar")]
    public void OpenCurrentSurpriseBox() 
    {
        _openBoxButton.SetActive(false);
        _currentSurpriseBox.OpenSurpriseBox();

        ShowDrawnPartDescription();
        StartCoroutine(ShowDescription());
    }

    private IEnumerator ShowDescription() 
    {
        yield return new WaitForSeconds(1f);

        _anim.SetTrigger("ShowDescription");
    } 

    public void ShowDrawnPartDescription() 
    {
        BuffDebuffTestScriptable drawnPart = _currentSurpriseBox.GetBuffDebuffInsideBox();

        _partNameText.text = drawnPart.PartName;
        _partDescriptionText.text = drawnPart.PartDescription;
    }
}
