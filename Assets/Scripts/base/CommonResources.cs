﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonResources : MonoBehaviour
{
    public enum Resource
    {
        Kabe,
        Abdulmuttalib,
        HzMuhammed,
        DarulErkam,
        Hamza,
        Omer,
        Unknown,
        Hatice,
        Hicret
    }


    public static string IdOf(Resource resource)
    {
        switch (resource)
        {
            case Resource.Kabe:          return Kabe;
            case Resource.Abdulmuttalib: return Abdulmuttalib;
            case Resource.HzMuhammed:    return HzMuhammed;
            case Resource.DarulErkam:    return DarulErkam;
            case Resource.Hamza:         return Hamza;
            case Resource.Omer:          return Omer;
            case Resource.Unknown:       return Unknown;
            case Resource.Hatice:        return Hatice;
            case Resource.Hicret:        return Hicret;
            default:
                throw new ArgumentOutOfRangeException("resource", resource, null);
        }
    }


    public enum ResourceTypes
    {
        Building,
        Badge
    }

    public ResourceTypes TypeOf(string id)
    {
        return Buildings.Contains(id) ? ResourceTypes.Building : ResourceTypes.Badge;
    }

    public static List<string> Buildings = new List<string>
    {
        Kabe,
        Abdulmuttalib,
        HzMuhammed,
        DarulErkam,
        Hamza,
        Omer,
        Unknown,
        Hatice,
        Hicret,
    };

    public static List<string> Badges;


    public static string Kabe
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Abdulmuttalib
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string HzMuhammed
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement02";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string DarulErkam
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement03";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Hamza
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Omer
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Unknown
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Hatice
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }

    public static string Hicret
    {
        get
        {
#if UNITY_EDITOR
            return "Achievement01";
#elif UNITY_ANDROID
            return "";
#elif UNITY_IOS
                        return "";
            #else
return "";
            #endif
        }
    }
}