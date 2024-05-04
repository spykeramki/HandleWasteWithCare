using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FactsData : ScriptableObject
{
    [Serializable]
    public struct FactData
    {
        public string text;
        public AudioClip audio;
    }

    public List<FactData> intro;
    public List<FactData> wastageFacts;

    public List<FactData> wastageRevolutionFacts;

    
    public List<FactData> bioWastageFacts;

}
