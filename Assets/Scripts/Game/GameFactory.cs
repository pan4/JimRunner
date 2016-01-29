using System;
using System.Collections.Generic;
using UnityEngine;

namespace JimRunner
{
    public static class GameFactory
    {
        private static Dictionary<string, string> _stringResourcesPath;

        private static Dictionary<string, UnityEngine.Object> _loadedPrefab;

        private static List<int> _respawnCosts;

        static GameFactory()
        {
            List<BlasterConfig> allBlasters = GameConfiguration.Instance.BlasterConfigManager.GetBlasterConfigs();
            string baseBlasterPath = "Prefabs/Equipments/Blasters/{0}";
            for (int i = 0; i < allBlasters.Count; i++)
            {
                _stringResourcesPath.Add(allBlasters[i].blasterName + "_uiBlast", allBlasters[i].uiPrefab);
                _stringResourcesPath.Add(allBlasters[i].blasterName, allBlasters[i].gamePrefab);
                _stringResourcesPath.Add(string.Format("{0}{1}", allBlasters[i].blasterName, "Enemy"), string.Format(baseBlasterPath, "Billy'sBlaster"));
            }
        }

        public static GameObject GetPrefab<T>() where T : MonoBehaviour
        {
            return GetPrefab(typeof(T));
        }

        public static GameObject GetPrefab(Type type)
        {
            return GetPrefab(type.ToString()) as GameObject;
        }

        private static UnityEngine.Object GetPrefab(string type)
        {
            if (!_stringResourcesPath.ContainsKey(type))
            {
                Debug.Log(string.Format("UnitFactory does not have string key {0}", type));
                return null;
            }

            if (!_loadedPrefab.ContainsKey(type))
                _loadedPrefab[type] = Resources.Load(_stringResourcesPath[type]);

            return _loadedPrefab[type];
        }



        public static GameObject GetBlasterPrefab(BlasterType blaster)
        {
            return GetPrefab(blaster.ToString()) as GameObject;
        }

        public static GameObject GetBlasterPrefabUI(BlasterType blaster)
        {
            return GetPrefab(blaster.ToString() + "_uiBlast") as GameObject;
        }

        public static GameObject GetEnemyBlasterPrefab(BlasterType blaster)
        {
            return GetPrefab(blaster.ToString() + "Enemy") as GameObject;
        }

        public static void Clear()
        {
            _loadedPrefab.Clear();
        }
    }
}

