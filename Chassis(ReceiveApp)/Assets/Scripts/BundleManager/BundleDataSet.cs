using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BundleDataSet
{
    public static Guid modelID;

    public const int INDEX_1 = 0;
    public const int INDEX_2 = 1;
    public const int INDEX_3 = 2;
    public const int INDEX_4 = 3;

    public static Vector3 ModelPosition;
    public static Quaternion ModelRotation;

    public enum ModelInstType
    {
        None,
        Model1,
        Model2,
        Model3,
        Model4
    }

    public static ModelInstType modelState = ModelInstType.None;

    public static GameObject instModel1;
    public static GameObject instModel2;
    public static GameObject instModel3;
    public static GameObject instModel4;

    public static GameObject model1;
    public static GameObject model2;
    public static GameObject model3;
    public static GameObject model4;

    public static bool appStart = false;

    public static string assetBundleFileName = "";

    public static string model1Name = "";
    public static string model2Name = "";
    public static string model3Name = "";
    public static string model4Name = "";

    public static void SetBundleDataSet(BundleCatalog bundleCatalog)
    {
        try
        {
            assetBundleFileName = bundleCatalog.modelsSource;

            model1Name = bundleCatalog.models[INDEX_1].modelName;
            model2Name = bundleCatalog.models[INDEX_2].modelName;
            model3Name = bundleCatalog.models[INDEX_3].modelName;
            model4Name = bundleCatalog.models[INDEX_4].modelName;
        }
        catch (Exception ex)
        {
            MainController.Log($"SetBundleDataSet ERROR: {ex.Message}");
        }
    }
}
