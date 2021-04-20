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

        AddLink(other, dist);
    }

    public void AddLink(Node other, float weight)
    {
        Link link = links.Find((Link l) => { return l.node == other; });

        if (other == this || link != null) // Don't try to link with yourself and don't link twice
            return;
        links.Add(new Link { node = other, weight = weight });
        other.links.Add(new Link { node = this, weight = weight }); // Link in both ways
    }

    public void RemoveLink(Node other)
    {
        Link link = links.Find((Link l) => { return l.node == other; });

        if (link != null) {
            links.Remove(link);
            other.RemoveLink(this);
        }
    }

    public static bool CanSeeNode(Node node, Vector3 pos)
    {
        return node.CanSeeNode(new Node { position = pos, links = null });
    }

    public bool CanSeeNode(Node node)
    {
        Vector3 dir = node.position - position;

        return CanSeeNode(node, dir.magnitude, dir);
    }

    public bool CanSeeNode(Node node, float dist, Vector3 dir)
    {
        return !Physics.SphereCast(position, NavigationGraph.nodeSize, dir, out _, dist) && !Physics.Raycast(position, dir, dist);
    }

    // Check this node links to remove non-valid ones
    public void CheckOldLinks()
    {
        for (int i = links.Count - 1; i >= 0; i--) { // iterating in reverse to safely remove items while iterationg
            Node other = links[i].node;

            // Check if we can still see the other node
            if (!CanSeeNode(other)) {
                // If we hit smth, remove the link : it's not accurate anymore
                RemoveLink(other);
            }
        }
    }

    // Check for new links inside the cluster
    public bool CheckNewLinks(bool canCreateNew = true)
    {
        List<Node> cluster = NavigationGraph.GetCluster(position);

        return CheckNewLinks(cluster, canCreateNew);
    }

    // Check for new links inside the cluster
    public bool CheckNewLinks(List<Node> cluster, bool canCreateNew = true)
    {
        bool addedNewNodes = false;

        foreach (var node in cluster.ToArray()) { // The ToArray is to iterate over a copy of the list, and so we can add new items while iterating
            Vector3 tmp_dir = (position - node.position);
            float dist = tmp_dir.magnitude;

            if (CanSeeNode(node, dist, tmp_dir)) {
                // If we can see the node, link the 2 nodes together


                // // Uncoment theses 2 lines to keep the graph simple with big links. 
                // // Will generate faster, but will be less precise for AI movement
                // AddLink(node, dist);
                // continue;

                // if dist > nodesMaxDistance, add in between nodes
                if (dist > NavigationGraph.maxLinkLength && dist < NavigationGraph.nodesMaxDistance && canCreateNew) {
                    int nb = (int)(dist / NavigationGraph.maxLinkLength); // compute nb of nodes to add
                    Vector3 dir = (node.position - position).normalized;
                    float intermediateDist = dist / (nb + 1); // distance between nodes
                    Node prev = this; // Keep track of the previous node to add in between links

                    while (nb > 0) {
                        Vector3 pos = prev.position + dir * intermediateDist;
                        Node tmp = NavigationGraph.CreateNode(pos, cluster);

                        if (tmp == null) { // If we fail creating an intermediate node, try to use the closest one
                            Node close = NavigationGraph.GetClosestNode(pos);

                            if (close == null && (close.position - pos).magnitude < NavigationGraph.wallOffset) {
                                tmp = close;
                            } else { // if we can't have a close node, just keep a long link and don't bother more
                                prev.AddLink(node);
                                break;
                            }
                        }
                        addedNewNodes = true;
                        // No need to raycast to check the link : we already did one before on a longest distance 
                        // (we already know that the fartest nodes can see each other)
                        tmp.AddLink(prev);
                        prev = tmp;
                        nb--;
                    }
                    prev.AddLink(node); // don't forget to link the last node
                } else if (dist < NavigationGraph.nodesMaxDistance) {
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
        }; // 8 main directions (N/S/E/W/NE/NW/SE/SW)


    // Constants used in the different computations
    public const float clusterWidth = 20;
    public const float wallOffset = 1F;
    public const float nodeSize = 0.5F;
    public const float nodesSpawnY = 1F;
    public const float maxLinkLength = 5F;
    public const float nodesMaxDistance = 20F;

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

            if (dist < minDist && Node.CanSeeNode(item, pos)) {
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

    // Create the node at the given position (ignore Y)
    public static Node CreateNode(Vector3 pos)
    {
        return CreateNode(pos, GetCluster(pos));
    }

    public static Node CreateNode(Vector3 pos, List<Node> cluster)
    {
        pos.y = nodesSpawnY; // Ensure all nodes spawn at the same height
        Node node = new Node {
            position = pos,
            links = new List<Link>()
        };
        Node close = GetClosestNode(pos);

        if (close != null && (close.position - pos).magnitude < wallOffset) // If we have a very close neighbour, abort
            return null;
        if (Physics.CheckSphere(node.position, nodeSize)) // if we are inside/close to a wall, abort
            return null;
        if (!Physics.Raycast(node.position, Vector3.down, 10)) // If no ground below, we are outside of the map so abort
            return null;

        cluster.Add(node); // Add the node in the coresponding cluster
        return node;
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
        AddNodes(bounds); // In the end, we just gonna call our overload function who's gonna take care of everything
    }

    // Add new nodes around this bounding box
    // Warning : doesn't add links between nodes
    public static void AddNodes(Bounds bounds)
    {
        if (bounds.extents == Vector3.zero) { // If we have an empty bounding volume, just create a node in the center 
            CreateNode(bounds.center);
            return;
        }
        foreach (Vector3 dir in ordinalDirections) {
            Vector3 pos = bounds.center + Vector3.Scale(bounds.extents, dir) + dir * nodeSize * 2;

            CreateNode(pos);
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
                    Vector3 tmp_dir = (node.position - node2.position);
                    float dist = tmp_dir.magnitude;

                    if (node.CanSeeNode(node2, dist, tmp_dir)) {
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
        if (addedNewNodes) { // If we added nodes in between long links, new links might be possible 
            foreach (var node in cluster) {
                node.CheckNewLinks(cluster, false);
            }
        }
        ReGenerateNeighbourClusterLinks(position);
    }

    // Regenerate all links, will remove non-valid ones, and add new ones
    public static void ReGenerateAllLinks()
    {
        // Just iterate through all clusters and regenerate them
        foreach (var cluster in graph.Keys.ToList()) {
            ReGenerateClusterLinks(cluster);
        }

       // Remove orphan nodes
       // TODO : also remove orphan sub-graphs
       foreach (var cluster in graph.Values) {
            foreach (var iter in cluster.ToArray()) {
                if (iter.links.Count == 0) {
                    cluster.Remove(iter);
                }
            }
        }
    }
}