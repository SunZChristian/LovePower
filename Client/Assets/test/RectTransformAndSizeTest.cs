using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformAndSizeTest : MonoBehaviour
{

    public RectTransform rect;
    public Slider slider;
    public int scale;

    public Button pickFile;

    public Text pathtxt;
    void Start() {
        slider.value = 0;
        pickFile.onClick.AddListener(PickFile);
    }

    void Update() {
        rect.offsetMin = new Vector2(slider.value * scale , 0);
        rect.offsetMax = new Vector2(-slider.value * scale , 0);
    }

    void PickFile() {
        NativeFilePicker.PickFile(Handler , new string [] { "*/*" });//所有文件
        //NativeFilePicker.PickFile(Handler , new string [] { "image/*" });//图片（png、jpg、jpeg）
        //NativeFilePicker.PickFile(Handler , new string [] { "Video files" });//视频（mp4、mov、wav、avi）
        //NativeFilePicker.PickFile(Handler , new string [] { "Audio files" });//视频（mp3、aac、flac）
    }

    void Handler(string path) {
        pathtxt.text = "path:" + path;
    }

    private void OnDestroy() {
        pickFile.onClick.RemoveAllListeners();
    }
}
