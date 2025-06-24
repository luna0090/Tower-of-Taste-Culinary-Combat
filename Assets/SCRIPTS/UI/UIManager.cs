using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject spellButton;
    public GameObject dishButton;
    public GameObject healthBarMask;
    public GameObject inventoryModal;
    public UIControls controls;

    private bool openInventoryPressed;
    public void closeInventoryModal()
    {
        inventoryModal.SetActive(false);
    }

    public void setHealthBar(float health)
    {
        LeanTween.value(healthBarMask, healthBarMask.GetComponent<Image>().fillAmount, 1 - health, 0.5f)
        .setOnUpdate((float val) =>
        {
            healthBarMask.GetComponent<Image>().fillAmount = val;
        });
    }

    public void setSpellButtonCooldown(float dur)
    {
        setHealthBar(healthBarMask.GetComponent<Image>().fillAmount - 0.2f); //temp

        spellButton.GetComponent<Button>().interactable = false;
        spellButton.GetComponent<Image>().fillAmount = 0;
        LeanTween.value(spellButton, 0, 1, dur)
        .setOnUpdate((float val) =>
        {
            spellButton.GetComponent<Image>().fillAmount = val;
        })
        .setOnComplete(() =>
        {
            spellButton.GetComponent<Button>().interactable = true;
        });
    }

    public void setDishButtonCooldown(float dur)
    {
        dishButton.GetComponent<Button>().interactable = false;
        dishButton.GetComponent<Image>().fillAmount = 0;
        LeanTween.value(dishButton, 0, 1, dur)
        .setOnUpdate((float val) =>
        {
            dishButton.GetComponent<Image>().fillAmount = val;
        })
        .setOnComplete(()=>
        {
            dishButton.GetComponent<Button>().interactable = true;
        });
    }

    private void Awake()
    {
        controls = new UIControls();
        controls.ClimbModeGameplay.OpenInventory.performed += ctx => openInventoryPressed = true;
        controls.ClimbModeGameplay.OpenInventory.canceled += ctx => openInventoryPressed = false;
    }

    private void OnEnable()
    {
        controls.ClimbModeGameplay.Enable();
    }

    private void OnDisable()
    {
        controls.ClimbModeGameplay.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (openInventoryPressed) inventoryModal.SetActive(true);
        openInventoryPressed = false;
    }
}
