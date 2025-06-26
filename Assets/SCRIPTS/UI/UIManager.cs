using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.InteropServices;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject spellButton;
    public GameObject dishButton;
    public GameObject healthBarMask;
    public GameObject mainModal;
    public GameObject tabsHolder;
    public UIControls controls;

    private bool openInventoryPressed;
    public void closeModal()
    {
        mainModal.SetActive(false);
    }

    public void showPage(GameObject which)
    {
        foreach (Transform page in mainModal.transform)
        {
            if (page.name.Contains("Page"))
            {
                page.gameObject.SetActive(false);
            }
        }

        which.SetActive(true);
    }

    public void setActiveTab(GameObject which)
    {
        /*foreach (Transform tab in tabsHolder.transform)
        {
            tab.GetComponent<Image>().color = Color.white;
            tab.GetComponentInChildren<TMP_Text>().color = Color.black;
        }

        which.GetComponent<Image>().color = Color.black;
        which.GetComponentInChildren<TMP_Text>().color = Color.white;*/
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
        if (openInventoryPressed)
        {
            foreach (Transform page in mainModal.transform)
            {
                if (page.name.Contains("Page1"))
                {
                    showPage(page.gameObject);
                }
            }

            setActiveTab(tabsHolder.transform.GetChild(0).gameObject);

            mainModal.SetActive(true);
        }
        openInventoryPressed = false;
    }
}
