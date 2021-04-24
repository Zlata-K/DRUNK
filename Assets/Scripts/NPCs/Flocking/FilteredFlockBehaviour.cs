using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking
{
    public abstract class FilteredFlockBehavior : FlockBehaviour
    {
        [SerializeField] protected PhysicsLayerFilter filter;
    
        protected List<Transform> RemoveElementFromContextByName(List<Transform> context, string gameObjectName)
        {
            List<Transform> filteredContext = new List<Transform>(context);
            foreach (Transform element in context)
            {
                if (element.name == gameObjectName)
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
}