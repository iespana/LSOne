namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum FormPart
    {
        Header = 0, 
        Line = 1, 
        Footer = 2
    }

    public enum valign
    {
        left, 
        center, 
        right
    }

    public enum PrintBehaviors
    {
        /// <summary>
        /// 0
        /// </summary>
        AlwaysPrint = 0,
        /// <summary>
        /// 1
        /// </summary>
        NeverPrint = 1,
        /// <summary>
        /// 2
        /// </summary>
        PromptUser = 2
        /*
        /// <summary>
        /// 3
        /// </summary>
        ShowPreview = 3
        */
    }
}
