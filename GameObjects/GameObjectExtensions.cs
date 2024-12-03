using System.Collections.Generic;
using UnityEngine;

namespace Libs.CustomExtensions;

public static class GameObjectExtensions
{
    public static void SetGameLayerRecursive(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
            SetGameLayerRecursive(child.gameObject, layer);
    }

    public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren)
    {
        if (includeChildren)
        {
            var transform = gameObject.transform;
            var transforms = new Queue<Transform>(transform.childCount);
            transforms.Enqueue(transform);

            while (transforms.TryDequeue(out var current))
            {
                current.gameObject.layer = layer;

                foreach (Transform child in current) 
                    transforms.Enqueue(child);
            }
        }
        else
        {
            gameObject.layer = layer;
        }
    }
}