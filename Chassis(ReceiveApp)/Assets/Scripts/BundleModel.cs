using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/*=======================================================================
 * This script is for loading the asset configuration and assets from the file.
 *=======================================================================*/


public class BundleModel : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     *=======================================*/
    [SerializeField, Header("UDPCommunication Object")]
    private GameObject UDPRecGameObj;
    private GameObject myDisplay;

    public GameObject modelManagerObject;

    private int model_count = 0;
    private int model_ok = 0;
    private int model_err = 0;
    /*=====================================
     * The end of a space for an variables. 
     *=====================================*/


    void Start()
    {
        myDisplay = GameObject.FindGameObjectWithTag("Display");
        BundleCatalog bundleCatalog = JSONReader.LoadBundleCatalog(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "chassisReceiveBundleConfig.json")); //Loading bundle configuration file.

        if (bundleCatalog == null) //Controling the whether the file exists or not.
        {
            //MainController.Log("Bundle configuration not available");
            Debug.Log("Bundle configuration not available");

            myDisplay.GetComponent<TextMeshPro>().text = "Bundle configuration not available! The chassisReceiveBundleConfig.json file is not in the picture directory or the file is damaged.";
        }

        else
        {
            BundleDataSet.SetBundleDataSet(bundleCatalog);

            string url_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), BundleDataSet.assetBundleFileName);

            WWW www = new WWW("file:///" + url_file);
            //MainController.Log("file:///" + url_file);
            Debug.Log("file:///" + url_file);
            StartCoroutine(WaitForReq(www));
        }
    }

    IEnumerator WaitForReq(WWW www)
    {
        yield return www;

        AssetBundle bundle = www.assetBundle;

        if (string.IsNullOrEmpty(www.error))
        {
            SetModels(bundle);
        }

        else
        {
            //MainController.Log(www.error);
            Debug.Log(www.error);

            myDisplay.GetComponent<TextMeshPro>().text = $"Model data is not available! The {BundleDataSet.assetBundleFileName} file is not in the picture directory or the file is damaged.";
        }
    }

    private async void SetModels(AssetBundle bundle)
    {
        try
        {
            BundleDataSet.model1 = (GameObject)bundle.LoadAsset(BundleDataSet.model1Name);

            //MainController.Log($"Model {BundleDataSet.model1Name} downloaded.");

            model_ok += 1;
        }
        catch (Exception ex)
        {
            model_err += 1;

            MainController.Log($"Model {BundleDataSet.model1Name} downloaded ERROR : {ex.Message}");

            myDisplay.GetComponent<TextMeshPro>().text = $"Model download error. Model {BundleDataSet.model1Name} downloaded ERROR.";
        }

        model_count += 1;

        myDisplay.GetComponent<TextMeshPro>().text = "Loading models...";

        await Task.Yield();

        try
        {
            BundleDataSet.model2 = (GameObject)bundle.LoadAsset(BundleDataSet.model2Name);

            //MainController.Log($"Model {BundleDataSet.model2Name} downloaded.");

            model_ok += 1;
        }
        catch (Exception ex)
        {
            model_err += 1;

            MainController.Log($"Model {BundleDataSet.model2Name} downloaded ERROR : {ex.Message}");

            myDisplay.GetComponent<TextMeshPro>().text = $"Model download error. Model {BundleDataSet.model2Name} downloaded ERROR.";
        }

        model_count += 1;

        myDisplay.GetComponent<TextMeshPro>().text = "Loading models...";

        await Task.Yield();

        try
        {
            BundleDataSet.model3 = (GameObject)bundle.LoadAsset(BundleDataSet.model3Name);

            //MainController.Log($"Model {BundleDataSet.model3Name} downloaded.");

            model_ok += 1;
        }
        catch (Exception ex)
        {
            model_err += 1;

            MainController.Log($"Model {BundleDataSet.model3Name} downloaded ERROR : {ex.Message}");

            myDisplay.GetComponent<TextMeshPro>().text = $"Model download error. Model {BundleDataSet.model3Name} downloaded ERROR.";
        }

        model_count += 1;

        myDisplay.GetComponent<TextMeshPro>().text = "Loading models...";

        await Task.Yield();

        try
        {
            BundleDataSet.model4 = (GameObject)bundle.LoadAsset(BundleDataSet.model4Name);

            //MainController.Log($"Model {BundleDataSet.model4Name} downloaded.");

            model_ok += 1;
        }
        catch (Exception ex)
        {
            model_err += 1;

            MainController.Log($"Model {BundleDataSet.model4Name} downloaded ERROR : {ex.Message}");

            myDisplay.GetComponent<TextMeshPro>().text = $"Model download error. Model {BundleDataSet.model4Name} downloaded ERROR.";
        }

        model_count += 1;

        myDisplay.GetComponent<TextMeshPro>().text = "Loading models...";

        await Task.Yield();

        BundleDataSet.appStart = true;
    }

    IEnumerator ModelsDownloaded()
    {
        myDisplay.GetComponent<TextMeshPro>().text = "Models downloaded! All models were downloaded properly. Choose your model using your voice.";

        yield return 10;

        myDisplay.GetComponent<TextMeshPro>().text = string.Empty;
    }

    void Update()
    {
        if (BundleDataSet.appStart)
        {
            BundleDataSet.appStart = false;

            modelManagerObject.SetActive(true);

            if (model_count != model_ok)
            {
                myDisplay.GetComponent<TextMeshPro>().text = $"Models download. Of the {model_count.ToString()} models downloaded, {model_ok.ToString()} models are OK and {model_err.ToString()} models are damaged.";
            }

            else
            {
                StartCoroutine(ModelsDownloaded());
            }
        }
    }
}
