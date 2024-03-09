using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private LevelManager endingLevel;
    [SerializeField] private DoorController entryDoor;
    [SerializeField] private string levelTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (entryDoor)
            {
                entryDoor.OpenDoor(false);
                entryDoor.LockDoor();
            }

            if (endingLevel != null)
            {
                endingLevel.EndLevel();
                Debug.Log("Trigger: Ending Level: " + endingLevel.gameObject.name);
            }

            if (levelTutorial != null && levelTutorial != "")
            {
                UIManager.instance.ShowTutorial(levelTutorial);
            }
            
            Destroy(gameObject);
        }
    }
}