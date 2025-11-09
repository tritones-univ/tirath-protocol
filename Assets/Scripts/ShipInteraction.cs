using UnityEngine;

public class ShipInteraction : MonoBehaviour
{
    public GameObject shipPanel;
    private bool playerInRange = false;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        shipPanel.SetActive(false);
    }

    void TogglePanel()
    {
        shipPanel.SetActive(!shipPanel.activeSelf);
        UIManager.Instance.HUDPanel.SetActive(!shipPanel.activeSelf);
    }

    private void OnInsteract()
    {
        if (playerInRange)
            TogglePanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.onInteract.AddListener(OnInsteract);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            shipPanel.SetActive(false);
            if (playerInteraction != null)
                playerInteraction.onInteract.RemoveListener(OnInsteract);

            playerInteraction = null;
        }
    }
}
