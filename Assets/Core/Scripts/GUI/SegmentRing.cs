using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GUI.SegmentRing
{
    public class SegmentRing : MonoBehaviour
    {
        [SerializeField]
        [Range(1, 64)]
        private int segmentCount = 1;

        [SerializeField]
        private Sprite sprite;
        private List<Image> images = new();

        public void SetVisibleSegments(int count)
        {
            for (int i = images.Count - 1; i >= 0; i--)
                images[i].gameObject.SetActive(i < count);
        }

        public void GenerateRings(int count)
        {
            if (segmentCount == count)
                return;
            segmentCount = count;
            GenerateRings();
        }

        private void GenerateRings()
        {
            foreach (Image image in images)
                Destroy(image);
            images.Clear();

            float fillAmount = (1.0f / segmentCount);
            fillAmount -= (fillAmount * 0.01f);
            for (int i = 0; i < segmentCount; i++)
            {
                Image image = CreateSegment(fillAmount, i);
                images.Add(image);
            }

            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }

        private Image CreateSegment(float fillAmount, int i)
        {
            GameObject go = new($"Segment_{i}");
            RectTransform rect = go.AddComponent<RectTransform>();
            SetRect(fillAmount, i, rect);
            Image image = go.AddComponent<Image>();
            SetImage(fillAmount, image);
            return image;
        }

        private void SetImage(float fillAmount, Image image)
        {
            image.sprite = sprite;
            image.fillClockwise = false;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillAmount = fillAmount;
        }

        private void SetRect(float fillAmount, int i, RectTransform rect)
        {
            rect.SetParent(transform);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition3D = Vector3.zero;
            rect.rotation = Quaternion.Euler(0, 0, i * fillAmount * 360.0f);
            rect.localScale = new Vector3(1, 2, 1);
        }
    }
}