namespace LSOne.DataLayer.BusinessObjects.Images
{
    public enum ImageTypeEnum
    {
        Other = 0,
        Button = 1,
        Logo = 2,
        ReceiptLogo = 3,
        Inventory = 4
    }

    public static class ImageTypeHelper
    {
        public static string ImageTypeEnumToString(ImageTypeEnum imageType)
        {
            switch (imageType)
            {
                case ImageTypeEnum.Other:
                    return Properties.Resources.ImageTypeOther;
                case ImageTypeEnum.Button:
                    return Properties.Resources.ImageTypeButton;
                case ImageTypeEnum.Logo:
                    return Properties.Resources.ImageTypeLogo;
                case ImageTypeEnum.ReceiptLogo:
                    return Properties.Resources.ImageTypeReceiptLogo;
                case ImageTypeEnum.Inventory:
                    return Properties.Resources.ImageTypeInventory;
                default:
                    return string.Empty;
            }
        }
    }
}
