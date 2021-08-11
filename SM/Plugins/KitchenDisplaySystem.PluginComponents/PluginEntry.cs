using System;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents
{
    public class PluginEntry
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        public static Guid DefaultChitGuid = new Guid("b07cbf50-10b4-11e3-8ffd-0800200c9a66");
        public static Guid OrderPaneGuid = new Guid("45DD00D2-9647-4479-AF5D-D06C987ACC7C");
        public static Guid ChitGuid = new Guid("a399fdb0-10b5-11e3-8ffd-0800200c9a66");
        public static Guid DefaultChitLineGuid = new Guid("5db8ae80-10bb-11e3-8ffd-0800200c9a66");
        public static Guid ChitLineGuid = new Guid("bc1469b0-10bb-11e3-8ffd-0800200c9a66");
        public static Guid ChitHeaderFooterGuid = new Guid("8b101570-10bc-11e3-8ffd-0800200c9a66");
        public static Guid ChitHeaderFooterPartGuid = new Guid("48d15020-10bc-11e3-8ffd-0800200c9a66");
        public static Guid ChitItemModifierGlyphGuid = new Guid("01901510-10bd-11e3-8ffd-0800200c9a66");
        public static Guid OrderMarkedAlertGuid = new Guid("1f3aad0a-e07a-4e9b-9cae-abb873253ae8");
    }
}
