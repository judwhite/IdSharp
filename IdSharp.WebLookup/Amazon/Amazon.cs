using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using IdSharp.Common.Utils;

namespace IdSharp.WebLookup.Amazon
{
    /// <summary>
    /// Amazon
    /// </summary>
    public static class Amazon
    {
        private static readonly Dictionary<AmazonServer, string> _amazonServers;
        internal const string HttpRequestUri = "/onca/xml";

        static Amazon()
        {
            _amazonServers = new Dictionary<AmazonServer, string>();

            _amazonServers.Add(AmazonServer.UnitedStates, "amazonaws.com");
            _amazonServers.Add(AmazonServer.Germany, "amazonaws.de");
            _amazonServers.Add(AmazonServer.Japan, "amazonaws.jp");
            _amazonServers.Add(AmazonServer.UnitedKingdom, "amazonaws.co.uk");
            _amazonServers.Add(AmazonServer.France, "amazonaws.fr");
            _amazonServers.Add(AmazonServer.Canada, "amazonaws.ca");
        }

        private static void FixSearchString(ref string value)
        {
            if (value == null)
                return;

            // remove everything after parentheses
            int parenIndex = value.IndexOf('(');
            if (parenIndex > 0)
                value = value.Substring(0, parenIndex);

            // remove all characters that are not letters, digits, or spaces
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (!char.IsLetterOrDigit(c) && c != ' ')
                {
                    value = value.Replace(c, ' ');
                    i--;
                }
            }

