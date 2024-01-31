using UnityEngine;

public class WindMovement : MonoBehaviour
{
    private bool canCollide = true;

    public bool CanCollide 
    { 
        get { return canCollide; } 
        set { canCollide = value; } 
    }
}
