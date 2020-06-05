﻿using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BatchMuxerNative.Properties;

namespace BatchMuxerNative.Modules
{
    class Functions
    {
        public static bool CheckPathAndLanguage(string path,string language="en",bool directRun=false)
        {
                if (directRun & !string.IsNullOrEmpty(Settings.Default.mkvmergePATH) &
                    File.Exists(Settings.Default.mkvmergePATH + @"\mkvmerge.exe"))
                {
                    return true;
                }
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show("Mkvmerge path not set");
                    return false;
                }

                if (!Directory.Exists(path)||!File.Exists(path + @"\mkvmerge.exe"))
                {
                    MessageBox.Show("MKV merge path is inCorrect");
                    return false;
                }

                if (language != null) return true;
                    MessageBox.Show("Please select correct language");
                return false;

            
        }

        public static DataTable CreateLanguageDt()
        {
            string languagecodes =

                "aa,ab,ae,af,ak,am,an,ar,as,av,ay,az,ba,be,bg,bh,bi,bm,bn,bo,br,bs,ca,ce,ch,co,cr,cs,cu,cv,cy,da,de,dv,dz,ee,el,en,eo,es,et,eu,fa,ff,fi,fj,fo,fr,fy,ga,gd,gl,gn,gu,gv,ha,he,hi,ho,hr,ht,hu,hy,hz,ia,id,ie,ig,ii,ik,io,is,it,iu,ja,jv,ka,kg,ki,kj,kk,kl,km,kn,ko,kr,ks,ku,kv,kw,ky,la,lb,lg,li,ln,lo,lt,lu,lv,mg,mh,mi,mk,ml,mn,mr,ms,mt,my,na,nb,nd,ne,ng,nl,nn,no,nr,nv,ny,oc,oj,om,or,os,pa,pi,pl,ps,pt,qu,rm,rn,ro,ru,rw,sa,sc,sd,se,sg,si,sk,sl,sm,sn,so,sq,sr,ss,st,su,sv,sw,ta,te,tg,th,ti,tk,tl,tn,to,tr,ts,tt,tw,ty,ug,uk,ur,uz,ve,vi,vo,wa,wo,xh,yi,yo,za,zh,zu";
            string languageNames =
                "Afar,Abkhazian,Avestan,Afrikaans,Akan,Amharic,Aragonese,Arabic,Assamese,Avaric,Aymara,Azerbaijani,Bashkir,Belarusian,Bulgarian,Bihari languages,Bislama,Bambara,Bengali,Tibetan,Breton,Bosnian,Catalan; Valencian,Chechen,Chamorro,Corsican,Cree,Czech,Church Slavic; Old Slavonic; Church Slavonic; Old Bulgarian; Old Church Slavonic,Chuvash,Welsh,Danish,German,Divehi; Dhivehi; Maldivian,Dzongkha,Ewe,Greek; Modern (1453-),English,Esperanto,Spanish; Castilian,Estonian,Basque,Persian,Fulah,Finnish,Fijian,Faroese,French,Western Frisian,Irish,Gaelic; Scottish Gaelic,Galician,Guarani,Gujarati,Manx,Hausa,Hebrew,Hindi,Hiri Motu,Croatian,Haitian; Haitian Creole,Hungarian,Armenian,Herero,Interlingua (International Auxiliary Language Association),Indonesian,Interlingue; Occidental,Igbo,Sichuan Yi; Nuosu,Inupiaq,Ido,Icelandic,Italian,Inuktitut,Japanese,Javanese,Georgian,Kongo,Kikuyu; Gikuyu,Kuanyama; Kwanyama,Kazakh,Kalaallisut; Greenlandic,Central Khmer,Kannada,Korean,Kanuri,Kashmiri,Kurdish,Komi,Cornish,Kirghiz; Kyrgyz,Latin,Luxembourgish; Letzeburgesch,Ganda,Limburgan; Limburger; Limburgish,Lingala,Lao,Lithuanian,Luba-Katanga,Latvian,Malagasy,Marshallese,Maori,Macedonian,Malayalam,Mongolian,Marathi,Malay,Maltese,Burmese,Nauru,BokmÃ¥l;  Norwegian; Norwegian BokmÃ¥l,Ndebele;  North; North Ndebele,Nepali,Ndonga,Dutch; Flemish,Norwegian Nynorsk; Nynorsk;  Norwegian,Norwegian,Ndebele;  South; South Ndebele,Navajo; Navaho,Chichewa; Chewa; Nyanja,Occitan (post 1500); ProvenÃ§al,Ojibwa,Oromo,Oriya,Ossetian; Ossetic,Panjabi; Punjabi,Pali,Polish,Pushto; Pashto,Portuguese,Quechua,Romansh,Rundi,Romanian; Moldavian; Moldovan,Russian,Kinyarwanda,Sanskrit,Sardinian,Sindhi,Northern Sami,Sango,Sinhala; Sinhalese,Slovak,Slovenian,Samoan,Shona,Somali,Albanian,Serbian,Swati,Sotho;  Southern,Sundanese,Swedish,Swahili,Tamil,Telugu,Tajik,Thai,Tigrinya,Turkmen,Tagalog,Tswana,Tonga (Tonga Islands),Turkish,Tsonga,Tatar,Twi,Tahitian,Uighur; Uyghur,Ukrainian,Urdu,Uzbek,Venda,Vietnamese,VolapÃ¼k,Walloon,Wolof,Xhosa,Yiddish,Yoruba,Zhuang; Chuang,Chinese,Zulu";
            string[] lcodes = Array.ConvertAll(languagecodes.Split(','), element => element.ToString());
            string[] lName = Array.ConvertAll(languageNames.Split(','), element => element.ToString());

            DataTable languages = new DataTable();
            languages.Columns.Add("Code");
            languages.Columns.Add("Name");
            for (int i = 0; i < lcodes.Length; i++)
            {
                languages.Rows.Add(lcodes[i], lName[i]);
            }

            return languages;
        }

