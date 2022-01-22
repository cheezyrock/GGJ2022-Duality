using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheezyTools
{
    public class SkintoneHelper : MonoBehaviour
    {
        public string skinMaterialName;
        public bool generateOnStart = true;
        public bool disableAfterGeneration = true;
        public ToneOption toneOption = ToneOption.Any;
        public Color centeredAroundColor = new Color(.55f, .46f, .4f);
        public float centeredAroundFloat = .47f;
        public OffsetDeviation deviation;
        public bool useTransparent = false;
        public bool useSicklyTones = false;
        public float percentOpaque = 100;
        [HideInInspector]
        public Color generatedColor = new Color(.55f, .46f, .40f);
        [HideInInspector]
        public List<Material> skinMaterials = new List<Material>();
        private bool isGenerating = false;

        public void Start()
        {
            if (generateOnStart)
            {
                GenerateSkinColor();
            }
        }

        public void GenerateSkinColor()
        {
            isGenerating = true;
            Renderer[] renderers = this.gameObject.GetComponentsInChildren<Renderer>();

            Color newColor = CalculateSkinColor(toneOption);
            foreach (Renderer renderer in renderers)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    if (skinMaterialName + " (Instance)" == renderer.materials[i].name)
                    {
                        skinMaterials.Add(renderer.materials[i]);
                    }
                }
            }

            if (useSicklyTones)
            {
                AddSicklyTint(newColor, out newColor);
            }

            if (useTransparent)
            {
                SetTransparency(newColor, percentOpaque, out newColor);
            }
            Debug.Log(newColor.ToString());
            generatedColor = newColor;
            ApplySkinColor(newColor);
            isGenerating = false;
            if (disableAfterGeneration)
            {
                this.enabled = false;
            }
        }

        public Color CalculateSkinColor(ToneOption tone = ToneOption.Any)
        {
            float r = Random.Range(.12f, .99f);
            float g = Random.Range(Mathf.Max(0f, r - Random.Range(.12f, .66f)), r - .075f);
            float b = Random.Range(Mathf.Max(0f, g - .66f), g - .1f);

            float change = (deviation == OffsetDeviation.Low ? .125f : (deviation == OffsetDeviation.High ? .25f : .5f));

            switch (tone)
            {
                case ToneOption.Any:
                    //Option set on init
                    break;
                case ToneOption.CenteredAroundColor:
                    r = Mathf.Clamp(Random.Range(centeredAroundColor.r - change, centeredAroundColor.r + change), .12f, .99f);
                    g = Mathf.Clamp(Random.Range(centeredAroundColor.g - change, centeredAroundColor.g + change), Mathf.Max(0f, r - Random.Range(.12f, .66f)), r - .075f);
                    b = Mathf.Clamp(Random.Range(centeredAroundColor.b - change, centeredAroundColor.b + change), Mathf.Max(0f, g - .66f), g - .1f);
                    break;
                case ToneOption.CenteredAroundSlider:
                    r = Mathf.Clamp(Random.Range((r + centeredAroundFloat) / 2 - change, (r + centeredAroundFloat) / 2 + change), .12f, .99f);
                    g = Mathf.Clamp(Random.Range((g + centeredAroundFloat) / 2 - change, (g + centeredAroundFloat) / 2 + change), Mathf.Max(0f, r - Random.Range(.12f, .66f)), r - .075f);
                    b = Mathf.Clamp(Random.Range((b + centeredAroundFloat) / 2 - change, (b + centeredAroundFloat) / 2 + change), Mathf.Max(0f, g - .66f), g - .1f);
                    break;
                default:
                    r = Random.Range(.11f, .99f);
                    g = Random.Range(Mathf.Max(0f, r - Random.Range(.12f, .66f)), r - .075f);
                    b = Random.Range(Mathf.Max(0f, g - .66f), g - .1f);
                    break;
            }

            return new Color(r, g, b);
        }

        public void ApplySkinColor(Color color)
        {
            foreach (Material mat in skinMaterials)
            {
                mat.color = color;
            }
        }

        public Color GetAverageColor(params Color[] colors)
        {
            float rVal = 0;
            float gVal = 0;
            float bVal = 0;
            float aVal = 0;

            foreach (Color color in colors)
            {
                rVal += color.r;
                gVal += color.g;
                bVal += color.b;
                aVal += color.a;
            }

            return new Color(rVal / colors.Count(), gVal / colors.Count(), bVal / colors.Count(), aVal / colors.Count());
        }

        public void AddSicklyTint(Color color, out Color newColor)
        {
            float offset = (Random.Range(.75f, .85f) - (color.g - color.b)) / 2;
            color.g = color.g + offset;
            color.b = color.b - offset;
            newColor = color;
            if (!isGenerating)
            {
                foreach (Material mat in skinMaterials)
                {
                    mat.color = color;
                }
            }
        }

        public void SetTransparency(Color color, float opaquePercentage, out Color newColor)
        {
            color.a = Mathf.Clamp(percentOpaque, 0, 100) / 100;
            newColor = color;
            if (!isGenerating)
            {
                foreach (Material mat in skinMaterials)
                {
                    mat.color = color;
                }
            }
        }

        public void SetCentertingParametersToDefault()
        {
            centeredAroundColor = new Color(.55f, .46f, .40f);
            centeredAroundFloat = .47f;
            deviation = OffsetDeviation.NoLimit;
            toneOption = ToneOption.Any;
        }

        public void SetCenteringParameters(Color color, OffsetDeviation offsetDeviation, bool switchToCenterByColor = true)
        {
            SetCenteringParameters(color, centeredAroundFloat, offsetDeviation, (switchToCenterByColor ? ToneOption.CenteredAroundColor : toneOption));
        }

        public void SetCenteringParameters(float centerFloatValue, OffsetDeviation offsetDeviation, bool switchToCenterFloat = true)
        {
            SetCenteringParameters(centeredAroundColor, centerFloatValue, offsetDeviation, (switchToCenterFloat ? ToneOption.CenteredAroundSlider : toneOption));
        }

        public void SetCenteringParameters(Color color, float centerFloatValue, OffsetDeviation offSetDeviation, ToneOption newToneOption = ToneOption.Any)
        {
            centeredAroundColor = color;
            centeredAroundFloat = centerFloatValue;
            deviation = offSetDeviation;
            toneOption = newToneOption;
        }

        public void RestoreColor()
        {
            ApplySkinColor(generatedColor);
        }
    }

    [System.Serializable]
    public enum ToneOption
    {
        Any = 0,
        CenteredAroundColor = 1,
        CenteredAroundSlider = 2,
    }

    [System.Serializable]
    public enum OffsetDeviation
    {
        NoLimit = 0,
        High = 1,
        Low = 2,
    }
}