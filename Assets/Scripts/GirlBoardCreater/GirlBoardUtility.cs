using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GirlBoardCreater
{
    public class GirlBoardUtility
    {
        public static void AddBoardData(string filePath, string newKey, string newValue)
        {
            InsertKey(filePath, PathDefine.BoardDataDicName + ":", PathDefine.DicKeysName + ":", newKey);
            InsertKey(filePath, PathDefine.BoardDataDicName + ":", PathDefine.DicValueName + ":", newValue);
        }

        /// <summary>
        /// 在指定文件的 boardPrefabDict: 下的 m_keys: 部分插入新行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="variable">属性名</param>
        /// <param name="variableSub">属性中的m_keys:或者m_values:</param>
        /// <param name="newKey">要插入的新字符串</param>
        private static void InsertKey(string filePath, string variable, string variableSub, string newKey)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件未找到", filePath);
            }

            // 读取文件的所有行
            var lines = File.ReadAllLines(filePath).ToList();

            // 查找 boardPrefabDict: 和 m_keys: 的位置
            int boardPrefabDictIndex = -1;
            int mKeysIndex = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Trim() == variable)
                {
                    Debug.Log(lines[i]);
                    boardPrefabDictIndex = i;
                    break;
                }
            }

            for (int i = boardPrefabDictIndex + 1; i < lines.Count; i++)
            {
                if (lines[i].Trim() == variableSub)
                {
                    Debug.Log(lines[i]);
                    mKeysIndex = i;
                    break;
                }
            }

            // 检查是否找到 boardPrefabDict: 和 m_keys:
            if (boardPrefabDictIndex == -1 || mKeysIndex == -1)
            {
                throw new InvalidOperationException("未找到 boardPrefabDict: 或 m_keys: 部分");
            }

            // 找到 m_keys: 的最后一行
            int lastKeyIndex = mKeysIndex;
            for (int i = mKeysIndex + 1; i < lines.Count; i++)
            {
                if (lines[i].Trim().StartsWith("-"))
                {
                    lastKeyIndex = i;
                }
                else
                {
                    break; // 遇到非键值行时退出
                }
            }

            // 插入新行
            lines.Insert(lastKeyIndex + 1, $"    - {newKey}");

            // 将修改后的内容写回文件
            File.WriteAllLines(filePath, lines);
            Debug.Log($"已插入！{newKey}");
        }

        /// <summary>
        /// 获取预制件的 GUID
        /// </summary>
        /// <param name="prefab">预制件的 GameObject</param>
        /// <returns>预制件的 GUID，如果失败则返回空字符串</returns>
        public static string GetPrefabGuid(GameObject prefab)
        {
#if UNITY_EDITOR
            if (prefab == null)
            {
                Debug.LogError("预制件参数为空");
                return string.Empty;
            }

            // 获取预制件的路径
            string prefabPath = AssetDatabase.GetAssetPath(prefab);

            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError("无法获取预制件路径，请确保传入的是一个预制件");
                return string.Empty;
            }

            // 通过路径获取 GUID
            string guid = AssetDatabase.AssetPathToGUID(prefabPath);
            return guid;
#else
        Debug.LogError("此方法仅在 Unity 编辑器中可用");
        return string.Empty;
#endif
        }
        
         public static string GetPrefabFileID(GameObject target)
        {
            // 1. 获取预制件的路径
            string prefabPath = UnityEditor.AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError("Target is not a prefab or not found in the AssetDatabase.");
                return null;
            }

            // 2. 读取预制件文件内容
            string[] lines = File.ReadAllLines(prefabPath);

            // 3. 找到包含预制件名称的行
            string prefabName = target.name;
            int nameLineIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains($"m_Name: {prefabName}"))
                {
                    nameLineIndex = i;
                    break;
                }
            }

            if (nameLineIndex == -1)
            {
                Debug.LogError("Prefab name not found in the file.");
                return null;
            }

            // 4. 向上搜索到第一个包含 "m_Component:" 的行
            int componentLineIndex = -1;
            for (int i = nameLineIndex; i >= 0; i--)
            {
                if (lines[i].Contains("m_Component:"))
                {
                    componentLineIndex = i;
                    break;
                }
            }

            if (componentLineIndex == -1)
            {
                Debug.LogError("m_Component line not found.");
                return null;
            }

            // 5. 向下搜索第二个包含 "- component: {fileID:" 的行
            int fileIDLineIndex = -1;
            int componentCount = 0;
            for (int i = componentLineIndex; i < lines.Length; i++)
            {
                if (lines[i].Contains("- component: {fileID:"))
                {
                    componentCount++;
                    if (componentCount == 2)
                    {
                        fileIDLineIndex = i;
                        break;
                    }
                }
            }

            if (fileIDLineIndex == -1)
            {
                Debug.LogError("Second component fileID line not found.");
                return null;
            }

            // 6. 提取 fileID
            string fileIDLine = lines[fileIDLineIndex];
            Match match = Regex.Match(fileIDLine, @"- component: \{fileID: (\d+)\}");
            if (match.Success)
            {
                string fileID = match.Groups[1].Value;
                return fileID;
            }
            else
            {
                Debug.LogError("Failed to extract fileID from the line.");
                return null;
            }
        }
         
        public static void GetPrefabScript(GameObject mTarget, string id)
        {
            Component[] components = mTarget.GetComponents<Component>();
            Component targetCom = null;
            foreach (var VARIABLE in components)
            {
                if (VARIABLE.GetType().Name == "UniqueBoard")
                {
                    targetCom = VARIABLE;
                    break;
                }
            }

            if (targetCom == null)
            {
                Debug.LogError("未获取到 UniqueBoard 脚本");
                return;
            }

            FieldInfo targetField = null;
            FieldInfo[] fields = targetCom.GetType().GetFields();
            foreach (var VARIABLE in fields)
            {
                if (VARIABLE.Name == "shapeID")
                {
                    targetField = VARIABLE;
                }

                Debug.Log(VARIABLE.Name);
            }

            if (targetField == null)
            {
                Debug.LogError("未获取到 ShapteID 变量");
                return;
            }

            Debug.Log(targetField.GetValue(targetCom));

            targetField.SetValue(targetCom, id);
            Debug.Log(targetField.GetValue(targetCom));
        }

        
        
    }
}