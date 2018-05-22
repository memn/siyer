﻿using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/*
 * This class manages all quests and switching between them.
 */
public class QuestsController : MonoBehaviour
{
    private static int _questIndex;

    public Quest[] Quests;

    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private GameObject _questButton;
    [SerializeField] private GameObject _playPauseButton;


    [SerializeField] private GameObject _videoPanel;
    private VideoPlayer _videoPlayer;


    // Use this for initialization
    private void Start()
    {
        _videoPlayer = _videoPanel.GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += EndVideo;
        InitiateQuest();
    }

    private void EndVideo(VideoPlayer source)
    {
        if (_videoPlayer.isPlaying)
            _videoPlayer.Pause();
        EndQuest();
    }

    private void EndQuest()
    {
        Color color;
        if (!ColorUtility.TryParseHtmlString("#92FF00FF", out color)) return;
        if (_nextButton.interactable)
        {
            _nextButton.image.color = color;
        }

        if (_previousButton.interactable)
        {
            _previousButton.image.color = color;
        }
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
        _playPauseButton.SetActive(Quests[_questIndex].isVideo);
        _questButton.SetActive(Quests[_questIndex].HasQuestion && !Quests[_questIndex].Answered);
    }

    [UsedImplicitly]
    public void PlayPause()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();
            _playPauseButton.GetComponent<Animator>().SetTrigger("Pause");
        }
        else
        {
            _videoPlayer.Play();
            _playPauseButton.GetComponent<Animator>().SetTrigger("Play");
        }
    }

    [UsedImplicitly]
    public void Back()
    {
        SceneManagementUtil.Load(SceneManagementUtil.Scenes.AnaEkran);
    }

    [UsedImplicitly]
    public void NextQuest()
    {
        CloseQuestion();
        StopQuest();
        _questIndex++;
        InitiateQuest();
    }

    [UsedImplicitly]
    public void PreviousQuest()
    {
        CloseQuestion();
        StopQuest();
        _questIndex--;
        InitiateQuest();
    }

    private void StopQuest()
    {
        var quest = Quests[_questIndex];
        if (quest.isVideo)
        {
            _videoPlayer.Stop();
        }

        _videoPanel.GetComponent<Animator>().Play("VideoIdle");
    }

    private void InitiateQuest()
    {
        UpdateButtonConditions();
        var quest = Quests[_questIndex];
        if (quest.isVideo)
        {
            StartCoroutine(PlayVideo(quest.VideoClip));
        }
    }

    [UsedImplicitly]
    public void CloseQuestion()
    {
        var quest = Quests[_questIndex];
        if (quest.isVideo && !_videoPlayer.isPlaying)
        {
            _videoPlayer.Play();
        }

        quest.gameObject.SetActive(false);
        quest.transform.parent.gameObject.SetActive(false);
        UserManager.Instance.UnlockAchievement(SiyerResources.achievement_achievementtest2);
    }

    [UsedImplicitly]
    public void OpenQuestion()
    {
        var quest = Quests[_questIndex];
        if (quest.isVideo && _videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();
        }

        quest.gameObject.SetActive(true);
        _questButton.SetActive(false);
    }

    private IEnumerator PlayVideo(VideoClip questVideoClip)
    {
        _videoPlayer.clip = questVideoClip;
        _videoPlayer.Prepare();

        while (!_videoPlayer.isPrepared)
        {
            yield return null;
        }

        _videoPanel.GetComponent<RawImage>().texture = _videoPlayer.texture;
        _videoPanel.GetComponent<Animator>().SetTrigger("VideoStart");
        _videoPlayer.Play();
    }
}