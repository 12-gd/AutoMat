using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;



public class AutoMat 
{
    public class MatTextures
    {
        public string TName;
        public Texture2D albedo;
        public Texture2D ao;
        public Texture2D roughness;
        public Texture2D metallic;
        public Texture2D normal;

        public MatTextures()
        {
            TName = string.Empty;
            albedo = null;
            ao = null;
            roughness = null;
            metallic = null;
            normal = null;
        }
    }

    [MenuItem("12Tools/AutoMat HDRP")]
    private static void Am()
    {
        const string sAlbedo = "albedo";
        const string sAo = "ao";
        const string sRoughness = "roughness";
        const string sMetallic = "metallic";
        const string sNormal = "normal";
        string pathh = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        List<MatTextures> matTexts = new List<MatTextures>();
        List<Texture2D> texs = new List<Texture2D>();
        List<string> TexNames = new List<string>();
        int aCount = 0;
        foreach (var o in Selection.objects)
        {
            Texture2D tex = (Texture2D)o as Texture2D;
            if (tex != null) texs.Add(tex);
            string[] subs = tex.name.Split('_');
            string ttype = subs[subs.Length - 1];
            string tname = subs[1]+ subs[2];

            if (ttype == sAlbedo)
            {
                TexNames.Add(tname);
                aCount++;
            }
        }
        Debug.Log("number of mats to be created: " + aCount + "  number of texturess: " + texs.Count);
        for (int i = 0; i < aCount; i++)
        {
            MatTextures mt = new MatTextures();
            mt.TName = TexNames[i];
            foreach (Texture2D t in texs)
            {
                string[] subs = t.name.Split('_');
                string ttype = subs[subs.Length - 1];
                string tname = subs[1] + subs[2];
                if(tname == mt.TName)
                {
                    switch (ttype)
                    {
                        case sAlbedo:
                            mt.albedo = t;
                            break;
                        case sAo:
                            mt.ao = t;
                            break;
                        case sRoughness:
                            mt.roughness = t;
                            break;
                        case sMetallic:
                            mt.metallic = t;
                            break;
                        case sNormal:
                            mt.normal = t;
                            break;
                        default: break;
                    }
                }
            }
            matTexts.Add(mt);
        }
        //creating materials
        foreach (MatTextures mts in matTexts)
        {
            var mat = new Material(Shader.Find("HDRP/Lit"));
            AssetDatabase.CreateAsset(mat, Path.Combine(pathh, string.Format("{0}.mat", mts.TName)));
            if(mts.albedo != null) mat.SetTexture("_BaseColorMap", mts.albedo);
            if (mts.normal != null) mat.SetTexture("_NormalMap", mts.normal);
            if (mts.albedo != null)
            {
                //mask texture
                Texture2D maskmap = new Texture2D(mts.albedo.width, mts.albedo.height);
                for (int x = 0; x < maskmap.width; x++)
                {
                    for (int y = 0; y < maskmap.height; y++)
                    {
                        float R, G, B, A;
                        if (mts.metallic != null)
                        {
                            R = mts.metallic.GetPixel(x, y).r;
                        }
                        else
                        {
                            R = 0f;//non metallic
                        }
                        if (mts.ao != null)
                        {
                            G = mts.ao.GetPixel(x, y).r;
                        }
                        else
                        {
                            G = 1f;//white ao
                        }
                        if (mts.roughness != null)
                        {
                            A = 1 - mts.roughness.GetPixel(x, y).r;//converted to smoothness
                        }
                        else
                        {
                            A = 1;//
                        }
                        B = 0f;//no detail mask
                        maskmap.SetPixel(x, y, new Color(R, G, B, A));
                    }
                }
                maskmap.Apply();
                var tgaTex = maskmap.EncodeToTGA();
                string mtpath = Path.Combine(pathh, string.Format("{0}.tga", mts.TName + "_mask"));
                File.WriteAllBytes(mtpath, tgaTex);

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

                Texture2D maskTex = (Texture2D)AssetDatabase.LoadAssetAtPath(mtpath, typeof(Texture2D));
                mat.SetTexture("_MaskMap", maskTex);
            }

            EditorUtility.SetDirty(mat);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

}
