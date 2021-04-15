using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public delegate double Heuristic(Node x, Node end);
    public static Heuristic EuclidianHeuristic = EuclidianHeuristicImpl;

    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> path = new List<Node>() { current };

        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    public static List<Node> Astar(Node start, Node end)
    {
        return Astar(start, end, EuclidianHeuristic);
    }

    public static List<Node> Astar(Node start, Node end, Heuristic h)
    {
        List<Node> open = new List<Node>() { start };
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, double> gScore = new Dictionary<Node, double>();
        Dictionary<Node, double> fScore = new Dictionary<Node, double>();

        foreach (var cluster in NavigationGraph.graph) {
            foreach (var node in cluster.Value) {
                fScore[node] = -1;
                gScore[node] = -1;
            }
        }
        fScore[start] = h(start, end);
        gScore[start] = 0;

        while (open.Count > 0) {
            Node current = null;
            double min = -1;
            foreach (var iter in open) {
                if (fScore[iter] < min || min == -1) {
                    current = iter;
                    min = fScore[iter];
                }
            }
            if (current == end) {
                return ReconstructPath(cameFrom, current);
            }
            open.Remove(current);
            foreach (var iter in current.links) {
                double tmp = gScore[current] + iter.weight;

                if (tmp != 0 && (gScore[iter.node] == -1 || gScore[iter.node] > tmp)) {
                    cameFrom[iter.node] = current;
                    gScore[iter.node] = tmp;
                    fScore[iter.node] = tmp + h(iter.node, end);
                    if (!open.Contains(iter.node)) {
                        open.Add(iter.node);
                    }
                }
            }
        }
        return null;
    }

    private static double EuclidianHeuristicImpl(Node x, Node end)
    {
        return (x.position - end.position).magnitude;
    }
}
