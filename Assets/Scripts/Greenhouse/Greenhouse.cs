using UnityEngine;

public class Greenhouse : MonoBehaviour
{
    public GameObject greenhousePanel;
    private bool playerInRange = false;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        greenhousePanel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInteraction = other.GetComponent<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.onInteract.AddListener(OnInteract);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            greenhousePanel.SetActive(false);

            if (playerInteraction != null)
                playerInteraction.onInteract.RemoveListener(OnInteract);

            playerInteraction = null;
        }
    }

    private void OnInteract()
    {
        if (playerInRange)
            TogglePanel();
    }

    private void TogglePanel()
    {
        greenhousePanel.SetActive(!greenhousePanel.activeSelf);
    }
}
