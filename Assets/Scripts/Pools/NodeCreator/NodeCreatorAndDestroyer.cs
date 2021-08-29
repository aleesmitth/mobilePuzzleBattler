using UnityEngine;

internal static class NodeCreatorAndDestroyer {
    public static GameObject Get(ElementType elementType) {
        switch (elementType) {
            case ElementType.FIRE: return ObjectPool.instance.GetObject(PoolsNames.FIRE);
            case ElementType.WATER: return ObjectPool.instance.GetObject(PoolsNames.WATER);
            case ElementType.EARTH: return ObjectPool.instance.GetObject(PoolsNames.EARTH);
            case ElementType.AIR: return ObjectPool.instance.GetObject(PoolsNames.AIR);
            default:
                return ObjectPool.instance.GetObject(PoolsNames.FIRE);
        }
    }

    public static void Destroy(ElementType elementType, GameObject gameObject) {
        switch (elementType) {
            case ElementType.FIRE: ObjectPool.instance.DestroyObject(PoolsNames.FIRE, gameObject);
                break;
            case ElementType.WATER: ObjectPool.instance.DestroyObject(PoolsNames.WATER, gameObject);
                break;
            case ElementType.EARTH: ObjectPool.instance.DestroyObject(PoolsNames.EARTH, gameObject);
                break;
            case ElementType.AIR: ObjectPool.instance.DestroyObject(PoolsNames.AIR, gameObject);
                break;
            default:
                ObjectPool.instance.DestroyObject(PoolsNames.FIRE, gameObject);
                break;
        }
    }
}