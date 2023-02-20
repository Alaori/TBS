using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Action onInteractComplete;
    private Animator animator;
    private float timer;
    private bool isActive;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition, this);
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            isActive = false;
            onInteractComplete();
        }
    }
    public bool DoorIsOpen()
    {
        return isOpen;
    }
    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = .5f;

        if (isOpen)
        {
            CloseDoor();

        }
        else
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);

        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
