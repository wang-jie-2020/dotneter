using System.Text.RegularExpressions;

namespace Hugin.Infrastructure.Helpers
{
    public static class StringHelper
    {
        public static bool IsDecimal(string strRegStr)
        {
            if (Regex.IsMatch(strRegStr, @"[^-\d.]") && Regex.IsMatch(strRegStr, @"\d"))
            {
                return false;
            }
            return Regex.IsMatch(strRegStr, @"^-?(\d*)(\.\d+){1}$");
        }

        public static bool IsInt(string strRegStr)
        {
            if (Regex.IsMatch(strRegStr, @"[^-\d.]") && Regex.IsMatch(strRegStr, @"\d"))
            {
                return false;
            }
            return Regex.IsMatch(strRegStr, @"^-?\d+$");
        }

        public static bool IsLetter(string strRegStr)
        {
            return Regex.IsMatch(strRegStr, "^[A-Za-z]+$");
        }

        public static bool IsLowerLetter(string strRegStr)
        {
            return Regex.IsMatch(strRegStr, "^[a-z]+$");
        }

        public static bool IsUpperLetter(string strRegStr)
        {
            return Regex.IsMatch(strRegStr, "^[A-Z]+$");
        }

        public static bool IsNumAndChar(string strRegStr)
        {
            return Regex.IsMatch(strRegStr, "^[A-Za-z0-9]+$");
        }

        public static bool IsChinese(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[\u4e00-\u9fa5]+$");
        }

        public static bool IsEmail(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsEnglish(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), "^[A-Za-z]+$");
        }

        public static bool IsIdCardNo(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"(^\d{18}$)|(^\d{15}$)");
        }

        public static bool IsIPv4(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"((?:(?:25[0-5]|2[0-4]\d|[01]?\d?\d)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d?\d))");
        }

        public static bool IsMacAddress(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), "^([0-9A-Fa-f]{2})(-[0-9A-Fa-f]{2}){5}|([0-9A-Fa-f]{2})(:[0-9A-Fa-f]{2}){5}$");
        }

        public static bool IsPhoneNumber(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^1\d{10}$");
        }

        public static bool IsUrl(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return Regex.IsMatch(obj.ToString(), @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$");
        }

        /// <summary>
        /// 校验密码复杂度
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckPassword(string password)
        {
            return (int)PasswordStrength(password) >= (int)Strength.Normal;
        }

        /// <summary>
        /// 密码强度
        /// </summary>
        private enum Strength
        {
            Invalid = 0, //无效密码
            Weak = 1, //低强度密码
            Normal = 2, //中强度密码
            Strong = 3 //高强度密码
        };

        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        private static Strength PasswordStrength(string password)
        {
            //空字符串强度值为0
            if (password == "") return Strength.Invalid;
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            if (iLtt == 0 && iSym == 0) return Strength.Weak; //纯数字密码
            if (iNum == 0 && iLtt == 0) return Strength.Weak; //纯符号密码
            if (iNum == 0 && iSym == 0) return Strength.Weak; //纯字母密码
            if (password.Length < 6) return Strength.Weak; //长度不大于6的密码
            if (iLtt == 0) return Strength.Normal; //数字和符号构成的密码
            if (iSym == 0) return Strength.Normal; //数字和字母构成的密码
            if (iNum == 0) return Strength.Normal; //字母和符号构成的密码
            if (password.Length <= 10) return Strength.Normal; //长度不大于10的密码
            return Strength.Strong; //由数字、字母、符号构成的密码
        }
    }
}
