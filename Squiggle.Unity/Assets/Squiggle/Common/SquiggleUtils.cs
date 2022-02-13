using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

namespace Squiggle.Unity
{
    public static class Utils
    {
        public static string ReadTextFileFromStreamingAssets(string sFileName)
        {
            //Debug.Log("Reading " + sFileName);
            //Check to see if the filename specified exists, if not try adding '.txt', otherwise fail
            string sFileNameFound = "";
            if (File.Exists(Application.streamingAssetsPath + "/" + sFileName))
            {
                //Debug.Log("Reading '" + sFileName + "'.");
                sFileNameFound = Application.streamingAssetsPath + "/" + sFileName; //file found
            }
            else if (File.Exists(Application.streamingAssetsPath + "/" + sFileName + ".txt"))
            {
                sFileNameFound = Application.streamingAssetsPath + "/" + sFileName + ".txt";
            }
            else
            {
                Debug.Log("Could not find file '" + sFileName + "'.");
                return null;
            }
    
            StreamReader sr;
            try
            {
                sr = new StreamReader(sFileNameFound);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Something went wrong with read.  " + e.Message);
                return null;
            }
    
            string fileContents = sr.ReadToEnd();
            sr.Close();
    
            return fileContents;
        }
    }
}