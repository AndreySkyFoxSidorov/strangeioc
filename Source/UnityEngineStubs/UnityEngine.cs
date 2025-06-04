namespace UnityEngine
{
    public class Object { }
    public class Component : Object { }
    public class Behaviour : Component { }
    public class Transform : Component
    {
        public Transform? parent { get; set; }
        public GameObject gameObject => null!;
    }
    public class GameObject : Object
    {
        public Transform transform => null!;
        public T? GetComponent<T>() where T : class => null;
        public T[]? GetComponentsInChildren<T>() where T : class => null;
    }
    public class MonoBehaviour : Behaviour
    {
        public GameObject gameObject => null!;
        public T? GetComponent<T>() where T : class => null;
        public T[]? GetComponentsInChildren<T>() where T : class => null;
    }
    public static class Debug
    {
        public static void Log(object message) { }
        public static void Log(string message) { }
        public static void LogError(object message) { }
        public static void LogWarning(object message) { }
    }
}
