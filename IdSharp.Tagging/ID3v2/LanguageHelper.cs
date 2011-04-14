using System.Collections.Generic;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// Static helper class for ISO-639-2 languages.
    /// </summary>
    public static class LanguageHelper
    {
        private static readonly Dictionary<string, string> m_Languages;

        #region <<< Sorted languages >>>

        private static readonly string[] m_SortedLanguages = new[]{
                                                                             "Abkhazian",
                                                                             "Achinese",
                                                                             "Acoli",
                                                                             "Adangme",
                                                                             "Afar",
                                                                             "Afrihili",
                                                                             "Afrikaans",
                                                                             "Afro-Asiatic (Other)",
                                                                             "Akan",
                                                                             "Akkadian",
                                                                             "Albanian",
                                                                             "Aleut",
                                                                             "Algonquian Languages",
                                                                             "Altaic (Other)",
                                                                             "Amharic",
                                                                             "Apache Languages",
                                                                             "Arabic",
                                                                             "Aramaic",
                                                                             "Arapaho",
                                                                             "Araucanian",
                                                                             "Arawak",
                                                                             "Armenian",
                                                                             "Artificial (Other)",
                                                                             "Assamese",
                                                                             "Athapascan Languages",
                                                                             "Austronesian (Other)",
                                                                             "Avaric",
                                                                             "Avestan",
                                                                             "Awadhi",
                                                                             "Aymara",
                                                                             "Azerbaijani",
                                                                             "Aztec",
                                                                             "Balinese",
                                                                             "Baltic (Other)",
                                                                             "Baluchi",
                                                                             "Bambara",
                                                                             "Bamileke Languages",
                                                                             "Banda",
                                                                             "Bantu (Other)",
                                                                             "Basa",
                                                                             "Bashkir",
                                                                             "Basque",
                                                                             "Beja",
                                                                             "Bemba",
                                                                             "Bengali",
                                                                             "Berber (Other)",
                                                                             "Bhojpuri",
                                                                             "Bihari",
                                                                             "Bikol",
                                                                             "Bini",
                                                                             "Bislama",
                                                                             "Bosnian",
                                                                             "Braj",
                                                                             "Breton",
                                                                             "Buginese",
                                                                             "Bulgarian",
                                                                             "Buriat",
                                                                             "Burmese",
                                                                             "Byelorussian",
                                                                             "Caddo",
                                                                             "Carib",
                                                                             "Catalan",
                                                                             "Caucasian (Other)",
                                                                             "Cebuano",
                                                                             "Celtic (Other)",
                                                                             "Central American Indian (Other)",
                                                                             "Chagatai",
                                                                             "Chamorro",
                                                                             "Chechen",
                                                                             "Cherokee",
                                                                             "Cheyenne",
                                                                             "Chibcha",
                                                                             "Chinese",
                                                                             "Chinook jargon",
                                                                             "Choctaw",
                                                                             "Church Slavic",
                                                                             "Chuvash",
                                                                             "Coptic",
                                                                             "Cornish",
                                                                             "Corsican",
                                                                             "Cree",
                                                                             "Creek",
                                                                             "Creoles and Pidgins (Other)",
                                                                             "Creoles and Pidgins, English-based (Other)",
                                                                             "Creoles and Pidgins, French-based (Other)",
                                                                             "Creoles and Pidgins, Portuguese-based (Other)",
                                                                             "Croatian",
                                                                             "Cushitic (Other)",
                                                                             "Czech",
                                                                             "Dakota",
                                                                             "Danish",
                                                                             "Delaware",
                                                                             "Dinka",
                                                                             "Divehi",
                                                                             "Dogri",
                                                                             "Dravidian (Other)",
                                                                             "Duala",
                                                                             "Dutch",
                                                                             "Dutch, Middle (ca. 1050-1350)",
                                                                             "Dyula",
                                                                             "Dzongkha",
                                                                             "Efik",
                                                                             "Egyptian (Ancient)",
                                                                             "Ekajuk",
                                                                             "Elamite",
                                                                             "English",
                                                                             "English, Old (ca. 450-1100)",
                                                                             "Eskimo (Other)",
                                                                             "Esperanto",
                                                                             "Estonian",
                                                                             "Ewe",
                                                                             "Ewondo",
                                                                             "Fang",
                                                                             "Fanti",
                                                                             "Faroese",
                                                                             "Fijian",
                                                                             "Finnish",
                                                                             "Finno-Ugrian (Other)",
                                                                             "Fon",
                                                                             "French",
                                                                             "French, Middle (ca. 1400-1600)",
                                                                             "French, Old (842- ca. 1400)",
                                                                             "Frisian",
                                                                             "Fulah",
                                                                             "Ga",
                                                                             "Gaelic (Scots)",
                                                                             "Gallegan",
                                                                             "Ganda",
                                                                             "Gayo",
                                                                             "Geez",
                                                                             "Georgian",
                                                                             "German",
                                                                             "German, Middle High (ca. 1050-1500)",
                                                                             "German, Old High (ca. 750-1050)",
                                                                             "Germanic (Other)",
                                                                             "Gilbertese",
                                                                             "Gondi",
                                                                             "Gothic",
                                                                             "Grebo",
                                                                             "Greek",
                                                                             "Greek, Ancient (to 1453)",
                                                                             "Greenlandic",
                                                                             "Guarani",
                                                                             "Gujarati",
                                                                             "Haida",
                                                                             "Hausa",
                                                                             "Hawaiian",
                                                                             "Hebrew",
                                                                             "Herero",
                                                                             "Hiligaynon",
                                                                             "Himachali",
                                                                             "Hindi",
                                                                             "Hiri Motu",
                                                                             "Hungarian",
                                                                             "Hupa",
                                                                             "Iban",
                                                                             "Icelandic",
                                                                             "Igbo",
                                                                             "Ijo",
                                                                             "Iloko",
                                                                             "Indic (Other)",
                                                                             "Indo-European (Other) Interlingue",
                                                                             "Indonesian",
                                                                             "Interlingua (International Auxiliary language Association)",
                                                                             "Inuktitut",
                                                                             "Inupiak",
                                                                             "Iranian (Other)",
                                                                             "Irish",
                                                                             "Irish, Middle (900 - 1200)",
                                                                             "Irish, Old (to 900)",
                                                                             "Iroquoian uages",
                                                                             "Italian",
                                                                             "Japanese",
                                                                             "Javanese",
                                                                             "Judeo-Arabic",
                                                                             "Judeo-Persian",
                                                                             "Kabyle",
                                                                             "Kachin",
                                                                             "Kamba",
                                                                             "Kannada",
                                                                             "Kanuri",
                                                                             "Kara-Kalpak",
                                                                             "Karen",
                                                                             "Kashmiri",
                                                                             "Kawi",
                                                                             "Kazakh",
                                                                             "Khasi",
                                                                             "Khmer",
                                                                             "Khoisan (Other)",
                                                                             "Khotanese",
                                                                             "Kikuyu",
                                                                             "Kinyarwanda",
                                                                             "Kirghiz",
                                                                             "Komi",
                                                                             "Kongo",
                                                                             "Konkani",
                                                                             "Korean",
                                                                             "Kpelle",
                                                                             "Kru",
                                                                             "Kuanyama",
                                                                             "Kumyk",
                                                                             "Kurdish",
                                                                             "Kurukh",
                                                                             "Kusaie",
                                                                             "Kutenai",
                                                                             "Ladino",
                                                                             "Lahnda",
                                                                             "Lamba",
                                                                             "Langue d'Oc (post 1500)",
                                                                             "Lao",
                                                                             "Latin",
                                                                             "Latvian",
                                                                             "Letzeburgesch",
                                                                             "Lezghian",
                                                                             "Lingala",
                                                                             "Lithuanian",
                                                                             "Lozi",
                                                                             "Luba-Katanga",
                                                                             "Luiseno",
                                                                             "Lunda",
                                                                             "Luo (Kenya and Tanzania)",
                                                                             "Macedonian",
                                                                             "Macedonian Makasar",
                                                                             "Madurese",
                                                                             "Magahi",
                                                                             "Maithili",
                                                                             "Malagasy",
                                                                             "Malay",
                                                                             "Malayalam",
                                                                             "Maltese",
                                                                             "Mandingo",
                                                                             "Manipuri",
                                                                             "Manobo Languages",
                                                                             "Manx",
                                                                             "Maori",
                                                                             "Marathi",
                                                                             "Mari",
                                                                             "Marshall",
                                                                             "Marwari",
                                                                             "Masai",
                                                                             "Mayan Languages",
                                                                             "Mende",
                                                                             "Micmac",
                                                                             "Middle English (ca. 1100-1500)",
                                                                             "Minangkabau",
                                                                             "Miscellaneous (Other)",
                                                                             "Mohawk",
                                                                             "Moldavian",
                                                                             "Mongo",
                                                                             "Mongolian",
                                                                             "Mon-Kmer (Other)",
                                                                             "Mossi",
                                                                             "Multiple Languages",
                                                                             "Munda Languages",
                                                                             "Nauru",
                                                                             "Navajo",
                                                                             "Ndebele, North",
                                                                             "Ndebele, South",
                                                                             "Ndongo",
                                                                             "Nepali",
                                                                             "Newari",
                                                                             "Niger-Kordofanian (Other)",
                                                                             "Nilo-Saharan (Other)",
                                                                             "Niuean",
                                                                             "Norse, Old",
                                                                             "North American Indian (Other)",
                                                                             "Norwegian",
                                                                             "Norwegian (Nynorsk)",
                                                                             "Nubian Languages",
                                                                             "Nyamwezi",
                                                                             "Nyanja",
                                                                             "Nyankole",
                                                                             "Nyoro",
                                                                             "Nzima",
                                                                             "Ojibwa",
                                                                             "Oriya",
                                                                             "Oromo",
                                                                             "Osage",
                                                                             "Ossetic",
                                                                             "Otomian Languages",
                                                                             "Pahlavi",
                                                                             "Palauan",
                                                                             "Pali",
                                                                             "Pampanga",
                                                                             "Pangasinan",
                                                                             "Panjabi",
                                                                             "Papiamento",
                                                                             "Papuan-Australian (Other)",
                                                                             "Persian",
                                                                             "Persian, Old (ca 600 - 400 B.C.)",
                                                                             "Phoenician",
                                                                             "Polish",
                                                                             "Ponape",
                                                                             "Portuguese",
                                                                             "Prakrit uages",
                                                                             "Provencal, Old (to 1500)",
                                                                             "Pushto",
                                                                             "Quechua",
                                                                             "Rajasthani",
                                                                             "Rarotongan",
                                                                             "Rhaeto-Romance",
                                                                             "Romance (Other)",
                                                                             "Romanian",
                                                                             "Romany",
                                                                             "Rundi",
                                                                             "Russian",
                                                                             "Salishan Languages",
                                                                             "Samaritan Aramaic",
                                                                             "Sami Languages",
                                                                             "Samoan",
                                                                             "Sandawe",
                                                                             "Sango",
                                                                             "Sanskrit",
                                                                             "Sardinian",
                                                                             "Scots",
                                                                             "Selkup",
                                                                             "Semitic (Other)",
                                                                             "Serbian",
                                                                             "Serer",
                                                                             "Shan",
                                                                             "Shona",
                                                                             "Sidamo",
                                                                             "Siksika",
                                                                             "Sindhi",
                                                                             "Singhalese",
                                                                             "Sino-Tibetan (Other)",
                                                                             "Siouan Languages",
                                                                             "Siswant Swazi",
                                                                             "Slavic (Other)",
                                                                             "Slovak",
                                                                             "Slovenian",
                                                                             "Sogdian",
                                                                             "Somali",
                                                                             "Songhai",
                                                                             "Sorbian Languages",
                                                                             "Sotho, Northern",
                                                                             "Sotho, Southern",
                                                                             "South American Indian (Other)",
                                                                             "Spanish",
                                                                             "Sudanese",
                                                                             "Sukuma",
                                                                             "Sumerian",
                                                                             "Susu",
                                                                             "Swahili",
                                                                             "Swedish",
                                                                             "Syriac",
                                                                             "Tagalog",
                                                                             "Tahitian",
                                                                             "Tajik",
                                                                             "Tamashek",
                                                                             "Tamil",
                                                                             "Tatar",
                                                                             "Telugu",
                                                                             "Tereno",
                                                                             "Thai",
                                                                             "Tibetan",
                                                                             "Tigre",
                                                                             "Tigrinya",
                                                                             "Timne",
                                                                             "Tivi",
                                                                             "Tlingit",
                                                                             "Tokelau",
                                                                             "Tonga (Nyasa)",
                                                                             "Tonga (Tonga Islands)",
                                                                             "Truk",
                                                                             "Tsimshian",
                                                                             "Tsonga",
                                                                             "Tswana",
                                                                             "Tumbuka",
                                                                             "Turkish",
                                                                             "Turkish, Ottoman (1500 - 1928)",
                                                                             "Turkmen",
                                                                             "Tuvinian",
                                                                             "Twi",
                                                                             "Ugaritic",
                                                                             "Uighur",
                                                                             "Ukrainian",
                                                                             "Umbundu",
                                                                             "Undetermined",
                                                                             "Urdu",
                                                                             "Uzbek",
                                                                             "Vai",
                                                                             "Venda",
                                                                             "Vietnamese",
                                                                             "Volapük",
                                                                             "Votic",
                                                                             "Wakashan Languages",
                                                                             "Walamo",
                                                                             "Waray",
                                                                             "Washo",
                                                                             "Welsh",
                                                                             "Wolof",
                                                                             "Xhosa",
                                                                             "Yakut",
                                                                             "Yao",
                                                                             "Yap",
                                                                             "Yiddish",
                                                                             "Yoruba",
                                                                             "Zapotec",
                                                                             "Zenaga",
                                                                             "Zhuang",
                                                                             "Zulu",
                                                                             "Zuni"
                                                                         };
        #endregion <<< Sorted languages >>>

        /// <summary>
        /// Gets a dictionary containing 3-letter ISO-639-2 language codes as the key and the English version of the language name as the value.
        /// </summary>
        /// <value>A dictionary containing 3-letter ISO-639-2 language codes as the key and the English version of the language name as the value.</value>
        public static Dictionary<string, string> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        /// <summary>
        /// Gets a string array of the English version of the language names sorted alphabetically.
        /// </summary>
        /// <value>A string array of the English version of the language names sorted alphabetically.</value>
        public static string[] SortedLanguages
        {
            get { return m_SortedLanguages; }
        }

        static LanguageHelper()
        {
            #region <<< Language code / English description dictionary

            m_Languages = new Dictionary<string, string>();
            m_Languages.Add("aar", "Afar");
            m_Languages.Add("abk", "Abkhazian");
            m_Languages.Add("ace", "Achinese");
            m_Languages.Add("ach", "Acoli");
            m_Languages.Add("ada", "Adangme");
            m_Languages.Add("afa", "Afro-Asiatic (Other)");
            m_Languages.Add("afh", "Afrihili");
            m_Languages.Add("afr", "Afrikaans");
            m_Languages.Add("aka", "Akan");
            m_Languages.Add("akk", "Akkadian");
            m_Languages.Add("alb", "Albanian");
            m_Languages.Add("ale", "Aleut");
            m_Languages.Add("alg", "Algonquian Languages");
            m_Languages.Add("amh", "Amharic");
            m_Languages.Add("ang", "English, Old (ca. 450-1100)");
            m_Languages.Add("apa", "Apache Languages");
            m_Languages.Add("ara", "Arabic");
            m_Languages.Add("arc", "Aramaic");
            m_Languages.Add("arm", "Armenian");
            m_Languages.Add("arn", "Araucanian");
            m_Languages.Add("arp", "Arapaho");
            m_Languages.Add("art", "Artificial (Other)");
            m_Languages.Add("arw", "Arawak");
            m_Languages.Add("asm", "Assamese");
            m_Languages.Add("ath", "Athapascan Languages");
            m_Languages.Add("ava", "Avaric");
            m_Languages.Add("ave", "Avestan");
            m_Languages.Add("awa", "Awadhi");
            m_Languages.Add("aym", "Aymara");
            m_Languages.Add("aze", "Azerbaijani");
            m_Languages.Add("bad", "Banda");
            m_Languages.Add("bai", "Bamileke Languages");
            m_Languages.Add("bak", "Bashkir");
            m_Languages.Add("bal", "Baluchi");
            m_Languages.Add("bam", "Bambara");
            m_Languages.Add("ban", "Balinese");
            m_Languages.Add("baq", "Basque");
            m_Languages.Add("bas", "Basa");
            m_Languages.Add("bat", "Baltic (Other)");
            m_Languages.Add("bej", "Beja");
            m_Languages.Add("bel", "Byelorussian");
            m_Languages.Add("bem", "Bemba");
            m_Languages.Add("ben", "Bengali");
            m_Languages.Add("ber", "Berber (Other)");
            m_Languages.Add("bho", "Bhojpuri");
            m_Languages.Add("bih", "Bihari");
            m_Languages.Add("bik", "Bikol");
            m_Languages.Add("bin", "Bini");
            m_Languages.Add("bis", "Bislama");
            m_Languages.Add("bla", "Siksika");
            m_Languages.Add("bnt", "Bantu (Other)");
            m_Languages.Add("bod", "Tibetan");
            m_Languages.Add("bra", "Braj");
            m_Languages.Add("bre", "Breton");
            m_Languages.Add("bua", "Buriat");
            m_Languages.Add("bug", "Buginese");
            m_Languages.Add("bul", "Bulgarian");
            m_Languages.Add("bur", "Burmese");
            m_Languages.Add("cad", "Caddo");
            m_Languages.Add("cai", "Central American Indian (Other)");
            m_Languages.Add("car", "Carib");
            m_Languages.Add("cat", "Catalan");
            m_Languages.Add("cau", "Caucasian (Other)");
            m_Languages.Add("ceb", "Cebuano");
            m_Languages.Add("cel", "Celtic (Other)");
            m_Languages.Add("ces", "Czech");
            m_Languages.Add("cha", "Chamorro");
            m_Languages.Add("chb", "Chibcha");
            m_Languages.Add("che", "Chechen");
            m_Languages.Add("chg", "Chagatai");
            m_Languages.Add("chi", "Chinese");
            m_Languages.Add("chm", "Mari");
            m_Languages.Add("chn", "Chinook jargon");
            m_Languages.Add("cho", "Choctaw");
            m_Languages.Add("chr", "Cherokee");
            m_Languages.Add("chu", "Church Slavic");
            m_Languages.Add("chv", "Chuvash");
            m_Languages.Add("chy", "Cheyenne");
            m_Languages.Add("cop", "Coptic");
            m_Languages.Add("cor", "Cornish");
            m_Languages.Add("cos", "Corsican");
            m_Languages.Add("cpe", "Creoles and Pidgins, English-based (Other)");
            m_Languages.Add("cpf", "Creoles and Pidgins, French-based (Other)");
            m_Languages.Add("cpp", "Creoles and Pidgins, Portuguese-based (Other)");
            m_Languages.Add("cre", "Cree");
            m_Languages.Add("crp", "Creoles and Pidgins (Other)");
            m_Languages.Add("cus", "Cushitic (Other)");
            m_Languages.Add("cym", "Welsh");
            m_Languages.Add("cze", "Czech");
            m_Languages.Add("dak", "Dakota");
            m_Languages.Add("dan", "Danish");
            m_Languages.Add("del", "Delaware");
            m_Languages.Add("deu", "German");
            m_Languages.Add("din", "Dinka");
            m_Languages.Add("div", "Divehi");
            m_Languages.Add("doi", "Dogri");
            m_Languages.Add("dra", "Dravidian (Other)");
            m_Languages.Add("dua", "Duala");
            m_Languages.Add("dum", "Dutch, Middle (ca. 1050-1350)");
            m_Languages.Add("dut", "Dutch");
            m_Languages.Add("dyu", "Dyula");
            m_Languages.Add("dzo", "Dzongkha");
            m_Languages.Add("efi", "Efik");
            m_Languages.Add("egy", "Egyptian (Ancient)");
            m_Languages.Add("eka", "Ekajuk");
            m_Languages.Add("ell", "Greek");
            m_Languages.Add("elx", "Elamite");
            m_Languages.Add("eng", "English");
            m_Languages.Add("enm", "Middle English (ca. 1100-1500)");
            m_Languages.Add("epo", "Esperanto");
            m_Languages.Add("esk", "Eskimo (Other)");
            m_Languages.Add("esl", "Spanish");
            m_Languages.Add("est", "Estonian");
            m_Languages.Add("eus", "Basque");
            m_Languages.Add("ewe", "Ewe");
            m_Languages.Add("ewo", "Ewondo");
            m_Languages.Add("fan", "Fang");
            m_Languages.Add("fao", "Faroese");
            m_Languages.Add("fas", "Persian");
            m_Languages.Add("fat", "Fanti");
            m_Languages.Add("fij", "Fijian");
            m_Languages.Add("fin", "Finnish");
            m_Languages.Add("fiu", "Finno-Ugrian (Other)");
            m_Languages.Add("fon", "Fon");
            m_Languages.Add("fra", "French");
            m_Languages.Add("fre", "French");
            m_Languages.Add("frm", "French, Middle (ca. 1400-1600)");
            m_Languages.Add("fro", "French, Old (842- ca. 1400)");
            m_Languages.Add("fry", "Frisian");
            m_Languages.Add("ful", "Fulah");
            m_Languages.Add("gaa", "Ga");
            m_Languages.Add("gae", "Gaelic (Scots)");
            m_Languages.Add("gai", "Irish");
            m_Languages.Add("gay", "Gayo");
            m_Languages.Add("gdh", "Gaelic (Scots)");
            m_Languages.Add("gem", "Germanic (Other)");
            m_Languages.Add("geo", "Georgian");
            m_Languages.Add("ger", "German");
            m_Languages.Add("gez", "Geez");
            m_Languages.Add("gil", "Gilbertese");
            m_Languages.Add("glg", "Gallegan");
            m_Languages.Add("gmh", "German, Middle High (ca. 1050-1500)");
            m_Languages.Add("goh", "German, Old High (ca. 750-1050)");
            m_Languages.Add("gon", "Gondi");
            m_Languages.Add("got", "Gothic");
            m_Languages.Add("grb", "Grebo");
            m_Languages.Add("grc", "Greek, Ancient (to 1453)");
            m_Languages.Add("gre", "Greek");
            m_Languages.Add("grn", "Guarani");
            m_Languages.Add("guj", "Gujarati");
            m_Languages.Add("hai", "Haida");
            m_Languages.Add("hau", "Hausa");
            m_Languages.Add("haw", "Hawaiian");
            m_Languages.Add("heb", "Hebrew");
            m_Languages.Add("her", "Herero");
            m_Languages.Add("hil", "Hiligaynon");
            m_Languages.Add("him", "Himachali");
            m_Languages.Add("hin", "Hindi");
            m_Languages.Add("hmo", "Hiri Motu");
            m_Languages.Add("hun", "Hungarian");
            m_Languages.Add("hup", "Hupa");
            m_Languages.Add("hye", "Armenian");
            m_Languages.Add("iba", "Iban");
            m_Languages.Add("ibo", "Igbo");
            m_Languages.Add("ice", "Icelandic");
            m_Languages.Add("ijo", "Ijo");
            m_Languages.Add("iku", "Inuktitut");
            m_Languages.Add("ilo", "Iloko");
            m_Languages.Add("ina", "Interlingua (International Auxiliary language Association)");
            m_Languages.Add("inc", "Indic (Other)");
            m_Languages.Add("ind", "Indonesian");
            m_Languages.Add("ine", "Indo-European (Other) Interlingue");
            m_Languages.Add("ipk", "Inupiak");
            m_Languages.Add("ira", "Iranian (Other)");
            m_Languages.Add("iri", "Irish");
            m_Languages.Add("iro", "Iroquoian uages");
            m_Languages.Add("isl", "Icelandic");
            m_Languages.Add("ita", "Italian");
            m_Languages.Add("jav", "Javanese");
            m_Languages.Add("jaw", "Javanese");
            m_Languages.Add("jpn", "Japanese");
            m_Languages.Add("jpr", "Judeo-Persian");
            m_Languages.Add("jrb", "Judeo-Arabic");
            m_Languages.Add("kaa", "Kara-Kalpak");
            m_Languages.Add("kab", "Kabyle");
            m_Languages.Add("kac", "Kachin");
            m_Languages.Add("kal", "Greenlandic");
            m_Languages.Add("kam", "Kamba");
            m_Languages.Add("kan", "Kannada");
            m_Languages.Add("kar", "Karen");
            m_Languages.Add("kas", "Kashmiri");
            m_Languages.Add("kat", "Georgian");
            m_Languages.Add("kau", "Kanuri");
            m_Languages.Add("kaw", "Kawi");
            m_Languages.Add("kaz", "Kazakh");
            m_Languages.Add("kha", "Khasi");
            m_Languages.Add("khi", "Khoisan (Other)");
            m_Languages.Add("khm", "Khmer");
            m_Languages.Add("kho", "Khotanese");
            m_Languages.Add("kik", "Kikuyu");
            m_Languages.Add("kin", "Kinyarwanda");
            m_Languages.Add("kir", "Kirghiz");
            m_Languages.Add("kok", "Konkani");
            m_Languages.Add("kom", "Komi");
            m_Languages.Add("kon", "Kongo");
            m_Languages.Add("kor", "Korean");
            m_Languages.Add("kpe", "Kpelle");
            m_Languages.Add("kro", "Kru");
            m_Languages.Add("kru", "Kurukh");
            m_Languages.Add("kua", "Kuanyama");
            m_Languages.Add("kum", "Kumyk");
            m_Languages.Add("kur", "Kurdish");
            m_Languages.Add("kus", "Kusaie");
            m_Languages.Add("kut", "Kutenai");
            m_Languages.Add("lad", "Ladino");
            m_Languages.Add("lah", "Lahnda");
            m_Languages.Add("lam", "Lamba");
            m_Languages.Add("lao", "Lao");
            m_Languages.Add("lat", "Latin");
            m_Languages.Add("lav", "Latvian");
            m_Languages.Add("lez", "Lezghian");
            m_Languages.Add("lin", "Lingala");
            m_Languages.Add("lit", "Lithuanian");
            m_Languages.Add("lol", "Mongo");
            m_Languages.Add("loz", "Lozi");
            m_Languages.Add("ltz", "Letzeburgesch");
            m_Languages.Add("lub", "Luba-Katanga");
            m_Languages.Add("lug", "Ganda");
            m_Languages.Add("lui", "Luiseno");
            m_Languages.Add("lun", "Lunda");
            m_Languages.Add("luo", "Luo (Kenya and Tanzania)");
            m_Languages.Add("mac", "Macedonian");
            m_Languages.Add("mad", "Madurese");
            m_Languages.Add("mag", "Magahi");
            m_Languages.Add("mah", "Marshall");
            m_Languages.Add("mai", "Maithili");
            m_Languages.Add("mak", "Macedonian Makasar");
            m_Languages.Add("mal", "Malayalam");
            m_Languages.Add("man", "Mandingo");
            m_Languages.Add("mao", "Maori");
            m_Languages.Add("map", "Austronesian (Other)");
            m_Languages.Add("mar", "Marathi");
            m_Languages.Add("mas", "Masai");
            m_Languages.Add("max", "Manx");
            m_Languages.Add("may", "Malay");
            m_Languages.Add("men", "Mende");
            m_Languages.Add("mga", "Irish, Middle (900 - 1200)");
            m_Languages.Add("mic", "Micmac");
            m_Languages.Add("min", "Minangkabau");
            m_Languages.Add("mis", "Miscellaneous (Other)");
            m_Languages.Add("mkh", "Mon-Kmer (Other)");
            m_Languages.Add("mlg", "Malagasy");
            m_Languages.Add("mlt", "Maltese");
            m_Languages.Add("mni", "Manipuri");
            m_Languages.Add("mno", "Manobo Languages");
            m_Languages.Add("moh", "Mohawk");
            m_Languages.Add("mol", "Moldavian");
            m_Languages.Add("mon", "Mongolian");
            m_Languages.Add("mos", "Mossi");
            m_Languages.Add("mri", "Maori");
            m_Languages.Add("msa", "Malay");
            m_Languages.Add("mul", "Multiple Languages");
            m_Languages.Add("mun", "Munda Languages");
            m_Languages.Add("mus", "Creek");
            m_Languages.Add("mwr", "Marwari");
            m_Languages.Add("mya", "Burmese");
            m_Languages.Add("myn", "Mayan Languages");
            m_Languages.Add("nah", "Aztec");
            m_Languages.Add("nai", "North American Indian (Other)");
            m_Languages.Add("nau", "Nauru");
            m_Languages.Add("nav", "Navajo");
            m_Languages.Add("nbl", "Ndebele, South");
            m_Languages.Add("nde", "Ndebele, North");
            m_Languages.Add("ndo", "Ndongo");
            m_Languages.Add("nep", "Nepali");
            m_Languages.Add("new", "Newari");
            m_Languages.Add("nic", "Niger-Kordofanian (Other)");
            m_Languages.Add("niu", "Niuean");
            m_Languages.Add("nla", "Dutch");
            m_Languages.Add("nno", "Norwegian (Nynorsk)");
            m_Languages.Add("non", "Norse, Old");
            m_Languages.Add("nor", "Norwegian");
            m_Languages.Add("nso", "Sotho, Northern");
            m_Languages.Add("nub", "Nubian Languages");
            m_Languages.Add("nya", "Nyanja");
            m_Languages.Add("nym", "Nyamwezi");
            m_Languages.Add("nyn", "Nyankole");
            m_Languages.Add("nyo", "Nyoro");
            m_Languages.Add("nzi", "Nzima");
            m_Languages.Add("oci", "Langue d'Oc (post 1500)");
            m_Languages.Add("oji", "Ojibwa");
            m_Languages.Add("ori", "Oriya");
            m_Languages.Add("orm", "Oromo");
            m_Languages.Add("osa", "Osage");
            m_Languages.Add("oss", "Ossetic");
            m_Languages.Add("ota", "Turkish, Ottoman (1500 - 1928)");
            m_Languages.Add("oto", "Otomian Languages");
            m_Languages.Add("paa", "Papuan-Australian (Other)");
            m_Languages.Add("pag", "Pangasinan");
            m_Languages.Add("pal", "Pahlavi");
            m_Languages.Add("pam", "Pampanga");
            m_Languages.Add("pan", "Panjabi");
            m_Languages.Add("pap", "Papiamento");
            m_Languages.Add("pau", "Palauan");
            m_Languages.Add("peo", "Persian, Old (ca 600 - 400 B.C.)");
            m_Languages.Add("per", "Persian");
            m_Languages.Add("phn", "Phoenician");
            m_Languages.Add("pli", "Pali");
            m_Languages.Add("pol", "Polish");
            m_Languages.Add("pon", "Ponape");
            m_Languages.Add("por", "Portuguese");
            m_Languages.Add("pra", "Prakrit uages");
            m_Languages.Add("pro", "Provencal, Old (to 1500)");
            m_Languages.Add("pus", "Pushto");
            m_Languages.Add("que", "Quechua");
            m_Languages.Add("raj", "Rajasthani");
            m_Languages.Add("rar", "Rarotongan");
            m_Languages.Add("roa", "Romance (Other)");
            m_Languages.Add("roh", "Rhaeto-Romance");
            m_Languages.Add("rom", "Romany");
            m_Languages.Add("ron", "Romanian");
            m_Languages.Add("rum", "Romanian");
            m_Languages.Add("run", "Rundi");
            m_Languages.Add("rus", "Russian");
            m_Languages.Add("sad", "Sandawe");
            m_Languages.Add("sag", "Sango");
            m_Languages.Add("sah", "Yakut");
            m_Languages.Add("sai", "South American Indian (Other)");
            m_Languages.Add("sal", "Salishan Languages");
            m_Languages.Add("sam", "Samaritan Aramaic");
            m_Languages.Add("san", "Sanskrit");
            m_Languages.Add("sco", "Scots");
            m_Languages.Add("scr", "Croatian");
            m_Languages.Add("sel", "Selkup");
            m_Languages.Add("sem", "Semitic (Other)");
            m_Languages.Add("sga", "Irish, Old (to 900)");
            m_Languages.Add("shn", "Shan");
            m_Languages.Add("sid", "Sidamo");
            m_Languages.Add("sin", "Singhalese");
            m_Languages.Add("sio", "Siouan Languages");
            m_Languages.Add("sit", "Sino-Tibetan (Other)");
            m_Languages.Add("sla", "Slavic (Other)");
            m_Languages.Add("slk", "Slovak");
            m_Languages.Add("slo", "Slovak");
            m_Languages.Add("slv", "Slovenian");
            m_Languages.Add("smi", "Sami Languages");
            m_Languages.Add("smo", "Samoan");
            m_Languages.Add("sna", "Shona");
            m_Languages.Add("snd", "Sindhi");
            m_Languages.Add("sog", "Sogdian");
            m_Languages.Add("som", "Somali");
            m_Languages.Add("son", "Songhai");
            m_Languages.Add("sot", "Sotho, Southern");
            m_Languages.Add("spa", "Spanish");
            m_Languages.Add("sqi", "Albanian");
            m_Languages.Add("srd", "Sardinian");
            m_Languages.Add("srr", "Serer");
            m_Languages.Add("ssa", "Nilo-Saharan (Other)");
            m_Languages.Add("ssw", "Siswant Swazi");
            m_Languages.Add("suk", "Sukuma");
            m_Languages.Add("sun", "Sudanese");
            m_Languages.Add("sus", "Susu");
            m_Languages.Add("sux", "Sumerian");
            m_Languages.Add("sve", "Swedish");
            m_Languages.Add("swa", "Swahili");
            m_Languages.Add("swe", "Swedish");
            m_Languages.Add("syr", "Syriac");
            m_Languages.Add("tah", "Tahitian");
            m_Languages.Add("tam", "Tamil");
            m_Languages.Add("tat", "Tatar");
            m_Languages.Add("tel", "Telugu");
            m_Languages.Add("tem", "Timne");
            m_Languages.Add("ter", "Tereno");
            m_Languages.Add("tgk", "Tajik");
            m_Languages.Add("tgl", "Tagalog");
            m_Languages.Add("tha", "Thai");
            m_Languages.Add("tib", "Tibetan");
            m_Languages.Add("tig", "Tigre");
            m_Languages.Add("tir", "Tigrinya");
            m_Languages.Add("tiv", "Tivi");
            m_Languages.Add("tli", "Tlingit");
            m_Languages.Add("tmh", "Tamashek");
            m_Languages.Add("tog", "Tonga (Nyasa)");
            m_Languages.Add("ton", "Tonga (Tonga Islands)");
            m_Languages.Add("tru", "Truk");
            m_Languages.Add("tsi", "Tsimshian");
            m_Languages.Add("tsn", "Tswana");
            m_Languages.Add("tso", "Tsonga");
            m_Languages.Add("tuk", "Turkmen");
            m_Languages.Add("tum", "Tumbuka");
            m_Languages.Add("tur", "Turkish");
            m_Languages.Add("tut", "Altaic (Other)");
            m_Languages.Add("twi", "Twi");
            m_Languages.Add("tyv", "Tuvinian");
            m_Languages.Add("uga", "Ugaritic");
            m_Languages.Add("uig", "Uighur");
            m_Languages.Add("ukr", "Ukrainian");
            m_Languages.Add("umb", "Umbundu");
            m_Languages.Add("und", "Undetermined");
            m_Languages.Add("urd", "Urdu");
            m_Languages.Add("uzb", "Uzbek");
            m_Languages.Add("vai", "Vai");
            m_Languages.Add("ven", "Venda");
            m_Languages.Add("vie", "Vietnamese");
            m_Languages.Add("vol", "Volapük");
            m_Languages.Add("vot", "Votic");
            m_Languages.Add("wak", "Wakashan Languages");
            m_Languages.Add("wal", "Walamo");
            m_Languages.Add("war", "Waray");
            m_Languages.Add("was", "Washo");
            m_Languages.Add("wel", "Welsh");
            m_Languages.Add("wen", "Sorbian Languages");
            m_Languages.Add("wol", "Wolof");
            m_Languages.Add("xho", "Xhosa");
            m_Languages.Add("yao", "Yao");
            m_Languages.Add("yap", "Yap");
            m_Languages.Add("yid", "Yiddish");
            m_Languages.Add("yor", "Yoruba");
            m_Languages.Add("zap", "Zapotec");
            m_Languages.Add("zen", "Zenaga");
            m_Languages.Add("zha", "Zhuang");
            m_Languages.Add("zho", "Chinese");
            m_Languages.Add("zul", "Zulu");
            m_Languages.Add("zun", "Zuni");
            m_Languages.Add("tkl", "Tokelau");
            m_Languages.Add("hrv", "Croatian");
            m_Languages.Add("bos", "Bosnian");
            m_Languages.Add("scc", "Serbian");
            m_Languages.Add("srp", "Serbian");

            #endregion <<< Language code / English description dictionary
        }
    }
}
