using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject HUDPanel;
    public GameObject inventoryPanel;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public InventoryUI inventoryUI;

    [Header("Pause Menu Buttons")]
    public Button[] pauseButtons;
    private int selectedButtonIndex = 0;
    private Button selectedButton;
    public bool IsHUDOpen => HUDPanel.activeSelf;
    public bool IsInventoryOpen => inventoryPanel.activeSelf;
    public bool IsPauseOpen => pausePanel.activeSelf;
    public bool IsOptionsOpen => optionsPanel.activeSelf;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        TooltipUI.Instance.Hide();
        ShowHUD();
        HideInventory();
        HidePauseMenu();
        HideOptions();
        HighlightButton(selectedButtonIndex);
    }

    #region Panel Show/Hide
    public void ShowHUD()
    {
        TooltipUI.Instance.Hide();
        HUDPanel.SetActive(true);
    }
    public void HideHUD()
    {
        TooltipUI.Instance.Hide();
        HUDPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        TooltipUI.Instance.Hide();
        inventoryPanel.SetActive(true);
        inventoryUI.RefreshUI();
    }
    public void HideInventory()
    {
        TooltipUI.Instance.Hide();
        inventoryPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        TooltipUI.Instance.Hide();
        pausePanel.SetActive(true);
        selectedButton = null;
        HighlightButton(selectedButtonIndex);
    }
    public void HidePauseMenu()
    {
        TooltipUI.Instance.Hide();
        pausePanel.SetActive(false);
    }

    public void ShowOptions()
    {
        TooltipUI.Instance.Hide();
        optionsPanel.SetActive(true);
    }
    public void HideOptions()
    {
        TooltipUI.Instance.Hide();
        optionsPanel.SetActive(false);
    }
    #endregion

    #region Input Handling
    public void HighlightButton(int index)
    {
        if (pauseButtons.Length == 0) return;

        if (selectedButton != null)
        {
            var oldColors = selectedButton.colors;
            oldColors.normalColor = Color.white;
            selectedButton.colors = oldColors;
        }
        selectedButtonIndex = index;
        selectedButton = pauseButtons[selectedButtonIndex];

        var newColors = selectedButton.colors;
        newColors.normalColor = Color.yellow;
        selectedButton.colors = newColors;
    }

    public void HandleE(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsHUDOpen)
        {
            HideHUD();
            ShowInventory();
        }
        else if (IsInventoryOpen)
        {
            HideInventory();
            ShowHUD();
        }
    }
    private void HandleEscape()
    {
        BuildingSystem.current.CancelPlacement();
    }
    public void HandleEscape(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsHUDOpen && BuildingSystem.current.objectToPlace != null)
        {
            HandleEscape();
            return;
        }
        if (IsPauseOpen && IsOptionsOpen)
        {
            HideOptions();
        }
        else if (IsPauseOpen && !IsOptionsOpen)
        {
            HidePauseMenu();
            ShowHUD();
        }
        else if (IsHUDOpen)
        {
            HideHUD();
            ShowPauseMenu();
        }
        else if (IsInventoryOpen)
        {
            HideInventory();
            ShowHUD();
        }
    }
    public void HandleUp(InputAction.CallbackContext context)
    {
        if (!context.performed || !IsPauseOpen) return;
        int newIndex = selectedButtonIndex - 1;
        if (newIndex < 0) newIndex = pauseButtons.Length - 1;
        HighlightButton(newIndex);
    }
    public void HandleDown(InputAction.CallbackContext context)
    {
        if (!context.performed || !IsPauseOpen) return;
        int newIndex = selectedButtonIndex + 1;
        if (newIndex >= pauseButtons.Length) newIndex = 0;
        HighlightButton(newIndex);
    }
    public void HandleSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (IsPauseOpen && !IsOptionsOpen) PressCurrentButton();
    }
    public void PressCurrentButton()
    {
        if (selectedButton == null) return;
        selectedButton.onClick.Invoke();
    }
    #endregion
    #region Button Managment
    public void ResumeGame()
    {
        if (IsOptionsOpen)
        {
            HideOptions();
        }
        if (IsPauseOpen)
        {
            HidePauseMenu();
        }
        ShowHUD();
    }
    #endregion

}
