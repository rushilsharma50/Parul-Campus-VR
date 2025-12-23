using UnityEngine;
using System.Collections.Generic;

public class StreetViewManager : MonoBehaviour
{
    public LocationNode startingNode;
    private LocationNode currentNode;

    void Start()
    {
        // FLAW FIX: Aggressively find ALL nodes and force them to hide
        LocationNode[] allNodes = FindObjectsOfType<LocationNode>();
        
        foreach(LocationNode node in allNodes)
        {
            if (node.arrowContainer != null)
            {
                node.arrowContainer.SetActive(false); // Force Hide EVERYTHING first
            }
        }

        // Now turn on only the start node
        if (startingNode != null)
        {
            LoadNode(startingNode);
        }
    }

    public void LoadNode(LocationNode newNode)
    {
        if (newNode == null) return;

        // 1. Hide Old
        if (currentNode != null && currentNode.arrowContainer != null)
        {
            currentNode.arrowContainer.SetActive(false);
        }

        // 2. Switch Skybox
        if (newNode.skyboxMaterial != null)
        {
            RenderSettings.skybox = newNode.skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }

        // 3. Show New
        if (newNode.arrowContainer != null)
        {
            newNode.arrowContainer.SetActive(true);
        }

        currentNode = newNode;
    }
}