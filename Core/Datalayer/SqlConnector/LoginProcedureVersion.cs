namespace LSOne.DataLayer.SqlConnector
{
    internal enum LoginProcedureVersion
    {
        Version1_1 = 0,
        Version1_2 = 1,
        Version1_3 = 2
    }

    internal static class LoginProcedureVersionHelper
    {
        public static string ToString(LoginProcedureVersion x)
        {
            switch(x)
            {
                case LoginProcedureVersion.Version1_3: return "spSECURITY_Login_1_3";
                case LoginProcedureVersion.Version1_2: return "spSECURITY_Login_1_2";
                case LoginProcedureVersion.Version1_1:
                default: return "spSECURITY_Login_1_1";
            }
        }
    }
}
