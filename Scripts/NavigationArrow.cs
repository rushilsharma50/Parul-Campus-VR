using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    public StreetViewNode currentNode; // Where we are now
    public StreetViewNode targetNode;  // Where this arrow leads

    void OnMouseDown()
    {
        // 1. Turn off the old spot
        currentNode.OnExitNode();

        // 2. Turn on the new spot
        targetNode.OnEnterNode();
    }
}