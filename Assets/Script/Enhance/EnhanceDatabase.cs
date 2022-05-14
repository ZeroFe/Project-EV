using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Enhance/EnhanceDatabase")]
public class EnhanceDatabase : ScriptableObject
{
    private static EnhanceDatabase s_Instance;

    [SerializeField] private List<Enhance> enhances;

    public IReadOnlyList<Enhance> Enhances => enhances;

#if UNITY_EDITOR
    [ContextMenu("FindItem")]
    private void FindItems()
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(Enhance)}");
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var quest = AssetDatabase.LoadAssetAtPath<Enhance>(assetPath);

            // FindAssets�� Type���� T�� ������ Quest�� Achievement�� ���� �����͹�����
            // Quest�� �޴� ���� ���ϹǷ� GetType()�� ���� ��ü�� Type�� T�� ������ Ȯ���Ѵ�
            if (quest.GetType() == typeof(Enhance))
            {
                enhances.Add(quest);

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }
    }
#endif

    public static EnhanceDatabase Instance
    {
        get
        {
            if (s_Instance == null)
            {
                var db = Resources.Load<EnhanceDatabase>("EnhanceDatabase");

                if (db == null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        db = CreateInstance<EnhanceDatabase>();

                        if (!System.IO.Directory.Exists(Application.dataPath + "/Resources"))
                            AssetDatabase.CreateFolder("Assets", "Resources");

                        AssetDatabase.CreateAsset(db, "Assets/Resources/EnhanceDatabase.asset");
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        Debug.LogError("Enhance Database couldn't be found.");
                        return null;
                    }
#else
                    Debug.LogError("Game Database couldn't be found.");
                    return null;
#endif
                }

                s_Instance = db;
            }

            return s_Instance;
        }
    }
}
