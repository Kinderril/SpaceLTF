[System.Serializable]
public abstract class BaseActionModul : BaseSupportModul
{
    public BaseActionModul(SimpleModulType type, int level)
        : base(type, level)
    {
#if UNITY_EDITOR
        UnityEditorID = 300000 + Utils.GetId();
#endif
    }
    // public abstract string DescSupport();

}
