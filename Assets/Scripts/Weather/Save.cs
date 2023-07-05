using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{
    public static string shotPath = $"{Application.persistentDataPath}/Shot";
    static string GetPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }
    public static void PlayerPrefsSave(string key,object data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }
    public static string PlayerPrefsLoad(string key)
    {
        return PlayerPrefs.GetString(key, null);
    }
    public static void JsonSave(string fileName,object data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(fileName), json);
        Debug.Log($"已保存{GetPath(fileName)}");
    }
    public static T JsonLoad<T>(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(GetPath(fileName));
            var data = JsonUtility.FromJson<T>(json);
            Debug.Log($"读取{path}"); 
            return data;
        }
        else
        {
            return default;
        }
    }
    public static void JsonDelete(string fileName)
    {
        File.Delete(GetPath(fileName));
    }
    public static string FindAuto()
    {
        if(Directory.Exists(Application.persistentDataPath))
        {
            FileInfo[] fileInfos = new DirectoryInfo(Application.persistentDataPath).GetFiles("*");
            for(int i=0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.EndsWith(".auto"))
                {
                    return fileInfos[i].Name;
                }
            }
        }
        return "";
    }
    public static void CameraCapture(int i,Camera camera,Rect rect)
    {
        //不存在文件夹就新建
        if (!Directory.Exists(Save.shotPath))
            Directory.CreateDirectory(Save.shotPath);
                string path = Path.Combine(Save.shotPath, $"{i}.png");
        int w = (int)rect.width;
        int h = (int)rect.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;
        Texture2D t2D = new Texture2D(w, h, TextureFormat.RGB24, true);
        t2D.ReadPixels(rect, 0, 0);
        t2D.Apply();
        byte[] bytes = t2D.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        camera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
    }
    public static Sprite LoadShot(int i)
    {
        var path = Path.Combine(shotPath, $"{i}.png");
        Texture2D t = new Texture2D(640, 360);
        t.LoadImage(GetImgByte(path));
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }
    static byte[] GetImgByte(string path)
    {
        FileStream s = new FileStream(path, FileMode.Open);
        byte[] imgByte = new byte[s.Length];
        s.Read(imgByte, 0, imgByte.Length);
        s.Close();
        return imgByte;
    }
    public static void DeleteShot(int i)
    {
        var path = Path.Combine(shotPath, $"{i}.png");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"删除截图{i}");
        }
    }
    #region 清空
#if UNITY_EDITOR

    [UnityEditor.MenuItem("Delete/Records List")]
    public static void DeleteRecord()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        Debug.Log("已经清空存档列表");
    }
    [UnityEditor.MenuItem("Delete/Record List")]
    public static void DeletePlayerData()
    {
        ClearDirectory(Application.persistentDataPath);
        Debug.Log("已清空玩家数据");
    }
    [UnityEditor.MenuItem("Delete/Shot")]
    public static void DeleteScreenShot()
    {
        ClearDirectory(shotPath);
        Debug.Log("已清空截图");
    }
     static void ClearDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            FileInfo[] f = new DirectoryInfo(path).GetFiles("*");
            for(int i = 0; i < f.Length; i++)
            {
                Debug.Log($"删除{f[i].Name}");
                File.Delete(f[i].FullName); 
            }
        }
    }
    [UnityEditor.MenuItem("Delete/All")]
    public static void DeleteAll()
    {
        DeletePlayerData();
        DeleteRecord();
        DeleteScreenShot();
    }
#endif
    #endregion

}
