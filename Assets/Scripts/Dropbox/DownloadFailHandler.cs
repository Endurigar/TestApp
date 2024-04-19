using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadFailHandler : Page
{
    public override void Show()
    {
        base.Show();
        StartCoroutine(DelayForFade());
    }

    private IEnumerator DelayForFade()
    {
        yield return new WaitForSeconds(1f);
        Hide();
    }
}
