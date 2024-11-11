using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Estados 
{
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
        ReceivePayment
    }

    public enum table
    {
        Thinking,
        toOrder,
        Waiting,
        toDeliver,
        Eating,
        toCollect
    }

    public enum food
    {
        cooking,
        Ready,
        Empty
    }
}
