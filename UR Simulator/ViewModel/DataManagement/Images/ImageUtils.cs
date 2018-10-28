using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

using UrbanRivalsCore.Model;
using UrbanRivalsManager;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public enum CharacterImageFormat
    {
        Color800x640 = 0,
        Color140x112,
        Gray140x112,
        TransparentHD,
        TransparentHDLarge,
    }

    internal class CharacterImageFileFormatString
    {
        private static readonly string FirstPart = "{0:D4}-{1}";
        public static readonly string Color800x640 = FirstPart + ".png";
        public static readonly string Color140x112 = FirstPart + "-small.gif";
        public static readonly string Gray140x112 = FirstPart + "-gray.gif";
        public static readonly string TransparentHD = FirstPart + "-hd.png";
        public static readonly string TransparentHDLarge = FirstPart + "-hdl.png";
    }

    internal class CharacterImageUrlStringFormat
    {
        private static readonly string FirstPart = @"ur-img.com/urimages/perso/{0}/{0}_{1}_N{2}_";
        public static readonly string Color800x640 = FirstPart + "std.png";
        public static readonly string Color140x112 = FirstPart + "std_160.gif";
        public static readonly string Gray140x112 = FirstPart + "std_160_gray.gif";
        public static readonly string TransparentHD = FirstPart + "hd_184_transparent.png";
        public static readonly string TransparentHDLarge = FirstPart + "hd_673_transparent.png";
    }

    internal class CharacterImageIdentifier
    {
        public int CardBaseId { get; }
        public string CardName { get; }
        public string ClanName { get; }
        public int Level { get; }
        public CharacterImageFormat Format { get; }

        private string _url = "";
        public string Url
        {
            get
            {
                if (_url == "")
                    _url = GetUrl();
                return _url;
            }
        }
        private string GetUrl()
        {
            string formatString = "";
            switch (Format)
            {
                case CharacterImageFormat.Color140x112:
                    formatString = CharacterImageUrlStringFormat.Color140x112;
                    break;
                case CharacterImageFormat.Color800x640:
                    formatString = CharacterImageUrlStringFormat.Color800x640;
                    break;
                case CharacterImageFormat.Gray140x112:
                    formatString = CharacterImageUrlStringFormat.Gray140x112;
                    break;
                case CharacterImageFormat.TransparentHD:
                    formatString = CharacterImageUrlStringFormat.TransparentHD;
                    break;
                case CharacterImageFormat.TransparentHDLarge:
                    formatString = CharacterImageUrlStringFormat.TransparentHDLarge;
                    break;
            }
            return String.Format(formatString, CardName, ClanName, Level);
        }

        private string _filename = "";
        public string Filename
        {
            get
            {
                if (_filename == "")
                    _filename = GetFilename();
                return _filename;
            }
        }
        private string GetFilename()
        {
            string formatString = "";
            switch (Format)
            {
                case CharacterImageFormat.Color800x640:
                    formatString = CharacterImageFileFormatString.Color800x640;
                    break;
                case CharacterImageFormat.Color140x112:
                    formatString = CharacterImageFileFormatString.Color140x112;
                    break;
                case CharacterImageFormat.Gray140x112:
                    formatString = CharacterImageFileFormatString.Gray140x112;
                    break;
                case CharacterImageFormat.TransparentHD:
                    formatString = CharacterImageFileFormatString.TransparentHD;
                    break;
                case CharacterImageFormat.TransparentHDLarge:
                    formatString = CharacterImageFileFormatString.TransparentHDLarge;
                    break;
            }
            return String.Format(formatString, CardBaseId, Level);
        }

        private CharacterImageIdentifier() { }
        public CharacterImageIdentifier(CardInstance cardInstance, CharacterImageFormat format)
            : this(cardInstance.cardBaseId, cardInstance.name, cardInstance.clan.name, cardInstance.level, format)
        { }
        public CharacterImageIdentifier(CardDefinition cardDefinition, int level, CharacterImageFormat format)
            : this(cardDefinition.id, cardDefinition.name, cardDefinition.clan.name, level, format)
        { }
        public CharacterImageIdentifier(int cardBaseId, string cardName, string clanName, int level, CharacterImageFormat format)
        {
            if (cardBaseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(cardBaseId), cardBaseId, "Must be greater than 0");
            if (String.IsNullOrWhiteSpace(cardName))
                throw new ArgumentNullException("Can't be null or empty", nameof(cardName));
            if (String.IsNullOrWhiteSpace(clanName))
                throw new ArgumentNullException("Can't be null or empty", nameof(clanName));
            if (level < 1 || level > 5)
                throw new ArgumentOutOfRangeException(nameof(level), level, "Must be between 1 and 5 inclusive");
            if ((int)format < 0 || (int)format > Constants.EnumMaxAllowedValues.CharacterImageFormat)
                throw new ArgumentOutOfRangeException(nameof(format), format, "Must be a valid " + nameof(CharacterImageFormat));

            CardBaseId = cardBaseId;
            CardName = cardName;
            ClanName = clanName;
            Level = level;
            Format = format;
        }
    }

    internal class DownloadImagesQueueDoWorkArgument
    {
        public ConcurrentQueue<CharacterImageIdentifier> Queue { get; private set; }
        public FilepathManager FilepathManagerInstance { get; private set; }

        private DownloadImagesQueueDoWorkArgument() { }
        public DownloadImagesQueueDoWorkArgument(ConcurrentQueue<CharacterImageIdentifier> queue, FilepathManager filepathManagerInstance)
        {
            Queue = queue;
            FilepathManagerInstance = filepathManagerInstance;
        }
    }
}
