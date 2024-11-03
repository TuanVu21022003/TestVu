using DragonBones;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    public static IEnumerator DelayedActionCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay); // Chờ thời gian delay
        action?.Invoke(); // Thực hiện hành động sau khi chờ
    }
    public static void _LoadData(string dragonBonesJSONPath, string textureAtlasJSONPath)
    {
        UnityFactory.factory.LoadDragonBonesData(dragonBonesJSONPath);
        UnityFactory.factory.LoadTextureAtlasData(textureAtlasJSONPath);
    }

    public static float RandomBetweenWithStep(float a, float b, float c)
    {
        // Đảm bảo rằng `c` là dương và `a` nhỏ hơn `b`
        if (c <= 0 || a >= b)
        {
            Debug.LogError("Giá trị không hợp lệ: Khoảng cách phải dương và `a` phải nhỏ hơn `b`.");
            return a;
        }

        // Tính số lượng bước có thể có giữa `a` và `b`
        int steps = Mathf.FloorToInt((b - a) / c);

        // Lấy một bước ngẫu nhiên
        int randomStep = Random.Range(0, steps + 1);

        // Trả về giá trị ngẫu nhiên trong khoảng từ `a` đến `b` với bước nhảy `c`
        return a + randomStep * c;
    }

    public static IEnumerator IncrementValue(int startValue, int endValue, float duration, Action<string> action)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Tăng dần giá trị theo thời gian và chuyển thành số nguyên
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, elapsedTime / duration));
            action?.Invoke(currentValue.ToString());

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo giá trị cuối cùng đạt đến `endValue`
        action?.Invoke(endValue.ToString());
    }

    public static string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

}
