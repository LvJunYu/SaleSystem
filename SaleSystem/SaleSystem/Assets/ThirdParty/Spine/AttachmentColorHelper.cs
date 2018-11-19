using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Modules.AttachmentTools
{
    public static class AttachmentColorHelper
    {
        private static int _blendColorVal = int.MinValue;

        public static int BlendColorVal
        {
            get
            {
                if (_blendColorVal == int.MinValue)
                {
                    _blendColorVal = Shader.PropertyToID("_BlendColor");
                }

                return _blendColorVal;
            }
        }

        static Dictionary<AtlasRegion, Dictionary<Color, Texture2D>> CachedRegionTextures =
            new Dictionary<AtlasRegion, Dictionary<Color, Texture2D>>();

        public static void ClearCache()
        {
            CachedRegionTextures.Clear();
        }

        //叠加材质上的颜色
        public static Texture2D ToColorTexture(this AtlasRegion ar, Material material,
            TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
        {
            if (material == null || !material.HasProperty(BlendColorVal))
            {
                return ar.ToTexture(true, textureFormat, mipmaps);
            }

            var blendColor = material.GetColor(BlendColorVal);
            Dictionary<Color, Texture2D> colorDic;
            if (!CachedRegionTextures.TryGetValue(ar, out colorDic))
            {
                colorDic = new Dictionary<Color, Texture2D>();
                CachedRegionTextures.Add(ar, colorDic);
            }

            Texture2D tex2D;
            if (!colorDic.TryGetValue(blendColor, out tex2D))
            {
                var sourceTexture = ar.GetMainTexture();
                var r = ar.GetUnityRect(sourceTexture.height);
                var width = (int) r.width;
                var height = (int) r.height;
                tex2D = new Texture2D(width, height, textureFormat, mipmaps);
                tex2D.name = ar.name;
                var pixelBuffer = sourceTexture.GetPixels((int) r.x, (int) r.y, width, height);
                for (int i = 0; i < pixelBuffer.Length; i++)
                {
                    pixelBuffer[i] = pixelBuffer[i].Overlay(blendColor);
                }

                tex2D.SetPixels(pixelBuffer);
                tex2D.Apply();
                colorDic.Add(blendColor, tex2D);
            }

            return tex2D;
        }

        static Color Overlay(this Color baseColor, Color blendColor)
        {
            if (baseColor != Color.clear)
            {
                if (baseColor.a < 0.01f)
                {
                    return Color.clear;
                }

                baseColor.r = Overlay(baseColor.r / baseColor.a, blendColor.r) * baseColor.a;
                baseColor.g = Overlay(baseColor.g / baseColor.a, blendColor.g) * baseColor.a;
                baseColor.b = Overlay(baseColor.b / baseColor.a, blendColor.b) * baseColor.a;
            }

            return baseColor;
        }

        static float Overlay(this float basePixel, float blendPixel)
        {
            if (basePixel < 0.5)
            {
                return 2f * basePixel * blendPixel;
            }
            else
            {
                return 1f - 2f * (1f - basePixel) * (1f - blendPixel);
            }
        }
    }
}