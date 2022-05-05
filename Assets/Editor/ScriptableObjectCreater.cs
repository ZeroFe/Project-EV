using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectCreater
{
    [MenuItem("Assets/Create/MagiMaker Assets/Example Assets", false, 10)]
    static void CreateExampleAssets()
    {
        var equip = new EquipItem();
        AssetDatabase.CreateAsset(equip, "Assets/Resources/Item/Test.asset");
    }
}