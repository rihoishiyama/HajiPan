using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class AddressableBuilder
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // ビルドボタンをpush
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
    }

    private static void BuildPlayerHandler(BuildPlayerOptions options)
    {
        // 必要に応じてclean
        AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        // Addressableをビルド
        AddressableAssetSettings.BuildPlayerContent();
        // Playerをビルド
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
    }
}
