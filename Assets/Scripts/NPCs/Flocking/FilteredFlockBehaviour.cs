using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilteredFlockBehavior : FlockBehaviour
{
    [SerializeField] protected ContextFilter filter;
    
    protected List<Transform> RemoveElementFromContextByName(List<Transform> context, string name)
    {
        List<Transform> filteredContext = new List<Transform>(context);
        foreach (Transform element in context)
        {
            if (element.name == name)
            {
                filteredContext.Remove(element);
            }
        }
        
        return filteredContext;
    }
    
    protected List<Transform> RemoveItSelfFromContext(List<Transform> context, NPCManager npc)
    {
        List<Transform> filteredContext = new List<Transform>(context);
        foreach (Transform element in context)
        {
            if (element == npc.transform)
            {
                filteredContext.Remove(element);
            }
        }
        
        return filteredContext;
    }
}