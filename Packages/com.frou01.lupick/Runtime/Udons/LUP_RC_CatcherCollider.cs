
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]

public class LUP_RC_CatcherCollider : UdonSharpBehaviour
{
    public int ID;
    public bool isHook;//Drop on entering
    public bool isSyncOwner;
    public Transform dropTarget;
    [SerializeField] public bool Tags_ExcludeExceptMode;
    [SerializeField] public string[] ExceptPickupTags = new string[0];
    [SerializeField] public string[] CatcherTags = new string[0];

    DataList CatchedPickups = new DataList();

    public virtual bool validationPickup(LUPickUpRC_RootChangeable pickup)
    {
        return true;
    }

    public virtual void PickupEnter(LUPickUpRC_RootChangeable pickup)
    {
        CatchedPickups.Add(pickup);
    }
    public virtual void PickupExit(LUPickUpRC_RootChangeable pickup)
    {
        CatchedPickups.Remove(pickup);
    }
}
