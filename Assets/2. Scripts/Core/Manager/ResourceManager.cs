using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        private Dictionary<string, UnityEngine.Object> resourceCache = new Dictionary<string, UnityEngine.Object>();

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            if (resourceCache.ContainsKey(path))
            {
                return resourceCache[path] as T;
            }

            T resource = Resources.Load<T>(path);
            if (resource != null)
            {
                resourceCache[path] = resource;
            }
            return resource;
        }

        public void ClearCache()
        {
            resourceCache.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}