public static class SceneData {
    public enum SceneType {
        None = -1,
        Main_Community = 0,
        Main_Seagrass = 1,
        Test =2
    }
    public static string GetSceneName(SceneType type) {
        switch (type) {
            case SceneType.None:
                return "None";
            case SceneType.Main_Community:
                return "Main_Community";
            case SceneType.Main_Seagrass:
                return "Main_Seagrass";
            case SceneType.Test:
                return "Test";
            default:
                return "None";
        }
    }
}