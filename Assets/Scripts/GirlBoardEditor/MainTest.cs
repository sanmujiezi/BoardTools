using System;
using System.Text.RegularExpressions;
using DefaultNamespace;
using GirlBoardEditor.Config;
using GirlBoardEditor.Tools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GirlBoardEditor
{
    public class MainTest : MonoBehaviour
    {
        public ConfigLoader configLoader;
        public ImageLoader imageLoader;
        public PrefabLoader prefabLoader;
        [SerializeField] public ScriptableObject boardConfigOS;
        [SerializeField] public Object imageResource;
    }

    [CustomEditor(typeof(MainTest))]
    public class MainTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MainTest mainTest = target as MainTest;
            var toolConfig = AssetDatabase.LoadAssetAtPath<ToolConfig>(PathDefine.ConfigPath);

            if (GUILayout.Button("Load Config"))
            {
                mainTest.configLoader = new ConfigLoader(toolConfig.configPath);
                DebugLogger.Instance.Log(this, $"Load {mainTest.configLoader.config} Config Success!");
                GameObject gameObject = null;
                ((mainTest.configLoader.config) as TestConfigOS).boardDic.TryGetValue("Girl001_board_1",
                    out gameObject);
                DebugLogger.Instance.Log(this, $"Load Resource \"{gameObject.name}\"");
            }

            if (GUILayout.Button("Load Image"))
            {
                var imagePath = AssetDatabase.GetAssetPath(mainTest.imageResource);
                mainTest.imageLoader = new ImageLoader(imagePath);
                DebugLogger.Instance.Log(this, $"Load {mainTest.imageLoader.images.Count} Image Success!");
            }

            if (GUILayout.Button("Load Prefab"))
            {
                mainTest.prefabLoader = new PrefabLoader(toolConfig.prefabPath);
                DebugLogger.Instance.Log(this, $"Load {mainTest.prefabLoader.templatePrefab} Prefab Success!");
            }

            if (GUILayout.Button("测试正则表达式"))
            {
                //string input = "联系方式：13812345678";
                //string result = Regex.Replace(input, @"(^联{1})\w{2}([^0-9]{2})\d+", "$1****$2****");

                #region 测试二

                // string input = "girl_half_6_1_fdaaf";
                // string pattern = @"girl_half_(\d)_(\d)";
                // Match match = Regex.Match(input, pattern);
                // Debug.Log(match);
                // Debug.Log(match.Groups.Count);
                // for (int i = 0; i < match.Groups.Count; i++)
                // {
                //     Debug.Log(match.Groups[i].Value);
                // }

                #endregion

                #region 测试三

                // Regex regex = new Regex(@"\b(?<word>\w+)\s+(\k<word>)\b");
                // string text = "The the quick brown fox  fox jumps over the lazy dog dog.";
                // MatchCollection matches = regex.Matches(text);
                // foreach (Match VARIABLE in matches)
                // {
                //     GroupCollection groups = VARIABLE.Groups;
                //     Debug.Log($"{groups["word"].Value} ");
                //     for (int i = 0; i < groups.Count; i++)
                //     {
                //         Debug.Log($"{groups[i].Value} {groups[i].Index} ");   
                //     }
                // }

                #endregion

                #region 测试四

                // int[] array = { 5, 4, 1, 2, 3, 9 };
                // Sorter sorter = new Sorter();
                //
                // // 升序排序
                // sorter.Sort(array, (t1, t2) => t1.CompareTo(t2) > 0);
                // Debug.Log("升序结果: " + string.Join(", ", array)); // 1, 2, 3, 4, 5, 9

                #endregion

                #region 测试五

                // string input = "+99 9922-234-224";
                // string input1 = "+as ksk sfa sdfa";
                //
                // string pattern = @"[0-9a-zA-Z]+@[0-9a-zA-Z]+\.[0-9a-zA-Z]+";
                // string pattern1 = @"\+[0-9]+\s[0-9]+-[0-9]+";
                //
                // Regex mailRegex = new Regex(pattern);
                // Regex phoneRegex = new Regex(pattern1);
                //
                // Match match = phoneRegex.Match(input);
                // Match match1 = phoneRegex.Match(input1);
                //
                // if (match.Success)
                // {
                //     Debug.Log($"{match.Value} {match.Index} {match.Groups[1]} 匹配成功");
                // }
                // else
                // {
                //     Debug.Log( $"{input} 格式出错了");
                // }
                // if (match1.Success)
                // {
                //     Debug.Log($"{match1.Value} {match1.Index} {match1.Groups[1]} 匹配成功");
                // }
                // else
                // {
                //     Debug.Log( $"{input1} 格式出错了");
                // }


                string date = DateTime.Now.ToString();
                string pattern = @"(\d{4})/(\d{1,2})/(\d{1,2})";
                Regex dateRegex = new Regex(pattern); 
                string result = dateRegex.Replace(date, "$1年$2月$3日");
                Debug.Log(result);

                #endregion
            }
        }

//调用：
    }

    public delegate bool Compare<T>(T t1, T t2);

    public class Sorter
    {
        // 泛型排序方法（冒泡排序变体）
        public void Sort<T>(T[] array, Compare<T> del)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    // 通过委托比较元素
                    if (del(array[i], array[j]))
                    {
                        // 交换元素
                        T temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }
    }
}