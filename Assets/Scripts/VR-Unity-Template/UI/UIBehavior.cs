using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _exampleSlider;
    [SerializeField] private TextMeshProUGUI _exampleSliderText;

    private void Start()
    {
        // Exit Button Functionality
        GiveFunctionalityToTheExitButton();

        // Example Slider Functionality
        GiveFunctionalityToTheSlider();
    }

    private void GiveFunctionalityToTheExitButton()
    {
        _exitButton.onClick.RemoveAllListeners();   // We wipe the current listeners

        _exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void GiveFunctionalityToTheSlider()
    {
        _exampleSlider.onValueChanged.RemoveAllListeners();   // We wipe the current listeners

        _exampleSlider.maxValue = 5;
        _exampleSlider.minValue = 0;
        _exampleSlider.value = 1;
        _exampleSlider.wholeNumbers = true;

        _exampleSlider.onValueChanged.AddListener((float value) =>
        {
            _exampleSliderText.text = ((int)value).ToString();
        });
    }
}