        public static bool RenameFile( )
        {
            bool hasRenamed = false;
            foreach (var fl in Globals.Fi)
            {
                if (fl.Extension != ".mkv")
                {
                    File.Move(fl.FullName, fl.FullName.Replace(fl.Extension, ".mkv")); //caution!
                    hasRenamed = true;
                }
            }

            return hasRenamed;
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void DeleteAndMove(string path)
        {
            foreach (var fl in Globals.Fi)
            {
                var subtitle = fl.Name.Replace(fl.Extension, ".srt");
                var subtitlePath = fl.FullName.Replace(fl.Extension, ".srt");
                if (File.Exists(path + @"\" + subtitle) && File.Exists(path + @"\muxed\" + fl.Name))
                {
                    File.Delete(fl.FullName);
                    File.Delete(subtitlePath);
                    File.Move(path + @"\muxed\" + fl.Name, fl.FullName);
                }
            }
            if (IsDirectoryEmpty(path + @"\muxed"))
                Directory.Delete(path + @"\muxed");
        }

        public static void ProcessFile(FileInfo fl, string path)
        {
            var subtitle = fl.Name.Replace(fl.Extension, ".srt");
            if (fl.Extension != ".mkv")
            {
                File.Move(fl.FullName, fl.FullName.Replace(fl.Extension, ".mkv"));//caution!
            }

            if (File.Exists(path + @"\" + subtitle) &&
                !File.Exists(path + @"\muxed\" + fl.Name))
            {
                var output = "\"" + path + @"\muxed\" + fl.Name + "\"";
                var oProcess = new Process();
                var oStartInfo = new ProcessStartInfo("CMD.EXE")
                {
                    WorkingDirectory = path,

                    Arguments = "/c \"\"" + Globals.Mmpath + "\"" + " -o " + output +
                                " --default-track 0 --language 0:" + Globals.Language + " \"" + subtitle + "\" \"" + fl.Name + "\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                oProcess.StartInfo = oStartInfo;
                oProcess.Start();
                oProcess.WaitForExit();
            }
        }

    }
}
