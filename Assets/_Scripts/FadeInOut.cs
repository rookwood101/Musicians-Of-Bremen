﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    TaskCompletionSource<bool> isFading = new TaskCompletionSource<bool>();
    bool fadingOut = false;
    bool fadingIn = false;

    private void Awake()
    {
        isFading.SetResult(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.AddListener(EventType.FadeIn, OnFadeIn);
        EventManager.AddListener(EventType.FadeOut, OnFadeOut);
    }

    private void OnFadeOut(object fadeCommand)
    {
        FadeCommand command = (FadeCommand)(fadeCommand);

        if (command.gameObjectToFade != gameObject.name)
        {
            return;
        }

        gameObject.SetActive(true);
        fadingOut = true;
        fadingIn = false;
        FadeOut(command.timeToFade);
    }

    private void OnFadeIn(object fadeCommand)
    {
        FadeCommand command = (FadeCommand)(fadeCommand);

        if (command.gameObjectToFade != gameObject.name)
        {
            return;
        }

        gameObject.SetActive(true);
        fadingIn = true;
        fadingOut = false;
        FadeIn(command.timeToFade);
    }

    private async void FadeIn(float timeToFade)
    {
        while (spriteRenderer.color.a < 1 && fadingIn)
        {
            Color color = spriteRenderer.color;
            color.a += 0.01f / timeToFade;
            spriteRenderer.color = color;
            await Wait.ForSeconds(0.01f);
        }
        fadingIn = false;
        EventManager.TriggerEvent(EventType.EndFadeIn, null);
    }

    private async void FadeOut(float timeToFade)
    {
        while (spriteRenderer.color.a > 0 && fadingOut)
        {
            Color color = spriteRenderer.color;
            color.a -= 0.01f / timeToFade;
            spriteRenderer.color = color;
            await Wait.ForSeconds(0.01f);
        }
        gameObject.SetActive(false);
        fadingOut = false;
        EventManager.TriggerEvent(EventType.EndFadeOut, null);
    }
}

