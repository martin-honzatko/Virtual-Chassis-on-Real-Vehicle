using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BundleCatalog
{
    /*=================================================
     * The start of a space for serializable variables. 
     *=================================================*/
    public string modelsSource;
    public Model[] models;
    /*===============================================
     * The end of a space for serializable variables. 
     *===============================================*/

    [Serializable]
    public class Model
    {
        public string modelName;
    }
}
