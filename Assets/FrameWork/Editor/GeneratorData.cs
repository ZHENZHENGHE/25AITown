using UnityEditor;
using UnityEngine;

namespace BFramework
{
    public  class GeneratorData
    {
        [MenuItem("BFramework/Generate DataTables")]
        private static void GenerateDataTables()
        {
            foreach (string dataTableName in Config.DataTableNames)
            {
                var path = Config.GetRegularPath(dataTableName);
                if (Config.FileisExists(path))
                {
                    return;
                }
                var s =Config.TxtToArrary(path);
                Config.GenerateDataFile(dataTableName, s);
                Config.GenerateCodeFile(dataTableName,s);
            }
            AssetDatabase.Refresh();
        }
     
        [MenuItem("BFramework/readDataTables")]
        private static void readDataTables()
        {
            Config.Init();
            Config.LoadConfig("role");
            Config.LoadConfig("skill");
        }
        [MenuItem("BFramework/debug")]
        private static void debug()
        {
            foreach (var kv in Config.Data)
            {
                foreach (var kvr in kv.Value)
                { 
                    var v = (roleRow)kvr.Value;
                    // Debug.Log(kvr.Key+"::"+v.name);
                    // Debug.Log(kvr.Key+"::"+v.num);
                }
            }
        }
        [MenuItem("BFramework/clear")]
        private static void clear()
        {
            Config.Clear();
        }
    } 
}
    

