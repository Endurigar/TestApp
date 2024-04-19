using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CategoryButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }
    
}
