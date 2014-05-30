public partial class NEditor : System.Web.UI.UserControl
{
    public string Text
    {
        get
        {
            return this.hdnEditorContent.Value;
        }
        set
        {
            this.hdnEditorContent.Value = value;
            this.hdnEditorContent.Value = value;
        }
    }
}
