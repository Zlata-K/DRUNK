using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public List<Link> links;

    public void AddLink(Node other)
    {
        float dist = (other.position - position).magnitude;

        links.Add(new Link { node = other, weight = dist });
        other.links.Add(new Link { node = this, weight = dist }); // Link in both ways
    }

    public void AddLink(Node other, float weight)
    {
        links.Add(new Link { node = other, weight = weight });
        other.links.Add(new Link { node = this, weight = weight }); // Link in both ways
    }

    public bool CanSeeNode(Node node)
    {
        return CanSeeNode(node, (node.position - position).magnitude);
    }

    public bool CanSeeNode(Node node, float dist)
    {
        return !Physics.SphereCast(position, 1, node.position - position, out _, dist);
    }

    // Check this node links to remove non-valid ones
    public void CheckOldLinks()
    {
        for (int i = links.Count - 1; i >= 0; i--) { // iterating in reverse to safely remove items while iterationg
            Node other = links[i].node;
            float dist = (position - other.position).magnitude;

            // Check if we can still see the other node
            if (!CanSeeNode(other, dist)) {
                // If we hit smth, remove the link : it's not accurate anymore
                links.RemoveAt(i);
            }
        }
    }

    // Check for new links inside the cluster
    public bool CheckNewLinks()
    {
        List<Node> cluster = NavigationGraph.GetCluster(position);

        return CheckNewLinks(cluster);
    }

    // Check for new links inside the cluster
    public bool CheckNewLinks(List<Node> cluster)
    {
        bool addedNewNodes = false;

        foreach (var node in cluster.ToArray()) { // The ToArray is to iterate over a copy of the list, and so we can add new items while iterating
            float dist = (position - node.position).magnitude;

            if (CanSeeNode(node, dist)) {
                // If we can see the node, link the 2 nodes together

                // if dist > nodesMaxDistance, add in between nodes
                if (dist > NavigationGraph.nodesMaxDistance && false) { // TODO temporarly disabled
                    int nb = (int)(dist / NavigationGraph.nodesMaxDistance); // compute nb of nodes to add
                    Vector3 dir = (node.position - position).normalized;
                    float intermediateDist = dist / (nb + 1); // distance between nodes
                    Node prev = this; // Keep track of the previous node to add in between links

                    addedNewNodes = true;
                    while (nb > 0) {
                        Node tmp = new Node {
                            position = prev.position + dir * intermediateDist,
                            links = new List<Link>()
                        };

                        // No need to raycast to check the link : we already did one before on a longest distance
                        tmp.AddLink(prev);
                        cluster.Add(tmp);
                        prev = tmp;
                        nb--;
                    }
                    prev.AddLink(node); // don't forget to link the last node
                } else {
                    AddLink(node, dist);
                }
            }
        }
        return addedNewNodes;
    }
}

public class Link
{
    public Node node;
    public float weight;
}

