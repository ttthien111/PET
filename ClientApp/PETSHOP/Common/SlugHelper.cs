using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASPCore_Final.Services
{
    public static class SlugHelper
    {
        //public static string GenerateSlug(this string phrase)
        //{
        //    string str = phrase.RemoveAccent().ToLower();
        //    // invalid chars           
        //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        //    // convert multiple spaces into one space   
        //    str = Regex.Replace(str, @"\s+", " ").Trim();
        //    // cut and trim 
        //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
        //    return str;
        //}

        //public static string RemoveAccent(this string txt)
        //{
        //    byte[] bytes = System.Text.Encoding.GetEncoding("Windows-1251").GetBytes(txt);
        //    return System.Text.Encoding.ASCII.GetString(bytes);
        //}


        public static bool CheckExtension(string extension)
        {
            string[] allowedExtensions = new[] { ".jpg", ".png" };
            return allowedExtensions.Contains(extension);
        }


        public static string GetFriendlyTitle(string title, bool remapToAscii = false, int maxlength = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }

            int length = title.Length;
            bool prevdash = false;
            System.Text.StringBuilder stringBuilder = new StringBuilder(length);
            char c;

            for (int i = 0; i < length; ++i)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if ((c == ' ') || (c == ',') || (c == '.') || (c == '/') ||
                  (c == '\\') || (c == '-') || (c == '_') || (c == '='))
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int previousLength = stringBuilder.Length;
                    stringBuilder.Append(RemapInternationalCharToAscii(c));
                    prevdash = false;
                }

                if (i == maxlength)
                {
                    break;
                }
            }

            if (prevdash)
            {
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Remaps the international character to their equivalent ASCII characters. See
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="character">The character to remap to its ASCII equivalent.</param>
        /// <returns>The remapped character</returns>
        private static string RemapInternationalCharToAscii(char character)
        {
            string s = character.ToString().ToLowerInvariant();
            if ("àạáãảấầâậẩẫăắằẳặẵ".Contains(s))
            {
                return "a";
            }
            else if ("éèẽẻẹêếềễểệ".Contains(s))
            {
                return "e";
            }
            else if ("ìíỉĩị".Contains(s))
            {
                return "i";
            }
            else if ("òóỏọõơớờởỡợôốồỗổộ".Contains(s))
            {
                return "o";
            }
            else if ("ùúủũụưứừữửự".Contains(s))
            {
                return "u";
            }
            else if ("ýỳỷỹỵ".Contains(s))
            {
                return "y";
            }
            else if ("đ".Contains(s))
            {
                return "d";
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
