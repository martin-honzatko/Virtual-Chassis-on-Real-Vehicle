using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA;
using UnityEngine.Windows.Speech;

/*================================================================================
 * This script is giving us a power to activate/deactivate a vehicle using bundles
 * in a scene and to recenter scene's positioning informations by voice commands.
 * ===============================================================================*/

public class BundleManager : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     * ======================================*/
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private string alreadyOnMsg = "This vehicle is already on.";

    public GameObject myCamera;
    /*=====================================
    * The end of a space for an variables. 
    * =====================================*/

    void Start()
    {
        //model2Method(); //activate Model2 by default
        Debug.developerConsoleVisible = false; //Deactivating dev console (some errors are not exactly errors and we don't need to see them)
        /*==========Create keywords for keyword recognizer==========*/
        keywords.Add($"Model one", () => //Keyword to activate Model1 in a scene.
        {
            model1Method();
        });

        keywords.Add($"Model two", () => //Keyword to activate Model2 in a scene.
        {
            model2Method();
        });

        keywords.Add($"Model three", () => //Keyword to activate Model3 in a scene.
        {
            model3Method();
        });

        keywords.Add($"Model four", () => //Keyword to activate Model4 in a scene.
        {
            model4Method();
        });

        keywords.Add("Recenter", () =>
        {
            try
            {
                InputTracking.Recenter(); //Changing position & rotation to 0,y,0 & 0,0,0
            }
            catch (Exception ex)
            {
                MainController.Log($"Error: {ex.Message}");
            }
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start(); //Start a recognition of keywords.
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        /* if the keyword recognized is in our dictionary, call that Action. */
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    private void model1Method()
    {
        ClearModels(BundleDataSet.modelState); //deactivate previous vehicle

        if (!BundleDataSet.instModel1)
        {
            BundleDataSet.instModel1 = Instantiate(BundleDataSet.model1, ModelPosition(), ModelRotation());
            BundleDataSet.instModel1.AddComponent<Transformace>().myCamera = myCamera;
            BundleDataSet.modelState = BundleDataSet.ModelInstType.Model1;
        }

        else
        {
            MainController.Log($"{alreadyOnMsg}");
        }
    }

    private void model2Method()
    {
        ClearModels(BundleDataSet.modelState); //deactivate previous vehicle

        if (!BundleDataSet.instModel2)
        {
            BundleDataSet.instModel2 = Instantiate(BundleDataSet.model2, ModelPosition(), ModelRotation());
            BundleDataSet.instModel2.AddComponent<Transformace>().myCamera = myCamera;
            BundleDataSet.modelState = BundleDataSet.ModelInstType.Model2;
        }

        else
        {
            MainController.Log($"{alreadyOnMsg}");
        }
    }

    private void model3Method()
    {
        ClearModels(BundleDataSet.modelState); //deactivate previous vehicle

        if (!BundleDataSet.instModel3)
        {
            BundleDataSet.instModel3 = Instantiate(BundleDataSet.model3, ModelPosition(), ModelRotation());
            BundleDataSet.instModel3.AddComponent<Transformace>().myCamera = myCamera;
            BundleDataSet.modelState = BundleDataSet.ModelInstType.Model3;
        }

        else
        {
            MainController.Log($"{alreadyOnMsg}");
        }
    }

    private void model4Method()
    {
        ClearModels(BundleDataSet.modelState); //deactivate previous vehicle

        if (!BundleDataSet.instModel4)
        {
            BundleDataSet.instModel4 = Instantiate(BundleDataSet.model4, ModelPosition(), ModelRotation());
            BundleDataSet.instModel4.AddComponent<Transformace>().myCamera = myCamera;
            BundleDataSet.modelState = BundleDataSet.ModelInstType.Model4;
        }

        else
        {
            MainController.Log($"{alreadyOnMsg}");
        }
    }

    private void ClearModels(BundleDataSet.ModelInstType modelState)
    {

        switch (modelState)
        {
            case BundleDataSet.ModelInstType.Model1:
                Destroy(BundleDataSet.instModel1);
                break;
            case BundleDataSet.ModelInstType.Model2:
                Destroy(BundleDataSet.instModel2);
                break;
            case BundleDataSet.ModelInstType.Model3:
                Destroy(BundleDataSet.instModel3);
                break;
            case BundleDataSet.ModelInstType.Model4:
                Destroy(BundleDataSet.instModel4);
                break;
            default:

                break;
        }
    }

    private Quaternion ModelRotation()
    {
        return BundleDataSet.ModelRotation;
    }

    private Vector3 ModelPosition()
    {
        return BundleDataSet.ModelPosition;
    }
}
