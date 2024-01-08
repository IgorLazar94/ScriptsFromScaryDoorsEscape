using UnityEngine;

public enum TypeOfElevator
{
    Simple,
    VIP
}
public class ElevatorController : MonoBehaviour
{
    [SerializeField] private TypeOfElevator typeOfElevator;
    private Animator doorAnimator;
    private BoxCollider elevatorTriggerCollider;

    private void Start()
    {
        doorAnimator = GetComponentInChildren<Animator>();
        elevatorTriggerCollider = GetComponent<BoxCollider>();
    }

    public void OpenDoorAnimation()
    {
        doorAnimator.SetBool(StringCollection.Instance.isOpen, true);
    }

    public void SimulateMoveElevator()
    {
        Debug.Log("move elevator"); // звук лифта, ещё чего?
    }

    public void DestroyElevator()
    {
        Destroy(this.gameObject);
    }

    public void DisableTriggerZone()
    {
        elevatorTriggerCollider.enabled = false;
    }

    public TypeOfElevator GetTypeOfElevator()
    {
        return typeOfElevator;
    }
}
