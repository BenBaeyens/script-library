// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script takes a URL image input and converts it to the currenty skybox texture.
// </summary>

using System.Collections;
using UnityEngine;

[AddComponentMenu("Ben's Script Library/Textures/Load Image To Skybox")]
public class LoadImageToSkybox : MonoBehaviour
{
    public string url = "Your URL Here";
    public int CubemapResolution = 512;
    Cubemap c;

    private Texture2D source;

    /// <summary>
    /// These are the faces of a cube
    /// </summary>
    private Vector3[][] faces =
    {
        new Vector3[] {
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f)
        }
    };


    private void Start() { // When the game starts, apply the skybox texture
        StartCoroutine(setImage());
    }

    IEnumerator setImage()
    {
        WWW www = new WWW(url);

        yield return www;
        Debug.Log(www.bytesDownloaded);
        Debug.Log(www.progress);
        Debug.Log(www.texture);


        source = new Texture2D(www.texture.width, www.texture.height);
        // we put the downloaded image into the new texture
        www.LoadImageIntoTexture(source);

        // new cubemap 
        c = new Cubemap(CubemapResolution, TextureFormat.RGBA32, false);

        Color[] CubeMapColors;

        for (int i = 0; i < 6; i++)
        {
            CubeMapColors = CreateCubemapTexture(CubemapResolution, (CubemapFace)i);
            c.SetPixels(CubeMapColors, (CubemapFace)i);
        }
        // we set the cubemap from the texture pixel by pixel
        c.Apply();

        //Destroy all unused textures
        DestroyImmediate(source);
        DestroyImmediate(www.texture);
        Texture2D[] texs = FindObjectsOfType<Texture2D>();
        for (int i = 0; i < texs.Length; i++)
        {
            DestroyImmediate(texs[i]);
        }

        // We change the Cubemap of the Skybox
        Material cubeMapMaterial = new Material(Shader.Find("Skybox/Cubemap"));
        cubeMapMaterial.SetTexture("_Tex", c);
        RenderSettings.skybox = cubeMapMaterial;
    }

    /// <summary>
    /// Generates a Texture that represents the given face for the cubemap.
    /// </summary>
    /// <param name="resolution">The targetresolution in pixels</param>
    /// <param name="face">The target face</param>
    /// <returns></returns>
    private Color[] CreateCubemapTexture(int resolution, CubemapFace face)
    {
        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);

        Vector3 texelX_Step = (faces[(int)face][1] - faces[(int)face][0]) / resolution;
        Vector3 texelY_Step = (faces[(int)face][3] - faces[(int)face][2]) / resolution;

        float texelSize = 1.0f / resolution;
        float texelIndex = 0.0f;

        //Create textured face
        Color[] cols = new Color[resolution];
        for (int y = 0; y < resolution; y++)
        {
            Vector3 texelX = faces[(int)face][0];
            Vector3 texelY = faces[(int)face][2];
            for (int x = 0; x < resolution; x++)
            {
                cols[x] = Project(Vector3.Lerp(texelX, texelY, texelIndex).normalized);
                texelX += texelX_Step;
                texelY += texelY_Step;
            }
            texture.SetPixels(0, y, resolution, 1, cols);
            texelIndex += texelSize;
        }
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        Color[] colors = texture.GetPixels();
        DestroyImmediate(texture);

        return colors;
    }

    /// <summary>
    /// Projects a directional vector to the texture using spherical mapping
    /// </summary>
    /// <param name="direction">The direction in which you view</param>
    /// <returns></returns>
    private Color Project(Vector3 direction)
    {
        float theta = Mathf.Atan2(direction.z, direction.x) + Mathf.PI / 180.0f;
        float phi = Mathf.Acos(direction.y);

        int texelX = (int)(((theta / Mathf.PI) * 0.5f + 0.5f) * source.width);
        if (texelX < 0) texelX = 0;
        if (texelX >= source.width) texelX = source.width - 1;
        int texelY = (int)((phi / Mathf.PI) * source.height);
        if (texelY < 0) texelY = 0;
        if (texelY >= source.height) texelY = source.height - 1;

        return source.GetPixel(texelX, source.height - texelY - 1);
    }
}