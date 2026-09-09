using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

enum ToolTipType
{
    Persistent, Vanishing, Destructive
}
[RequireComponent(typeof(BoxCollider2D))]
public class ToolTipController : MonoBehaviour
{
    // TODO: Fix the getcomponent in children
    public bool UsingImages { get { return usingImages; }  }

    [SerializeField] ToolTipType toolTipType;
    [SerializeField] GameObject tooltipTextObject;
    [SerializeField] bool startsDisabled = false;

    [SerializeField] string keyboardText;
    [SerializeField] string controllerText;
    [SerializeField] bool usingImages;

    [SerializeField] Sprite keyboardTextAsImage;
    [SerializeField] Sprite controllerTextAsImage;
    TextMeshPro tooltipText;
    SpriteRenderer tooltipImage;
    bool isKeyboardAndMouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        tooltipText = tooltipTextObject.GetComponent<TextMeshPro>();
        tooltipImage = tooltipTextObject.GetComponent<SpriteRenderer>();
        if (tooltipImage) usingImages = true;
        PlayerInputManager.InputDeviceChanged.AddListener(UpdateInputDevice);
    }
    private void OnDisable()
    {
        PlayerInputManager.InputDeviceChanged.RemoveListener(UpdateInputDevice);
    }
    private void UpdateInputDevice(InputDevice newDevice)
    {
        if(isKeyboardAndMouse == PlayerInputManager.UsingKeyboardAndMouse) return;
        isKeyboardAndMouse = PlayerInputManager.UsingKeyboardAndMouse;
        if (usingImages)
        {
            if (isKeyboardAndMouse && tooltipImage.sprite != keyboardTextAsImage) tooltipImage.sprite = keyboardTextAsImage;
            if (!isKeyboardAndMouse && tooltipImage.sprite != controllerTextAsImage) tooltipImage.sprite = controllerTextAsImage;
        }
        else
        {
            if (isKeyboardAndMouse && tooltipText.text != keyboardText) tooltipText.text = keyboardText;
            if (!isKeyboardAndMouse && tooltipText.text != controllerText) tooltipText.text = controllerText;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (toolTipType == ToolTipType.Vanishing || startsDisabled)
        {
            tooltipTextObject.SetActive(true);
            startsDisabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (toolTipType == ToolTipType.Vanishing || toolTipType == ToolTipType.Destructive)
        {
            tooltipTextObject.SetActive(false);
        }
    }
}
