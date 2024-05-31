using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using System.IO;

public class ProfilerController : MonoBehaviour {

    string statsText;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder playerLoopTimeRecorder;
    ProfilerRecorder cameraRenderTimeRecorder;
    ProfilerRecorder batchesCountRecorder;
    ProfilerRecorder setPassCallsCountRecorder;
    ProfilerRecorder texturesChangesCountRecorder;

    //ProfilerRecorder gcMemoryRecorder;
    //ProfilerRecorder combineJobResultsTimeRecorder;
    //ProfilerRecorder renderThreadTimeRecorder;
    //ProfilerRecorder waitForGPUTimeRecorder;

    List<string> systemMemoryList = new List<string>();
    List<string> mainThreadTimeList = new List<string>();
    List<string> playerLoopTimeList = new List<string>();
    List<string> cameraRenderTimeList = new List<string>();
    List<string> batchesCountList = new List<string>();
    List<string> setPassCallsCountList = new List<string>();
    List<string> textureChangesCountList = new List<string>();
    List<string> frameIndexList = new List<string>();
    List<string> frameRateList = new List<string>();

    bool recordingData = false;

    static double GetRecorderFrameAverage(ProfilerRecorder recorder) {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0) {
            return 0;
        }

        double r = 0;
        var samples = new List<ProfilerRecorderSample>(samplesCount);
        recorder.CopyTo(samples);
        for (var i = 0; i < samples.Count; ++i) {
            r += samples[i].Value;
        }
        r /= samplesCount;

        return r;
    }

    void OnEnable() {
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread");
        playerLoopTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "PlayerLoop");
        cameraRenderTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Camera.Render");
        batchesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        setPassCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        texturesChangesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Changes Count");

        //renderThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Render Thread");
        //combineJobResultsTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CombineJobResults");
        //gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        //waitForGPUTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "GfxDeviceD3D12.WaitForGPU");
    }

    void OnDisable() {
        systemMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
        playerLoopTimeRecorder.Dispose();
        cameraRenderTimeRecorder.Dispose();
        batchesCountRecorder.Dispose();
        setPassCallsCountRecorder.Dispose();
        texturesChangesCountRecorder.Dispose();

        //renderThreadTimeRecorder.Dispose();
        //combineJobResultsTimeRecorder.Dispose();
        //gcMemoryRecorder.Dispose();
        //waitForGPUTimeRecorder.Dispose();
    }

    void Update() {
        WriteDataToFile();
        //WriteToConsole();
    }

    private void WriteDataToFile() {

        if (Input.GetKeyDown(KeyCode.P)) {
            recordingData = !recordingData;
        }

        if (recordingData) {
            systemMemoryList.Add((systemMemoryRecorder.LastValue / (1024f * 1024f)).ToString());
            mainThreadTimeList.Add(mainThreadTimeRecorder.LastValue.ToString());
            playerLoopTimeList.Add(playerLoopTimeRecorder.LastValue.ToString());
            cameraRenderTimeList.Add(cameraRenderTimeRecorder.LastValue.ToString());
            batchesCountList.Add(batchesCountRecorder.LastValue.ToString());
            setPassCallsCountList.Add(setPassCallsCountRecorder.LastValue.ToString());
            textureChangesCountList.Add(texturesChangesCountRecorder.LastValue.ToString());
            frameIndexList.Add(Time.frameCount.ToString());
            frameRateList.Add((1 / Time.deltaTime).ToString());
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            for (int i = 0; i < frameIndexList.Count; i++) {
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\SystemMemory.txt", systemMemoryList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\MainThreadTime.txt", mainThreadTimeList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\PlayerLoopTime.txt", playerLoopTimeList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\CameraRenderTime.txt", cameraRenderTimeList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\BatchesCount.txt", batchesCountList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\SetPassCallsCount.txt", setPassCallsCountList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\TextureChangesCount.txt", textureChangesCountList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\FrameIndex.txt", frameIndexList);
                System.IO.File.WriteAllLines(@"D:\Repos\Unity3D Course\Undead Elusion - Data\No Optimizations\FrameRate.txt", frameRateList);
            }
        }
    }

    private void WriteToConsole() {
        Debug.Log("-------------------------------------------------------------------------");
        Debug.Log("Frame number: " + Time.frameCount);
        Debug.Log($"Frame Time 1: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
        Debug.Log("Frame Time 2: " + mainThreadTimeRecorder.LastValue + " ns");

        Debug.Log($"Player Loop Time: {playerLoopTimeRecorder.LastValue * (1e-6f):F2} ms");

        Debug.Log($"Camera Render Time: {cameraRenderTimeRecorder.LastValue * (1e-6f):F2} ms");

        Debug.Log($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");

        Debug.Log("Batches Count: " + batchesCountRecorder.LastValue);
        Debug.Log("SetPass Calls Count: " + setPassCallsCountRecorder.LastValue);
        Debug.Log("Textures Changes Count: " + texturesChangesCountRecorder.LastValue);

        Debug.Log($"Current Frame Rate: {1 / Time.deltaTime} FPS");

        //Debug.Log($"Wait for GPU Time: {waitForGPUTimeRecorder.LastValue * (1e-6f):F2} ms");

        //Debug.Log("Render Thread Time: " + renderThreadTimeRecorder.LastValue + " ns");

        //Debug.Log($"Combine all jobs time: {combineJobResultsTimeRecorder.LastValue * (1e-6f):F1} ms");
        //Debug.Log("Combine all jobs time: " + combineJobResultsTimeRecorder.LastValue + " ns");

        // Below ones could still be usuable, just disabled to make the rest more clear
        //Debug.Log($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
    }
}
