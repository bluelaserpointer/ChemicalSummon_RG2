using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldEventObject : AbstractWorldEventObject
{
    [SerializeField]
    Event progressEvent;

    //data
    Event generatedEvent;

    protected override void DoEvent()
    {
        if (generatedEvent != null)
            return;
        generatedEvent = PlayerSave.StartEvent(progressEvent);
        generatedEvent.OnEventFinish.AddListener(() =>
        {
            WorldManager.Player.OccupyingMovementEventObject = null;
            generatedEvent = null;
        });
        WorldManager.Player.OccupyingMovementEventObject = this;
    }
    public override void Submit()
    {
        generatedEvent.Progress();
    }
}