public class NavigationGraph
{
    public static readonly Vector3[] ordinalDirections = {
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 0, -1),
            new Vector3(1, 0, -1),
        }; // (N/S/E/W/NE/NW/SE/SW)


    // Constants used in the different computations
    public const float clusterWidth = 40;
    public const float wallOffset = 1F;
    public const float nodeSize = 0.5F;
    public const float nodesSpawnY = 1.5F;
    public const float nodesMaxDistance = 5F;

    // Name of the environment layer (used to detect walls)
    private const string environementLayerName = "Environment";

    // The actual graph. It's a clustered graph, the Vector3 represent a cluster (a square of clusterWidth * clusterWidth, 
    // so the Vector3 is actually the bottom left corner of the cluster) and the value represents the nodes in the cluster
    // It's a public get, so if needed an external class can go through it manually. 
    // Altho, it is preferable to use the fonctions to access it more easily.
    public static Dictionary<Vector3, List<Node>> graph { get; private set; } = new Dictionary<Vector3, List<Node>>();

    // A function to get the cluster to which a position would be mapped to.
    public static List<Node> GetCluster(Vector3 pos)
    {
        pos.y = nodesSpawnY; // Ensure the keys in the graph has the same height
        foreach (var key in graph.Keys) {
            if (Mathf.Abs(key.x - pos.x) < clusterWidth && Mathf.Abs(key.z - pos.z) < clusterWidth) {
                return graph[key];
            }
        }
        // If we didn't found the key, we just gonna create it and return the new cluster
        Vector3 vec = new Vector3((int)(pos.x / clusterWidth) * clusterWidth, nodesSpawnY, (int)(pos.z / clusterWidth) * clusterWidth);
        List<Node> res = new List<Node>();

        graph.Add(vec, res);
        return res;
    }

    // A function to get the closest node to a point
    // TODO (non-urgent) : check in neighbouring clusters too, maybe there will be a closer point 
    public static Node GetClosestNode(Vector3 pos)
    {
        List<Node> nodes = GetCluster(pos); // Get the cluster containing this position
        Node res = null;
        float minDist = clusterWidth * 2; // Big init value

        foreach (var item in nodes) { // Look over all the nodes to find the closest
            float dist = (item.position - pos).magnitude;

            if (dist < minDist) {
                res = item;
                minDist = dist;
            }
        }
        return res;
    }

    // Internal utility fonction to recursively get the compoments of "Environment" gameObjects.
    // (used to get bounding boxes of the map objects)
    private static T[] GetEnvironmentComponents<T>(GameObject obj)
    {
        List<T> res = new List<T>();

        foreach (Transform child in obj.transform) {
            if (child.gameObject.layer == LayerMask.NameToLayer(environementLayerName)) {
                res.AddRange(child.gameObject.GetComponents<T>());
                res.AddRange(GetEnvironmentComponents<T>(child.gameObject));
            }
        }
        return res.ToArray();
    }

    // Use this game object to create new nodes around it
    // Warning : doesn't add links between nodes
    public static void AddNodes(GameObject obj)
    {
        Bounds bounds = new Bounds(obj.transform.position, Vector3.zero); // Create an ampty bounding volume in the place of the obj

        var collider = GetEnvironmentComponents<Collider>(obj);
        var renderer = GetEnvironmentComponents<Renderer>(obj);

        if (collider.Length > 0) { // If there is colliders, we gonna use this data to compute the bounding volume
            foreach (var iter in collider)
                bounds.Encapsulate(iter.bounds);
        } else if (renderer.Length > 0) { // If there is no collider, we fallback on renderers (if any)
            foreach (var iter in renderer)
                bounds.Encapsulate(iter.bounds);
        }
        NavigationGraph.AddNodes(bounds); // In the end, we just gonna call our overload function who's gonna take care of everything
    }

    // Add new nodes around this bounding box
    // Warning : doesn't add links between nodes
    public static void AddNodes(Bounds bounds)
    {
        foreach (Vector3 dir in ordinalDirections) {
            // The ternary is here to avoid creating a lot of points around a empty bounding volume. Instead, we just gonna create one in the center of it
            Vector3 pos = bounds.extents != Vector3.zero ? (bounds.center + Vector3.Scale(bounds.extents, dir) + dir * nodeSize * 2) : bounds.center;
            pos.y = nodesSpawnY; // Ensure all nodes spawn at the same height
            Node node = new Node {
                position = pos,
                links = new List<Link>()
            };
            Node close = GetClosestNode(pos);

            if (close != null && (close.position - pos).magnitude < wallOffset) // If we have a very close neighbour, abort
                continue;
            if (Physics.CheckSphere(node.position, nodeSize)) // if we are inside/close to a wall, abort
                continue;
            if (!Physics.Raycast(node.position, Vector3.down, 10)) // If no ground below, we are outside of the map so abort
                continue;

            GetCluster(pos).Add(node); // Add the node in the coresponding cluster
            if (bounds.extents == Vector3.zero) // If we have an empty bounding volume, we don't need to add more point, the center is enough
                return;
        }
    }

    // Generate links between neighbour clusters
    private static void ReGenerateNeighbourClusterLinks(Vector3 position)
    {
        List<Node> cluster = GetCluster(position);

        // Look for the clusters around the current one
        foreach (var dir in ordinalDirections) {
            Vector3 pos = position + (dir * clusterWidth); // Use the current cluster position to get the next one
            List<Node> neighbourCluster = GetCluster(pos);
            
            foreach (var node in cluster) {
                foreach (var node2 in neighbourCluster) {
                    float dist = (node.position - node2.position).magnitude;

                    if (node.CanSeeNode(node2, dist)) {
                        node.AddLink(node2, dist);
                    }
                }
            }
        }
    }

    // Regenerate this cluster links, will remove non-valid ones, and add new ones
    public static void ReGenerateClusterLinks(Vector3 position)
    {
        List<Node> cluster = GetCluster(position);
        bool addedNewNodes = false;

        foreach (var node in cluster.ToArray()) { // The ToArray is to iterate over a copy of the list, and so we can add new items while iterating
            node.CheckOldLinks(); // Remove old links that are not valid anymore

            // Now let's find new links for this node in the cluster
            addedNewNodes = node.CheckNewLinks(cluster);
        }
        if (addedNewNodes) // If we added nodes in between long links, new links might be possible
            ReGenerateClusterLinks(position); 
        else // If all the nodes are added, regenerate neightbour links
            ReGenerateNeighbourClusterLinks(position);
    }

    // Regenerate all links, will remove non-valid ones, and add new ones
    public static void ReGenerateAllLinks()
    {
        // Just iterate through all clusters and regenerate them
        foreach (var cluster in graph.Keys.ToList()) {
            ReGenerateClusterLinks(cluster);
        }
    }
}