using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using LSOne.DataLayer.GenericConnector.DataEntities;

namespace LSOne.DataLayer.GenericConnector
{
    public class Setting
    {
        public Setting(bool userSettingExists, string userSetting, string systemSetting, SettingType settingType)
        {
            this.UserSetting = userSetting;
            this.UserSettingExists = userSettingExists;
            this.SystemSetting = systemSetting;
            this.SettingType = settingType;
        }
        [StringLength(50)]
        public string UserSetting { get; set; }

        public string LongUserSetting { get; set; }

        [StringLength(50)]
        public string SystemSetting { get; private set; }

        public bool UserSettingExists { get; set; }

        public SettingType SettingType { get; set; }

        public string Value
        {
            get
            {
                return UserSettingExists ? (!string.IsNullOrEmpty(LongUserSetting) ? LongUserSetting :  UserSetting) : SystemSetting;
            }
            set
            {
                if (UserSettingExists)
                {
                    UserSetting = value;
                }
                else
                {
                    SystemSetting = value;
                }
            }
        }

        public int IntValue
        {
            get
            {
                return UserSettingExists ? Convert.ToInt32(UserSetting) : Convert.ToInt32(SystemSetting);
            }
            set
            {
                if (UserSettingExists)
                {
                    UserSetting = value.ToString();
                }
                else
                {
                    SystemSetting = value.ToString();
                }
            }
        }

        public bool BoolValue
        {
            get
            {
                return UserSettingExists ? Convert.ToInt32(UserSetting) != 0 : Convert.ToInt32(SystemSetting) != 0;
            }
            set
            {
                if (UserSettingExists)
                {
                    UserSetting = value ? "1" : "0";
                }
                else
                {
                    SystemSetting = value ? "1" : "0";
                }
            }
        }

        public Rectangle RectangleValue
        {
            get
            {
                string[] items = Value.Split(',');
                if (items.Length == 4)
                {
                    try
                    {
                        var rectangle = new Rectangle
                                            {
                                                X = Int32.Parse(items[0].Substring(2)),
                                                Y = Int32.Parse(items[1].Substring(2)),
                                                Width = Int32.Parse(items[2].Substring(2)),
                                                Height = Int32.Parse(items[3].Substring(2))
                                            };

                        return rectangle;
                    }
                    catch
                    {
                    }
                }
                return Rectangle.Empty;
            }
            set
            {
                Value = String.Format("X={0},Y={1},W={2},H={3}",
                                      value.X,
                                      value.Y,
                                      value.Width,
                                      value.Height);
            }
        }

        public T Get<T>() where T : class 
        {
            return (T) Convert.ChangeType(UserSettingExists ? UserSetting : SystemSetting, typeof(T));
        }

        public void Set<T>(T userSetting, T systemSetting) where T : class
        {
            if (userSetting != null)
            {
                UserSetting = userSetting.ToString();
            }
            if (systemSetting != null)
            {
                SystemSetting = systemSetting.ToString();
            }
        }
    }
}

