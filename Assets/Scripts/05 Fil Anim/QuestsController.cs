﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class manages all quests and switching between them.
 */
public class QuestsController : MonoBehaviour
{
    private int _questIndex = 0;

    private Quest[] Quests;
    public CongratsUtil Congrats;
    public GameObject WarningScreen;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _questButton;
    public GameObject DownloadScreen;

    private VideoHandler _videoHandler;

    private static CommonResources.Building Reward
    {
        get
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (SceneManagementUtil.ActiveScene)
            {
                case SceneManagementUtil.Scenes.Kabe:
                    return CommonResources.Building.Abdulmuttalib;
                case SceneManagementUtil.Scenes.HzMuhammed:
                    return CommonResources.Building.DarulErkam;
                case SceneManagementUtil.Scenes.Hamza:
                    return CommonResources.Building.EbuTalib;
                case SceneManagementUtil.Scenes.Hatice:
                    return CommonResources.Building.Omer;
                case SceneManagementUtil.Scenes.Ebubekir:
                    return CommonResources.Building.Muhacir;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        _videoHandler = GetComponent<VideoHandler>();
        Quests = GetComponent<QuestsConstructor>().Construct();
        _questIndex = 0;
        StopBackgroundMusic();
    }

    private static void StopBackgroundMusic()
    {
        if (FindObjectOfType<MusicManager>())
            FindObjectOfType<MusicManager>().GetComponent<AudioSource>().Stop();

        if (FindObjectOfType<DesertMusicManager>())
            FindObjectOfType<DesertMusicManager>().GetComponent<AudioSource>().Stop();
    }

    internal void EndQuest()
    {
        Quests[_questIndex].Completed = true;
        SaveQuest();
        Color color;
        if (!ColorUtility.TryParseHtmlString("#92FF00FF", out color)) return;
        if (_nextButton.interactable)
        {
            _nextButton.image.color = color;
            StartCoroutine(_videoHandler.AutoPlay());
        }

        if (_previousButton.interactable)
            _previousButton.image.color = color;

        // Check achievement Conditions
        if (Quests.All(quest => quest.Completed))
        {
            UserManager.Instance.UnlockAchievement(CommonResources.IdOf(Reward), 100);
            if (_nextButton.interactable) return;
            Invoke("Back", 1.5f);
        }
        else if (!_nextButton.interactable)
        {
            CloseQuestion();
            _videoHandler.Stop();
            _questIndex = Array.FindIndex(Quests, quest => !quest.Completed);
            InitiateQuest();
        }
    }

    internal void SaveQuest()
    {
        Util.SaveQuest(Quests[_questIndex], _questIndex);
    }

    private void UpdateButtonConditions()
    {
        // home button is normal
        // if have next button is active
        // if have back button is active
        _nextButton.image.color = Color.white;
        _previousButton.image.color = Color.white;

        _nextButton.interactable = _questIndex < Quests.Length - 1;
        _previousButton.interactable = _questIndex > 0;
        _videoHandler.PlayPauseButton.SetActive(Quests[_questIndex].VideoClipAvailable);
        _questButton.SetActive(Quests[_questIndex].IsQuestionActive);
    }

    [UsedImplicitly]
    public void ConditionalBack()
    {
        if (!Quests.All(quest => quest.Completed))
        {
            WarningScreen.SetActive(true);
            _videoHandler.Pause();
        }
        else
            Back();
    }

    [UsedImplicitly]
    public void Back()
    {
        if (FindObjectOfType<MusicManager>())
            FindObjectOfType<MusicManager>().GetComponent<AudioSource>().Play();
        if (FindObjectOfType<DesertMusicManager>())
            FindObjectOfType<DesertMusicManager>().GetComponent<AudioSource>().Play();
        SceneManagementUtil.Load(SceneManagementUtil.Scenes.AnaEkran);
    }

    [UsedImplicitly]
    public void NextQuest()
    {
        CloseQuestion();
        _videoHandler.Stop();
        _questIndex++;
        InitiateQuest();
    }

    [UsedImplicitly]
    public void PreviousQuest()
    {
        CloseQuestion();
        _videoHandler.Stop();
        _questIndex--;
        InitiateQuest();
    }

    public void InitiateQuest()
    {
        if (Quests.All(quest => quest.VideoClipAvailable))
        {
            UpdateButtonConditions();
            var quest = Quests[_questIndex];
            StartCoroutine(_videoHandler.PlayVideo(quest));
        }
        else
            DownloadScreen.SetActive(true);
    }

    public Text DownloadPercentage;
    private readonly Dictionary<Quest, float> _downloadPercentages = new Dictionary<Quest, float>();

    [UsedImplicitly]
    public void Download()
    {
        StartCoroutine(CheckDownloadEnd());
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        Quests.All(quest =>
        {
            if (!quest.VideoClipAvailable)
            {
                StartCoroutine(Util.DownloadFile(quest.Url, quest.VideoLocation, www =>
                {
                    _downloadPercentages.Add(quest, 0f);
                    StartCoroutine(Progress(www, quest));
                }));
            }
            return true;
        });
    }

    private IEnumerator Progress(WWW www, Quest quest)
    {
        while (www != null && !www.isDone)
        {
            _downloadPercentages[quest] = www.progress;
            DownloadPercentage.text = _downloadPercentages.Values.Average().ToString("F1") + " %";
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CheckDownloadEnd()
    {
        while (Quests.Any(quest => !quest.VideoClipAvailable))
            yield return null;

        foreach (Transform child in DownloadScreen.transform)
            if (child.name == "Info")
                child.GetComponent<Text>().text = "Indirme islemi bitti";
            else if (child.name == "Loading")
                child.gameObject.SetActive(false);
            else if (child.name == "Close")
                child.gameObject.SetActive(true);
    }

    [UsedImplicitly]
    public void CloseQuestion()
    {
        var quest = Quests[_questIndex];
        _videoHandler.Play();

        quest.gameObject.SetActive(false);
        quest.transform.parent.gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void OpenQuestion()
    {
        var quest = Quests[_questIndex];
        _videoHandler.Pause();

        quest.gameObject.SetActive(true);
        _questButton.SetActive(false);
    }
}