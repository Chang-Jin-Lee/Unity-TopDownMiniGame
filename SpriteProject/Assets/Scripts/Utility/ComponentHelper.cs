using UnityEngine;

public class ComponentHelper : MonoBehaviour
{
    // GameObject의 자식들을 전부 돌며 처음 만난 T 컴포넌트를 반환
    public static T FindInterface<T>(GameObject target) where T : class
    {
        var components = target.GetComponents<MonoBehaviour>();

        foreach (var comp in components)
        {
            if (comp is T match)
            {
                return match;
            }
        }

        return null;
    }
}
