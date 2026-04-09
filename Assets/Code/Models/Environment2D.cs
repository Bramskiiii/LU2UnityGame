using Newtonsoft.Json;
using System;

/**
 * Bijzonderheden wegens beperkingen van JsonUtility:
 * - De id is een string in plaats van een Guid omdat Unity een Guid niet in de Inspector kan tonen. Gelukkig geeft dit geen probleem omdat de backend API de string correct zal interpreteren als een Guid.
 */
[Serializable]
public class Environment2D
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

    public string Id;

    public string Name;
}