            // remove dangling digits
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsDigit(c))
                {
                    if (i > 0)
                    {
                        if (value[i - 1] != ' ' || (i != value.Length - 1 && value[i + 1] != ' '))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (i != value.Length - 1 && value[i + 1] != ' ')
                        {
                            continue;
                        }
                    }

                    value = value.Remove(i, 1);
                    i--;
                }
            }

            // remove common words
            value = value.ToLower();
            
            value = value.Replace(" the ", " ");
            value = value.Replace(" a ", " ");
            value = value.Replace(" of ", " ");
            value = value.Replace(" and ", " ");
            value = value.Replace(" an ", " ");
            value = value.Replace(" ost ", " ");
            value = value.Replace(" in ", " ");
            value = value.Replace(" disc ", " ");
            value = value.Replace(" disk ", " ");
            value = value.Replace(" cd ", " ");

            value = value.Trim();

            if (value == "various")
                value = string.Empty;

            //value = value.Replace(' ', '+');
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="awsAccessKeyId">Required: Your AWSAccessKeyId.</param>
        /// <param name="secretAccessKey">Required: Your Secret Access Key.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="album">The album.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="page">The page.</param>
        public static AmazonSearchResponse Search(AmazonServer server, string awsAccessKeyId, string secretAccessKey, string artist, string album, string keywords, int page)
        {
            if (string.IsNullOrWhiteSpace(awsAccessKeyId))
                throw new ArgumentNullException("awsAccessKeyId");
            if (string.IsNullOrWhiteSpace(secretAccessKey))
                throw new ArgumentNullException("secretAccessKey");

            String amazonDomain = GetDomain(server);

            FixSearchString(ref artist);
            FixSearchString(ref album);
            FixSearchString(ref keywords);

            string sort = (String.IsNullOrEmpty(artist) ? "artistrank" : "titlerank");
            List<PostData> postData = new List<PostData>();
            postData.Add(new PostData("Service", "AWSECommerceService"));
            postData.Add(new PostData("AWSAccessKeyId", awsAccessKeyId));
            postData.Add(new PostData("Operation", "ItemSearch"));
            postData.Add(new PostData("SearchIndex", "Music"));
            if (!string.IsNullOrEmpty(artist)) postData.Add(new PostData("Artist", artist));
            if (!string.IsNullOrEmpty(album)) postData.Add(new PostData("Title", album));
            if (!string.IsNullOrEmpty(keywords)) postData.Add(new PostData("Keywords", keywords));
            postData.Add(new PostData("ItemPage", page.ToString()));
            postData.Add(new PostData("Sort", sort));
            postData.Add(new PostData("Timestamp", string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}Z", DateTime.UtcNow)));

            string hostHeader = string.Format("ecs.{0}", amazonDomain);
            string signature = GetSignature(postData, hostHeader, secretAccessKey);
            postData.Add(new PostData("Signature", signature));

            string requestUri = string.Format("http://{0}{1}", hostHeader, HttpRequestUri);
            requestUri = Http.GetQueryString(requestUri, postData);
            byte[] byteResponse = Http.Get(requestUri);
            if (byteResponse == null)
                throw new WebException(string.Format("Response from {0} was null", amazonDomain));
            string response = Encoding.UTF8.GetString(byteResponse);

            AmazonSearchResponse result = new AmazonSearchResponse();
            result.TotalPages = 0;
            result.TotalResults = 0;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(response);
            foreach (XmlNode node in xmlDocument.ChildNodes)
            {
                if (node.Name == "ItemSearchResponse")
                {
                    foreach (XmlNode responseNode in node.ChildNodes)
                    {
                        if (responseNode.Name == "OperationRequest")
                        {
                            foreach (XmlNode opReqNode in responseNode.ChildNodes)
                            {
                                if (opReqNode.Name == "Errors")
                                {
                                    string fullErrorMessage = "";
                                    foreach (XmlNode errorNode in opReqNode.ChildNodes)
                                    {
                                        if (errorNode.Name == "Error")
                                        {
                                            string errorMessage = "";
                                            string errorCode = "";
                                            foreach (XmlNode errorItemNode in errorNode.ChildNodes)
                                            {
                                                if (errorItemNode.Name == "Code")
                                                {
                                                    errorCode = errorItemNode.InnerText;
                                                }
                                                else if (errorItemNode.Name == "Message")
                                                {
                                                    errorMessage = errorItemNode.InnerText;
                                                }
                                            }

                                            if (errorMessage != "" || errorCode != "")
                                            {
                                                if (errorCode != "") 
                                                    errorMessage = string.Format("{0} ({1})", errorMessage, errorCode);
                                                if (fullErrorMessage != "") 
                                                    fullErrorMessage += "\n\n";
                                                fullErrorMessage += errorMessage;
                                            }
                                        }
                                    }

                                    if (fullErrorMessage != "")
                                        throw new Exception(fullErrorMessage);
                                }
                            }
                        }
                        else if (responseNode.Name == "Items")
                        {
                            foreach (XmlNode itemNode in responseNode.ChildNodes)
                            {
                                if (itemNode.Name == "TotalResults")
                                {
                                    int totalResults;
                                    if (int.TryParse(itemNode.InnerText, out totalResults))
                                        result.TotalResults = totalResults;
                                }
                                else if (itemNode.Name == "TotalPages")
                                {
                                    int totalPages;
                                    if (int.TryParse(itemNode.InnerText, out totalPages))
                                        result.TotalPages = totalPages;
                                }
                                else if (itemNode.Name == "Item")
                                {
                                    AmazonAlbum albumItem = new AmazonAlbum(server, awsAccessKeyId, secretAccessKey);
                                    result.Items.Add(albumItem);
                                    foreach (XmlNode itemDetail in itemNode.ChildNodes)
                                    {
                                        if (itemDetail.Name == "ASIN")
                                        {
                                            albumItem.Asin = itemDetail.InnerText;
                                        }
                                        else if (itemDetail.Name == "DetailPageURL")
                                        {
                                            albumItem.DetailPageUrl = itemDetail.InnerText;
                                        }
                                        else if (itemDetail.Name == "ItemAttributes")
                                        {
                                            foreach (XmlNode itemAttribute in itemDetail.ChildNodes)
                                            {
                                                if (itemAttribute.Name == "Artist")
                                                {
                                                    albumItem.Artist = itemAttribute.InnerText;
                                                }
                                                else if (itemAttribute.Name == "Manufacturer")
                                                {
                                                    albumItem.Manufacturer = itemAttribute.InnerText;
                                                }
                                                else if (itemAttribute.Name == "Title")
                                                {
                                                    albumItem.Album = itemAttribute.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        internal static string GetSignature(List<PostData> postData, string hostHeader, string secretAccessKey)
        {
            List<PostData> ordered = GetOrderedPostData(postData);
            StringBuilder getString = new StringBuilder();
            foreach (PostData item in ordered)
            {
                if (getString.Length != 0)
                    getString.Append("&");
                getString.Append(item.Field);
                getString.Append("=");
                foreach (char c in item.Value)
                {
                    if (c <= 255)
                    {
                        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c == '-') || (c == '_') || (c == '.') || (c == '~'))
                        {
                            getString.Append(c);
                        }
                        else
                        {
                            getString.Append(string.Format("%{0:X2}", (int)c));
                        }
                    }
                }
            }

            string stringToSign = string.Format("GET{0}{1}{0}{2}{0}{3}", "\n", hostHeader, HttpRequestUri, getString);

            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretAccessKey));
            byte[] hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(stringToSign));
            string signature = Encoding.ASCII.GetString(Base64Encoder.Encode(hash)).Replace("+", "%2B").Replace("=", "%3D");

            return signature;
        }

        private static List<PostData> GetOrderedPostData(IEnumerable<PostData> postData)
        {
            List<PostData> newList = new List<PostData>(postData);
            newList.Sort(new PostDataComparer());
            return newList;
        }

        private class PostDataComparer : Comparer<PostData>
        {
            public override int Compare(PostData x, PostData y)
            {
                return string.CompareOrdinal(x.Field, y.Field);
            }
        }

        /// <summary>
        /// Gets the domain for a specified AmazonServer.
        /// </summary>
        /// <param name="server">The server.</param>
        public static string GetDomain(AmazonServer server)
        {
            return _amazonServers[server];
        }
    }
}
