using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Estados 
{
    public enum selection
    {
        toAttendTo,
        toDeliver,
        toReDeliver,
        Nothing
    }
    public enum customer
    {
        Waiting,
        Entering,
        Sitting,
        leaving
    }

    public enum waiter
    {
        Walked,
        Waiting,
        Attending,
        TakingOrder,
        Delivering,
        incorrectDelivery,
        ReceivePayment
    }

    public enum table
    {
        Thinking,
        toOrder,
        Waiting,
        Eating,
        toCollect
    }

    public enum foodInKitchen
    {
        cooking,
        Ready,
        Delivering,
        Delivered,
        Discarded
    }

    public enum tableFood
    {
        NotYetDelivered,
        Ontable,
        Done
    }
}
