using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTextureArray : MonoBehaviour
{

    [SerializeField] Texture2D[] textures;
    [Tooltip("This must match the name of the texture2DArray in the shader")]
    [SerializeField] string textureName = "_MainTex";

    private void Start()
    {
        //create texture2D array
        Texture2DArray texture2DArray = new
        Texture2DArray(textures[0].width, textures[0].height, textures.Length, TextureFormat.RGBA32, true, false)
        {
            //apply settings
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Repeat
        };

        // Loop through ordinary textures and copy pixels to the Texture2DArray
        for (int i = 0; i < textures.Length; i++)
        {
            texture2DArray.SetPixels(textures[i].GetPixels(0),
                i, 0);
        }

        // Apply our changes
        texture2DArray.Apply();

        GetComponent<Renderer>().sharedMaterial.SetTexture(textureName, texture2DArray);
    }
}
