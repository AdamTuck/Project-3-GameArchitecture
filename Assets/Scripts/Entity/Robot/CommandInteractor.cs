using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommandInteractor : Interact
{
    Queue<Command> commands = new Queue<Command>();
    Queue<GameObject> pointerSpots = new Queue<GameObject>();

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject pointerPrefab;
    [SerializeField] private Camera cam;

    private Command currentCommand;
    private GameObject currentPointer;

    public override void Interaction()
    {
        if (playerInput.commandPressed)
        {
            Debug.Log("Commanding robot");
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));

            if (Physics.Raycast(ray, out var hitInfo))
            {
                if (hitInfo.transform.CompareTag("Ground"))
                {
                    GameObject pointer = Instantiate(pointerPrefab);
                    pointer.transform.position = hitInfo.point;
                    pointerSpots.Enqueue(pointer);

                    commands.Enqueue(new MoveCommand(agent, hitInfo.point));
                }
                else if (hitInfo.transform.CompareTag("Builder"))
                {
                    commands.Enqueue(new BuildCommand(agent, hitInfo.transform.GetComponent<Builder>()));
                }
            }
        }

        ProcessCommand();
    }

    void ProcessCommand ()
    {
        if (currentCommand != null && !currentCommand.IsComplete)
            return;

        if (commands.Count == 0)
            return;

        Destroy(currentPointer);

        currentCommand = commands.Dequeue();
        currentPointer = pointerSpots.Dequeue();

        currentCommand.Execute();
    }
}