using System;
using Unity.Netcode;
using System.Collections;
using UnityEngine;
public class Day_NightCycle : NetworkBehaviour
{
    public enum TimeState { Morning, Day, Night};
    public NetworkVariable<TimeState> currentTimeState = new NetworkVariable<TimeState>();

    public float DayTime;

    public override void OnNetworkSpawn()
    {
        currentTimeState.Value = TimeState.Morning;
        base.OnNetworkSpawn();
    }

    void Start()
    {
        currentTimeState.OnValueChanged += OnStateChanged;
    }

    [Rpc(SendTo.Server)]
    public void ChangeStateRpc()
    {
        if(currentTimeState.Value == TimeState.Morning)
        {
            currentTimeState.Value = TimeState.Day;
        }
        else if(currentTimeState.Value == TimeState.Day)
        {
            currentTimeState.Value = TimeState.Night;
        }
        else if(currentTimeState.Value == TimeState.Night)
        {
            currentTimeState.Value = TimeState.Morning;
        }
    }

    public void OnStateChanged(TimeState oldValue, TimeState newValue)
    {
        Debug.Log($"Current time state changed from {oldValue} to {newValue}");
    }

  
}
