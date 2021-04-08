using UnityEngine;

internal static class PoolManager {
    public static GameObject Get(NodeType nodeType) {
        switch (nodeType) {
            case NodeType.Cube: return CubePool.instance.Get();
                break;
            case NodeType.Sphere: return SpherePool.instance.Get();
                break;
            case NodeType.Cylinder: return CylinderPool.instance.Get();
                break;
            case NodeType.Capsule: return CapsulePool.instance.Get();
                break;
            default:
                return CubePool.instance.Get();
                break;
        }
    }

    public static void Destroy(NodeType nodeType, GameObject gameObject) {
        switch (nodeType) {
            case NodeType.Cube: CubePool.instance.DestroyObject(gameObject);
                break;
            case NodeType.Sphere: SpherePool.instance.DestroyObject(gameObject);
                break;
            case NodeType.Cylinder: CylinderPool.instance.DestroyObject(gameObject);
                break;
            case NodeType.Capsule: CapsulePool.instance.DestroyObject(gameObject);
                break;
            default:
                CubePool.instance.DestroyObject(gameObject);
                break;
        }
    }
}