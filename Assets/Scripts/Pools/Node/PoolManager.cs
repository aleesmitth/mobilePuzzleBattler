using UnityEngine;

internal static class PoolManager {
    public static GameObject Get(ElementType elementType) {
        switch (elementType) {
            case ElementType.FIRE: return FirePool.instance.Get();
            case ElementType.WATER: return WaterPool.instance.Get();
            case ElementType.EARTH: return EarthPool.instance.Get();
            case ElementType.AIR: return AirPool.instance.Get();
            default:
                return FirePool.instance.Get();
        }
    }

    public static void Destroy(ElementType elementType, GameObject gameObject) {
        switch (elementType) {
            case ElementType.FIRE: FirePool.instance.DestroyObject(gameObject);
                break;
            case ElementType.WATER: WaterPool.instance.DestroyObject(gameObject);
                break;
            case ElementType.EARTH: EarthPool.instance.DestroyObject(gameObject);
                break;
            case ElementType.AIR: AirPool.instance.DestroyObject(gameObject);
                break;
            default:
                FirePool.instance.DestroyObject(gameObject);
                break;
        }
    }
}