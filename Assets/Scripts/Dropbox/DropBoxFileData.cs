using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DropBoxFileData
{
    [JsonProperty(".tag")]
    public string tag { get; set; }
    public string name { get; set; }
    public string path_lower { get; set; }
}
